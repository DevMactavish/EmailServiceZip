using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SNet.ZipService.Common.Entities;
using SNet.ZipService.Common.Helpers;

namespace SNet.ZipService.ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            string watchFolder = ConfigurationManager.AppSettings["WacthFolder"];
            string zipFolder = ConfigurationManager.AppSettings["ZipFolder"];
            string password = ConfigurationManager.AppSettings["ZipEncryptionPassword"];
            var fileList = IOHelper.GetExcellFileList(watchFolder);
            foreach (var obj in fileList)
            {
                if (IOHelper.IsCompressible(obj))
                    FileZipHelper.FileZipWithPassword(obj, password);
            }

            var zipFileList = IOHelper.GetZipFileList(watchFolder);
            foreach (var obj in zipFileList)
            {
                var newPath = zipFolder + IOHelper.GetFileName(obj);
                IOHelper.Move(obj, newPath);
            }
            MailTemplate mailTemplate = new MailTemplate();
            try
            {
                string data = File.ReadAllText(ConfigurationManager.AppSettings["EmailFile"]);

                mailTemplate = JsonConvert.DeserializeObject<MailTemplate>(data);

                if (!string.IsNullOrEmpty(mailTemplate.Link))
                {
                    foreach (var email in mailTemplate.Emails)
                    {
                        MailHelper.Send(email, "Deneme", mailTemplate.Link);
                    }
                }
            }
            catch
            {
            }
        }



    }
}
