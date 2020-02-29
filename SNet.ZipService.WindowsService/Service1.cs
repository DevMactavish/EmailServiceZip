using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Newtonsoft.Json;
using SNet.ZipService.Common.Entities;
using SNet.ZipService.Common.Helpers;

namespace SNet.ZipService.WindowsService
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        private Timer _timer;
        protected override void OnStart(string[] args)
        {
            _timer=new Timer();

            _timer.Interval = 8 * 60000;
            _timer.Elapsed += TimerElapsed;
            _timer.Start();
        }

        public  void TimerElapsed(Object o,ElapsedEventArgs e)
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
        protected override void OnStop()
        {
            _timer.Stop();
        }
    }
}
