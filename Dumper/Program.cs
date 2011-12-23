namespace Dumper
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Text;
	using EveCache;

	static class Program
	{
		static string nspaces(int n)
		{
			n *= 2;
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < n; i++) 
				sb.Append(" "); // horrendously inefficient 4tw

			return sb.ToString();
		}

		static string dump(SNodeContainer stream, int level)
		{
			StringBuilder sb = new StringBuilder();
			foreach (SNode node in stream)
			{
				sb.Append(nspaces(level));
				sb.Append(" " + node.ToString() +  " \n");
				if (node.Members.Length > 0) 
				{ // generic catch all members with nested members
					SNodeContainer ste = node.Members;
					sb.Append(nspaces(level));
					sb.Append(" (\n");

					sb.Append(dump(ste, level + 1));

					sb.Append(nspaces(level));
					sb.Append(" )\n");
				}
			}
			return sb.ToString();
		}

		static string market(SNode start)
		{
			StringBuilder sb = new StringBuilder();
			MarketParser mp = new MarketParser(start);
			try
			{
				mp.Parse();
			} 
			catch (ParseException e)
			{
				sb.Append("Not a valid orders file due to " + e.Message);
				return null;
			}
			MarketList list = mp.List;

			DateTime t = list.TimeStamp;

			string timeString = t.ToString("yyyy-MM-dd HH:mm:ss");
			sb.Append("MarketList for region " + list.Region + " and type " + list.Type + 
								" at time " + timeString + "\n");

			sb.Append("price,volRemaining,typeID,range,orderID,volEntered,minVolume,bid,issued,duration,stationID,regionID,solarSystemID,jumps,\n");

			List<MarketOrder> buy = list.BuyOrders;
			List<MarketOrder> sell = list.SellOrders;
			foreach (MarketOrder o in sell)
			{
				sb.Append(o.ToCsv() + "\n");
			}
			foreach (MarketOrder o in buy)
			{
				sb.Append(o.ToCsv() + "\n");
			}
			return sb.ToString();
		}

		static int Main(string[] args)
		{
			if (args.Length < 1)
			{
				Console.WriteLine("Syntax: " + Environment.CommandLine + " [options] [filenames+]");
				Console.WriteLine("Options: --market       Digest a market order file, converting it to a .CSV file");
				Console.WriteLine("         --structure    Print an AST of the cache file");
				return -1;
			}
			bool dumpStructure = false;
			bool dumpMarket = false;
			int argsconsumed = 0;

			// Parse options in simple mode
			if (args.Length > 1)
			{
				for (int i = 0; i < args.Length; i++)
				{
					if (args[i] == "--market")
					{
						dumpMarket = true;
						argsconsumed++;
					}
					if (args[i] == "--structure")
					{
						dumpStructure = true;
						argsconsumed++;
					}
				}
			}
			else
				dumpStructure = true; // default


			for (int filelen = argsconsumed; filelen < args.Length; filelen++)
			{
				string fileName = args[filelen];
				CacheFileReader cfr;
				try { cfr = new CacheFileReader(fileName); }
				catch (System.IO.FileNotFoundException) { continue; }

				CacheFileParser parser = new CacheFileParser(cfr);

				try
				{
					parser.Parse();
				}
				catch (ParseException e)
				{
				    Console.WriteLine("Parse exception " + e.Message);		
				}

				if (dumpStructure)
				{
					// TODO: more than one stream
					for (int i = 0; i < parser.Streams.Count; i++)
					{
						SNodeContainer streams = parser.Streams[i].Members;
						File.WriteAllText(Path.ChangeExtension(fileName, ".structure"), dump(streams, 0));
					}
				}
				if (dumpMarket)
				{
					for (int i = 0; i < parser.Streams.Count; i++)
					{
						SNode snode = parser.Streams[i];
						File.WriteAllText(Path.ChangeExtension(fileName, ".market"),market(snode));
					}
				}
				Console.WriteLine();
			}
			return 1;
		}
	}
}
