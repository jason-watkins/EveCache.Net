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
	using System.IO;
	using System.Text;

	public partial class SNode
	{
		#region Fields
		private SNodeContainer _Members;
		private EStreamCode _Type;
		#endregion Fields

		#region Properties
		public virtual SNodeContainer Members { get { return _Members; } protected set { _Members = value; } }
		public virtual EStreamCode Type { get { return _Type; } set { _Type = value; } }
		#endregion Properties

		#region Constructors
		public SNode(EStreamCode t)
		{
			__id__ = __count__++;
			__nodes__.Add(this);
			Members = new SNodeContainer();
			Type = t;
		}

		public SNode(SNode source)
		{
			Members = new SNodeContainer(source.Members);
			Type = source.Type;
		}
		#endregion Constructors

		#region Methods
		public virtual void AddMember(SNode node)
		{
			Members.Add(node);
		}

		public virtual SNode Clone()
		{
			return new SNode(this);
		}

		public override string ToString()
		{
			return "<SNode [" + Type.ToString() + "]>";
		}
		#endregion Methods
	}
}
