using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace QianShi.Music.Mini
{
    public class ZWinUtil
    {
        #region ShowWindow 方法窗体状态的参数枚举

        /// <summary>
        /// 隐藏窗口并激活其他窗口
        /// </summary>
        private const int SW_HIDE = 0;

        /// <summary>
        /// 激活并显示一个窗口。如果窗口被最小化或最大化，系统将其恢复到原来的尺寸和大小。应用程序在第一次显示窗口的时候应该指定此标志
        /// </summary>
        private const int SW_SHOWNORMAL = 1;

        /// <summary>
        /// 激活窗口并将其最小化
        /// </summary>
        private const int SW_SHOWMINIMIZED = 2;

        /// <summary>
        /// 激活窗口并将其最大化
        /// </summary>
        private const int SW_SHOWMAXIMIZED = 3;

        /// <summary>
        /// 以窗口最近一次的大小和状态显示窗口。此值与SW_SHOWNORMAL相似，只是窗口没有被激活
        /// </summary>
        private const int SW_SHOWNOACTIVATE = 4;

        /// <summary>
        /// 在窗口原来的位置以原来的尺寸激活和显示窗口
        /// </summary>
        private const int SW_SHOW = 5;

        /// <summary>
        /// 最小化指定的窗口并且激活在Z序中的下一个顶层窗口
        /// </summary>
        private const int SW_MINIMIZE = 6;

        /// <summary>
        /// 最小化的方式显示窗口，此值与SW_SHOWMINIMIZED相似，只是窗口没有被激活
        /// </summary>
        private const int SW_SHOWMINNOACTIVE = 7;

        /// <summary>
        /// 以窗口原来的状态显示窗口。此值与SW_SHOW相似，只是窗口没有被激活
        /// </summary>
        private const int SW_SHOWNA = 8;

        /// <summary>
        /// 激活并显示窗口。如果窗口最小化或最大化，则系统将窗口恢复到原来的尺寸和位置。在恢复最小化窗口时，应用程序应该指定这个标志
        /// </summary>
        private const int SW_RESTORE = 9;

        /// <summary>
        /// 依据在STARTUPINFO结构中指定的SW_FLAG标志设定显示状态，STARTUPINFO 结构是由启动应用程序的程序传递给CreateProcess函数的
        /// </summary>
        private const int SW_SHOWDEFAULT = 10;

        /// <summary>
        /// 最小化窗口，即使拥有窗口的线程被挂起也会最小化。在从其他线程最小化窗口时才使用这个参数
        /// </summary>
        private const int SW_FORCEMINIMIZE = 11;

        #endregion ShowWindow 方法窗体状态的参数枚举

        //窗体置顶
        private static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);

        //取消窗体置顶
        private static readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2);

        //不调整窗体位置
        private const uint SWP_NOMOVE = 0x0002;

        //不调整窗体大小
        private const uint SWP_NOSIZE = 0x0001;

        [DllImport("User32.dll", EntryPoint = "FindWindow")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, uint uFlags);

        [DllImport("user32.dll", EntryPoint = "ShowWindow")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        /// <summary>
        /// 使窗体置顶
        /// </summary>
        /// <param name="Name">需要置顶的窗体的名字</param>
        public static void SetTop(string Name)
        {
            IntPtr CustomBar = FindWindow(null, Name);
            if (CustomBar != null)
            {
                SetWindowPos(CustomBar, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE);
            }
        }

        /// <summary>
        /// 取消窗体置顶
        /// </summary>
        /// <param name="Name">需要置顶的窗体的名字</param>
        public static void SetTopNO(string Name)
        {
            IntPtr CustomBar = FindWindow(null, Name);
            if (CustomBar != null)
            {
                SetWindowPos(CustomBar, HWND_NOTOPMOST, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE);
            }
        }

        /// <summary>
        /// 显示窗口
        /// </summary>
        /// <param name="Name"></param>
        public static void SetWinShow(string Name)
        {
            IntPtr CustomBar = FindWindow(null, Name);
            if (CustomBar != null)
            {
                ShowWindow(CustomBar, SW_SHOW);
            }
        }

        /// <summary>
        /// 隐藏窗口
        /// </summary>
        /// <param name="Name"></param>
        public static void SetWinHide(string Name)
        {
            IntPtr CustomBar = FindWindow(null, Name);
            if (CustomBar != null)
            {
                ShowWindow(CustomBar, SW_HIDE);
            }
        }
    }
}
