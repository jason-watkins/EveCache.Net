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
			string fileName = "4b51.cache";
			string pathName = "../../../TestFiles/" + fileName;
			CacheFileReader cfr;
			cfr = new CacheFileReader(pathName);
			Parser parser = new Parser(cfr);
			//File.WriteAllText("dump.txt", cfr.DumpBuffer());

			try
			{
				parser.Parse();
			}
			catch (ParseException e)
			{
				Console.WriteLine("Parse exception " + e.Message);
			}

			SNode.DumpNodes(fileName);
		}
	}
}
