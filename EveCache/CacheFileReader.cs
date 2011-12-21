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
	using System.IO;
	using System.Text;

	/// <summary>
	/// A memory stream representing a binary cache file.
	/// </summary>
	public class CacheFileReader
	{
		#region Fields
		private byte[] _Buffer;
		private string _FileName;
		private int _Limit;
		private int _Position;
		private bool _setLimit;
		#endregion Fields

		#region Properties
		public bool AtEnd { get { return !(Position < Length); } }
		private byte[] Buffer { get { return _Buffer; } set { _Buffer = value; } }
		public string FileName { get { return _FileName; } private set { _FileName = value; } }
		public int Length { get { return _Buffer.Length; } }
		public int Limit
		{
			get { return _setLimit ? _Limit : Length; }
			set
			{
				_Limit = value <= Length ? value : Length;
				_setLimit = true;
			}
		}
		public int Position
		{
			get { return _Position; }
			protected set
			{
				if (value < 0)
					_Position = 0;
				else if (value >= Limit)
					_Position = Limit - 1;
				else
					_Position = value;
			}
		}
		#endregion Properties

		#region Constructors
		public CacheFileReader(string fileName)
		{
			FileStream file = File.OpenRead(fileName);
			Buffer = new byte[file.Length + 16];
			file.Seek(0, SeekOrigin.Begin);
			file.Read(Buffer, 0, (int)file.Length);
			file.Close();
			FileName = fileName;
			Position = 0;
		}

		public CacheFileReader(byte[] buffer)
		{
			Buffer = new byte[buffer.Length + 16];
			Array.Copy(buffer, Buffer, buffer.Length);
			FileName = String.Empty;
			Position = 0;
		}

		public CacheFileReader(CacheFileReader source)
		{
			Buffer = source.Buffer;
			FileName = source.FileName;
			Limit = source.Limit;
			Position = source.Position;
		}

		public CacheFileReader(CacheFileReader source, int limit) : this(source)
		{
			Limit = limit + source.Position;
			Position = source.Position;
		}
		#endregion Constructors

		#region Static Methods
		public static bool operator ==(CacheFileReader lhs, CacheFileReader rhs)
		{
			return (lhs.Buffer == rhs.Buffer && lhs.Position == rhs.Position);
		}

		public static bool operator !=(CacheFileReader lhs, CacheFileReader rhs)
		{
			return !(lhs == rhs);
		}
		#endregion
		#region Methods
		public override bool Equals(object obj)
		{
			return this == (CacheFileReader)obj;
		}

		public override int GetHashCode()
		{
			return Buffer.GetHashCode();
		}

		private byte GetByte()
		{
			return Buffer[Position];
		}

		private int GetBytes(byte[] destination, int count)
		{
			int copyLength;
			if (Position + count < Length)
				copyLength = count;
			else
				copyLength = Length - Position;

			Array.Copy(Buffer, Position, destination, 0, copyLength);
			return copyLength;
		}


		public byte PeekByte()
		{
			return GetByte();
		}

		public double PeekDouble()
		{
			byte[] temp = new byte[8];
			GetBytes(temp, 8);
			return BitConverter.ToDouble(temp, 0);
		}

		public float PeekFloat()
		{
			byte[] temp = new byte[4];
			GetBytes(temp, 4);
			return BitConverter.ToSingle(temp, 0);
		}

		public long PeekLong()
		{
			byte[] temp = new byte[8];
			GetBytes(temp, 8);
			return BitConverter.ToInt64(temp, 0);
		}

		public int PeekInt()
		{
			byte[] temp = new byte[4];
			GetBytes(temp, 4);
			return BitConverter.ToInt32(temp, 0);
		}

		public short PeekShort()
		{
			byte[] temp = new byte[2];
			GetBytes(temp, 2);
			return BitConverter.ToInt16(temp, 0);
		}

		public string PeekString(int length)
		{
			byte[] temp = new byte[length];
			GetBytes(temp, length);
			return Encoding.ASCII.GetString(temp);
		}


		public byte ReadByte()
		{
			byte temp = PeekByte();
			Seek(1);
			return temp;
		}

		public double ReadDouble()
		{
			double temp = PeekDouble();
			Seek(8);
			return temp;
		}

		public float ReadFloat()
		{
			float temp = PeekFloat();
			Seek(4);
			return temp;
		}

		public long ReadLong()
		{
			long temp = PeekLong();
			Seek(8);
			return temp;
		}

		public int ReadInt()
		{
			int temp = PeekInt();
			Seek(4);
			return temp;
		}

		public short ReadShort()
		{
			short temp = PeekShort();
			Seek(2);
			return temp;
		}

		public string ReadString(int length)
		{
			string temp = PeekString(length);
			Seek(length);
			return temp;
		}


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
					Position = Limit - offset - 1;
					break;
				default:
					throw new IOException("Invalid origin");
			}
			return Position;
		}
		#endregion Methods
	}
}
