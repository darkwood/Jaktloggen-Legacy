
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
		public string html = "";
		MFMailComposeViewController _mail;
		public ReportJakt (Jakt j) : base("ReportJakt", null)
		{
			jakt = j;
			
		}
		
		public ReportJakt (string _html) : base("ReportJakt", null)
		{
			html = _html;
		}
		
		public override void ViewDidLoad ()
		{
			Title = Utils.Translate("preview");
			var doneButton = new UIBarButtonItem(Utils.Translate("done"), UIBarButtonItemStyle.Done, DoneBarButtonClicked);
			navigationBar.LeftBarButtonItem = doneButton;
			
			var rightbtn = new UIBarButtonItem(Utils.Translate("send"), UIBarButtonItemStyle.Done, RightBarButtonClicked);
			NavigationItem.RightBarButtonItem = rightbtn;
			
			if(jakt != null)
				html = GenerateHtml();
			
			webView.LoadHtmlString(html, new NSUrl(""));
			base.ViewDidLoad ();
		}
		
		private void RightBarButtonClicked(object sender, EventArgs args){
			CreateMail(html);
		}
		
		private void CreateMail(string html)
		{
			if (MFMailComposeViewController.CanSendMail) {
	            _mail = new MFMailComposeViewController ();
				_mail.NavigationBar.BarStyle = UIBarStyle.Black;
				_mail.Title = Utils.Translate("sendmail");
				if(jakt != null)
					_mail.SetSubject(string.Format(Utils.Translate("mailsubject"), jakt.Sted, jakt.DatoFra.ToLocalDateAndYearString()));
	            else 
					_mail.SetSubject(Utils.Translate("mailsubject_generic"));
				
				_mail.SetMessageBody (html, true);
	            _mail.Finished += HandleMailFinished;
				
				
				//Get e-mails:
				var selectedJegerIds = new List<int>();
				var jegerIds = JaktLoggApp.instance.JegerList.Select(j => j.ID).ToList<int>();
				if(jakt != null)
					jegerIds = jakt.JegerIds;
				
				var jegerScreen = new JegerPickerScreen(jegerIds, screen => {
					selectedJegerIds = screen.jegerIds;
					//get email pr. jegerid
					if(selectedJegerIds.Count > 0){
						List<string> emails = new List<string>();
						foreach(var jegerId in selectedJegerIds){
							var jeger = JaktLoggApp.instance.JegerList.Where(j => j.ID == jegerId).FirstOrDefault();
							if(jeger.Email != "")
								emails.Add(jeger.Email);
							else
								MessageBox.Show(string.Format(Utils.Translate("report.mailmissing"), jeger.Fornavn), "");
						}
						if(emails.Count > 0)
							_mail.SetToRecipients(emails.ToArray());
						
					}
					this.PresentModalViewController (_mail, true);
				});
				jegerScreen.Title = Utils.Translate("report.jegereheader");
				jegerScreen.Footer = Utils.Translate("report.jegerefooter");
				jegerScreen.ModalTransitionStyle = UIModalTransitionStyle.CoverVertical;
				
				this.NavigationController.PushViewController(jegerScreen, true);
	            
	        } 
			else {
	        	MessageBox.Show(Utils.Translate("sorry"), Utils.Translate("error_mailnotsupported"));
	        }

		}
		
		void HandleMailFinished (object sender, MFComposeResultEventArgs e)
		{
		    if (e.Result == MFMailComposeResult.Sent) {
				MessageBox.Show(Utils.Translate("emailsent"), "");
		    }
			else if(e.Result == MFMailComposeResult.Failed){
				MessageBox.Show(Utils.Translate("error"), Utils.Translate("error_mailfail"));
			}
			
		    e.Controller.DismissModalViewControllerAnimated (true);
		}
		
		private string GenerateHtml()
		{
			var logger = JaktLoggApp.instance.LoggList.Where(l => l.JaktId == jakt.ID).ToList<Logg>();
			var jegere = JaktLoggApp.instance.JegerList.Where(j => jakt.JegerIds.Contains(j.ID)).ToList<Jeger>();
			var dogs =  JaktLoggApp.instance.DogList.Where(d => jakt.DogIds.Contains(d.ID)).ToList<Dog>();
			
			StringBuilder s = new StringBuilder();
			
			//JAKT
			s.Append("<html><head><title>"+jakt.Sted+"</title></head><body>");
			s.Append("<h1>"+jakt.Sted+"</h1>");
			s.Append("<span class='dato'>" + jakt.DatoFra.ToLocalDateString() == jakt.DatoTil.ToLocalDateString() ? jakt.DatoFra.ToLocalDateString() : jakt.DatoFra.ToLocalDateString() + " - " + jakt.DatoTil.ToLocalDateAndYearString()+"</span>");
			if(jakt.Notes.Length > 0)
				s.Append("<p>" + jakt.Notes + "</p>");
			
			//JEGERE
			if(jegere.Count() > 0){
				s.Append("<h2>"+Utils.Translate("hunters")+"</h2>");
				s.Append("<ul id='Jegere'>");
				foreach(var jeger in jegere)
				{
					s.Append("<li>" + jeger.Navn + "</li>");
				}
				s.Append("</ul>");
			}
			
			//HUNDER
			if(dogs.Count() > 0){
				s.Append("<h2>"+Utils.Translate("dogs")+"</h2>");
				s.Append("<ul id='Dogs'>");
				foreach(var dog in dogs)
				{
					s.Append("<li>" + dog.Navn + "</li>");
				}
				s.Append("</ul>");
			}
			
			//RESULTAT
			s.Append("<h2>"+Utils.Translate("result")+"</h2>");
			s.Append(string.Format("<table cellspacing='0'><tr><th>{0}</th><th>{1}</th><th>{2}</th><th>{3}</th></tr>",
									Utils.Translate("specie"),
									Utils.Translate("seen"),
									Utils.Translate("shots"),
									Utils.Translate("hits")));
				
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
			
			//RESULTAT PR JEGER
			if(jegere.Count() > 1)
			{
				s.Append("<h2>"+Utils.Translate("result")+", "+Utils.Translate("hunter")+"</h2>");
				foreach(var jeger in jegere)
				{
					s.Append("<h3>-"+jeger.Navn + "-</h3>");
					s.Append(string.Format("<table cellspacing='0'><tr><th>{0}</th><th>{1}</th><th>{2}</th><th>{3}</th></tr>",
									Utils.Translate("specie"),
									Utils.Translate("seen"),
									Utils.Translate("shots"),
									Utils.Translate("hits")));
					
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
				
			}
			
			//LOGGFØRINGER
			s.Append("<h2>"+Utils.Translate("logs")+"</h2>");
			
			
			s.Append("<table cellspacing='0'>");
			
			s.Append("<tr>");
			
			s.Append("<th>"+Utils.Translate("date")+"/"+Utils.Translate("time")+"</th>");
			s.Append("<th>"+Utils.Translate("hunter")+"</th>");
			s.Append("<th>"+Utils.Translate("dog")+"</th>");
			s.Append("<th>"+Utils.Translate("specie")+"</th>");
			s.Append("<th>"+Utils.Translate("shots")+"</th>");
			s.Append("<th>"+Utils.Translate("hits")+"</th>");
			s.Append("<th>"+Utils.Translate("seen")+"</th>");
			s.Append("<th>"+Utils.Translate("latitude")+"</th>");
			s.Append("<th>"+Utils.Translate("longitude")+"</th>");
			s.Append("<th>"+Utils.Translate("notes")+"</th>");
			
			foreach(var loggTypeId in JaktLoggApp.instance.SelectedLoggTypeIds)
			{
				var lt = JaktLoggApp.instance.LoggTypeList.Where(x => x.Key == loggTypeId).FirstOrDefault();
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
			var art = JaktLoggApp.instance.GetArt(logg.ArtId);
			StringBuilder s = new StringBuilder();
			s.Append("<tr>");
			
			s.Append("<td>" + logg.Dato.ToLocalDateString()+" kl."+logg.Dato.ToLocalTimeString() + "</td>");
			s.Append("<td>" + (logg.JegerId > 0 ? JaktLoggApp.instance.GetJeger(logg.JegerId).Fornavn : "") + "</td>");
			s.Append("<td>" + (logg.DogId > 0 ? JaktLoggApp.instance.GetDog(logg.DogId).Navn : "") + "</td>");
			s.Append("<td>" + (art == null ? "" : art.Navn) + "</td>");
			s.Append("<td>" + logg.Skudd + "</td>");
			s.Append("<td>" + logg.Treff + "</td>");
			s.Append("<td>" + logg.Sett + "</td>");
			s.Append("<td>" + logg.Latitude + "</td>");
			s.Append("<td>" + logg.Longitude + "</td>");
			s.Append("<td>" + logg.Notes + "</td>");
			
			foreach(var loggTypeId in JaktLoggApp.instance.SelectedLoggTypeIds)
			{
				var lt = JaktLoggApp.instance.LoggTypeList.Where(x => x.Key == loggTypeId).FirstOrDefault();
				if(lt != null){
					s.Append("<td>"+logg.GetType().GetProperty(lt.Key).GetValue(logg, null)+"</td>");
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
			s.Append(@" body { font-size:12px; font-family: Arial, Helvetica, Verdana; color:#353535 }
						h1,h2,h3,p,hr,ul,li{ margin:0; padding:0; }
						h1,h2,h3{ line-height:1.3em; margin:10px 0px;}
						hr { border:solid 1px #666; }
						li {list-style-type:none; }
						td,th {font-size:11px;}
						table, td {border:solid 1px #989898; }
		
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

