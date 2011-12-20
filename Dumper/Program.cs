namespace Dumper
{
	using System;
	using System.Collections.Generic;
	using EveCache;

	static class Program
	{
		static void nspaces(int n)
		{
			n *= 2;
			for (int i = 0; i < n; i++) {
				Console.Write(" "); // horrendously inefficient 4tw
			}
		}

		static void dump(SNodeReader stream, int level)
		{
			for (SNode vi = stream.Begin; vi != stream.End; vi = stream.Next)
			{
				nspaces(level);
				Console.WriteLine("" + vi.Repl() +  " ");
				if (vi.Members.Length > 0) 
				{ // generic catch all members with nested members
					SNode sn = vi;
					SNodeReader ste = sn.Members;
					nspaces(level);
					Console.WriteLine(" (");

					dump(ste, level + 1);

					nspaces(level);
					Console.WriteLine(" )");
				}
			}

		}

		static void market(SNodeReader node)
		{
			MarketParser mp = new MarketParser(node);
			try
			{
				mp.Parse();
			} 
			catch (ParseException e)
			{
				Console.WriteLine("Not a valid orders file due to " + e.Message);
				return;
			}
			MarketList list = mp.List;

			DateTime t = list.TimeStamp;

			string timeString = t.ToString("{0:yyyy-mm-dd HH:mm:ss}");
			Console.WriteLine("MarketList for region " + list.Region + " and type " + list.Type + 
								" at time " + timeString + " " + list.TimeStamp.Ticks);

			Console.WriteLine("price,volRemaining,typeID,range,orderID,volEntered,minVolume,bid,issued,duration,stationID,regionID,solarSystemID,jumps,");

			List<MarketOrder> buy = list.BuyOrders;
			List<MarketOrder> sell = list.SellOrders;
			foreach (MarketOrder o in sell)
			{
				Console.WriteLine(o.ToCsv());
			}
			foreach (MarketOrder o in buy)
			{
				Console.WriteLine(o.ToCsv());
			}
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
				CacheFile cF = new CacheFile(fileName);
				if (!cF.ReadFile())
				{
					// Not a valid file, skip
					Console.WriteLine("Can't open " + fileName);
					continue;
				}

				CacheFileReader cfr = cF.Reader;
				Parser parser = new Parser(cfr);

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
						SNodeReader streams = parser.Streams[i].Members;
						dump(streams, 0);
					}
				}
				if (dumpMarket)
				{
					for (int i = 0; i < parser.Streams.Count; i++)
					{
						SNode snode = parser.Streams[i];
						market(snode);
					}
				}
				Console.WriteLine();
			}
		}
	}
}
