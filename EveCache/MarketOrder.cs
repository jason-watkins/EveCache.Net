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
	using System.Text;

	public class MarketOrder
	{
		#region Fields
		private long _Price;
        private double _VolRemaining;
        private int _Range;
        private long _OrderID;
        private int _VolEntered;
        private int _MinVolume;
        private bool _IsBid;
        private long _Issued;
        private int _Duration;
        private int _StationID;
        private int _SolarSystemID;
        private int _RegionID;
        private int _Jumps;
        private int _Type;
		#endregion Fields

		#region Properties
		public virtual long Price { get { return _Price; } set { _Price = value; } }
		public virtual double VolRemaining { get { return _VolRemaining; } set { _VolRemaining = value; } }
		public virtual int Range { get { return _Range; } set { _Range = value; } }
		public virtual long OrderID { get { return _OrderID; } set { _OrderID = value; } }
		public virtual int VolEntered { get { return _VolEntered; } set { _VolEntered = value; } }
		public virtual int MinVolume { get { return _MinVolume; } set { _MinVolume = value; } }
		public virtual bool IsBid { get { return _IsBid; } set { _IsBid = value; } }
		public virtual long Issued { get { return _Issued; } set { _Issued = value; } }
		public virtual int Duration { get { return _Duration; } set { _Duration = value; } }
		public virtual int StationID { get { return _StationID; } set { _StationID = value; } }
		public virtual int SolarSystemID { get { return _SolarSystemID; } set { _SolarSystemID = value; } }
		public virtual int RegionID { get { return _RegionID; } set { _RegionID = value; } }
		public virtual int Jumps { get { return _Jumps; } set { _Jumps = value; } }
		public virtual int Type { get { return _Type; } set { _Type = value; } }
		#endregion Properties

		#region Constructors
		public MarketOrder()
		{
			Price = 0;
			VolRemaining = 0;
			Range = 0;
			OrderID = 0;
			VolEntered = 0;
			MinVolume = 0;
			IsBid = false;
			Issued = 0;
			Duration = 0;
			StationID = 0;
			SolarSystemID = 0;
			RegionID = 0;
			Jumps = 0;
			Type = 0;
		}
		#endregion Constructors

		#region Methods
		public virtual string ToCsv()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append(String.Format("{0:0.00}", Price / 10000D));

			sb.Append("," + String.Format("{0:0.0}", VolRemaining));

			sb.Append("," + Type);
			sb.Append("," + Range);
			sb.Append("," + OrderID);
			sb.Append("," + VolEntered);
			sb.Append("," + MinVolume);

			if (IsBid)
				sb.Append("True");
			else
				sb.Append("False");

			DateTime dt = new DateTime((long)Issued);
			string dtString = String.Format("0:YYYY-MM-dd HH:mm:ss", dt);

			sb.Append("," + dt + ".000");
			sb.Append("," + Duration);
			sb.Append("," + StationID);
			sb.Append("," + RegionID);
			sb.Append("," + SolarSystemID);
			sb.Append("," + Jumps);
			return sb.ToString();
		}
		#endregion Methods
	}
}
