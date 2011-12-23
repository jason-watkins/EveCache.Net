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

	public class SObject : SNode
	{
		#region Properties
		public string Name
		{ 
			get 
			{
				SNode current = this;
				while (current.Members.Length > 0)
					current = current.Members[0];

				SString str = current as SString;

				if (str != null)
					return str.Value;

				return string.Empty;
			} 
		}
		#endregion Properties

		#region Constructors
		public SObject() : base(EStreamCode.EObject) { }

		public SObject(SObject source) : base(source) { }
		#endregion Constructors

		#region Methods
		public override SNode Clone()
		{
			return new SObject(this);
		}

		public override string ToString()
		{
			return String.Format("<SObject '{0}' [{1:X4}]>", Name, __id__);
		}
		#endregion Methods
	}
}
