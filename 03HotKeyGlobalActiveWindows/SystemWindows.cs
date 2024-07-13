using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interop;
using System.Windows;

namespace HotKeyGlobalActiveWindows
{

    public static class SystemWindows
    {
        #region 常量

        const UInt32 SWP_NOSIZE = 0x0001; // 不改变窗口大小
        const UInt32 SWP_NOMOVE = 0x0002; // 不改变窗口位置
        const UInt32 SWP_SHOWWINDOW = 0x0040; // 显示窗口

        #endregion

        /// <summary>
        /// 通过将当前窗口附加到前台窗口来激活任意窗口
        /// </summary>
        public static void GlobalActivate(this Window w)
        {
            // 获取此窗口线程的进程ID
            var interopHelper = new WindowInteropHelper(w);
            var thisWindowThreadId = GetWindowThreadProcessId(interopHelper.Handle, IntPtr.Zero);

            // 获取前台窗口线程的进程ID
            var currentForegroundWindow = GetForegroundWindow();
            var currentForegroundWindowThreadId = GetWindowThreadProcessId(currentForegroundWindow, IntPtr.Zero);

            // 将此窗口线程附加到当前窗口线程
            AttachThreadInput(currentForegroundWindowThreadId, thisWindowThreadId, true);

            // 设置窗口位置
            SetWindowPos(interopHelper.Handle, new IntPtr(0), 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE | SWP_SHOWWINDOW);

            // 将此窗口线程从当前窗口线程分离
            AttachThreadInput(currentForegroundWindowThreadId, thisWindowThreadId, false);

            // 显示并激活窗口
            if (w.WindowState == WindowState.Minimized) w.WindowState = WindowState.Normal;
            w.Show();
            w.Activate();
        }

        #region DllImport

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow(); // 获取前台窗口句柄

        [DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, IntPtr ProcessId); // 获取窗口所属线程的进程ID

        [DllImport("user32.dll")]
        private static extern bool AttachThreadInput(uint idAttach, uint idAttachTo, bool fAttach); // 将一个线程附加到另一个线程

        [DllImport("user32.dll")]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags); // 设置窗口位置和状态

        #endregion
    }
}
