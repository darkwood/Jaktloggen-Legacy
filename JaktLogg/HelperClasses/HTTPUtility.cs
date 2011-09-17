using System;
namespace JaktLogg
{
	public static class HTTPUtility
	{
		
		private static string[,] _chars = new string[,]
		{
		/*{ "%", "%25" },     // this is the first one
		{ "$" , "%24" },
		{ "&", "%26" },
		{ "+", "%2B" },
		{ ",", "%2C" },
		{ "/", "%2F" },
		{ ":", "%3A" },
		{ ";", "%3B" },
		{ "=", "%3D" },
		{ "?", "%3F" },
		{ "@", "%40" },
		{ " ", "%20" },
		{ "\"" , "%22" },
		{ "<", "%3C" },
		{ ">", "%3E" },
		{ "#", "%23" },
		{ "{", "%7B" },
		{ "}", "%7D" },
		{ "|", "%7C" },
		{ "\\", "%5C" },
		{ "^", "%5E" },
		{ "~", "%7E" },
		{ "[", "%5B" },
		{ "]", "%5D" },
		{ "`", "%60" },*/
		{ "æ", "%E6" },
		{ "ø", "%F8" },
		{ "å", "%E5" },
		{ "Æ", "%C6" },
		{ "Ø", "%D8" },
		{ "Å", "%C5" }};
		public static string URLEncode(string url)
		{
		    for (int i = 0; i < _chars.GetUpperBound(0); i++)
		        url = url.Replace(_chars[i, 0], _chars[i, 1]);
		    return url;
		}
		public static string URLDecode(string url)
		{
		    for (int i = 0; i < _chars.GetUpperBound(0); i++)
		        url = url.Replace(_chars[i, 1], _chars[i, 0]);
		    return url;
		}
	}
}

