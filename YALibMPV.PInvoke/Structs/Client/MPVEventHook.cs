﻿using System.Runtime.InteropServices;

namespace YALibMPV.PInvoke.Structs.Client;

[StructLayout(LayoutKind.Sequential, Size = 16)]
public struct MPVEventHook
{
    internal IntPtr _namePtr;
    
    /// <summary>The hook name as passed to mpv_hook_add().</summary>
    public string Name => Marshal.PtrToStringUTF8(_namePtr) ?? string.Empty;
    
    /// <summary>Internal ID that must be passed to mpv_hook_continue().</summary>
    public long Id;
}