using System;
using System.Collections.Generic;
using MonoTouch.UIKit;
namespace JaktLogg
{
	public class SectionMapping
	{
		public string Header = string.Empty;
		public string Footer = string.Empty;
		public List<RowItemMapping> Rows = new List<RowItemMapping>();
	
		public SectionMapping(string header, string footer){
			Header = header;
			Footer = footer;
		}
	}
	public class RowItemMapping
	{
		public string Label;
		public Func<string> GetValue;
		public Action RowSelected;
		public string Value;
		public string ImageFile;
		
	}
}

