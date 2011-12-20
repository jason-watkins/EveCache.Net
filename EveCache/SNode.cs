namespace EveCache
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	public class SNode
	{
		#region Fields
		private List<SNode> _Members;
		private EStreamCode _Type;
		#endregion Fields

		#region Properties
		public virtual List<SNode> Members { get { return _Members; } protected set { _Members = value; } }
		public virtual EStreamCode Type { get { return _Type; } set { _Type = value; } }
		#endregion Properties

		#region Constructors
		public SNode(EStreamCode t)
		{
			Members = new List<SNode>();
			Type = t;
		}

		public SNode(SNode source)
		{
			Members = new List<SNode>();
			foreach (SNode node in source.Members)
				Members.Add(node.Clone());

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
			return "<SNode type " + String.Format("0:x2", Type) + ">";
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
		public override SStreamNode Clone()
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
		public override SDBHeader Clone()
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
			if (!(Members.Count < GivenLength))
				throw new SystemException();

			Members.Add(node);
		}

		public override STuple Clone()
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
			if (!(Members.Count < GivenLength))
				throw new SystemException();

			Members.Add(node);
		}

		public override SDict Clone()
		{
			return new SDict(this);
		}

		public override SNode GetByName(string target)
		{
			if (Members.Count < 2 || (Members.Count & 1) > 0)
				return null;

			LinkedList<SNode> linkedMembers = new LinkedList<SNode>(Members);
			LinkedListNode<SNode> i = linkedMembers.First.Next;
			for (; i.Next != linkedMembers.Last; i = i.Next.Next)
			{
				if (i.Value is SIdent && ((SIdent)i.Value).Value == target)
					return i.Previous.Value;
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
		public override SNone Clone()
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
		public override SMarker Clone()
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
		public virtual SIdent Clone()
		{
			return new SIdent(Value);
		}

		public virtual string Repl()
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
		public override SString Clone()
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
		public override SInt Clone()
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
		public override SReal Clone()
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
		public override SLong Clone()
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
				while (current.Members.Count > 0)
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
		public override SObject Clone()
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
		public override SSubstream Clone()
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
		public override SDBRow Clone()
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
		public override SDBRecords Clone()
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
