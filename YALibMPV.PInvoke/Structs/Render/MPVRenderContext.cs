using System.Runtime.InteropServices;

namespace YALibMPV.PInvoke.Structs.Render;

[StructLayout(LayoutKind.Sequential)]
public struct MPVRenderContext
{
    public IntPtr ContextHandle;
}