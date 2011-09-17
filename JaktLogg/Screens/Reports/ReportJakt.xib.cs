
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Text;
using System.IO;
using MonoTouch.MessageUI;

namespace JaktLogg
{
	public partial class ReportJakt: UIJaktViewController
	{
		public Jakt jakt;
		MFMailComposeViewController _mail;
		public ReportJakt (Jakt j) : base("ReportJakt", null)
		{
			jakt = j;
		}
		
		public override void ViewDidLoad ()
		{
			var doneButton = new UIBarButtonItem("Lagre", UIBarButtonItemStyle.Done, DoneBarButtonClicked);
			navigationBar.LeftBarButtonItem = doneButton;
			
			var rightbtn = new UIBarButtonItem(UIBarButtonSystemItem.Compose, RightBarButtonClicked);
			NavigationItem.RightBarButtonItem = rightbtn;
			
			var html = GenerateHtml();
			
			webView.LoadHtmlString(html, new NSUrl());
			base.ViewDidLoad ();
		}
		
		private void RightBarButtonClicked(object sender, EventArgs args)
		{
			if (MFMailComposeViewController.CanSendMail) {
	            _mail = new MFMailComposeViewController ();
				_mail.Title = "Send e-post";
				_mail.SetSubject("Jaktrapport for " + jakt.Sted + " " + jakt.DatoFra.ToNorwegianDateAndYearString());
	            _mail.SetMessageBody (GenerateHtml(), true);
	            _mail.Finished += HandleMailFinished;
	            this.PresentModalViewController (_mail, true);
	        } 
			else {
	        	MessageBox.Show("Beklager", "Denne enheten kan desverre ikke sende e-post");
	        }

		}
		
		void HandleMailFinished (object sender, MFComposeResultEventArgs e)
		{
		    if (e.Result == MFMailComposeResult.Sent) {
				MessageBox.Show("E-post sendt", "");
		    }
			else if(e.Result == MFMailComposeResult.Failed){
				MessageBox.Show("Å nei!", "E-post feilet. Prøv igjen senere, eller meld inn feil.");
			}
			
		    e.Controller.DismissModalViewControllerAnimated (true);
		}
		
		private string GenerateHtml()
		{
			var logger = JaktLoggApp.instance.LoggList.Where(l => l.JaktId == jakt.ID).ToList<Logg>();
			var jegere = JaktLoggApp.instance.JegerList.Where(j => jakt.JegerIds.Contains(j.ID)).ToList<Jeger>();
			
			StringBuilder s = new StringBuilder();
			
			//JAKT
			s.Append("<html><head><title>Jaktrapport - "+jakt.Sted+"</title></head><body>");
			s.Append("<h1>"+jakt.Sted+"</h1>");
			s.Append("<span class='dato'>" + jakt.DatoFra.ToNorwegianDateString() == jakt.DatoTil.ToNorwegianDateString() ? jakt.DatoFra.ToNorwegianDateString() : jakt.DatoFra.ToNorwegianDateString() + " til " + jakt.DatoTil.ToNorwegianDateAndYearString()+"</span>");
			if(jakt.Notes.Length > 0)
				s.Append("<p>" + jakt.Notes + "</p>");
			
			//JEGERE
			if(jegere.Count() > 0){
				s.Append("<h2>Jegere</h2>");
				s.Append("<ul id='Jegere'>");
				foreach(var jeger in jegere)
				{
					s.Append("<li>" + jeger.Navn + "</li>");
				}
				s.Append("</ul>");
			}
			s.Append("<hr/>");
			
			//RESULTAT
			s.Append("<h2>Jaktresultat</h2>");
			s.Append("<table><tr><th>Art</th><th>Sett</th><th>Skudd</th><th>Treff</th></tr>");
			foreach(int artid in logger.Select(a => a.ArtId).Distinct())
			{
				var l = logger.Where(a => a.ArtId == artid);
				var hits = l.Sum(x => x.Treff);
				var shots = l.Sum(x => x.Skudd);
				var seen = l.Sum(x => x.Sett);
				s.Append("<tr>");
				
				s.Append("<td>"+ (artid == 0 ? "" : JaktLoggApp.instance.ArtList.SingleOrDefault(a => a.ID == artid).Navn) + "</td>");
				s.Append("<td>" + seen +  "</td>");
				s.Append("<td>"+ shots +  "</td>");
				s.Append("<td>" + hits +  "</td>");
				
				s.Append("</tr>");
			}
			s.Append("</table>");
			s.Append("<hr/>");
			
			//RESULTAT PR JEGER
			if(jegere.Count() > 1)
			{
				s.Append("<h2>Resultat pr. jeger</h2>");
				foreach(var jeger in jegere)
				{
					s.Append("<h3>-"+jeger.Navn + "-</h3>");
					s.Append("<table><tr><th>Art</th><th>Sett</th><th>Skudd</th><th>Treff</th></tr>");
					foreach(var artid in logger.Where(l => l.JegerId == jeger.ID).Select(a => a.ArtId).Distinct())
					{
						var l = logger.Where(a => a.ArtId == artid);
						var hits = l.Sum(x => x.Treff);
						var shots = l.Sum(x => x.Skudd);
						var seen = l.Sum(x => x.Sett);
						s.Append("<tr>");
						
						s.Append("<td>"+ (artid == 0 ? "" : JaktLoggApp.instance.ArtList.SingleOrDefault(b => b.ID == artid).Navn) + "</td>");
						s.Append("<td>" + seen +  "</td>");
						s.Append("<td>"+ shots +  "</td>");
						s.Append("<td>" + hits +  "</td>");
						
						s.Append("</tr>");
					}
					s.Append("</table>");
				}
				
				s.Append("<hr/>");
			}
			
			//LOGGFØRINGER
			s.Append("<h2>Loggføringer</h2>");
			
			
			s.Append("<table>");
			
			s.Append("<tr>");
			
			s.Append("<th>Dato/Tid</th>");
			s.Append("<th>Jeger</th>");
			s.Append("<th>Art</th>");
			s.Append("<th>Skudd</th>");
			s.Append("<th>Treff</th>");
			s.Append("<th>Sett</th>");
			s.Append("<th>Latitude</th>");
			s.Append("<th>Longitude</th>");
			s.Append("<th>Notater</th>");
			
			foreach(var loggTypeId in JaktLoggApp.instance.SelectedLoggTypeIds)
			{
				var lt = JaktLoggApp.instance.LoggTypeList.Where(x => x.ID == loggTypeId).FirstOrDefault();
				if(lt != null){
					s.Append("<th>" + lt.Navn + "</th>");
				}
			}
			
			s.Append("</tr>");
			
			foreach(var logg in logger)
			{
				s.Append(GenerateLoggItemHtml(logg));
			}
			s.Append("</table>");
			
         	s.Append(GetStyles());
			
			s.Append("</body></html>");
			return s.ToString();
		}
		
		private string GenerateLoggItemHtml(Logg logg)
		{
			StringBuilder s = new StringBuilder();
			s.Append("<tr>");
			
			s.Append("<td>" + logg.Dato.ToNorwegianDateString()+" kl."+logg.Dato.ToNorwegianTimeString() + "</td>");
			s.Append("<td>" + (logg.JegerId > 0 ? JaktLoggApp.instance.GetJeger(logg.JegerId).Fornavn : "") + "</td>");
			s.Append("<td>" + (logg.ArtId == 0 ? "" : JaktLoggApp.instance.GetArt(logg.ArtId).Navn) + "</td>");
			s.Append("<td>" + logg.Skudd + "</td>");
			s.Append("<td>" + logg.Treff + "</td>");
			s.Append("<td>" + logg.Sett + "</td>");
			s.Append("<td>" + logg.Latitude + "</td>");
			s.Append("<td>" + logg.Longitude + "</td>");
			s.Append("<td>" + logg.Notes + "</td>");
			
			foreach(var loggTypeId in JaktLoggApp.instance.SelectedLoggTypeIds)
			{
				var lt = JaktLoggApp.instance.LoggTypeList.Where(x => x.ID == loggTypeId).FirstOrDefault();
				if(lt != null){
					s.Append("<th>" +  + "</th>");
				}
			}
			
			s.Append("</tr>");
			
			
			//if(logg.ImagePath.Length > 0)
				//s.Append("<img style='padding:20px; border:ridge 10px #5272a7; -webkit-transform:rotate(7deg);width:200px' src='file://"+logg.ImagePath+"' />");

				/*var mapstr = string.Format("http://maps.google.com/maps/api/staticmap?center={0},{1}&zoom=12&size=200x200&sensor=true&&markers=color:green%7Clabel:PONG%7C{0},{1}", 
				                           logg.Latitude, logg.Longitude);
				s.Append("<img style='padding:10px;' src='"+mapstr+"' />");*/
			
			return s.ToString();
		}
		
		private string GetStyles()
		{
			StringBuilder s = new StringBuilder();
			s.Append("<style type='text/css'>");
			s.Append(@" body { font-size:12px; font-family: Arial, Helvetica, Verdana; background-color:#EFEFEF; color:#353535 }
						h1,h2,h3,p,hr,ul,li{ margin:0, padding:0; }
						h1,h2,h3{ line-height:1.3em;}
						hr { border:slid 1px #666; }
						li {list-style-type:none;padding-left:0px;margin-left:0px;}
						td,th {font-size:11px; border-bottom:solid 1px #989898; }
						");
			s.Append("</style>");
			return s.ToString();
		}
		
		private void DoneBarButtonClicked(object sender, EventArgs args)
		{
			this.DismissModalViewControllerAnimated(true);
		}
		
		private int rnd(int min, int max)
		{
			var i = new Random().Next(min, max);
			return i;
		}
		
	}
}

