namespace EveCache
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.IO;

	public class SNodeContainer : IEnumerable<SNode>
	{
		#region Fields
		private List<SNode> _Data;
		#endregion Fields

		#region Properties
		private List<SNode> Data { get { return _Data; } set { _Data = value; } }
		public SNode First { get { return Data[0]; } }
		public SNode Last { get { return Data[Data.Count - 1]; } }
		public int Length { get { return Data.Count; } }
		#endregion Properties

		#region Constructors
		public SNodeContainer() { Data = new List<SNode>(); }

		public SNodeContainer(IEnumerable<SNode> source) { Data = new List<SNode>(source); }

		public SNodeContainer(SNodeContainer source, int start) : this(source, start, source.Length) { }

		public SNodeContainer(SNodeContainer source, int start, int limit)
		{
			Data = new List<SNode>(limit - start);
			for (int i = start; i < limit; i++)
				Data[i] = source[i];
		}
		#endregion Constructors

		#region Indexers
		public SNode this[int index] { get { return Data[index]; } }
		#endregion Indexers

		#region Methods
		public void Add(SNode node) { Data.Add(node); }

		public IEnumerator<SNode> GetEnumerator() { return new Iterator(this); }

		IEnumerator IEnumerable.GetEnumerator() { return new Iterator(this); }

		public Iterator GetIterator() { return new Iterator(this); }
		#endregion Methods

		#region Nested Types
		public class Iterator : IEnumerator<SNode>
		{
			#region Fields
			private List<SNode> _Data;
			private int _Position;
			#endregion Fields

			#region Properties
			public SNode Current
			{
				get
				{
					if (Position < 0)
						return Data[0];
					else if (Position >= Data.Count)
						return Data[Data.Count - 1];
					else
						return Data[Position];
				}
			}
			private List<SNode> Data { get { return _Data; } set { _Data = value; } }
			public SNode Next
			{
				get
				{
					if (Position + 1 < Data.Count)
						return Data[++Position];
					else
						return Data[Data.Count - 1];
				}
			}
			public int Position
			{
				get { return _Position; }
				set
				{
					if (value < 0)
						_Position = 0;
					else if (value >= Data.Count)
						_Position = Data.Count - 1;
					else
						_Position = value;
				}
			}
			public SNode Previous
			{
				get
				{
					if (Position - 1 > 0)
						return Data[--Position];
					else
						return Data[0];
				}
			}
			object IEnumerator.Current { get { return Data[Position]; } }
			#endregion Properties

			#region Constructors
			public Iterator(SNodeContainer parent)
			{
				Data = parent._Data;
				Position = 0;
			}
			#endregion Constructors

			#region Methods
			public void Dispose() { }

			public bool MoveNext() { return ++Position < Data.Count; }

			public void Reset() { Position = -1; }

			public int Seek(int offset) { return Seek(offset, SeekOrigin.Current); }

			public int Seek(int offset, SeekOrigin origin)
			{
				switch (origin)
				{
					case SeekOrigin.Begin:
						Position = offset;
						break;
					case SeekOrigin.Current:
						Position += offset;
						break;
					case SeekOrigin.End:
						Position = Data.Count - offset - 1;
						break;
					default:
						throw new IOException("Invalid origin");
				}
				return Position;
			}
			#endregion Methods
		}
		#endregion Nested Types
	}
}
