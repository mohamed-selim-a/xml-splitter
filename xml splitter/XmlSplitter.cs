using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace xml_splitter
{

	/// <summary>
	/// takes tags list and split it into parts depending on penalty tag, 
	/// takes every part and form a text array to dump to file
	/// </summary>
	class XmlSplitter
	{
		List<List<XmlTag>> parts = new List<List<XmlTag>>();

		List<XmlTag> currentPart= new List<XmlTag>();

		List<string[]> outFiles = new List<string[]>();

		public XmlSplitter(XmlTagsList tags, string splitTag)
		{
			//split tags
			foreach (XmlTag tag in tags.tagsList)
			{
				if (tag.element.Name.ToString() == splitTag)
				{
					parts.Add(currentPart);
					currentPart = new List<XmlTag>();
				}
				else
				{
					currentPart.Add(tag);
				}
			}
			//end
			parts.Add(currentPart);

			//reform a doc from every part
			foreach (List<XmlTag> part in parts)
			{
				if (part.Count == 0) continue;
				//
				outFiles.Add(formDocument(part));
			}
		}

		private string[] formDocument(List<XmlTag> part)
		{
			bool oneRoot = true; //a flag indicating the doc has one root
			int index = 0; //the iteration index

			Stack<XmlTag> tagsStack = new Stack<XmlTag>();
			LinkedList<string> lines = new LinkedList<string>();

			foreach (XmlTag tg in part)
			{
				if (tg.type == XmlTagType.openingTag)
				{
					if (index != 0 && tagsStack.Count == 0) //not first tag and stack is empty
					{
						oneRoot= false;
					}
					tagsStack.Push(tg);
					lines.AddLast(tg.render());
					
				}
				else if (tg.type == XmlTagType.closingTag)
				{
					if (tagsStack.Count != 0)
					{
						tagsStack.Pop();
						lines.AddLast(tg.render());
					}
					else //stack is empty
					{
						lines.AddLast(tg.render());
						lines.AddFirst(tg.renderOpeningTag());
						//if (index == part.Count - 1) //the last tag
						//{
							oneRoot = true;
						//}
					}
				}
				else //empty element
				{
					lines.AddLast(tg.render());
					if (index != 0 && tagsStack.Count == 0) //not first tag and stack is empty
					{
						oneRoot = false;
					}
				}

				//increment index
				index ++;

			}//end looping on tags

			//check if the stack isn't empty
			while (tagsStack.Count != 0)
			{
				//still openning tags exist with no matching closing tags
				lines.AddLast(
					tagsStack.Pop().renderClosingTag()
					);
			}

			//check if we need a dummy root
			if (!oneRoot)
			{
				lines.AddFirst("<noroot>");
				lines.AddLast("</noroot>");
			}

			//the declaration line
			lines.AddFirst("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
			
			//
			return lines.ToArray();
		}

		public void writeOutputFiles(string originalFile)
		{
			string folder = originalFile.Substring(0, originalFile.LastIndexOf('.'));
			string ext = Path.GetExtension(originalFile);
			Directory.CreateDirectory(folder);

			for (int i = 0; i < outFiles.Count; i++)
			{
				File.WriteAllLines(Path.Combine(folder, (i+1)+ext), outFiles[i]);
			}
		}

	}
}
