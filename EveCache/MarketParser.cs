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

	public class MarketParser
	{
		#region Fields
		private MarketList _List;
		private SNodeReader _Stream;
		private bool _Valid;
		#endregion Fields

		#region Properties
		public virtual MarketList List { get { return _List; } private set { _List = value; } }
		protected virtual SNodeReader Stream { get { return _Stream; } set { _Stream = value; } }
		private bool Valid { get { return _Valid; } set { _Valid = value; } }
		#endregion Properties

		#region Constructors
		public MarketParser(SNodeReader stream)
		{
			Stream = stream;
			Valid = false;
		}

		public MarketParser(string fileName)
		{
			try
			{ 
				InitWithFile(fileName); 
			}
			catch (ParseException)
			{ 
				return; 
			}
		}
		#endregion Constructors

		#region Methods
		public void Parse()
		{
			if (Stream == null)
				return;

			/* Todo: fixed offsets = bad :) */
			/* Step 1: Determine if this is a market order file */
			if (Stream[0].Members.Length < 1)
				throw new ParseException("Not a valid file");

			SNode baseNode = Stream[0].Members[0];

			if (baseNode.Members.Length < 1)
				throw new ParseException("Not a valid orders file");
			if (baseNode.Members[0].Members.Length < 2)
				throw new ParseException("Not a valid orders file");

			SIdent id = baseNode.Members[0].Members[1] as SIdent;
			if (id == null)
				throw new ParseException("Can't determine method name");
			if (id.Value != "GetOrders")
				throw new ParseException("Not a valid orders file");

			/* Retrieve the region and type */
			SInt region = baseNode.Members[0].Members[2] as SInt;
			SInt type = baseNode.Members[0].Members[3] as SInt;

			List.Region = region.Value;
			List.Type = type.Value;

			/* Try to extract the in-file timestamp */
			SDict dict = baseNode.Members[1] as SDict;
			if (dict == null)
				throw new ParseException("Can't read file timestamp");

			// Grab the version entry of the version tuple
			SLong time = dict.GetByName("version").Members[0] as SLong;
			if (time == null)
				throw new ParseException("Can't read file timestamp");

			Console.WriteLine("TS: " + time.Value);
			List.TimeStamp = new DateTime(time.Value);

			SNode obj = baseNode.Members[1].Members[0];
			if (obj == null)
				return;
			Parse(obj);
			Valid = true;
		}

		private void InitWithFile(string fileName)
		{
			CacheFile cf = new CacheFile(fileName);
			if (!cf.ReadFile())
				throw new ParseException("Can't open file " + fileName);

			CacheFileReader cfReader = cf.Reader;
			Parser parser = new Parser(cfReader);
			parser.Parse();
			SNode sNode = parser.Streams[0];
			Stream = sNode.Members;
			Parse();
			Stream = null;
			Valid = true;
		}

		private void Parse(SNode node)
		{
			if (node.Members.Length > 0)
			{
				SNodeReader members = node.Members;
				for (SNode n = members.Begin; n != members.End; n = members.Next)
				{
					SDBRow dbrow = n as SDBRow;
					if (dbrow != null)
						ParseDbRow(members.Next);
					else
						Parse(n);
				}
			}
		}

		private void ParseDbRow(SNode node)
		{
			MarketOrder order = new MarketOrder();

			SNodeReader members = node.Members;
			for (SNode i = members.Begin; i != members.End; i = members.Next)
			{
				SNode value = i;
				i = members.Next;
				SMarker key = i as SMarker;
				SIdent ident = i as SIdent;

				int typeKey = -1;

				if (key != null)
					typeKey = key.ID;
				else if (ident != null && ident.Value == "issueDate")
					typeKey = 131;
				else
				{
					Console.WriteLine("Can't parse - giving up");
					break;
				}

				SInt intV = value as SInt;
				SLong longV = value as SLong;
				SReal realV = value as SReal;

				int sintV = 0;
				long slongV = 0;
				double srealV = 0.0;

				if (intV != null)
					sintV = intV.Value;
				else if (longV != null)
					slongV = longV.Value;
				else if (realV != null)
					srealV = realV.Value;

				switch (typeKey)
				{
					case 139:
					    order.Price = slongV;
					    break;
					case 161:
					    order.VolRemaining = srealV;
					    break;
					case 131:
					    order.Issued = slongV;
					    break;
					case 138:
					    order.OrderID = slongV;
					    break;
					case 160:
					    order.VolEntered = sintV;
					    break;
					case 137:
					    order.MinVolume = sintV;
					    break;
					case 155:
					    order.StationID = sintV;
					    break;
					case 141:
					    order.RegionID = sintV;
					    List.Region = sintV;
					    break;
					case 150:
					    order.SolarSystemID = sintV;
					    break;
					case 41:
					    order.Jumps = sintV;
					    break;
					case 74:
					    order.Type = sintV;
					    List.Type = sintV;
					    break;
					case 140:
					    order.Range = sintV;
					    break;
					case 126:
					    order.Duration = sintV;
					    break;
					case 116:
					    if ( sintV != 0)
					        order.IsBid = true;
					    else
					        order.IsBid = false;
					    break;
					default:
						Console.WriteLine("Unknown key ID:" + key.ID +  " r: " + srealV + " l: " + slongV + " i: " + sintV);
					    break;
				}
			}

			List.AddOrder(order);
		}
		#endregion Methods
	}
}
