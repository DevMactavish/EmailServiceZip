using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;

namespace SNet.ZipService.Common.Helpers
{
    public static class IOHelper
    {
        public static  bool Move(string sourcePath, string destinationPath)
        {
            if (string.IsNullOrEmpty((sourcePath)) || string.IsNullOrEmpty(destinationPath))
                return false;
            try
            {
                if (!File.Exists(sourcePath))
                    return false;
                File.Move(sourcePath,destinationPath);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public static bool IsCompressible(string path)
        {
            if (string.IsNullOrEmpty((path)))
                return false;
            try
            {

                FileInfo fileInfo = new FileInfo(path);
                var lastWriteDate = fileInfo.LastWriteTime;
                var result = DateTime.Now - lastWriteDate;
                if (result.TotalMinutes <= 1 || result.TotalMinutes>=10)
                    return false;
            }
            catch
            {
                return false;
            }

            return true;
        }

        public static List<string> GetExcellFileList(string path)
        {
            return  Directory.GetFiles(path,"*xls").ToList();
        }

        public static string GetFileName(string obj)
        {
            return obj.Substring(obj.LastIndexOf("\\"));
        }

        public static List<string> GetZipFileList(string path)
        {
            return Directory.GetFiles(path, "*zip").ToList();
        }
    }
}
