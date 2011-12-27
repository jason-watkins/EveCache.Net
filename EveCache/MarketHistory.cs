namespace EveCache
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	public class MarketHistory
	{
		#region Fields
		private int _Region;
		private int _Type;
		private DateTime _HistoryDate;
		private long _LowPrice;
		private long _HighPrice;
		private long _AveragePrice;
		private long _Volume;
		private int _Orders;
		#endregion Fields

		#region Properties
		public int Region { get { return _Region; } set { _Region = value; } }
		public int Type { get { return _Type; } set { _Type = value; } }
		public DateTime HistoryDate { get { return _HistoryDate; } set { _HistoryDate = value; } }
		public long HistoryTicks { set { _HistoryDate = new DateTime(value + 504911232000000000); } }
		public long LowPrice { get { return _LowPrice; } set { _LowPrice = value; } }
		public long HighPrice { get { return _HighPrice; } set { _HighPrice = value; } }
		public long AveragePrice { get { return _AveragePrice; } set { _AveragePrice = value; } }
		public long Volume { get { return _Volume; } set { _Volume = value; } }
		public int Orders { get { return _Orders; } set { _Orders = value; } }
		#endregion Properties

		#region Constructors
		public MarketHistory()
		{
			// Number of ticks to set date to Jan 1, 1601
			// Adding cached date to this date will give the correct DateTime
			HistoryDate = new DateTime(504911232000000000);
			LowPrice = 0;
			HighPrice = 0;
			AveragePrice = 0;
			Volume = 0;
			Orders = 0;
		}
		#endregion Constructors

		#region Methods
		public virtual string ToCsv()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append(HistoryDate.ToString("yyyy-MM-dd HH:mm:ss") + ".000");
			sb.Append("," + String.Format("{0:0.00}", LowPrice / 10000D));
			sb.Append("," + String.Format("{0:0.00}", HighPrice / 10000D));
			sb.Append("," + String.Format("{0:0.00}", AveragePrice / 10000D));
			sb.Append("," + Volume);
			sb.Append("," + Orders);
			return sb.ToString();
		}
		#endregion Methods
	}
}
