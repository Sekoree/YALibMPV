using System.Runtime.InteropServices;
using YALibMPV.PInvoke.Enums.Client;

namespace YALibMPV.PInvoke.Structs.Client;

[StructLayout(LayoutKind.Explicit, Size = 24)]
public struct MPVEventProperty
{
    [FieldOffset(0)]
    internal IntPtr _namePtr;
    
    public string Name => Marshal.PtrToStringUTF8(_namePtr) ?? string.Empty;
    
    [FieldOffset(8)]
    public MPVFormat Format;
    
    [FieldOffset(16)]
    internal IntPtr _dataPtr; //Expand to all formats
}