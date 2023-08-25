using System.Runtime.InteropServices;

namespace YALibMPV.PInvoke.Structs.Client;

/// <summary>(see mpv_node)</summary>
[StructLayout(LayoutKind.Sequential, Size = 16)]
public struct MPVByteArray
{
    /// <summary>
    /// <para>Pointer to the data. In what format the data is stored is up to whatever</para>
    /// <para>uses MPV_FORMAT_BYTE_ARRAY.</para>
    /// </summary>
    public IntPtr Data;
    
    /// <summary>Size of the data pointed to by ptr.</summary>
    public ulong Size;
}