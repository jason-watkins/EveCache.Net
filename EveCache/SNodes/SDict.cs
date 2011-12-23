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

	public class SDict : SNode
	{
		#region Fields
		private uint _GivenLength;
		#endregion Fields

		#region Properties
		public uint GivenLength { get { return _GivenLength; } protected set { _GivenLength = value; } }
		#endregion Properties

		#region Constructors
		public SDict(uint length) : base(EStreamCode.EDict)
		{
			GivenLength = length;
		}

		public SDict(SDict source) : base(source)
		{
			GivenLength = source.GivenLength;
		}
		#endregion Constructors

		#region Methods
		public override void AddMember(SNode node)
		{
			if (!(Members.Length < GivenLength))
				throw new SystemException();

			Members.Add(node);
		}

		public override SNode Clone()
		{
			return new SDict(this);
		}

		public virtual SNode GetByName(string target)
		{
			if (Members.Length < 2 || (Members.Length & 1) > 0)
				return null;

			for (int i = 1; i < Members.Length; i += 2)
			{
				if (Members[i] is SIdent && ((SIdent)Members[i]).Value == target)
					return Members[i - 1];
			}

			return null;
		}

		public override string ToString()
		{
			return "<SDict>";
		}
		#endregion Methods
	}
}
