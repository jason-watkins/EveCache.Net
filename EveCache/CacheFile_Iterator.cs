#region License
///<license>
/// EveCache.Net - EVE Cache File Reader Library
/// Copyright (C) 2011 Jason Watkins
/// 
/// Based on libevecache
/// Copyright (C) 2009-2010  StackFoundry LLC and Yann Ramin
/// http://dev.eve-central.com/libevecache/
/// http://gitorious.org/libevecache
/// 
/// This library is free software; you can redistribute it and/or
/// modify it under the terms of the GNU General Public
/// License as published by the Free Software Foundation; either
/// version 2 of the License, or (at your option) any later version.
/// 
/// This library is distributed in the hope that it will be useful,
/// but WITHOUT ANY WARRANTY; without even the implied warranty of
/// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
/// General Public License for more details.
/// 
/// You should have received a copy of the GNU General Public
/// License along with this library; if not, write to the Free Software
/// Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA
///</license>
#endregion

namespace EveCache
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	public class CacheFile_Iterator
	{
		#region Fields
        private CacheFile _CacheFile;
        private int _LastPeek;
        private int _Position;
        private int _Limit;
		#endregion Fields

		#region Properties
		public bool AtEnd { get { return !(Position <= Limit); } }
		private CacheFile CacheFile { get { return _CacheFile; } set { _CacheFile = value; } }
		private int LastPeek { get { return _LastPeek; } set { _LastPeek = value; } }
		public int Position { get { return _Position; } private set { _Position = value; } }
		public int Limit { get { return _Limit - _Position; } set { _Limit = _Position + value; } }
		#endregion Properties

		#region Constructors
        public CacheFile_Iterator(CacheFile cf, int position, int valid_length)
		{
			CacheFile = cf;
			LastPeek = 0;
			Position = position;
			Limit = valid_length;
		}

        public CacheFile_Iterator(CacheFile_Iterator cfi)
		{
			CacheFile = cfi.CacheFile;
			LastPeek = cfi.LastPeek;
			Position = cfi.Position;
			Limit = cfi.Limit;			
		}
		#endregion Constructors

		#region Methods
		public static bool operator ==(CacheFile_Iterator lhs, CacheFile_Iterator rhs)
		{
			if (lhs.Position == rhs.Position && lhs.CacheFile == rhs.CacheFile)
				return true;
			else
				return false;
		}

		public static bool operator !=(CacheFile_Iterator lhs, CacheFile_Iterator rhs)
		{
			return !(lhs == rhs);
		}

		public static CacheFile_Iterator operator +(CacheFile_Iterator lhs, int len)
		{
			lhs.Advance(len);
			return lhs;
		}

		public bool Advance(int len)
		{
			Position += len;
			return AtEnd;
		}

		public virtual byte PeekChar()
		{
			return CacheFile.ByteAt(Position);
		}

		public virtual double PeekDouble()
		{
			byte[] bytes = new byte[8];
			CacheFile.PeekAt(bytes, Position, 8);
			return BitConverter.ToDouble(bytes, 0);
		}

		public virtual float PeekFloat()
		{
			byte[] bytes = new byte[4];
			CacheFile.PeekAt(bytes, Position, 4);
			return BitConverter.ToSingle(bytes, 0);
		}

		public virtual int PeekInt()
		{
			byte[] bytes = new byte[4];
			CacheFile.PeekAt(bytes, Position, 4);
			return BitConverter.ToInt32(bytes, 0);
		}

		public virtual int PeekShort()
		{
			byte[] bytes = new byte[2];
			CacheFile.PeekAt(bytes, Position, 2);
			return BitConverter.ToInt16(bytes, 0);
		}

		public virtual string PeekString(int len)
		{
			byte[] bytes = new byte[len];
			CacheFile.PeekAt(bytes, Position, len);
			return Encoding.ASCII.GetString(bytes);
		}

		public byte ReadChar()
		{
			byte r = PeekChar();
			Advance(1);
			return r;
		}

		public double ReadDouble()
		{
			double r = PeekDouble();
			Advance(8);
			return r;
		}

		public float ReadFloat()
		{
			float r = PeekFloat();
			Advance(4);
			return r;
		}

		public int ReadInt()
		{
			int r = PeekInt();
			Advance(4);
			return r;
		}

		public int ReadShort()
		{
			int r = PeekShort();
			Advance(2);
			return r;
		}

		public string ReadString(int len)
		{
			string r = PeekString(len);
			Advance(len);
			return r;
		}

		public long ReadLong()
		{
			uint a = (uint)ReadInt();
			uint b = (uint)ReadInt();
			long r = (long)a | ((long)b << 32);
			return r;
		}

		public void Seek(int pos)
		{
			Position = pos;
		}
		#endregion Methods
	}
}
