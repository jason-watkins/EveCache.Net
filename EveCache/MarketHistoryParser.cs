namespace EveCache
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	public class MarketHistoryParser
	{
		#region Fields
		private List<MarketHistory> _List;
		private SNode _Stream;
		private bool _Valid;
		private int _Region;
		private int _Type;
		#endregion Fields

		#region Properties
		public virtual List<MarketHistory> List { get { return _List; } private set { _List = value; } }
		protected virtual SNode Stream { get { return _Stream; } set { _Stream = value; } }
		public bool Valid { get { return _Valid; } set { _Valid = value; } }
		public int Region { get { return _Region; } private set { _Region = value; } }
		public int Type { get { return _Type; } private set { _Type = value; } }
		#endregion Properties

		#region Constructors
		public MarketHistoryParser(SNode stream)
		{
			List = new List<MarketHistory>();
			Stream = stream;
			Valid = false;
		}

		public MarketHistoryParser(string fileName)
		{
			List = new List<MarketHistory>();
			try { InitWithFile(fileName); }
			catch (ParseException) { return; }
		}
		#endregion Constructors

		#region Methods
		public void Parse()
		{
			if (Stream == null)
				return;

			/* Todo: fixed offsets = bad :) */
			/* Step 1: Determine if this is a market order file */
			if (Stream.Members.Length < 1)
				throw new ParseException("Not a valid file");

			SNode baseNode = Stream.Members[0];

			if (baseNode.Members.Length < 1)
				throw new ParseException("Not a valid history file");
			if (baseNode.Members[0].Members.Length < 2)
				throw new ParseException("Not a valid history file");

			SIdent id = baseNode.Members[0].Members[1] as SIdent;
			if (id == null)
				throw new ParseException("Can't determine method name");
			if (id.Value != "GetNewPriceHistory" && id.Value != "GetOldPriceHistory")
				throw new ParseException("Not a valid history file");

			/* Retrieve the region and type */
			SInt region = baseNode.Members[0].Members[2] as SInt;
			if (region != null)
				Region = region.Value;
			SInt type = baseNode.Members[0].Members[3] as SInt;
			if (type != null)
				Type = type.Value;

			// Don't need file timestamp. Market histories already have dates
			/* Try to extract the in-file timestamp */
			//SDict dict = baseNode.Members[1] as SDict;
			//if (dict == null)
			//    throw new ParseException("Can't read file timestamp");

			//// Grab the version entry of the version tuple
			//SLong time = dict.GetByName("version").Members[0] as SLong;
			//if (time == null)
			//    throw new ParseException("Can't read file timestamp");

			//Console.WriteLine("TS: " + time.Value);
			//List.TimeStamp = new DateTime(time.Value + 504911232000000000);

			if (id.Value == "GetNewPriceHistory")
				ParseNewHistory(baseNode);
		}

		private void ParseNewHistory(SNode baseNode)
		{
			SNode obj = baseNode.Members[1].Members[6];
			if (obj == null)
				return;
			Parse(obj);
			Valid = true;
		}

		private void InitWithFile(string fileName)
		{
			CacheFileReader cfReader = new CacheFileReader(fileName);

			CacheFileParser parser = new CacheFileParser(cfReader);
			parser.Parse();
			SNode sNode = parser.Streams[0];
			Stream = sNode.Members[0];
			Parse();
			Stream = null;
			Valid = true;
		}

		private void Parse(SNode node)
		{
			if (node.Members.Length > 0)
			{
				SNodeContainer members = node.Members;
				for (int i = 0; i < members.Length; i++)
				{
					SDBRow dbrow = members[i] as SDBRow;
					if (dbrow != null)
						ParseDbRow(members[++i]);
					else
						Parse(members[i]);
				}
			}
		}

		private void ParseDbRow(SNode node)
		{
			MarketHistory history = new MarketHistory();
			history.Region = Region;
			history.Type = Type;

			SNodeContainer members = node.Members;
			for ( int i = 0; i < members.Length; i++)
			{
				SNode value = members[i++];
				SIdent ident = members[i] as SIdent;

				SInt intV = value as SInt;
				SLong longV = value as SLong;

				int sintV = 0;
				long slongV = 0;

				if (intV != null)
					sintV = intV.Value;
				else if (longV != null)
					slongV = longV.Value;

				switch (ident.Value)
				{
					case "historyDate":
						history.HistoryTicks = slongV;
						break;
					case "lowPrice":
						history.LowPrice = slongV;
						break;
					case "highPrice":
						history.HighPrice = slongV;
						break;
					case "avgPrice":
						history.AveragePrice = slongV;
						break;
					case "volume":
						history.Volume = slongV;
						break;
					case "orders":
						history.Orders = sintV;
						break;
					default:
						throw new ParseException("Can't parse " + ident);
				}
			}
			List.Add(history);
		}
		#endregion Methods
	}
}
