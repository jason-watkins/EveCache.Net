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

	public class MarketOrder
	{
		#region Fields
		private ulong _Price;
        private double _VolRemaining;
        private uint _Range;
        private ulong _OrderID;
        private uint _VolEntered;
        private uint _MinVolume;
        private bool _Bid;
        private ulong _Issued;
        private uint _Duration;
        private uint _StationID;
        private uint _SolarSystemID;
        private uint _RegionID;
        private uint _Jumps;
        private uint _Type;
		#endregion Fields

		#region Properties
		public ulong Price { get { return _Price; } set { _Price = value; } }
		public double VolRemaining { get { return _VolRemaining; } set { _VolRemaining = value; } }
		public uint Range { get { return _Range; } set { _Range = value; } }
		
		//void setOrderID(unsigned long long v) { _orderID = v; }
		//void setVolEntered(unsigned int v) { _volEntered = v; }
		//void setMinVolume(unsigned int v) { _minVolume = v; }
		//void setBid(bool b) { _bid = b; }
		//void setIssued(unsigned long long v) { _issued = v; }
		//void setDuration(unsigned int v) { _duration = v; }
		//void setStationID(unsigned int v) { _stationID = v; }
		//void setRegionID(unsigned int v) { _regionID = v; }
		//void setSolarSystemID(unsigned int v) { _solarSystemID = v; }
		//void setJumps(unsigned int v) { _jumps = v; }
		//void setType(unsigned int v) { _type = v; }
	
		//unsigned long long orderID() const { return _orderID; }
		//unsigned int volEntered() const { return _volEntered; }
		//unsigned int minVolume() const { return _minVolume; }
		//bool isBid() const  { return _bid; }
		//unsigned long long issued() const  { return _issued; }
		//unsigned int duration() const  { return _duration; }
		//unsigned int stationID() const  { return _stationID; }
		//unsigned int regionID() const { return _regionID; }
		//unsigned int solarSystemID() const  { return _solarSystemID; }
		//unsigned int jumps() const  { return _jumps; }
		//unsigned int type() const { return _type; }
		#endregion Properties

		#region Constructors
		public MarketOrder()
		{

		}
		#endregion Constructors

		#region Methods

		#endregion Methods
			//    MarketOrder() : _price(0), _volRemaining(0), _range(0), _orderID(0), _volEntered(0), _minVolume(0), _bid(0), _issued(0), 
			//_duration(0), _stationID(0), _regionID(0), _solarSystemID(0), _jumps(0), _type(0) {}

        /**
         * Utility
         */

        /**
         * Return this item as a standard CSV file export line
         */
        //std::string toCsv() const;
	}
}
