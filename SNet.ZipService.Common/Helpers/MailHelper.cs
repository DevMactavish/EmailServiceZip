using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using SNet.ZipService.Common.Entities;

namespace SNet.ZipService.Common.Helpers
{
    public class MailHelper
    {
        
        public static MailSettings Settings;

        #region MailSend
        public static void Send(string To, string Subject, string Message)
        {
            if (Settings == null)
                LoadSettings();
            try
            {
                using (SmtpClient client = new SmtpClient())
                {
                    client.Port = Settings.Port;
                    client.Host = Settings.Host;
                    client.EnableSsl = true;
                    client.Timeout = Settings.Timeout;
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.UseDefaultCredentials = true;
                    client.Credentials = new System.Net.NetworkCredential(Settings.Mail, Settings.Password);

                    MailMessage mm = new MailMessage(Settings.Mail, To, Subject, Message);

                    mm.BodyEncoding = UTF8Encoding.UTF8;
                    mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

                    client.Send(mm);

                    Console.WriteLine("Mail Atiliyor : {0} > {1} ", To, Message);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        #endregion

        #region SettingsLoad
        private static void LoadSettings()
        {
            Settings = new MailSettings();
            try
            {
                Settings.Port = !string.IsNullOrEmpty(ConfigurationManager.AppSettings["MailPort"]) ? Convert.ToInt32(ConfigurationManager.AppSettings["MailPort"]) : 587;
                Settings.Timeout = !string.IsNullOrEmpty(ConfigurationManager.AppSettings["Timeout"]) ? Convert.ToInt32(ConfigurationManager.AppSettings["Timeout"]) : 15000;
                Settings.Host = !string.IsNullOrEmpty(ConfigurationManager.AppSettings["MailHost"]) ? ConfigurationManager.AppSettings["MailHost"] : "smtp.office365.com";
                Settings.Mail = !string.IsNullOrEmpty(ConfigurationManager.AppSettings["MailAddress"]) ? ConfigurationManager.AppSettings["MailAddress"] : "support@mowico.com";
                Settings.Password = !string.IsNullOrEmpty(ConfigurationManager.AppSettings["MailPassword"]) ? ConfigurationManager.AppSettings["MailPassword"] : "Kag54306";
            }
            catch (Exception ex)
            {
                Console.WriteLine("Mail ayarları alınamadı. \r\n Hata Mesajı :" + ex.Message);
            }
        }
        #endregion
    }
}

