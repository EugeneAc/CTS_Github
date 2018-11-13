using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;
using System.Configuration;

namespace CTS_Core
{
	public static class CtsEmailSender
	{
		static SmtpClient _smtpClient;
		static MailAddress _mailSender;

		static CtsEmailSender()
		{
			_smtpClient = new SmtpClient();
			//_smtpClient.Host = "smtp.mail.ru";
			//_smtpClient.Port = 25;
			//_smtpClient.Timeout = 30000;
			//_smtpClient.EnableSsl = true;
			//_smtpClient.Credentials = new NetworkCredential("julia-aniskina@mail.ru", "");
			//_mailSender = new MailAddress("julia-aniskina@mail.ru", "Yuliya");
			var assemblyName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
			var config = (SmtpConfigSection)ConfigurationManager.OpenExeConfiguration(assemblyName + @".dll").GetSection("ctsSmtp");
			_smtpClient.Host = config.SmtpServer.Host;
			_smtpClient.Port = config.SmtpServer.Port;
			_smtpClient.Timeout = config.SmtpServer.Timeout;
			_smtpClient.EnableSsl = config.SmtpServer.EnableSSL;
			if(config.SmtpServer.Login != "")
			{
				_smtpClient.Credentials = new NetworkCredential(config.SmtpServer.Login, config.SmtpServer.Password);
			}	
			_mailSender = new MailAddress(config.SmtpServer.SenderMail, config.SmtpServer.SenderName);
		}

		static public bool SendMail(string[] receivers, string subject, string body, bool isBodyHtml)
		{

			var message = new MailMessage();
			message.From = _mailSender;
			foreach(var r in receivers)
			{
				message.To.Add(r);
			}
			message.Subject = subject;
			message.Body = body;
			message.IsBodyHtml = isBodyHtml;

			try
			{
				_smtpClient.Send(message);

				return true;
			}
			catch(Exception ex)
			{
				Logger.MakeLog(ex.ToString());
				
				return false;
			}			
		}

		static public void SendMailTest()
		{
			var receivers = new string[1] { "julia-aniskina@mail.ru" };

			SendMail(receivers, "Тест второй", "<h3>Письмо-тест работы smtp-клиента</h3>", true);
		}
	}
}
