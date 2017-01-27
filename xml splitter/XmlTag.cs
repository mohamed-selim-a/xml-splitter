using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace xml_splitter
{
	enum XmlTagType {openingTag, closingTag, emptyElement }

	/// <summary>
	/// represents an xml tag of an xml element
	/// </summary>
	class XmlTag
	{
		
		public XmlTagType type;
		public XElement element; //only to hold name and attributes

		
		public XmlTag(XmlTagType type, XElement element)
		{
			this.type = type;
			this.element = element;
		}
		
		public string render()
		{
			if (type == XmlTagType.openingTag)
			{
				return renderOpeningTag();
			}
			else if (type == XmlTagType.closingTag)
			{
				return renderClosingTag();
			}
			else //empty element
			{
				return renderEmptyElement();
			}
		}

		private string renderEmptyElement()
		{
			StringBuilder st = new StringBuilder();
			st.AppendFormat("<{0} ", element.Name);
			foreach (XAttribute attr in element.Attributes())
			{
				st.AppendFormat("{0}=\"{1}\" ", attr.Name, attr.Value);
			}

			st.Append("/>");
			//
			return st.ToString();
		}

		public string renderClosingTag()
		{
			return string.Format("</{0}>", element.Name);
		}

		public string renderOpeningTag()
		{
			StringBuilder st = new StringBuilder();
			st.AppendFormat("<{0} ", element.Name);
			foreach (XAttribute attr in element.Attributes())
			{
				st.AppendFormat("{0}=\"{1}\" ", attr.Name, attr.Value);
			}

			st.Append(">");
			//
			return st.ToString();
		}
	}
}
