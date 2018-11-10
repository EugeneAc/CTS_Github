using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CTS_Analytics
{
	public class Tag
	{
		public int hItem { get; private set; }
		public string TagName { get; set; }
		public Tag(string tagname)
		{
			
			TagName = tagname;
		}
		public virtual string value
		{
			get; set;
		}
	}
	public class AnalogTag : Tag
	{
		public AnalogTag(string tagname) : base(tagname)
		{
		}

		public float value
		{
			get; set;
		}
	}

	public class DigitalTag : Tag
	{
		public DigitalTag(string tagname) : base(tagname)
		{
		}

		public bool value
		{
			get; set;
		}
	}
}