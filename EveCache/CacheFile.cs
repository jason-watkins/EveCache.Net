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
	using System.IO;

	public class CacheFile
	{
		#region Fields
        private byte[] _Contents;
        private int _Length;
        private bool _Valid;
        private string _FileName;
		#endregion Fields

		#region Properties
		private byte[] Contents { get { return _Contents; } set { _Contents = value; } }
		public int Length
		{
			get
			{
				if (!Valid)
					return -1;
				else
					return _Length;
			}
			private set { _Length = value; } 
		}
		private bool Valid { get { return _Valid; } set { _Valid = value; } }
		private string FileName { get { return _FileName; } set { _FileName = value; } }
		#endregion Properties

		#region Constructors
        public CacheFile(string filename)
		{
			Contents = null;
			Length = 0;
			Valid = false;
			this.FileName = filename;
		}

        public CacheFile(CacheFile cf)
		{
			Length = cf.Length;
			Valid = cf.Valid;
			FileName = cf.FileName;
			Contents = new byte[Length];
			Array.Copy(cf.Contents, Contents, Length);
		}

		public CacheFile(List<byte> buf)
		{
			Length = buf.Count + 16;
			Contents = new byte[Length];
			int i;
			for (i = 0; i < buf.Count; i++)
			{
				Contents[i] = buf[i];
			}
			while (i < Length)
			{
				i++;
				Contents[i] = 0;
			}

			Valid = true;
		}
		#endregion Constructors

		#region Methods
        public bool ReadFile()
		{
			FileStream file = File.Open(FileName, FileMode.Open, FileAccess.Read);
			int size = (int)file.Length;
			byte[] buffer = new byte[size];
			file.Seek(0, SeekOrigin.Begin);
			file.Read(buffer, 0, size);
			file.Close();
			Contents = buffer;
			Valid = true;
			Length = (int)size;
	
			return Valid;
		}

        public virtual CacheFile_Iterator End()
		{
			return new CacheFile_Iterator(this, 0, Length);
		}

        public virtual CacheFile_Iterator Begin()
		{
			return new CacheFile_Iterator(this, Length, Length);
		}

		public virtual byte ByteAt(int pos)
		{
			if (pos >= 0 && pos < Length)
				return Contents[pos];
			throw new EndOfFileException();
		}

		public virtual void PeekAt(byte[] data, int at, int len)
		{
			// Broken for big endian...
			Array.Copy(Contents, at, data, 0, len);
		}
		#endregion Methods
	}
}
