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
		public EStreamCode Type { get { return _Type; } set { _Type = value; } }
		#endregion Properties

		#region Constructors
		public SNode(EStreamCode t);
		public SNode(SNode node);
		#endregion Constructors

		#region Methods
		public virtual void AddMember(SNode node);

		public virtual SNode Clone();

		public virtual string Repl();
		#endregion Methods
	}

	public class SStreamNode : SNode
	{
		#region Fields
		#endregion Fields

		#region Properties

		#endregion Properties

		#region Constructors
		public SStreamNode();

		public SStreamNode(EStreamCode t);

		SStreamNode(SStreamNode rhs);
		#endregion Constructors

		#region Methods
		public virtual SStreamNode Clone();

		public virtual string Repl();
		#endregion Methods
	}

	public class SDBHeader : SNode
	{
		#region Fields
		#endregion Fields

		#region Properties
		#endregion Properties

		#region Constructors
		public SDBHeader();
		#endregion Constructors

		#region Methods
		public virtual SDBHeader Clone();

		public virtual string Repl();
		#endregion Methods
	}

	public class STuple : SNode
	{
		#region Fields
		private uint _GivenLength;
		#endregion Fields

		#region Properties
		protected uint GivenLength { get { return _GivenLength; } private set { _GivenLength = value; } }
		#endregion Properties

		#region Constructors
		STuple(uint len);

		STuple(STuple rhs);
		#endregion Constructors

		#region Methods
		public virtual void AddMember(SNode node);

		public virtual STuple Clone();

		public virtual string Repl();
		#endregion Methods
	}

	public class SDict : SNode
	{
		#region Fields
		private uint _GivenLength;
		#endregion Fields

		#region Properties
		protected uint GivenLength { get { return _GivenLength; } private set { _GivenLength = value; } }
		#endregion Properties

		#region Constructors
		public SDict(uint len);

		public SDict(SDict rhs);
		#endregion Constructors

		#region Methods
		public virtual void AddMember(SNode node);

		public virtual SDict Clone();

		public virtual SNode GetByName(string target); 

		public virtual string Repl();
		#endregion Methods
	}

	public class SNone : SNode
	{
		#region Fields
		#endregion Fields

		#region Properties
		#endregion Properties

		#region Constructors
		public SNone();
		#endregion Constructors

		#region Methods
		public virtual SNone Clone();

		public virtual string Repl();
		#endregion Methods
	}

	public class SMarker : SNode
	{
		#region Fields
		private byte _ID;
		#endregion Fields

		#region Properties
		protected byte ID { get { return _ID; } private set { _ID = value; } }
		#endregion Properties

		#region Constructors
		public SMarker(byte id);
		#endregion Constructors

		#region Methods
		public virtual SMarker Clone();

		public virtual string Repl();

		public virtual string String();
		#endregion Methods
	}

	public class SIdent : SNode
	{
		#region Fields
		private string _Name;
		#endregion Fields

		#region Properties
		protected string Name { get { return _Name; } private set { _Name = value; } }
		#endregion Properties

		#region Constructors
		public SIdent(string m);
		#endregion Constructors

		#region Methods
		public virtual SIdent Clone();

		public virtual string Repl();
		#endregion Methods
	}

	public class SString : SNode
	{
		#region Fields
		private string _Name;
		#endregion Fields

		#region Properties
		protected string Name { get { return _Name; } private set { _Name = value; } }
		#endregion Properties

		#region Constructors
		public SString(string m);
		#endregion Constructors

		#region Methods
		public virtual SString Clone();

		public virtual string Repl();

		public virtual string String();
		#endregion Methods
	}

	public class SInt : SNode
	{
		#region Fields
		private int _Value;
		#endregion Fields

		#region Properties
		protected int Value { get { return _Value; } private set { _Value = value; } }
		#endregion Properties

		#region Constructors
		public SInt(int val);
		#endregion Constructors

		#region Methods
		public virtual SInt Clone();

		public virtual string Repl();
		#endregion Methods
	}

	public class SReal : SNode
	{
		#region Fields
		private double _Value;
		#endregion Fields

		#region Properties
		protected double Value { get { return _Value; } private set { _Value = value; } }
		#endregion Properties

		#region Constructors
		public SReal(double val);
		#endregion Constructors

		#region Methods
		public virtual SReal Clone();

		public virtual string Repl();

		public virtual string String();
		#endregion Methods
	}

	public class SLong : SNode
	{
		#region Fields
		private long _Value;
		#endregion Fields

		#region Properties
		protected long Value { get { return _Value; } private set { _Value = value; } }
		#endregion Properties

		#region Constructors
		public SLong(double val);
		#endregion Constructors

		#region Methods
		public virtual SLong Clone();

		public virtual string Repl();

		public virtual string String();
		#endregion Methods
	}

	public class SObject : SNode
	{
		#region Fields
		#endregion Fields

		#region Properties
		public string Name { get { return ""; } }
		#endregion Properties

		#region Constructors
		public SObject();
		#endregion Constructors

		#region Methods
		public virtual SObject Clone();

		public virtual string Repl();
		#endregion Methods
	}

	public class SSubstream : SNode
	{
		#region Fields
		private int _Length;
		#endregion Fields

		#region Properties
		protected int Length { get { return _Length; } private set { _Length = value; } }
		#endregion Properties

		#region Constructors
		public SSubstream(int length);
		#endregion Constructors

		#region Methods
		public virtual SSubstream Clone();

		public virtual string Repl();
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
		public SDBRow(int magic, List<byte> data);
		#endregion Constructors

		#region Methods
		public virtual SDBRow Clone();

		public virtual string Repl();
		#endregion Methods
	}

	public class SDBRecords : SNode
	{
		#region Fields
		#endregion Fields

		#region Properties
		#endregion Properties

		#region Constructors
		public SDBRecords();
		#endregion Constructors

		#region Methods
		public virtual SMarker Clone();

		public virtual string Repl();
		#endregion Methods
	}
}
