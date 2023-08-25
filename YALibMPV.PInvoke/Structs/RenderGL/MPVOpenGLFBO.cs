﻿using System.Runtime.InteropServices;

namespace YALibMPV.PInvoke.Structs.RenderGL;

/// <summary>For MPV_RENDER_PARAM_OPENGL_FBO.</summary>
[StructLayout(LayoutKind.Sequential, Size = 16)]
public struct MPVOpenGLFBO
{
    /// <summary>
    /// <para>Framebuffer object name. This must be either a valid FBO generated by</para>
    /// <para>glGenFramebuffers() that is complete and color-renderable, or 0. If the</para>
    /// <para>value is 0, this refers to the OpenGL default framebuffer.</para>
    /// </summary>
    public int Fbo;
    
    /// <summary>
    /// <para>Valid dimensions. This must refer to the size of the framebuffer. This</para>
    /// <para>must always be set.</para>
    /// </summary>
    public int W;
    
    /// <summary>
    /// <para>Valid dimensions. This must refer to the size of the framebuffer. This</para>
    /// <para>must always be set.</para>
    /// </summary>
    public int H;
    
    /// <summary>
    /// <para>Underlying texture internal format (e.g. GL_RGBA8), or 0 if unknown. If</para>
    /// <para>this is the default framebuffer, this can be an equivalent.</para>
    /// </summary>
    public int InternalFormat;
}