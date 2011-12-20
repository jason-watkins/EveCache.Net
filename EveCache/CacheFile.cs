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

	public class CacheFile
	{
		#region Fields
		private byte[] _Contents;
		private string _FileName;
		#endregion Fields

		#region Properties
		private byte[] Contents { get { return _Contents; } set { _Contents = value; } }
		private string FileName { get { return _FileName; } set { _FileName = value; } }
		public int Length { get { return Valid ? Contents.Length : -1; } }
		public CacheFileReader Reader { get { return new CacheFileReader(this); } }
		private bool Valid { get { return Contents != null; } }
		#endregion Properties

		#region Constructors
		public CacheFile(string filename)
		{
			Contents = null;
			FileName = filename;
		}

		public CacheFile(byte[] buffer)
		{
			Contents = new byte[buffer.Length + 16];
			Array.Copy(buffer, Contents, buffer.Length);
			FileName = string.Empty;
		}

		public CacheFile(CacheFile source)
		{
			FileName = source.FileName;
			if (source.Valid)
			{
				Contents = new byte[source.Length];
				Array.Copy(source.Contents, Contents, source.Length);
			}
			else
				Contents = null;
		}
		#endregion Constructors

		#region Methods
		public virtual byte Peek(int position)
		{
			if (position >= 0 && position < Length)
				return Contents[position];
			else
				throw new EndOfFileException();
		}

		public virtual void Peek(byte[] data, int position, int length)
		{
			// Broken for big endian...
			Array.Copy(Contents, position, data, 0, length);
		}

		public bool ReadFile()
		{
			FileStream file = File.Open(FileName, FileMode.Open, FileAccess.Read);
			int size = (int)file.Length;
			byte[] buffer = new byte[size + 16];
			file.Seek(0, SeekOrigin.Begin);
			file.Read(buffer, 0, size);
			file.Close();
			Contents = buffer;

			return Valid;
		}
		#endregion Methods
	}
}
