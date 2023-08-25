namespace YALibMPV.PInvoke.Enums.Render;

/// <summary>
/// <para>Parameters for mpv_render_param (which is used in a few places such as</para>
/// <para>mpv_render_context_create().</para>
/// </summary>
/// <remarks>Also see mpv_render_param for conventions and how to use it.</remarks>
public enum MPVRenderParamType
{
    /// <summary>
    /// <para>Not a valid value, but also used to terminate a params array. Its value</para>
    /// <para>is always guaranteed to be 0 (even if the ABI changes in the future).</para>
    /// </summary>
    Invalid = 0,

    /// <summary>The render API to use. Valid for mpv_render_context_create().</summary>
    /// <remarks>
    /// <para>Type: char*</para>
    /// <para>Defined APIs:</para>
    /// <para>MPV_RENDER_API_TYPE_OPENGL:</para>
    /// <para>OpenGL desktop 2.1 or later (preferably core profile compatible to</para>
    /// <para>OpenGL 3.2), or OpenGLES 2.0 or later.</para>
    /// <para>Providing MPV_RENDER_PARAM_OPENGL_INIT_PARAMS is required.</para>
    /// <para>It is expected that an OpenGL context is valid and &quot;current&quot; when</para>
    /// <para>calling mpv_render_* functions (unless specified otherwise). It</para>
    /// <para>must be the same context for the same mpv_render_context.</para>
    /// </remarks>
    ApiType = 1,

    /// <summary>
    /// <para>Required parameters for initializing the OpenGL renderer. Valid for</para>
    /// <para>mpv_render_context_create().</para>
    /// <para>Type: mpv_opengl_init_params*</para>
    /// </summary>
    OpenglInitParams = 2,

    /// <summary>
    /// <para>Describes a GL render target. Valid for mpv_render_context_render().</para>
    /// <para>Type: mpv_opengl_fbo*</para>
    /// </summary>
    OpenglFbo = 3,

    /// <summary>
    /// <para>Control flipped rendering. Valid for mpv_render_context_render().</para>
    /// <para>Type: int*</para>
    /// <para>If the value is set to 0, render normally. Otherwise, render it flipped,</para>
    /// <para>which is needed e.g. when rendering to an OpenGL default framebuffer</para>
    /// <para>(which has a flipped coordinate system).</para>
    /// </summary>
    FlipY = 4,

    /// <summary>
    /// <para>Control surface depth. Valid for mpv_render_context_render().</para>
    /// <para>Type: int*</para>
    /// <para>This implies the depth of the surface passed to the render function in</para>
    /// <para>bits per channel. If omitted or set to 0, the renderer will assume 8.</para>
    /// <para>Typically used to control dithering.</para>
    /// </summary>
    Depth = 5,

    /// <summary>
    /// <para>ICC profile blob. Valid for mpv_render_context_set_parameter().</para>
    /// <para>Type: mpv_byte_array*</para>
    /// <para>Set an ICC profile for use with the &quot;icc-profile-auto&quot; option. (If the</para>
    /// <para>option is not enabled, the ICC data will not be used.)</para>
    /// </summary>
    IccProfile = 6,

    /// <summary>
    /// <para>Ambient light in lux. Valid for mpv_render_context_set_parameter().</para>
    /// <para>Type: int*</para>
    /// <para>This can be used for automatic gamma correction.</para>
    /// </summary>
    AmbientLight = 7,

    /// <summary>
    /// <para>X11 Display, sometimes used for hwdec. Valid for</para>
    /// <para>mpv_render_context_create(). The Display must stay valid for the lifetime</para>
    /// <para>of the mpv_render_context.</para>
    /// <para>Type: Display*</para>
    /// </summary>
    X11Display = 8,

    /// <summary>
    /// <para>Wayland display, sometimes used for hwdec. Valid for</para>
    /// <para>mpv_render_context_create(). The wl_display must stay valid for the</para>
    /// <para>lifetime of the mpv_render_context.</para>
    /// <para>Type: struct wl_display*</para>
    /// </summary>
    WlDisplay = 9,

    /// <summary>
    /// <para>Better control about rendering and enabling some advanced features. Valid</para>
    /// <para>for mpv_render_context_create().</para>
    /// </summary>
    /// <remarks>
    /// <para>This conflates multiple requirements the API user promises to abide if</para>
    /// <para>this option is enabled:</para>
    /// <para>- The API user's render thread, which is calling the mpv_render_*()</para>
    /// <para>functions, never waits for the core. Otherwise deadlocks can happen.</para>
    /// <para>See &quot;Threading&quot; section.</para>
    /// <para>- The callback set with mpv_render_context_set_update_callback() can now</para>
    /// <para>be called even if there is no new frame. The API user should call the</para>
    /// <para>mpv_render_context_update() function, and interpret the return value</para>
    /// <para>for whether a new frame should be rendered.</para>
    /// <para>- Correct functionality is impossible if the update callback is not set,</para>
    /// <para>or not set soon enough after mpv_render_context_create() (the core can</para>
    /// <para>block while waiting for you to call mpv_render_context_update(), and</para>
    /// <para>if the update callback is not correctly set, it will deadlock, or</para>
    /// <para>block for too long).</para>
    /// <para>In general, setting this option will enable the following features (and</para>
    /// <para>possibly more):</para>
    /// <para>- &quot;Direct rendering&quot;, which means the player decodes directly to a</para>
    /// <para>texture, which saves a copy per video frame (&quot;vd-lavc-dr&quot; option</para>
    /// <para>needs to be enabled, and the rendering backend as well as the</para>
    /// <para>underlying GPU API/driver needs to have support for it).</para>
    /// <para>- Rendering screenshots with the GPU API if supported by the backend</para>
    /// <para>(instead of using a suboptimal software fallback via libswscale).</para>
    /// <para>Warning: do not just add this without reading the &quot;Threading&quot; section</para>
    /// <para>above, and then wondering that deadlocks happen. The</para>
    /// <para>requirements are tricky. But also note that even if advanced</para>
    /// <para>control is disabled, not adhering to the rules will lead to</para>
    /// <para>playback problems. Enabling advanced controls simply makes</para>
    /// <para>violating these rules fatal.</para>
    /// <para>Type: int*: 0 for disable (default), 1 for enable</para>
    /// </remarks>
    AdvancedControl = 10,

    /// <summary>
    /// <para>Return information about the next frame to render. Valid for</para>
    /// <para>mpv_render_context_get_info().</para>
    /// </summary>
    /// <remarks>
    /// <para>Type: mpv_render_frame_info*</para>
    /// <para>It strictly returns information about the _next_ frame. The implication</para>
    /// <para>is that e.g. mpv_render_context_update()'s return value will have</para>
    /// <para>MPV_RENDER_UPDATE_FRAME set, and the user is supposed to call</para>
    /// <para>mpv_render_context_render(). If there is no next frame, then the</para>
    /// <para>return value will have is_valid set to 0.</para>
    /// </remarks>
    NextFrameInfo = 11,

    /// <summary>Enable or disable video timing. Valid for mpv_render_context_render().</summary>
    /// <remarks>
    /// <para>Type: int*: 0 for disable, 1 for enable (default)</para>
    /// <para>When video is timed to audio, the player attempts to render video a bit</para>
    /// <para>ahead, and then do a blocking wait until the target display time is</para>
    /// <para>reached. This blocks mpv_render_context_render() for up to the amount</para>
    /// <para>specified with the &quot;video-timing-offset&quot; global option. You can set</para>
    /// <para>this parameter to 0 to disable this kind of waiting. If you do, it's</para>
    /// <para>recommended to use the target time value in mpv_render_frame_info to</para>
    /// <para>wait yourself, or to set the &quot;video-timing-offset&quot; to 0 instead.</para>
    /// <para>Disabling this without doing anything in addition will result in A/V sync</para>
    /// <para>being slightly off.</para>
    /// </remarks>
    BlockForTargetTime = 12,

    /// <summary>Use to skip rendering in mpv_render_context_render().</summary>
    /// <remarks>
    /// <para>Type: int*: 0 for rendering (default), 1 for skipping</para>
    /// <para>If this is set, you don't need to pass a target surface to the render</para>
    /// <para>function (and if you do, it's completely ignored). This can still call</para>
    /// <para>into the lower level APIs (i.e. if you use OpenGL, the OpenGL context</para>
    /// <para>must be set).</para>
    /// <para>Be aware that the render API will consider this frame as having been</para>
    /// <para>rendered. All other normal rules also apply, for example about whether</para>
    /// <para>you have to call mpv_render_context_report_swap(). It also does timing</para>
    /// <para>in the same way.</para>
    /// </remarks>
    SkipRendering = 13,

    /// <summary>
    /// <para>Deprecated. Not supported. Use MPV_RENDER_PARAM_DRM_DISPLAY_V2 instead.</para>
    /// <para>Type : struct mpv_opengl_drm_params*</para>
    /// </summary>
    DrmDisplay = 14,

    /// <summary>
    /// <para>DRM draw surface size, contains draw surface dimensions.</para>
    /// <para>Valid for mpv_render_context_create().</para>
    /// <para>Type : struct mpv_opengl_drm_draw_surface_size*</para>
    /// </summary>
    DrmDrawSurfaceSize = 15,

    /// <summary>
    /// <para>DRM display, contains drm display handles.</para>
    /// <para>Valid for mpv_render_context_create().</para>
    /// <para>Type : struct mpv_opengl_drm_params_v2*</para>
    /// </summary>
    DrmDisplayV2 = 16,

    /// <summary>
    /// <para>MPV_RENDER_API_TYPE_SW only: rendering target surface size, mandatory.</para>
    /// <para>Valid for MPV_RENDER_API_TYPE_SW&amp;mpv_render_context_render().</para>
    /// <para>Type: int[2] (e.g.: int s[2] = {w, h}; param.data =&amp;s[0];)</para>
    /// </summary>
    /// <remarks>
    /// <para>The video frame is transformed as with other VOs. Typically, this means</para>
    /// <para>the video gets scaled and black bars are added if the video size or</para>
    /// <para>aspect ratio mismatches with the target size.</para>
    /// </remarks>
    SwSize = 17,

    /// <summary>
    /// <para>MPV_RENDER_API_TYPE_SW only: rendering target surface pixel format,</para>
    /// <para>mandatory.</para>
    /// <para>Valid for MPV_RENDER_API_TYPE_SW&amp;mpv_render_context_render().</para>
    /// <para>Type: char* (e.g.: char *f = &quot;rgb0&quot;; param.data = f;)</para>
    /// </summary>
    /// <remarks>
    /// <para>Valid values are:</para>
    /// <para>&quot;rgb0&quot;, &quot;bgr0&quot;, &quot;0bgr&quot;, &quot;0rgb&quot;</para>
    /// <para>4 bytes per pixel RGB, 1 byte (8 bit) per component, component bytes</para>
    /// <para>with increasing address from left to right (e.g. &quot;rgb0&quot; has r at</para>
    /// <para>address 0), the &quot;0&quot; component contains uninitialized garbage (often</para>
    /// <para>the value 0, but not necessarily; the bad naming is inherited from</para>
    /// <para>FFmpeg)</para>
    /// <para>Pixel alignment size: 4 bytes</para>
    /// <para>&quot;rgb24&quot;</para>
    /// <para>3 bytes per pixel RGB. This is strongly discouraged because it is</para>
    /// <para>very slow.</para>
    /// <para>Pixel alignment size: 1 bytes</para>
    /// <para>other</para>
    /// <para>The API may accept other pixel formats, using mpv internal format</para>
    /// <para>names, as long as it's internally marked as RGB, has exactly 1</para>
    /// <para>plane, and is supported as conversion output. It is not a good idea</para>
    /// <para>to rely on any of these. Their semantics and handling could change.</para>
    /// </remarks>
    SwFormat = 18,

    /// <summary>
    /// <para>MPV_RENDER_API_TYPE_SW only: rendering target surface bytes per line,</para>
    /// <para>mandatory.</para>
    /// <para>Valid for MPV_RENDER_API_TYPE_SW&amp;mpv_render_context_render().</para>
    /// <para>Type: size_t*</para>
    /// </summary>
    /// <remarks>
    /// <para>This is the number of bytes between a pixel (x, y) and (x, y + 1) on the</para>
    /// <para>target surface. It must be a multiple of the pixel size, and have space</para>
    /// <para>for the surface width as specified by MPV_RENDER_PARAM_SW_SIZE.</para>
    /// <para>Both stride and pointer value should be a multiple of 64 to facilitate</para>
    /// <para>fast SIMD operation. Lower alignment might trigger slower code paths,</para>
    /// <para>and in the worst case, will copy the entire target frame. If mpv is built</para>
    /// <para>with zimg (and zimg is not disabled), the performance impact might be</para>
    /// <para>less.</para>
    /// <para>In either cases, the pointer and stride must be aligned at least to the</para>
    /// <para>pixel alignment size. Otherwise, crashes and undefined behavior is</para>
    /// <para>possible on platforms which do not support unaligned accesses (either</para>
    /// <para>through normal memory access or aligned SIMD memory access instructions).</para>
    /// </remarks>
    SwStride = 19,

    /// <summary>
    /// <para>MPV_RENDER_API_TYPE_SW only: rendering target surface bytes per line,</para>
    /// <para>mandatory.</para>
    /// <para>Valid for MPV_RENDER_API_TYPE_SW&amp;mpv_render_context_render().</para>
    /// <para>Type: size_t*</para>
    /// </summary>
    /// <remarks>
    /// <para>This is the number of bytes between a pixel (x, y) and (x, y + 1) on the</para>
    /// <para>target surface. It must be a multiple of the pixel size, and have space</para>
    /// <para>for the surface width as specified by MPV_RENDER_PARAM_SW_SIZE.</para>
    /// <para>Both stride and pointer value should be a multiple of 64 to facilitate</para>
    /// <para>fast SIMD operation. Lower alignment might trigger slower code paths,</para>
    /// <para>and in the worst case, will copy the entire target frame. If mpv is built</para>
    /// <para>with zimg (and zimg is not disabled), the performance impact might be</para>
    /// <para>less.</para>
    /// <para>In either cases, the pointer and stride must be aligned at least to the</para>
    /// <para>pixel alignment size. Otherwise, crashes and undefined behavior is</para>
    /// <para>possible on platforms which do not support unaligned accesses (either</para>
    /// <para>through normal memory access or aligned SIMD memory access instructions).</para>
    /// </remarks>
    SwPointer = 20
}