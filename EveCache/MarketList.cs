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
	using System.Collections.Generic;

	public class MarketList
	{
		#region Fields
		private List<MarketOrder> _BuyOrders;
        private int _Region;
		private List<MarketOrder> _SellOrders;
		private int _Type;
        private DateTime _TimeStamp;
		#endregion Fields

		#region Properties
		public virtual List<MarketOrder> BuyOrders { get { return _BuyOrders; } private set { _BuyOrders = value; } }
		public int Region { get { return _Region; } set { _Region = value; } }
		public virtual List<MarketOrder> SellOrders { get { return _SellOrders; } private set { _SellOrders = value; } }
		public int Type { get { return _Type; } set { _Type = value; } }
		public DateTime TimeStamp { get { return _TimeStamp; } set { _TimeStamp = value; } }
		#endregion Properties

		#region Constructors
		public MarketList(int type, int region) : this()
		{
			Region = region;
			Type = type;
		}

		public MarketList()
		{
			BuyOrders = new List<MarketOrder>();
			SellOrders = new List<MarketOrder>();

			Region = 0;
			Type = 0;
			TimeStamp = new DateTime(0);
		}

		public MarketList(MarketList source)
		{
			BuyOrders = source.BuyOrders;
			Region = source.Region;
			SellOrders = source.SellOrders;
			Type = source.Type;
			TimeStamp = source.TimeStamp;
		}
		#endregion Constructors

		#region Methods
		public void AddOrder(MarketOrder order)
		{
			if (order.IsBid)
				BuyOrders.Add(order);
			else
				SellOrders.Add(order);
		}
		#endregion Methods
	}
}
