using System;
using System.Diagnostics;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Platform;
using YALibMPV.PInvoke;
using YALibMPV.PInvoke.Enums.Client;
using YALibMPV.PInvoke.Structs.Client;

namespace YALibMPV.Samples.Avalonia;

public class MPVControl : NativeControlHost
{
    private readonly MPVHandle _handle;

    public MPVControl()
    {
        _handle = Interop.MPVCreate();
        var error = Interop.MPVInitialize(_handle);
        Debug.WriteLine("MPVInitialize: " + error);
    }

    private void Attach()
    {
        var err = Interop.MPVCommandString(_handle, "loadfile http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/BigBuckBunny.mp4 replace");
        Debug.WriteLine("MPVCommand: " + err);
    }

    protected override IPlatformHandle CreateNativeControlCore(IPlatformHandle parent)
    {
        var platformHandle = base.CreateNativeControlCore(parent);
        
        
        var handleInt = platformHandle.Handle.ToInt64();
        Interop.MPVSetOption(_handle, "wid", MPVFormat.Int64, ref handleInt);
        
        Attach();
        
        return platformHandle;
    }
    
    protected override void DestroyNativeControlCore(IPlatformHandle control)
    {
        Interop.MPVTerminateDestroy(_handle);
        base.DestroyNativeControlCore(control);
    }
}