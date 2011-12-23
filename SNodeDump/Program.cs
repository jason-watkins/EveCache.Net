namespace SNodeDump
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.IO;
	using EveCache;

	class Program
	{
		static void Main(string[] args)
		{
			foreach (string pathName in Directory.GetFiles(@"C:\Users\Jason\AppData\Local\CCP\EVE\d_eve_tranquility\cache\MachoNet\87.237.38.200\302\CachedMethodCalls"))
			{
				CacheFileReader cfr;
				cfr = new CacheFileReader(pathName);
				CacheFileParser parser = new CacheFileParser(cfr);

				try { parser.Parse(); }
				catch (ParseException e) { Console.WriteLine("Parse exception " + e.Message); }

				SNode.DumpNodes(Path.GetFileName(pathName));
			}
		}
	}
}
