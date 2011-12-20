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
	using System.IO;
	using System.Text;

	public class CacheFileReader
	{
		#region Fields
		private CacheFile _data;
		private int _Length;
		private int _Position;

		#endregion Fields

		#region Properties
		public bool AtEnd { get { return !(Position <= Length); } }
		public int Position { get { return _Position; } private set { _Position = value; } }
		public int Length { get { return _Length; } set { _Length = value; } }
		#endregion Properties

		#region Constructors
		public CacheFileReader(CacheFile cf)
		{
			_data = cf;
			Position = 0;
		}

		public CacheFileReader(CacheFile cf, int position) : this(cf)
		{
			Position = position;
		}

		public CacheFileReader(CacheFileReader source)
		{
			_data = source._data;
			Position = source.Position;
		}
		#endregion Constructors

		#region Methods
		public static bool operator ==(CacheFileReader lhs, CacheFileReader rhs)
		{
			if (lhs.Position == rhs.Position && lhs._data == rhs._data)
				return true;
			else
				return false;
		}

		public static bool operator !=(CacheFileReader lhs, CacheFileReader rhs)
		{
			return !(lhs == rhs);
		}

		public override bool Equals(object obj)
		{
			return this == (CacheFileReader)obj;
		}

		public override int GetHashCode()
		{
			return _data.GetHashCode();
		}

		public virtual byte PeekByte()
		{
			return _data.Peek(Position);
		}

		public virtual double PeekDouble()
		{
			byte[] bytes = new byte[8];
			_data.Peek(bytes, Position, 8);
			return BitConverter.ToDouble(bytes, 0);
		}

		public virtual float PeekFloat()
		{
			byte[] bytes = new byte[4];
			_data.Peek(bytes, Position, 4);
			return BitConverter.ToSingle(bytes, 0);
		}

		public virtual int PeekInt()
		{
			byte[] bytes = new byte[4];
			_data.Peek(bytes, Position, 4);
			return BitConverter.ToInt32(bytes, 0);
		}

		public virtual int PeekShort()
		{
			byte[] bytes = new byte[2];
			_data.Peek(bytes, Position, 2);
			return BitConverter.ToInt16(bytes, 0);
		}

		public virtual string PeekString(int len)
		{
			byte[] bytes = new byte[len];
			_data.Peek(bytes, Position, len);
			return Encoding.ASCII.GetString(bytes);
		}


		public byte ReadByte()
		{
			byte r = PeekByte();
			Position += 1;
			return r;
		}

		public double ReadDouble()
		{
			double r = PeekDouble();
			Position += 8;
			return r;
		}

		public float ReadFloat()
		{
			float r = PeekFloat();
			Position += 4;
			return r;
		}

		public int ReadInt()
		{
			int r = PeekInt();
			Position += 4;
			return r;
		}

		public int ReadShort()
		{
			int r = PeekShort();
			Position += 2;
			return r;
		}

		public string ReadString(int len)
		{
			string r = PeekString(len);
			Position += len;
			return r;
		}

		public long ReadLong()
		{
			uint a = (uint)ReadInt();
			uint b = (uint)ReadInt();
			long r = (long)b | ((long)a << 32);
			return r;
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
						Position = offset;
					break;
				case SeekOrigin.Current:
					if (Position + offset < 0)
						throw new SystemException("offset would result in a negative position");
					else if (Position + offset >= Length)
						throw new SystemException("offset + Position is greater than Length");
					else
						Position += offset;
					break;
				case SeekOrigin.End:
					if (offset > 0)
						throw new SystemException("offset cannot be positive with SeekOrigin.End");
					else if (offset + Length < 0)
						throw new SystemException("offset would result in a negative position");
					else
						Position = Length + offset - 1;
					break;
				default:
					throw new SystemException("invalid origin");
			}
		}
		#endregion Methods
	}
}
