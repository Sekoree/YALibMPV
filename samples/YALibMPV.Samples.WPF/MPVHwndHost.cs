using System;
using System.Runtime.InteropServices;
using System.Windows.Interop;

namespace YALibMPV.Samples.WPF;

public class MPVHwndHost : HwndHost
{
    protected override HandleRef BuildWindowCore(HandleRef hwndParent)
    {
        var handle = CreateWindowEx(ExtendedWindow32Styles.WS_EX_TRANSPARENT, "static", string.Empty,
            Window32Styles.WS_CHILD | Window32Styles.WS_VISIBLE, 
            0, 0, 0, 0, 
            hwndParent.Handle, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);

        return new HandleRef(this, handle);
    }

    protected override void DestroyWindowCore(HandleRef hwnd)
    {
        DestroyWindow(hwnd.Handle);
    }

    [Flags]
    internal enum ExtendedWindow32Styles : int
    {
        WS_EX_TRANSPARENT = 0x00000020
    }

    [Flags]
    internal enum Window32Styles : int
    {
        WS_CHILD = 0x40000000,
        WS_VISIBLE = 0x10000000
    }

    [DllImport("user32.dll")]
    internal static extern IntPtr CreateWindowEx(ExtendedWindow32Styles dwExStyle,
        string lpszClassName,
        string lpszWindowName,
        Window32Styles style,
        int x, int y, int width, int height,
        IntPtr hwndParent,
        IntPtr hMenu,
        IntPtr hInst,
        IntPtr lpParam);


    [DllImport("user32.dll")]
    internal static extern bool DestroyWindow(IntPtr hwnd);
}