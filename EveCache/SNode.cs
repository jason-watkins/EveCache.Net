#region License
/* EveCache.Net - C# EVE Cache File Reader Library
 * Copyright (C) 2011 Jason Watkins <jason@blacksunsystems.net>
 *
 * Based on libevecache
 * Copyright (C) 2009-2010  StackFoundry LLC and Yann Ramin
 * http://dev.eve-central.com/libevecache/
 * http://gitorious.org/libevecache
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU General Public
 * License as published by the Free Software Foundation; either
 * version 2 of the License, or (at your option) any later version.
 *
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * General Public License for more details.
 *
 * You should have received a copy of the GNU General Public
 * License along with this library; if not, write to the Free Software
 * Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA
 */
#endregion

namespace EveCache
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Text;

	public class SNode
	{
		#region Fields
		private SNodeContainer _Members;
		private EStreamCode _Type;
		#endregion Fields

		#region Properties
		public virtual SNodeContainer Members { get { return _Members; } protected set { _Members = value; } }
		public virtual EStreamCode Type { get { return _Type; } set { _Type = value; } }
		#endregion Properties

		#region Constructors
		public SNode(EStreamCode t)
		{
			Members = new SNodeContainer();
			Type = t;
		}

		public SNode(SNode source)
		{
			Members = new SNodeContainer(source.Members);
			Type = source.Type;
		}
		#endregion Constructors

		#region Methods
		public virtual void AddMember(SNode node)
		{
			Members.Add(node);
		}

		public virtual SNode Clone()
		{
			return new SNode(this);
		}

		public virtual string Repl()
		{
			return " <SNode type " + String.Format("0:x2", Type) + "> ";
		}
		#endregion Methods
	}

	public class SStreamNode : SNode
	{
		#region Constructors
		public SStreamNode() : base(EStreamCode.EStreamStart) { }

		public SStreamNode(EStreamCode t) : base(t) { }

		SStreamNode(SStreamNode source) : base(source) { }
		#endregion Constructors

		#region Methods
		public override SNode Clone()
		{
			return new SStreamNode(this);
		}

		public override string Repl()
		{
			return " <SStreamNode> ";
		}
		#endregion Methods
	}

	public class SDBHeader : SNode
	{
		#region Constructors
		public SDBHeader() : base(EStreamCode.ECompressedRow) { }
		#endregion Constructors

		#region Methods
		public override SNode Clone()
		{
			return new SDBHeader();
		}

		public override string Repl()
		{
			return "<SDBHeader>";
		}
		#endregion Methods
	}

	public class STuple : SNode
	{
		#region Fields
		private uint _GivenLength;
		#endregion Fields

		#region Properties
		public virtual uint GivenLength { get { return _GivenLength; } protected set { _GivenLength = value; } }
		#endregion Properties

		#region Constructors
		public STuple(uint len) : base(EStreamCode.ETuple)
		{
			GivenLength = len;
		}

		public STuple(STuple source) : base(source)
		{
			GivenLength = source.GivenLength;
		}
		#endregion Constructors

		#region Methods
		public override void AddMember(SNode node)
		{
			if (!(Members.Length < GivenLength))
				throw new SystemException();

			Members.Add(node);
		}

		public override SNode Clone()
		{
			return new STuple(this);
		}

		public override string Repl()
		{
			return " <STuple> ";
		}
		#endregion Methods
	}

	public class SDict : SNode
	{
		#region Fields
		private uint _GivenLength;
		#endregion Fields

		#region Properties
		public uint GivenLength { get { return _GivenLength; } protected set { _GivenLength = value; } }
		#endregion Properties

		#region Constructors
		public SDict(uint length) : base(EStreamCode.EDict)
		{
			GivenLength = length;
		}

		public SDict(SDict source) : base(source)
		{
			GivenLength = source.GivenLength;
		}
		#endregion Constructors

		#region Methods
		public override void AddMember(SNode node)
		{
			if (!(Members.Length < GivenLength))
				throw new SystemException();

			Members.Add(node);
		}

		public override SNode Clone()
		{
			return new SDict(this);
		}

		public virtual SNode GetByName(string target)
		{
			if (Members.Length < 2 || (Members.Length & 1) > 0)
				return null;

			SNodeContainer.Iterator iterator = Members.GetIterator();
			iterator.Seek(1);
			SNode n = iterator.Current;
			while (n != Members.Last)
			{
				if (n is SIdent && ((SIdent)n).Value == target)
					return iterator.Previous;

				iterator.Seek(2);
				n = iterator.Current;
			}

			return null;
		}

		public override string Repl()
		{
			return " <SDict> ";
		}
		#endregion Methods
	}
	
	public class SNone : SNode
	{
		#region Constructors
		public SNone() : base(EStreamCode.ENone) { }
		#endregion Constructors

		#region Methods
		public override SNode Clone()
		{
			return new SNone();
		}

		public override string Repl()
		{
			return " <NONE> ";
		}
		#endregion Methods
	}

	public class SMarker : SNode
	{
		#region Fields
		private byte _ID;
		#endregion Fields

		#region Properties
		public byte ID { get { return _ID; } protected set { _ID = value; } }
		#endregion Properties

		#region Constructors
		public SMarker(byte id) : base(EStreamCode.EMarker)
		{
			ID = id;
		}
		#endregion Constructors

		#region Methods
		public override SNode Clone()
		{
			return new SMarker(this.ID);
		}

		public override string Repl()
		{
			return " <SMarker ID: " + ID + " '" + ToString() + "' > ";
		}

		public override string ToString()
		{
			string name = ColumnLookup.LookupName(ID);
			if (name == string.Empty)
				return "UNKNOWN:" + ID;
			else
				return name;
		}
		#endregion Methods
	}

	public class SIdent : SNode
	{
		#region Fields
		private string _Value;
		#endregion Fields

		#region Properties
		public string Value { get { return _Value; } protected set { _Value = value; } }
		#endregion Properties

		#region Constructors
		public SIdent(string name) : base(EStreamCode.EIdent)
		{
			Value = name;
		}
		#endregion Constructors

		#region Methods
		public override SNode Clone()
		{
			return new SIdent(Value);
		}

		public override string Repl()
		{
			return " <SIdent '" + Value + "'> ";
		}
		#endregion Methods
	}

	public class SString : SNode
	{
		#region Fields
		private string _Value;
		#endregion Fields

		#region Properties
		public string Value { get { return _Value; } protected set { _Value = value; } }
		#endregion Properties

		#region Constructors
		public SString(string value) : base(EStreamCode.EString)
		{
			Value = value;
		}
		#endregion Constructors

		#region Methods
		public override SNode Clone()
		{
			return new SString(Value);
		}

		public override string Repl()
		{
			return " <SString '" + Value + "'> ";
		}

		public override string ToString()
		{
			return Value;
		}
		#endregion Methods
	}

	public class SInt : SNode
	{
		#region Fields
		private int _Value;
		#endregion Fields

		#region Properties
		public int Value { get { return _Value; } private set { _Value = value; } }
		#endregion Properties

		#region Constructors
		public SInt(int value) : base(EStreamCode.EInteger)
		{
			Value = Value;
		}
		#endregion Constructors

		#region Methods
		public override SNode Clone()
		{
			return new SInt(Value);
		}

		public override string Repl()
		{
			return " <Sint '" + Value + "'> ";
		}
		#endregion Methods
	}

	public class SReal : SNode
	{
		#region Fields
		private double _Value;
		#endregion Fields

		#region Properties
		public double Value { get { return _Value; } private set { _Value = value; } }
		#endregion Properties

		#region Constructors
		public SReal(double value) : base(EStreamCode.EReal)
		{
			Value = value;
		}
		#endregion Constructors

		#region Methods
		public override SNode Clone()
		{
			return new SReal(Value);
		}

		public override string Repl()
		{
			return " <SReal '" + Value + "'> ";
		}
		#endregion Methods
	}

	public class SLong : SNode
	{
		#region Fields
		private long _Value;
		#endregion Fields

		#region Properties
		public long Value { get { return _Value; } private set { _Value = value; } }
		#endregion Properties

		#region Constructors
		public SLong(long value) : base(EStreamCode.ELong)
		{
			Value = value;
		}
		#endregion Constructors

		#region Methods
		public override SNode Clone()
		{
			return new SLong(Value);
		}

		public override string Repl()
		{
			return " <SLongLong '" + Value + "'> ";
		}
		#endregion Methods
	}

	public class SObject : SNode
	{
		#region Properties
		public string Name
		{ 
			get 
			{
				SNode current = this;
				while (current.Members.Length > 0)
					current = current.Members[0];

				SString str = current as SString;

				if (str != null)
					return str.ToString();

				return string.Empty;
			} 
		}
		#endregion Properties

		#region Constructors
		public SObject() : base(EStreamCode.EObject) { }
		#endregion Constructors

		#region Methods
		public override SNode Clone()
		{
			return new SObject();
		}

		public override string Repl()
		{
			return " <SObject '" + Name + "' " + this + "> ";
		}
		#endregion Methods
	}
	
	public class SSubstream : SNode
	{
		#region Fields
		private int _Length;
		#endregion Fields

		#region Properties
		private int Length { get { return _Length; } set { _Length = value; } }
		#endregion Properties

		#region Constructors
		public SSubstream(int length) : base(EStreamCode.ESubstream)
		{
			Length = length;
		}
		#endregion Constructors

		#region Methods
		public override SNode Clone()
		{
			return new SSubstream(Length);
		}

		public override string Repl()
		{
			return " <SSubStream> ";
		}
		#endregion Methods
	}
	
	public class SDBRow : SNode
	{
		#region Fields
		private List<byte> _Data;
		private int _ID;
		private bool _IsLast;
		#endregion Fields

		#region Properties
		private List<byte> Data { get { return _Data; } set { _Data = value; } }
		private int ID { get { return _ID; } set { _ID = value; } }
		public bool IsLast { get { return _IsLast; } set { _IsLast = value; } }
		#endregion Properties

		#region Constructors
		public SDBRow(int magic, List<byte> data) : base(EStreamCode.ECompressedRow)
		{
			Data = data;
			ID = magic;
			IsLast = false;
		}
		#endregion Constructors

		#region Methods
		public override SNode Clone()
		{
			return new SDBRow(ID, Data);
		}

		public override string Repl()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append(" <DBRow ");

			for (int i = 0; i < Data.Count; i++)
				sb.Append(String.Format("0:x2", Data[i]));

			if (IsLast)
				sb.Append(" LAST");

			sb.Append(">");
			return sb.ToString();
		}
		#endregion Methods
	}
	
	public class SDBRecords : SNode
	{
		#region Constructors
		public SDBRecords() : base(EStreamCode.ECompressedRow) { }
		#endregion Constructors

		#region Methods
		public override SNode Clone()
		{
			return new SDBRecords();
		}

		public override string Repl()
		{
			return string.Empty;
		}
		#endregion Methods
	}
}
