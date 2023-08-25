using System.Runtime.InteropServices;

namespace YALibMPV.PInvoke.Structs.RenderGL;

/// <summary>For MPV_RENDER_PARAM_DRM_DRAW_SURFACE_SIZE.</summary>
[StructLayout(LayoutKind.Sequential, Size = 8)]
public struct MPVOpenGLDRMDrawSurfaceSize
{
    /// <summary>size of the draw plane surface in pixels.</summary>
    public int Width;
    
    /// <summary>size of the draw plane surface in pixels.</summary>
    public int Height;
}