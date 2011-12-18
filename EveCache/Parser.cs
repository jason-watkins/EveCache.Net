namespace EveCache
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	public class Parser
	{
		#region Fields
		private CacheFile_Iterator _Iter;
		private int _Length;
		private uint _ShareCount;
		private uint _ShareCursor;
		SNode[][] _ShareObj;
		uint[] _ShareMap;
		private List<SNode[]> _Streams;
		#endregion Fields

		#region Properties
		private CacheFile_Iterator Iter { get { return _Iter; } set { _Iter = value; } }
		protected int Length { get { return _Length; } set { _Length = value; } }
		private uint ShareCount { get { return _ShareCount; } set { _ShareCount = value; } }
		private uint ShareCursor { get { return _ShareCursor; } set { _ShareCursor = value; } }
		private SNode[][] ShareObj { get { return _ShareObj; } set { _ShareObj = value; } }
		private uint[] ShareMap { get { return _ShareMap; } set { _ShareMap = value; } }
		public List<SNode[]> Streams { get { return _Streams; } private set { _Streams = value; } }
		#endregion Properties

		#region Events
		#endregion Events

		#region Constructors
		public Parser(CacheFile_Iterator iter);
		#endregion Constructors

		#region Methods
		protected SNode[] GetDBRow();

		public void Parse();

		protected void Parse(SNode[] node, int limit);

		protected SNode[] ParseOne;

		protected void SharAdd(SNode[] obj);

		protected SNode[] ShareGet(uint id);

		protected uint ShareInit();

		protected void SharSkip();
		#endregion Methods

		#region Enums
		#endregion Enums
	}
}
