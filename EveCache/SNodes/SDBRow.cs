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
	using System.Collections.Generic;
	using System.Text;
	
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
		public override SNode Clone()
		{
			return new SDBRow(ID, Data);
		}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("<DBRow ");

			for (int i = 0; i < Data.Count; i++)
				sb.Append(String.Format("{0:X2}", Data[i]));

			if (IsLast)
				sb.Append(" LAST");

			sb.Append(">");
			return sb.ToString();
		}
		#endregion Methods
	}
}
