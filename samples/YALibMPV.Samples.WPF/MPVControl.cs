using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using YALibMPV.PInvoke;
using YALibMPV.PInvoke.Enums.Client;
using YALibMPV.PInvoke.Structs.Client;

namespace YALibMPV.Samples.WPF;

[TemplatePart(Name = PART_MPVHost, Type = typeof(MPVHwndHost))]
public class MPVControl : ContentControl, IDisposable
{
    private const string PART_MPVHost = "PART_MPVHost";

    private MPVHwndHost? _host = null;
    private MPVHandle? _mpvHandle;

    private bool IsDesignMode =>
        (bool)DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(DependencyObject)).DefaultValue;

    public MPVControl()
    {
        DefaultStyleKey = typeof(MPVControl);

        _host = new MPVHwndHost();
        _mpvHandle = Interop.MPVCreate();

        var error = Interop.MPVInitialize(_mpvHandle.Value);
        Debug.WriteLine("MPVInitialize: " + error);
    }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        if (IsDesignMode)
            return;

        if (Template.FindName(PART_MPVHost, this) is not MPVHwndHost controlHost)
        {
            Trace.WriteLine($"Couldn't find {PART_MPVHost} of type {nameof(MPVHwndHost)}");
            return;
        }

        _host = controlHost;

        var hostPtr = _host.Handle.ToInt64();
        var error = Interop.MPVSetOption(_mpvHandle!.Value, "wid", MPVFormat.Int64, ref hostPtr);
        Debug.WriteLine("MPVSetOption: " + error);

        if (error != MPVError.Success)
            return;

        var loadErr = Interop.MPVCommandString(_mpvHandle.Value,
            "loadfile http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/BigBuckBunny.mp4 replace");
        Debug.WriteLine("MPVCommand: " + loadErr);
    }

    public void Dispose()
    {
        _host?.Dispose();
        if (_host != null)
            Interop.MPVDestroy(_mpvHandle!.Value);
    }
}