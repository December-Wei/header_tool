using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.IO;

namespace header_tool
{
    public partial class Updaterform : Form
    {
        private const string remoteMasterIP = "10.104.27.111";
        private const string xmlName = "tool_describe.xml";
        private static string TempFilename
        {
            get
            { return $"~${Application.ProductName}.exe"; }
        }
        private static string CurrentFilename
        {
            get
            { return $"{Application.ProductName}.exe"; }
        }

        public Updaterform()
        {
            InitializeComponent();
        }

        private static byte[] GetRemoteFile(string downloadPath)
        {
            byte[] docByte = null;
            
            try
            {
                System.Net.HttpWebRequest.DefaultWebProxy = null;
                System.Net.HttpWebRequest Myrq = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(downloadPath);

                System.Net.HttpWebResponse myrp = (System.Net.HttpWebResponse)Myrq.GetResponse();

                System.IO.Stream st = myrp.GetResponseStream();

                docByte = new byte[myrp.ContentLength + 1];

                byte[] tmpByte = new byte[1024];
                int osize;
                int offset = 0;
                while ((osize = st.Read(tmpByte, 0, 1024)) > 0)
                {
                    Array.Copy(tmpByte, 0, docByte, offset, osize);
                    offset += osize;
                }

                st.Close();

                Console.WriteLine("total down lenth:" + osize);
                Console.WriteLine("response lenth:" + myrp.ContentLength);
                Console.WriteLine("int lenth:" + ((int)myrp.ContentLength).ToString());
                st.Close();
            }
            catch (Exception)
            {
                Console.WriteLine("[Error] Download <{0}> fail!", downloadPath);
                return null;
            }


            return docByte;
        }

        private static byte[] GetRemoteDescribe()
        {
            return GetRemoteFile($"http://{remoteMasterIP}/{xmlName}");
        }

        public static bool CheckHasNewVersion()
        {
            byte[] xmlByte;

            /* 更新检查时顺便删除临时文件 */
            if (File.Exists(TempFilename))
            {
                Console.WriteLine("delete tmp file success");
                System.IO.File.Delete(TempFilename);
            }
            else
            {
                Console.WriteLine("not found delete tmp file");
            }

            xmlByte = GetRemoteDescribe();
            if (xmlByte == null)
            {
                System.Console.WriteLine("Get remote describe file fail");
                return false;
            }

            XmlDocument xmlDoc = new XmlDocument();

            xmlDoc.LoadXml(System.Text.Encoding.UTF8.GetString(xmlByte));

            XmlNode verNode = xmlDoc.SelectSingleNode("root");
            if (verNode != null)
            {
                Console.WriteLine("found node <{0}>", verNode.InnerText);

                if (string.Compare(verNode.InnerText, Application.ProductVersion) > 0)
                {
                    MessageBox.Show("Found a new version, update now...\r\n\r\n" +
                        $"# old version:{Application.ProductVersion}\r\n" +
                        $"# new version:{verNode.InnerText}",
                        $"{Application.ProductName} update",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    return true;
                }

            }
            else
            {
                Console.WriteLine("not found node");
            }

            return false;
        }

        public static bool DownloadNewProgram()
        {
            byte[] newProgram;

            newProgram = GetRemoteFile($"http://{remoteMasterIP}/{CurrentFilename}");
            if (newProgram == null)
            {
                Console.WriteLine("Download new program fail.");
                MessageBox.Show("Error occured when download remote file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            Console.WriteLine("Download new program success.");

            File.SetAttributes(CurrentFilename, FileAttributes.Hidden);
            File.Move(CurrentFilename, TempFilename);
            Stream so = new System.IO.FileStream(CurrentFilename, System.IO.FileMode.Create);
            so.Write(newProgram, 0, newProgram.Length);
            so.Close();

            return true;
        }
    }
}
