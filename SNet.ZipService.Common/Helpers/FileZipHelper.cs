using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.Zip;

namespace SNet.ZipService.Common.Helpers
{
    public class FileZipHelper
    {
        public static void FileZipWithPassword(string path,string password)
        {
            var firstPath = path;
            path = path.Substring(0, path.IndexOf(('.')));
            using (ZipOutputStream st=new ZipOutputStream(File.Create(path + ".zip")))
            {
             st.SetLevel(9);
             st.Password = password;
                byte[] buffer=new byte[4096];
                ZipEntry entry=new ZipEntry(firstPath.Substring(firstPath.LastIndexOf('\\')));
                entry.DateTime=DateTime.Now;
                st.PutNextEntry(entry);
                using (FileStream fs=File.OpenRead(firstPath))
                {
                    int sourcebyte;
                    do
                    {
                        sourcebyte = fs.Read(buffer, 0, buffer.Length);
                        st.Write(buffer,0,sourcebyte);
                    } while (sourcebyte > 0);
                }
                st.Finish();
                st.Close();
            }

        }
    }
}   
