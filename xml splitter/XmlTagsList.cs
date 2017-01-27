using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace xml_splitter
{

	/// <summary>
	/// takes an xml file name and extracts all tags inside
	/// </summary>
	class XmlTagsList
	{
		private XDocument doc;

		public List<XmlTag> tagsList = new List<XmlTag>();

		public XmlTagsList(string file)
		{
			this.doc = GetXDocument(file);
			
			//fill the tags list
			addTags(doc.Root);
		}

		

		void addTags(XElement node)
		{
			if (!node.HasElements)
			{
				//the element has no child nodes, so the element is represented by one tag
				tagsList.Add(new XmlTag(XmlTagType.emptyElement, node));
			}
			else
			{
				//add the starting tag, then recurse on the children, 
				//and add the closing tag
				tagsList.Add(new XmlTag(XmlTagType.openingTag, node));
				//
				foreach (XElement item in node.Elements())
				{
					addTags(item);
				}
				//
				tagsList.Add(new XmlTag(XmlTagType.closingTag, node));
			}
		}

		private static XDocument GetXDocument(string filename)
		{
			var xml = GetFileContents(filename);
			return XDocument.Parse(xml);
		}

		private static string GetFileContents(string filename)
		{
			using (var fileStream = new FileStream(filename, FileMode.Open))
			using (var streamReader = new StreamReader(fileStream))
			{
				return streamReader.ReadToEnd();
			}
		}

	}
}
