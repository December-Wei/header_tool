using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace header_tool
{
    public partial class Mainform : Form
    {
        public const string applicationSettingXmlPath = "app.xml"; 
        public class ProcessFileConfig
        {
            private const Project defaultProject = Project.G4;
            private const string defaultDeviceType = "0x20000013,0x20000020,0x2000001c,0x2000002b,0x2000002c";

            public ProcessFileConfig(string name)
            {
                this.name = name;
                this.project = defaultProject;
                this.deviceType = defaultDeviceType;
            }
            public ProcessFileConfig(string name, Project project)
            {
                this.name = name;
                this.project = project;
                switch (project)
                {
                    case Project.G4:
                        this.deviceType = "0x20000013,0x20000020,0x2000001c,0x2000002b,0x2000002c";
                        break;
                    case Project.LiteH:
                        this.deviceType = "0x1000000b";
                        break;
                    default:
                        this.project = defaultProject;
                        this.deviceType = defaultDeviceType;
                        break;
                }
            }

            public string ShowString()
            {
                return $"name:{this.name} project:{this.project} deviceType:{this.deviceType}";
            }

            public enum Project
            {
                G4 = 0,
                LiteH,
            }
            public string name;
            public Project project;
            public string deviceType;
        }

        partial class ProcessFilePanel
        {
            public enum ProcessState
            {
                None = 0,
                Processing,
                ProcessDone,
                ProcessError,
            }

            private void PanelAttributeInit(ProcessFileConfig processFileConfig)
            {
                System.Console.WriteLine("\nSource File Name = " + processFileConfig.name);
                if (processFileConfig.name.LastIndexOf('.') < 0)
                {
                    System.Console.WriteLine("Can't find file suffix in \"" + srcFileName + " \", use \".bin\" as the suffix");
                    this.srcFileName = srcFileName + ".bin";
                }
                else
                {
                    this.srcFileName = processFileConfig.name;
                }
                this.finalHeaderName = this.srcFileName.Insert(this.srcFileName.LastIndexOf('.'), "_header");
                System.Console.WriteLine("Header File Name = " + this.finalHeaderName);

                this.fileVersion = null;
                this.fileDate = null;
                this.config = processFileConfig;

                this.state = ProcessState.None;
            }

            private uint FileCrc32(byte[] bytes)
            {
                const uint initValue = 0xffffffff;
                const uint xorot = 0xffffffff;

                System.UInt32[] crcTable =
                {
                    0x00000000, 0x77073096, 0xee0e612c, 0x990951ba, 0x076dc419, 0x706af48f, 0xe963a535, 0x9e6495a3,
                    0x0edb8832, 0x79dcb8a4, 0xe0d5e91e, 0x97d2d988, 0x09b64c2b, 0x7eb17cbd, 0xe7b82d07, 0x90bf1d91,
                    0x1db71064, 0x6ab020f2, 0xf3b97148, 0x84be41de, 0x1adad47d, 0x6ddde4eb, 0xf4d4b551, 0x83d385c7,
                    0x136c9856, 0x646ba8c0, 0xfd62f97a, 0x8a65c9ec, 0x14015c4f, 0x63066cd9, 0xfa0f3d63, 0x8d080df5,
                    0x3b6e20c8, 0x4c69105e, 0xd56041e4, 0xa2677172, 0x3c03e4d1, 0x4b04d447, 0xd20d85fd, 0xa50ab56b,
                    0x35b5a8fa, 0x42b2986c, 0xdbbbc9d6, 0xacbcf940, 0x32d86ce3, 0x45df5c75, 0xdcd60dcf, 0xabd13d59,
                    0x26d930ac, 0x51de003a, 0xc8d75180, 0xbfd06116, 0x21b4f4b5, 0x56b3c423, 0xcfba9599, 0xb8bda50f,
                    0x2802b89e, 0x5f058808, 0xc60cd9b2, 0xb10be924, 0x2f6f7c87, 0x58684c11, 0xc1611dab, 0xb6662d3d,
                    0x76dc4190, 0x01db7106, 0x98d220bc, 0xefd5102a, 0x71b18589, 0x06b6b51f, 0x9fbfe4a5, 0xe8b8d433,
                    0x7807c9a2, 0x0f00f934, 0x9609a88e, 0xe10e9818, 0x7f6a0dbb, 0x086d3d2d, 0x91646c97, 0xe6635c01,
                    0x6b6b51f4, 0x1c6c6162, 0x856530d8, 0xf262004e, 0x6c0695ed, 0x1b01a57b, 0x8208f4c1, 0xf50fc457,
                    0x65b0d9c6, 0x12b7e950, 0x8bbeb8ea, 0xfcb9887c, 0x62dd1ddf, 0x15da2d49, 0x8cd37cf3, 0xfbd44c65,
                    0x4db26158, 0x3ab551ce, 0xa3bc0074, 0xd4bb30e2, 0x4adfa541, 0x3dd895d7, 0xa4d1c46d, 0xd3d6f4fb,
                    0x4369e96a, 0x346ed9fc, 0xad678846, 0xda60b8d0, 0x44042d73, 0x33031de5, 0xaa0a4c5f, 0xdd0d7cc9,
                    0x5005713c, 0x270241aa, 0xbe0b1010, 0xc90c2086, 0x5768b525, 0x206f85b3, 0xb966d409, 0xce61e49f,
                    0x5edef90e, 0x29d9c998, 0xb0d09822, 0xc7d7a8b4, 0x59b33d17, 0x2eb40d81, 0xb7bd5c3b, 0xc0ba6cad,
                    0xedb88320, 0x9abfb3b6, 0x03b6e20c, 0x74b1d29a, 0xead54739, 0x9dd277af, 0x04db2615, 0x73dc1683,
                    0xe3630b12, 0x94643b84, 0x0d6d6a3e, 0x7a6a5aa8, 0xe40ecf0b, 0x9309ff9d, 0x0a00ae27, 0x7d079eb1,
                    0xf00f9344, 0x8708a3d2, 0x1e01f268, 0x6906c2fe, 0xf762575d, 0x806567cb, 0x196c3671, 0x6e6b06e7,
                    0xfed41b76, 0x89d32be0, 0x10da7a5a, 0x67dd4acc, 0xf9b9df6f, 0x8ebeeff9, 0x17b7be43, 0x60b08ed5,
                    0xd6d6a3e8, 0xa1d1937e, 0x38d8c2c4, 0x4fdff252, 0xd1bb67f1, 0xa6bc5767, 0x3fb506dd, 0x48b2364b,
                    0xd80d2bda, 0xaf0a1b4c, 0x36034af6, 0x41047a60, 0xdf60efc3, 0xa867df55, 0x316e8eef, 0x4669be79,
                    0xcb61b38c, 0xbc66831a, 0x256fd2a0, 0x5268e236, 0xcc0c7795, 0xbb0b4703, 0x220216b9, 0x5505262f,
                    0xc5ba3bbe, 0xb2bd0b28, 0x2bb45a92, 0x5cb36a04, 0xc2d7ffa7, 0xb5d0cf31, 0x2cd99e8b, 0x5bdeae1d,
                    0x9b64c2b0, 0xec63f226, 0x756aa39c, 0x026d930a, 0x9c0906a9, 0xeb0e363f, 0x72076785, 0x05005713,
                    0x95bf4a82, 0xe2b87a14, 0x7bb12bae, 0x0cb61b38, 0x92d28e9b, 0xe5d5be0d, 0x7cdcefb7, 0x0bdbdf21,
                    0x86d3d2d4, 0xf1d4e242, 0x68ddb3f8, 0x1fda836e, 0x81be16cd, 0xf6b9265b, 0x6fb077e1, 0x18b74777,
                    0x88085ae6, 0xff0f6a70, 0x66063bca, 0x11010b5c, 0x8f659eff, 0xf862ae69, 0x616bffd3, 0x166ccf45,
                    0xa00ae278, 0xd70dd2ee, 0x4e048354, 0x3903b3c2, 0xa7672661, 0xd06016f7, 0x4969474d, 0x3e6e77db,
                    0xaed16a4a, 0xd9d65adc, 0x40df0b66, 0x37d83bf0, 0xa9bcae53, 0xdebb9ec5, 0x47b2cf7f, 0x30b5ffe9,
                    0xbdbdf21c, 0xcabac28a, 0x53b39330, 0x24b4a3a6, 0xbad03605, 0xcdd70693, 0x54de5729, 0x23d967bf,
                    0xb3667a2e, 0xc4614ab8, 0x5d681b02, 0x2a6f2b94, 0xb40bbe37, 0xc30c8ea1, 0x5a05df1b, 0x2d02ef8d
                };
                int iCount = bytes.Length;
                uint crc = initValue;

                Console.WriteLine("crc lenth : {0}", iCount);

                for (int i = 0; i < iCount; i++)
                {
                    crc = crcTable[(crc ^ bytes[i]) & 0xff] ^ (crc >> 8);
                }

                return crc ^ xorot;
            }

            private void UrlTextBox_TextChanged(object sender, EventArgs e)
            {
                Match m;

                if (String.IsNullOrEmpty(UrlTextBox.Text))
                {
                    checkBoxUrl.Checked = false;
                    return;
                }

                checkBoxUrl.Checked = true;

                m = Regex.Match(UrlTextBox.Text, @"ver([^_]+)_([^_]+)_([^_.]+)\.bin");
                if (m.Groups.Count < 0)
                {
                    this.fileVersion = null;
                    this.fileDate = null;
                    this.fileImpTime = null;

                    System.Console.WriteLine("Imp File name: " + this.finalImpHeaderName);
                    return;
                }

                System.Console.WriteLine("Match: " + m.Groups.Count.ToString());
                for (int i = 0; i < m.Groups.Count; i++)
                {
                    System.Console.WriteLine("Match_" + i.ToString() + ": " + m.Groups[i].Value);
                }
                System.Console.WriteLine("version: " + m.Groups[1].Value);
                System.Console.WriteLine("fileDate: " + m.Groups[2].Value);
                System.Console.WriteLine("fileImpTime: " + m.Groups[3].Value);
                this.fileVersion = m.Groups[1].Value.ToLower();
                this.fileDate = m.Groups[2].Value.ToLower();
                this.fileImpTime = m.Groups[3].Value.ToLower();

                System.Console.WriteLine("Imp File name: " + this.finalImpHeaderName);
            }

            private void DownloadFile()
            {
                long percent;

                try
                {
                    System.Net.HttpWebRequest.DefaultWebProxy = null;
                    System.Net.HttpWebRequest Myrq = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(this.UrlTextBox.Text);
                    System.Net.HttpWebResponse myrp = (System.Net.HttpWebResponse)Myrq.GetResponse();

                    long totalBytes = myrp.ContentLength;

                    this.prgDownload.Maximum = (int)totalBytes;

                    System.IO.Stream st = myrp.GetResponseStream();
                    System.IO.Stream so = new System.IO.FileStream(this.srcFileName, System.IO.FileMode.Create);

                    long totalDownloadedByte = 0;
                    byte[] by = new byte[1024];
                    int osize;
                    int byteCount = 1024;

                    while ((osize = st.Read(by, 0, (int)by.Length)) > 0)
                    {
                        totalDownloadedByte = osize + totalDownloadedByte;
                        System.Windows.Forms.Application.DoEvents();
                        so.Write(by, 0, osize);
                        this.prgDownload.Value = (int)totalDownloadedByte;
                        //osize = st.Read(by, 0, (int)by.Length);

                        byteCount++;
                        if (byteCount >= 1024)
                        {
                            percent = 100 * totalDownloadedByte / totalBytes;
                            this.lblDownload.Text = "Downloading " + percent.ToString() + "%";
                            System.Windows.Forms.Application.DoEvents(); //必须加注这句代码，否则label将因为循环执行太快而来不及显示信息
                            byteCount = 0;
                        }
                    }
                    this.lblDownload.Text = "Downloading 100%";
                    System.Windows.Forms.Application.DoEvents(); //必须加注这句代码，否则label将因为循环执行太快而来不及显示信息
                    so.Close();
                    st.Close();
                }
                catch (System.Exception)
                {
                    this.state = ProcessState.ProcessError;
                    MessageBox.Show(this.srcFileName + " download fail!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    throw;
                }
            }

            private void FileHeaderAdd()
            {
                uint crc32Result;

                try
                {
                    crc32Result = this.FileCrc32(File.ReadAllBytes(this.srcFileName));
                    System.IO.Stream si = new System.IO.FileStream(this.srcFileName, System.IO.FileMode.Open);
                    System.IO.Stream so = new System.IO.FileStream(this.finalHeaderName, System.IO.FileMode.Create);

                    // header information
                    byte[] headerInfo;
                    headerInfo = Encoding.Default.GetBytes(string.Format("{0}\n" +
                        "FILEHEADER(\n" +
                        "DEVTYPE={1}\n" +
                        "TYPE={2}\n" +
                        "SLOT={3}\n" +
                        "CHIPNAME={4}\n" +
                        "VERSION=0x{5}-0x{6}\n" +
                        "FILETYPE={7}\n" +
                        "CRC=0x{8:x8}\n" +
                        ")\n",
                        this.srcFileName,
                        this.config.deviceType,
                        "fpga",
                        "1",
                        "fpga",
                        this.fileDate, this.fileVersion,
                        "ISC",
                        crc32Result));

                    Console.WriteLine(System.Text.Encoding.UTF8.GetString(headerInfo));

                    so.Write(headerInfo, 0, headerInfo.Length);

                    byte[] by = new byte[1024];
                    int osize;
                    while ((osize = si.Read(by, 0, (int)by.Length)) > 0)
                    {
                        so.Write(by, 0, osize);
                    }
                    so.Close();
                    si.Close();
                    File.Delete(this.srcFileName);
                }
                catch (System.Exception)
                {
                    throw;
                }
            }

            private void BWDoWork(object sender, System.ComponentModel.DoWorkEventArgs args)

            {
                this.DownloadFile();
                this.FileHeaderAdd();
            }

            private void BWRunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs args)
            {
                if (this.state == ProcessState.ProcessError)
                {
                    return;
                }

                this.state = ProcessState.ProcessDone;
            }

            public void StartProcess()
            {
                if (!this.checkBoxUrl.Checked)
                {
                    this.state = ProcessState.None;
                    return;
                }

                this.state = ProcessState.Processing;

                System.ComponentModel.BackgroundWorker bw = new System.ComponentModel.BackgroundWorker();
                // 定义需要在子线程中干的事情
                bw.DoWork += new System.ComponentModel.DoWorkEventHandler(BWDoWork);
                // 定义执行完毕后需要做的事情
                bw.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(BWRunWorkerCompleted);
                // 开始执行
                bw.RunWorkerAsync();
            }

            public void HeaderToImpHeader()
            {
                if (!this.checkBoxUrl.Checked)
                {
                    return;
                }

                if (!File.Exists(this.finalHeaderName))
                {
                    System.Console.WriteLine("Header file not exist");
                    return;
                }
                System.Console.WriteLine(this.finalHeaderName);
                System.Console.WriteLine(this.finalImpHeaderName);

                if (File.Exists(this.finalImpHeaderName))
                {
                    /* 删除已存在目标文件 */
                    File.Delete(this.finalImpHeaderName);
                }

                if (String.Compare(this.finalHeaderName, this.finalImpHeaderName) == 0)
                {
                    /* 文件名称相同，可能finalImpHeaderName解析错误 */
                    MessageBox.Show($"Phase new name fail when rename file [{this.finalHeaderName}]", "Rename Fail", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                File.Move(this.finalHeaderName, this.finalImpHeaderName);
                System.Console.WriteLine("Move success");
            }

            public ProcessState state;
            public string srcFileName;
            private string fileVersion;
            private string fileDate;
            private string fileImpTime;
            public string finalHeaderName;
            public string finalImpHeaderName
            {
                get
                {
                    string tmpStr;
                    if (string.IsNullOrEmpty(this.fileVersion)
                        || string.IsNullOrEmpty(this.fileDate)
                        || string.IsNullOrEmpty(this.fileImpTime))
                    {
                        return this.srcFileName.Insert(this.srcFileName.LastIndexOf('.'), "_header");
                    }

                    tmpStr = string.Format("_{0}_{1}_{2}_header",
                        this.fileVersion,
                        this.fileDate,
                        this.fileImpTime);
                    return this.srcFileName.Insert(this.srcFileName.LastIndexOf('.'), tmpStr);
                }
            }
            private ProcessFileConfig config;
        }

        private void ApplicationSetting()
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(applicationSettingXmlPath);

                Console.WriteLine("load done");
                XmlNodeList fileListNode = xmlDoc.SelectNodes("Filelist/File");
                Console.WriteLine(fileListNode.Count);
                this.config = new ProcessFileConfig[fileListNode.Count];
                int i = 0;

                foreach (XmlNode node in fileListNode)
                {
                    XmlAttributeCollection attributes = node.Attributes;
                    
                    if (attributes["project"] == null)
                    {
                        this.config[i] = new ProcessFileConfig(attributes["name"].Value);
                    }
                    else
                    {
                        this.config[i] = new ProcessFileConfig(attributes["name"].Value,
                            (ProcessFileConfig.Project)Enum.Parse(typeof(ProcessFileConfig.Project), attributes["project"].Value));
                    }
                    if (attributes["deviceType"] != null)
                    {
                        this.config[i].deviceType = attributes["deviceType"].Value;
                    }
                    i++;
                }

            }
            catch (System.Exception)
            {
                Console.WriteLine($"{applicationSettingXmlPath} file config fail, use default setting...");
                this.config = new ProcessFileConfig[2];
                this.config[0] = new ProcessFileConfig("common_fb_up_fpga_ps.bin");
                this.config[1] = new ProcessFileConfig("common_fb_dw_fpga_ps.bin");
            }

            foreach (ProcessFileConfig tmp in this.config)
            {
                Console.WriteLine(tmp.ShowString());
            }
        }

        public Mainform()
        {
            ApplicationSetting();
            InitializeComponent();
        }

        private void BWDoWork(object sender, System.ComponentModel.DoWorkEventArgs args)
        {
            bool allDone;
            
            do
            {
                Thread.Sleep(100);

                allDone = true;
                for (int i = 0; i < subPanel.Length; i++)
                {
                    if (this.subPanel[i].state == ProcessFilePanel.ProcessState.Processing)
                    {
                        allDone = false;
                    }
                }
            } while (!allDone);


            /* 判断是否有错误发生 */
            args.Result = true;
            for (int i = 0; i < subPanel.Length; i++)
            {
                if (this.subPanel[i].state == ProcessFilePanel.ProcessState.ProcessError)
                {
                    args.Result = false;
                }
            }
        }

        private void BWRunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs args)
        {
            string localIp;
            string userStr;

            if ((bool)args.Result == false)
            {
                this.btStart.Enabled = true;

                return;
            }

            System.Net.IPHostEntry hostIP = System.Net.Dns.GetHostEntry(Environment.MachineName);
            localIp = null;
            foreach (System.Net.IPAddress ipStr in hostIP.AddressList)
            {
                if (ipStr.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    localIp = ipStr.ToString();
                    Console.WriteLine("local ip: " + ipStr.ToString());
                    break;
                }
            }

            if (String.IsNullOrEmpty(localIp))
            {
                Console.WriteLine("local ip get fail!");
                return;

            }

            userStr = "";
            if (MessageBox.Show("All file process done! Do you need rename the file?", "Rename the file?",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                == DialogResult.Yes)
            {
                for (int i = 0; i < subPanel.Length; i++)
                {
                    if (this.subPanel[i].state != ProcessFilePanel.ProcessState.ProcessDone)
                    {
                        continue;
                    }
                    this.subPanel[i].HeaderToImpHeader();
                    userStr += String.Format("copy oob_http://{0}/{1} flash:{1} via mgmt 0\r\n",
                        localIp,
                        subPanel[i].finalImpHeaderName);
                    userStr += String.Format("tftp_tipc_client_elf 790 /data/{0} /usr/local/fpga-ps-firmware/{1} 1\r\n",
                        subPanel[i].finalImpHeaderName, subPanel[i].finalHeaderName);
                }
            }
            else
            {
                for (int i = 0; i < subPanel.Length; i++)
                {
                    if (this.subPanel[i].state != ProcessFilePanel.ProcessState.ProcessDone)
                    {
                        continue;
                    }
                    userStr += String.Format("copy oob_http://{0}/{1} flash:{1} via mgmt 0\r\n",
                        localIp,
                        subPanel[i].finalHeaderName);
                    userStr += String.Format("tftp_tipc_client_elf 790 /data/{0} /usr/local/fpga-ps-firmware/{0} 1\r\n",
                        subPanel[i].finalHeaderName);
                }

            }

            this.outputPanel.TextOutput(userStr);

            MessageBox.Show("Process done!");

            this.btStart.Enabled = true;
        }

        private void btStart_Click(object sender, EventArgs e)
        {
            bool noProcess;

            this.outputPanel.TextOutput("\r\nStart to file process.\t"+System.DateTime.Now.ToString()+"\r\n");
            this.outputPanel.TextOutput("-------------------------------------------\r\n");

            this.btStart.Enabled = false;
            Console.WriteLine(System.Windows.Forms.Application.StartupPath);

            noProcess = true;
            for (int i = 0; i < subPanel.Length; i++)
            {
                this.subPanel[i].StartProcess();
                if (this.subPanel[i].state != ProcessFilePanel.ProcessState.None)
                {
                    noProcess = false;
                }
            }

            if (noProcess)
            {
                this.outputPanel.TextOutputLn("No process file...");
                this.btStart.Enabled = true;
                return;
            }

            System.ComponentModel.BackgroundWorker bw = new System.ComponentModel.BackgroundWorker();
            // 定义需要在子线程中干的事情
            bw.DoWork += new System.ComponentModel.DoWorkEventHandler(BWDoWork);
            // 定义执行完毕后需要做的事情
            bw.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(BWRunWorkerCompleted);
            // 开始执行
            bw.RunWorkerAsync();
        }

        private void Mainform_Load(object sender, EventArgs e)
        {
            this.outputPanel.TextOutputLn(Application.ProductName + "\tversion:" + Application.ProductVersion);
            this.outputPanel.TextOutputLn("");
            this.outputPanel.TextOutputLn("Description:");
        }

        private ProcessFileConfig[] config;
    }
}
