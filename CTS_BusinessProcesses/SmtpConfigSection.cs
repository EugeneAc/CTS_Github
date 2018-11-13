using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTS_Core
{
	public class SmtpConfigSection : ConfigurationSection
	{
		[ConfigurationProperty("smtpServer")]
		public SmtpServerElement SmtpServer { get { return (SmtpServerElement)this["smtpServer"]; } }
	}

	public class SmtpServerElement : ConfigurationElement
	{
		[ConfigurationProperty("host")]
		public string Host { get { return (string)this["host"]; } }

		[ConfigurationProperty("port")]
		public int Port { get { return (int)this["port"]; } }

		[ConfigurationProperty("timeout")]
		public int Timeout { get { return (int)this["timeout"]; } }

		[ConfigurationProperty("enablessl")]
		public bool EnableSSL { get { return (bool)this["enablessl"]; } }

		[ConfigurationProperty("login")]
		public string Login { get { return (string)this["login"]; } }

		[ConfigurationProperty("password")]
		public string Password { get { return (string)this["password"]; } }

		[ConfigurationProperty("senderMail")]
		public string SenderMail { get { return (string)this["senderMail"]; } }

		[ConfigurationProperty("senderName")]
		public string SenderName { get { return (string)this["senderName"]; } }
	}
}
