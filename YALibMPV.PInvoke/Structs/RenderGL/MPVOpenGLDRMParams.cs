using System.Runtime.InteropServices;

namespace YALibMPV.PInvoke.Structs.RenderGL;

/// <summary>Deprecated. For MPV_RENDER_PARAM_DRM_DISPLAY.</summary>
[StructLayout(LayoutKind.Sequential, Size = 32)]
public struct MPVOpenGLDRMParams
{
    public int Fd;
    
    public int CrtcId;
    
    public int ConnectorId;
    
    public IntPtr AtomicRequestPtr; //WAT
    
    public int RenderFd;
}