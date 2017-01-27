using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;


namespace xml_splitter
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.ForegroundColor = ConsoleColor.Yellow;

			if (args.Length == 0)
			{
				Console.WriteLine("no files passed");
			}
			else
			{
				Console.WriteLine("what's the name of the element you want to split on?");
				string splitTag= Console.ReadLine();

				foreach (string filePath in args)
				{
					try
					{
						//extract tags
						XmlTagsList tagsList = new XmlTagsList(filePath);
						//
						new XmlSplitter(tagsList, splitTag).writeOutputFiles(filePath);
						//
						Console.WriteLine("\r\nfile: " + filePath);
						Console.WriteLine("completed successfully \r\n");
					}
					catch (Exception exp)
					{
						Console.WriteLine("\r\nERROR in file: " + filePath);
						Console.WriteLine(exp);
						Console.WriteLine(exp.Message);
					}
				}
			}

			//keep console open
			Console.WriteLine("Press any key to close...");
			Console.ReadKey();
		}
	}
}
