using System.Runtime.InteropServices;

namespace YALibMPV.PInvoke.Structs.Client;

[StructLayout(LayoutKind.Sequential, Size = 16)]
public struct MPVEventCommand
{
    internal IntPtr _resultPtr;
    
    /// <summary>
    /// <para>Result data of the command. Note that success/failure is signaled</para>
    /// <para>separately via mpv_event.error. This field is only for result data</para>
    /// <para>in case of success. Most commands leave it at MPV_FORMAT_NONE. Set</para>
    /// <para>to MPV_FORMAT_NONE on failure.</para>
    /// </summary>
    public MPVNode Result => Marshal.PtrToStructure<MPVNode>(_resultPtr);
}