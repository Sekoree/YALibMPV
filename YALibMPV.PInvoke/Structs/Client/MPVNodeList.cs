using System.Runtime.InteropServices;

namespace YALibMPV.PInvoke.Structs.Client;

/// <summary>(see mpv_node)</summary>
[StructLayout(LayoutKind.Explicit, Size = 24)]
public struct MPVNodeList
{
    /// <summary>Number of entries. Negative values are not allowed.</summary>
    [FieldOffset(0)]
    public int Num;
    
    [FieldOffset(8)]
    internal IntPtr _nodesPtr;
    
    /// <summary>
    /// <para>MPV_FORMAT_NODE_ARRAY:</para>
    /// <para>unused (typically NULL), access is not allowed</para>
    /// </summary>
    /// <remarks>
    /// <para>MPV_FORMAT_NODE_MAP:</para>
    /// <para>keys[N] refers to key of the Nth key/value pair. If num &gt; 0, keys[0] to</para>
    /// <para>keys[num-1] (inclusive) are valid. Otherwise, this can be NULL.</para>
    /// <para>The keys are in random order. The only guarantee is that keys[N] belongs</para>
    /// <para>to the value values[N]. NULL keys are not allowed.</para>
    /// </remarks>
    [FieldOffset(16)]
    internal IntPtr _keysPtr;

    /// <summary>
    /// <para>MPV_FORMAT_NODE_ARRAY:</para>
    /// <para>values[N] refers to value of the Nth item</para>
    /// </summary>
    public MPVNodeList[]? ValuesArray => this.ToArray();
    
    /// <summary>
    /// <para>MPV_FORMAT_NODE_MAP:</para>
    /// <para>values[N] refers to value of the Nth key/value pair</para>
    /// <para>If num &gt; 0, values[0] to values[num-1] (inclusive) are valid.</para>
    /// <para>Otherwise, this can be NULL</para>
    /// </summary>
    public Dictionary<string, MPVNodeList>? ValuesMap => this.ToDictionary();

    public MPVNodeList(params MPVNodeList[] values)
    {
        Num = values.Length;
        _nodesPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf<MPVNodeList>() * Num);
        for (int i = 0; i < Num; i++)
            Marshal.StructureToPtr(values[i], _nodesPtr + (i * Marshal.SizeOf<MPVNodeList>()), false);
    }

    public MPVNodeList(Dictionary<string, MPVNodeList> values)
    {
        Num = values.Count;
        var valKeys = values.Keys.ToArray();
        _keysPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf<IntPtr>() * Num);
        for (int i = 0; i < Num; i++)
        {
            var ptr = Marshal.StringToCoTaskMemUTF8(valKeys[i]);
            Marshal.WriteIntPtr(_keysPtr, i * Marshal.SizeOf<IntPtr>(), ptr);
        }
        
        _nodesPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf<MPVNodeList>() * Num);
        int c = 0;
        foreach (var item in values)
        {
            Marshal.StructureToPtr(item.Value, _nodesPtr + (c * Marshal.SizeOf<MPVNodeList>()), false);
            c++;
        }
    }
    
    private MPVNodeList[]? ToArray()
    {
        if (_nodesPtr == IntPtr.Zero)
            return null;
        var result = new MPVNodeList[Num];
        for (int i = 0; i < Num; i++)
            result[i] = Marshal.PtrToStructure<MPVNodeList>(_nodesPtr + (i * Marshal.SizeOf<MPVNodeList>()));
        return result;
    }
    
    private Dictionary<string, MPVNodeList>? ToDictionary()
    {
        if (_keysPtr == IntPtr.Zero || _nodesPtr == IntPtr.Zero)
            return null;
        var result = new Dictionary<string, MPVNodeList>(Num);
        for (int i = 0; i < Num; i++)
        {
            var val = Marshal.PtrToStructure<MPVNodeList>(_nodesPtr + (i * Marshal.SizeOf<MPVNodeList>()));
            var keyPtr = Marshal.ReadIntPtr(_keysPtr, i * Marshal.SizeOf<IntPtr>());
            var key = Marshal.PtrToStringUTF8(keyPtr);
            result.Add(key!, val);
        }

        return result;
    }
}