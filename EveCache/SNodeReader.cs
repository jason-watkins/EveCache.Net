#region License
/* EveCache.Net - EVE Cache File Reader Library
 * Copyright (C) 2011 Jason Watkins <jason@blacksunsystems.net>
 *
 * Based on libevecache
 * Copyright (C) 2009-2010  StackFoundry LLC and Yann Ramin
 * http: * dev.eve-central.com/libevecache/
 * http: * gitorious.org/libevecache
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
	using System.Collections;
	using System.Collections.Generic;
	using System.IO;

	public class SNodeReader : IEnumerable
	{
		#region Fields
		private List<SNode> _data;
		private int _position;
		#endregion Fields

		#region Properties
		public SNode Begin { get { return _data[0]; } }
		public SNode Current { get { return _data[_position]; } }
		public SNode End { get { return _data[_data.Count - 1]; } }
		public SNode Next { get { return _position + 1 < Length ? _data[++_position] : End; } }
		public SNode Previous { get { return _position - 1 > 0 ? _data[--_position] : Begin; } }
		public int Length { get { return _data.Count; } }
		#endregion Properties

		#region Constructors
		public SNodeReader()
		{
			_data = new List<SNode>();
			_position = 0;
		}

		public SNodeReader(LinkedList<SNode> source) : this() { _data = new List<SNode>(source); }

		public SNodeReader(SNodeReader source) : this() { _data = new List<SNode>(source._data); }

		public SNodeReader(SNodeReader source, int start) : this(source, start, source.Length) { }

		public SNodeReader(SNodeReader source, int start, int limit) : this()
		{
			_data = new List<SNode>();
			for (int i = start; i < limit; i++)
				Add(source[i]);
		}
		#endregion Constructors

		#region Indexers
		public SNode this[int index]
		{
			get { return _data[index]; }
		}
		#endregion Indexers

		#region Methods
		public void Add(SNode node)
		{
			_data.Add(node);
		}

		public IEnumerator GetEnumerator()
		{
			return _data.GetEnumerator();
		}

		public void Seek(int offset) { Seek(offset, SeekOrigin.Current); }

		public void Seek(int offset, SeekOrigin origin)
		{
			switch (origin)
			{
				case SeekOrigin.Begin:
					if (offset < 0)
						throw new SystemException("offset cannot be negative with SeekOrigin.Begin");
					else if (offset >= Length)
						throw new SystemException("offset is greater than Length");
					else
						_position = offset;
					break;
				case SeekOrigin.Current:
					if (_position + offset < 0)
						throw new SystemException("offset would result in a negative position");
					else if (_position + offset >= Length)
						throw new SystemException("offset + Position is greater than Length");
					else
						_position += offset;
					break;
				case SeekOrigin.End:
					if (offset > 0)
						throw new SystemException("offset cannot be positive with SeekOrigin.End");
					else if (offset + Length < 0)
						throw new SystemException("offset would result in a negative position");
					else
						_position = Length + offset - 1;
					break;
				default:
					throw new SystemException("invalid origin");
			}
		}
		#endregion Methods
	}
}
