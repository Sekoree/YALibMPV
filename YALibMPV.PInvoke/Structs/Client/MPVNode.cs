using System.Runtime.InteropServices;
using YALibMPV.PInvoke.Enums.Client;

namespace YALibMPV.PInvoke.Structs.Client;

/// <summary>Generic data storage.</summary>
/// <remarks>
/// <para>If mpv writes this struct (e.g. via mpv_get_property()), you must not change</para>
/// <para>the data. In some cases (mpv_get_property()), you have to free it with</para>
/// <para>mpv_free_node_contents(). If you fill this struct yourself, you're also</para>
/// <para>responsible for freeing it, and you must not call mpv_free_node_contents().</para>
/// </remarks>
[StructLayout(LayoutKind.Explicit, Size = 16)]
public struct MPVNode
{
    /// <summary>valid if format==MPV_FORMAT_STRING</summary>
    public string? StringValue => Marshal.PtrToStringUTF8(_structuredValue);

    [FieldOffset(0)] 
    internal IntPtr _structuredValue;
    
    /// <summary>valid if format==MPV_FORMAT_FLAG</summary>
    /// <remarks>0 = no; 1 = yes</remarks>
    [FieldOffset(0)]
    public readonly int YesNoFlag;

    /// <summary>valid if format==MPV_FORMAT_INT64</summary>
    [FieldOffset(0)] 
    public readonly long IntegerValue;

    /// <summary>valid if format==MPV_FORMAT_DOUBLE</summary>
    [FieldOffset(0)] 
    public readonly double DoubleValue;

    /// <summary>valid if format==MPV_FORMAT_NODE_ARRAY</summary>
    /// <summary>or if format==MPV_FORMAT_NODE_MAP</summary>
    public MPVNodeList RemoteNodeListValue => Marshal.PtrToStructure<MPVNodeList>(_structuredValue);

    /// <summary>valid if format==MPV_FORMAT_BYTE_ARRAY</summary>
    public MPVByteArray ByteArrayValue => Marshal.PtrToStructure<MPVByteArray>(_structuredValue);
    
    /// <summary>
    /// <para>Type of the data stored in this struct. This value rules what members in</para>
    /// <para>the given union can be accessed. The following formats are currently</para>
    /// <para>defined to be allowed in mpv_node:</para>
    /// </summary>
    /// <remarks>
    /// <para>MPV_FORMAT_STRING       (u.string)</para>
    /// <para>MPV_FORMAT_FLAG         (u.flag)</para>
    /// <para>MPV_FORMAT_INT64        (u.int64)</para>
    /// <para>MPV_FORMAT_DOUBLE       (u.double_)</para>
    /// <para>MPV_FORMAT_NODE_ARRAY   (u.list)</para>
    /// <para>MPV_FORMAT_NODE_MAP     (u.list)</para>
    /// <para>MPV_FORMAT_BYTE_ARRAY   (u.ba)</para>
    /// <para>MPV_FORMAT_NONE         (no member)</para>
    /// <para>If you encounter a value you don't know, you must not make any</para>
    /// <para>assumptions about the contents of union u.</para>
    /// </remarks>
    [FieldOffset(8)] 
    public MPVFormat Format;
    
    public MPVNode(string? value)
    {
        Format = MPVFormat.String;
        _structuredValue = Marshal.StringToCoTaskMemUTF8(value);
    }

    public MPVNode(bool value)
    {
        Format = MPVFormat.Flag;
        YesNoFlag = value ? 1 : 0;
    }

    public MPVNode(long value)
    {
        Format = MPVFormat.Int64;
        IntegerValue = value;
    }

    public MPVNode(double value)
    {
        Format = MPVFormat.Double;
        DoubleValue = value;
    }

    public MPVNode(MPVNodeList value)
    {
        Format = value._keysPtr != IntPtr.Zero ? MPVFormat.NodeMap : MPVFormat.NodeArray;
        _structuredValue = Marshal.AllocCoTaskMem(Marshal.SizeOf<MPVNodeList>());
        Marshal.StructureToPtr(value, _structuredValue, false);
    }

    public MPVNode(MPVByteArray value)
    {
        Format = MPVFormat.ByteArray;
        _structuredValue = Marshal.AllocCoTaskMem(Marshal.SizeOf<MPVByteArray>());
        Marshal.StructureToPtr(value, _structuredValue, false);
    }
}