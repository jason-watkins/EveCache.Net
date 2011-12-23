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
		#region Static Fields
		private static int __count__;
		private static Dictionary<int, bool> __nodeConsumed__;
		private static List<SNode> __nodes__;
		#endregion
		#region Fields
		protected int __id__;
		#endregion Fields

		#region Constructors
		static SNode()
		{
			__count__ = 0;
			__nodeConsumed__ = new Dictionary<int, bool>();
			__nodes__ = new List<SNode>();
		}
		#endregion Constructors

		#region Static Methods
		public static void DumpNodes(string fileName)
		{
			foreach (SNode node in __nodes__)
			{
				__nodeConsumed__[node.__id__] = false;
			}

			StringBuilder fileContents = new StringBuilder();
			foreach (SNode n in __nodes__)
			{
				if (__nodeConsumed__[n.__id__])
					continue;
				if (n.Type == EStreamCode.EStreamStart)
				{
					__nodeConsumed__[n.__id__] = true;
					continue;
				}

				fileContents.Append(n.ToString());
				fileContents.Append(String.Format("[{0:00}]\n", n.__id__));
				fileContents.Append(DumpNode(n, 1));

				__nodeConsumed__[n.__id__] = true;
			}
			File.WriteAllText(Path.ChangeExtension(fileName, ".structure"), fileContents.ToString());
		}

		public static string DumpNode(SNode node, int offset)
		{
			if (node.Members.Length == 0)
				return "";

			StringBuilder sb = new StringBuilder();
			sb.Append("(\n".PadLeft(2 * offset));
			foreach (SNode n in node.Members)
			{
				sb.Append(n.ToString().PadLeft((2 * offset) + n.ToString().Length));
				sb.Append(String.Format("[{0:00}]\n", n.__id__));
				if (n.Members.Length > 0)
					sb.Append(DumpNode(n, offset + 1));
				__nodeConsumed__[n.__id__] = true;
			}
			sb.Append(")\n".PadLeft(2 * offset));

			return sb.ToString();
		}
		#endregion
	}
}
