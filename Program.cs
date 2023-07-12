/*
 * Copyright(C) 2022 December. All rights reserved.
 */
/*
 * Original Author: weijiong@ruijie.com.cn 2022-10-13
 *
 * History
 * v1.0.0.2
 * 下载失败时增加窗口提示
 *
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace header_tool
{
    internal static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>

#if DEBUG
        [DllImport("kernel32.dll")]
        public static extern bool AllocConsole();
        [DllImport("kernel32.dll")]
        public static extern bool FreeConsole();
#endif

        static void Main()
        {
#if DEBUG
            AllocConsole();
#endif

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (Updaterform.CheckHasNewVersion())
            {
                // Application.Run(new Updaterform());
                if (Updaterform.DownloadNewProgram())
                {
                    // download new version program success
#if DEBUG
                    FreeConsole();
#endif
                    MessageBox.Show("Update complete!", "Success");

                    System.Diagnostics.Process.GetCurrentProcess().Close();
                    Application.Restart();

                    return;
                }
            }

            Application.Run(new Mainform());


        }
    }
}
