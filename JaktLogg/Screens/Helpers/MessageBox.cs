using System;
using MonoTouch.UIKit;

namespace JaktLogg
{
	public class MessageBox
	{
		private const string Button_Text = "Ok";
		
		public MessageBox ()
		{
		}
		
		public static void Show(string title, string text)
		{
			Show(title, text, Button_Text);
		}
		
		public static void Show(string title, string text, string buttonText)
		{
			using (var alert = new UIAlertView(title, text, null, buttonText, null))
			{
				alert.Show();
			}
		}
	}
}

