using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Diagnostics;

namespace Toolkit
{
    public static class Window
    {
        internal delegate int WindowEnumProc(IntPtr hwnd, IntPtr lparam);
        private delegate bool EnumWindowProc(IntPtr hwnd, IntPtr lParam);
        delegate bool EnumThreadDelegate(IntPtr hWnd, IntPtr lParam);

        [DllImport("user32.dll")]
        static extern bool EnumThreadWindows(int dwThreadId, EnumThreadDelegate lpfn, IntPtr lParam);

        // For Windows Mobile, replace user32.dll with coredll.dll
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        // Find window by Caption only. Note you must pass IntPtr.Zero as the first parameter.

        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        static extern IntPtr FindWindowByCaption(IntPtr ZeroOnly, string lpWindowName);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool EnumChildWindows(IntPtr window, EnumWindowProc callback, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetWindowRect(IntPtr hWnd, ref RECT lpRect);

        [DllImport("user32.dll")]
        static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        const UInt32 WM_CLOSE = 0x0010;


        public static List<IntPtr> GetAllChildHandles(IntPtr parent_handle)
        {
            List<IntPtr> childHandles = new List<IntPtr>();

            GCHandle gcChildhandlesList = GCHandle.Alloc(childHandles);
            IntPtr pointerChildHandlesList = GCHandle.ToIntPtr(gcChildhandlesList);

            try
            {
                EnumWindowProc childProc = new EnumWindowProc(EnumWindow);
                EnumChildWindows(parent_handle, childProc, pointerChildHandlesList);
            }
            finally
            {
                gcChildhandlesList.Free();
            }

            return childHandles;
        }

        public static IEnumerable<IntPtr> EnumerateProcessWindowHandles(int processId)
        {
            var handles = new List<IntPtr>();

            foreach (ProcessThread thread in System.Diagnostics.Process.GetProcessById(processId).Threads)
                EnumThreadWindows(thread.Id,
                    (hWnd, lParam) => { handles.Add(hWnd); return true; }, IntPtr.Zero);

            return handles;
        }


        public static string GetClassName(IntPtr hWnd)
        {
            StringBuilder ClassName = new StringBuilder(256);
            //Get the window class name
            int nRet = GetClassName(hWnd, ClassName, ClassName.Capacity);

            return ClassName.ToString();
        }

        public static Point GetPoint(IntPtr hWnd)
        {
            RECT rct = new RECT();
            GetWindowRect(hWnd, ref rct);
            return new Point(rct.Left, rct.Top);
        }

        public static RECT GetRect(IntPtr hWnd)
        {
            RECT rct = new RECT();
            GetWindowRect(hWnd, ref rct);
            return rct;
        }

        public static int GetLeft(IntPtr hWnd)
        {
            RECT rct = new RECT();
            GetWindowRect(hWnd, ref rct);
            return rct.Left;
        }

        public static int GetTop(IntPtr hWnd)
        {
            RECT rct = new RECT();
            GetWindowRect(hWnd, ref rct);
            return rct.Top;
        }

        public static int GetRight(IntPtr hWnd)
        {
            RECT rct = new RECT();
            GetWindowRect(hWnd, ref rct);
            return rct.Right;
        }

        public static int GetBottom(IntPtr hWnd)
        {
            RECT rct = new RECT();
            GetWindowRect(hWnd, ref rct);
            return rct.Bottom;
        }

        public static IntPtr GetHwnd(string WindowClassName)
        {
            return FindWindow(WindowClassName, null);
        }


        public static bool CloseWindow(String WindowName, int timeout = 20 * 1000)
        {
            "FindWindow".StartTimer();

            while ("FindWindow".GetTimer() < timeout)
            {
                IntPtr windowPtr = FindWindowByCaption(IntPtr.Zero, WindowName);

                if (windowPtr != IntPtr.Zero)
                {
                    SendMessage(windowPtr, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
                    break;
                }

                (0.5).Sleep();
            }

            return true;
        }



        private static bool EnumWindow(IntPtr hWnd, IntPtr lParam)
        {
            GCHandle gcChildhandlesList = GCHandle.FromIntPtr(lParam);

            if (gcChildhandlesList == null || gcChildhandlesList.Target == null)
            {
                return false;
            }

            List<IntPtr> childHandles = gcChildhandlesList.Target as List<IntPtr>;
            childHandles.Add(hWnd);

            return true;
        }
    }
}
