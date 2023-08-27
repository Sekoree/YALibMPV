using System.Runtime.InteropServices;
using YALibMPV.PInvoke.Enums.Client;
using YALibMPV.PInvoke.Enums.Render;
using YALibMPV.PInvoke.Marshaller;
using YALibMPV.PInvoke.Structs.Client;
using YALibMPV.PInvoke.Structs.Render;
using YALibMPV.PInvoke.Structs.StreamCB;

namespace YALibMPV.PInvoke;

public static partial class Interop
{
    #if WINDOWS
    private const string LibraryName = "libmpv-2";
    #elif LINUX
    private const string LibraryName = "libmpv.so"; //Should be the right one, .so seems ot be important
    #elif OSX
    private const string LibraryName = "libmpv.dylib"; //I have no hardware to test this, so no clue if it works
    #else
    private const string LibraryName = "libmpv-2"; //Set your own in debug I guess
    #endif


    #region client.h

    /// <summary>Return the MPV_CLIENT_API_VERSION the mpv source has been compiled with.</summary>
    [LibraryImport(LibraryName, EntryPoint = "mpv_client_api_version")]
    public static partial ulong MPVClientAPIVersion();

    /// <summary>
    /// <para>Return a string describing the error. For unknown errors, the string</para>
    /// <para>&quot;unknown error&quot; is returned.</para>
    /// </summary>
    /// <param name="error">error number, see enum mpv_error</param>
    /// <returns>
    /// <para>A static string describing the error. The string is completely</para>
    /// <para>static, i.e. doesn't need to be deallocated, and is valid forever.</para>
    /// </returns>
    [LibraryImport(LibraryName, EntryPoint = "mpv_error_string", StringMarshalling = StringMarshalling.Custom,
        StringMarshallingCustomType = typeof(MPVStringMarshaller))]
    public static partial string MPVErrorString(int error);

    /// <summary>
    /// <para>Return a string describing the error. For unknown errors, the string</para>
    /// <para>&quot;unknown error&quot; is returned.</para>
    /// </summary>
    /// <param name="error">error number, see enum mpv_error</param>
    /// <returns>
    /// <para>A static string describing the error. The string is completely</para>
    /// <para>static, i.e. doesn't need to be deallocated, and is valid forever.</para>
    /// </returns>
    [LibraryImport(LibraryName, EntryPoint = "mpv_error_string", StringMarshalling = StringMarshalling.Custom,
        StringMarshallingCustomType = typeof(MPVStringMarshaller))]
    public static partial string MPVErrorString(MPVError error);

    #region MPVFree

    /// <summary>
    /// <para>General function to deallocate memory returned by some of the API functions.</para>
    /// <para>Call this only if it's explicitly documented as allowed. Calling this on</para>
    /// <para>mpv memory not owned by the caller will lead to undefined behavior.</para>
    /// </summary>
    /// <param name="data">A valid pointer returned by the API, or NULL.</param>
    [LibraryImport(LibraryName, EntryPoint = "mpv_free")]
    public static partial void MPVFree(ref IntPtr data);

    /// <summary>
    /// <para>General function to deallocate memory returned by some of the API functions.</para>
    /// <para>Call this only if it's explicitly documented as allowed. Calling this on</para>
    /// <para>mpv memory not owned by the caller will lead to undefined behavior.</para>
    /// </summary>
    /// <param name="data">A valid pointer returned by the API, or NULL.</param>
    [LibraryImport(LibraryName, EntryPoint = "mpv_free")]
    public static partial void MPVFree(ref MPVNode data);

    /// <summary>
    /// <para>General function to deallocate memory returned by some of the API functions.</para>
    /// <para>Call this only if it's explicitly documented as allowed. Calling this on</para>
    /// <para>mpv memory not owned by the caller will lead to undefined behavior.</para>
    /// </summary>
    /// <param name="data">A valid pointer returned by the API, or NULL.</param>
    [LibraryImport(LibraryName, EntryPoint = "mpv_free")]
    public static partial void MPVFree(ref MPVNodeList data);

    /// <summary>
    /// <para>General function to deallocate memory returned by some of the API functions.</para>
    /// <para>Call this only if it's explicitly documented as allowed. Calling this on</para>
    /// <para>mpv memory not owned by the caller will lead to undefined behavior.</para>
    /// </summary>
    /// <param name="data">A valid pointer returned by the API, or NULL.</param>
    [LibraryImport(LibraryName, EntryPoint = "mpv_free")]
    public static partial void MPVFree(ref MPVByteArray data);

    #endregion

    /// <summary>
    /// <para>Return the name of this client handle. Every client has its own unique</para>
    /// <para>name, which is mostly used for user interface purposes.</para>
    /// </summary>
    /// <returns>
    /// <para>The client name. The string is read-only and is valid until the</para>
    /// <para>mpv_handle is destroyed.</para>
    /// </returns>
    [LibraryImport(LibraryName, EntryPoint = "mpv_client_name", StringMarshalling = StringMarshalling.Custom,
        StringMarshallingCustomType = typeof(MPVStringMarshaller))]
    public static partial string MPVClientName(MPVHandle ctx);

    /// <summary>
    /// <para>Return the ID of this client handle. Every client has its own unique ID. This</para>
    /// <para>ID is never reused by the core, even if the mpv_handle at hand gets destroyed</para>
    /// <para>and new handles get allocated.</para>
    /// </summary>
    /// <returns>The client ID.</returns>
    /// <remarks>
    /// <para>IDs are never 0 or negative.</para>
    /// <para>Some mpv APIs (not necessarily all) accept a name in the form &quot;&lt;&gt;id&gt;&quot; in</para>
    /// <para>addition of the proper mpv_client_name(), where &quot;&quot; is the ID in decimal</para>
    /// <para>form (e.g. &quot;@123&quot;). For example, the &quot;script-message-to&quot; command takes the</para>
    /// <para>client name as first argument, but also accepts the client ID formatted in</para>
    /// <para>this manner.</para>
    /// </remarks>
    [LibraryImport(LibraryName, EntryPoint = "mpv_client_id")]
    public static partial long MPVClientId(MPVHandle ctx);

    /// <summary>
    /// <para>Create a new mpv instance and an associated client API handle to control</para>
    /// <para>the mpv instance. This instance is in a pre-initialized state,</para>
    /// <para>and needs to be initialized to be actually used with most other API</para>
    /// <para>functions.</para>
    /// </summary>
    /// <returns>
    /// <para>a new mpv client API handle. Returns NULL on error. Currently, this</para>
    /// <para>can happen in the following situations:</para>
    /// <para>- out of memory</para>
    /// <para>- LC_NUMERIC is not set to &quot;C&quot; (see general remarks)</para>
    /// </returns>
    /// <remarks>
    /// <para>Some API functions will return MPV_ERROR_UNINITIALIZED in the uninitialized</para>
    /// <para>state. You can call mpv_set_property() (or mpv_set_property_string() and</para>
    /// <para>other variants, and before mpv 0.21.0 mpv_set_option() etc.) to set initial</para>
    /// <para>options. After this, call mpv_initialize() to start the player, and then use</para>
    /// <para>e.g. mpv_command() to start playback of a file.</para>
    /// <para>The point of separating handle creation and actual initialization is that</para>
    /// <para>you can configure things which can't be changed during runtime.</para>
    /// <para>Unlike the command line player, this will have initial settings suitable</para>
    /// <para>for embedding in applications. The following settings are different:</para>
    /// <para>- stdin/stdout/stderr and the terminal will never be accessed. This is</para>
    /// <para>equivalent to setting the --no-terminal option.</para>
    /// <para>(Technically, this also suppresses C signal handling.)</para>
    /// <para>- No config files will be loaded. This is roughly equivalent to using</para>
    /// <para>--config=no. Since libmpv 1.15, you can actually re-enable this option,</para>
    /// <para>which will make libmpv load config files during mpv_initialize(). If you</para>
    /// <para>do this, you are strongly encouraged to set the &quot;config-dir&quot; option too.</para>
    /// <para>(Otherwise it will load the mpv command line player's config.)</para>
    /// <para>For example:</para>
    /// <para>mpv_set_option_string(mpv, &quot;config-dir&quot;, &quot;/my/path&quot;); // set config root</para>
    /// <para>mpv_set_option_string(mpv, &quot;config&quot;, &quot;yes&quot;); // enable config loading</para>
    /// <para>(call mpv_initialize() _after_ this)</para>
    /// <para>- Idle mode is enabled, which means the playback core will enter idle mode</para>
    /// <para>if there are no more files to play on the internal playlist, instead of</para>
    /// <para>exiting. This is equivalent to the --idle option.</para>
    /// <para>- Disable parts of input handling.</para>
    /// <para>- Most of the different settings can be viewed with the command line player</para>
    /// <para>by running &quot;mpv --show-profile=libmpv&quot;.</para>
    /// <para>All this assumes that API users want a mpv instance that is strictly</para>
    /// <para>isolated from the command line player's configuration, user settings, and</para>
    /// <para>so on. You can re-enable disabled features by setting the appropriate</para>
    /// <para>options.</para>
    /// <para>The mpv command line parser is not available through this API, but you can</para>
    /// <para>set individual options with mpv_set_property(). Files for playback must be</para>
    /// <para>loaded with mpv_command() or others.</para>
    /// <para>Note that you should avoid doing concurrent accesses on the uninitialized</para>
    /// <para>client handle. (Whether concurrent access is definitely allowed or not has</para>
    /// <para>yet to be decided.)</para>
    /// </remarks>
    [LibraryImport(LibraryName, EntryPoint = "mpv_create")]
    public static partial MPVHandle MPVCreate();

    /// <summary>
    /// <para>Initialize an uninitialized mpv instance. If the mpv instance is already</para>
    /// <para>running, an error is returned.</para>
    /// </summary>
    /// <param name="ctx">The mpv handle.</param>
    /// <returns>error code</returns>
    /// <remarks>
    /// <para>This function needs to be called to make full use of the client API if the</para>
    /// <para>client API handle was created with mpv_create().</para>
    /// <para>Only the following options are required to be set _before_ mpv_initialize():</para>
    /// <para>- options which are only read at initialization time:</para>
    /// <para>- config</para>
    /// <para>- config-dir</para>
    /// <para>- input-conf</para>
    /// <para>- load-scripts</para>
    /// <para>- script</para>
    /// <para>- player-operation-mode</para>
    /// <para>- input-app-events (OSX)</para>
    /// <para>- all encoding mode options</para>
    /// </remarks>
    [LibraryImport(LibraryName, EntryPoint = "mpv_initialize")]
    public static partial MPVError MPVInitialize(MPVHandle ctx);

    /// <summary>
    /// <para>Disconnect and destroy the mpv_handle. ctx will be deallocated with this</para>
    /// <para>API call.</para>
    /// </summary>
    /// <remarks>
    /// <para>If the last mpv_handle is detached, the core player is destroyed. In</para>
    /// <para>addition, if there are only weak mpv_handles (such as created by</para>
    /// <para>mpv_create_weak_client() or internal scripts), these mpv_handles will</para>
    /// <para>be sent MPV_EVENT_SHUTDOWN. This function may block until these clients</para>
    /// <para>have responded to the shutdown event, and the core is finally destroyed.</para>
    /// </remarks>
    [LibraryImport(LibraryName, EntryPoint = "mpv_destroy")]
    public static partial void MPVDestroy(MPVHandle ctx);

    /// <summary>
    /// <para>Similar to mpv_destroy(), but brings the player and all clients down</para>
    /// <para>as well, and waits until all of them are destroyed. This function blocks. The</para>
    /// <para>advantage over mpv_destroy() is that while mpv_destroy() merely</para>
    /// <para>detaches the client handle from the player, this function quits the player,</para>
    /// <para>waits until all other clients are destroyed (i.e. all mpv_handles are</para>
    /// <para>detached), and also waits for the final termination of the player.</para>
    /// </summary>
    /// <remarks>
    /// <para>Since mpv_destroy() is called somewhere on the way, it's not safe to</para>
    /// <para>call other functions concurrently on the same context.</para>
    /// <para>Since mpv client API version 1.29:</para>
    /// <para>The first call on any mpv_handle will block until the core is destroyed.</para>
    /// <para>This means it will wait until other mpv_handle have been destroyed. If you</para>
    /// <para>want asynchronous destruction, just run the &quot;quit&quot; command, and then react</para>
    /// <para>to the MPV_EVENT_SHUTDOWN event.</para>
    /// <para>If another mpv_handle already called mpv_terminate_destroy(), this call will</para>
    /// <para>not actually block. It will destroy the mpv_handle, and exit immediately,</para>
    /// <para>while other mpv_handles might still be uninitializing.</para>
    /// <para>Before mpv client API version 1.29:</para>
    /// <para>If this is called on a mpv_handle that was not created with mpv_create(),</para>
    /// <para>this function will merely send a quit command and then call</para>
    /// <para>mpv_destroy(), without waiting for the actual shutdown.</para>
    /// </remarks>
    [LibraryImport(LibraryName, EntryPoint = "mpv_terminate_destroy")]
    public static partial void MPVTerminateDestroy(MPVHandle ctx);

    /// <summary>
    /// <para>Create a new client handle connected to the same player core as ctx. This</para>
    /// <para>context has its own event queue, its own mpv_request_event() state, its own</para>
    /// <para>mpv_request_log_messages() state, its own set of observed properties, and</para>
    /// <para>its own state for asynchronous operations. Otherwise, everything is shared.</para>
    /// </summary>
    /// <param name="ctx">
    /// <para>Used to get the reference to the mpv core; handle-specific</para>
    /// <para>settings and parameters are not used.</para>
    /// <para>If NULL, this function behaves like mpv_create() (ignores name).</para>
    /// </param>
    /// <param name="name">
    /// <para>The client name. This will be returned by mpv_client_name(). If</para>
    /// <para>the name is already in use, or contains non-alphanumeric</para>
    /// <para>characters (other than '_'), the name is modified to fit.</para>
    /// <para>If NULL, an arbitrary name is automatically chosen.</para>
    /// </param>
    /// <returns>a new handle, or NULL on error</returns>
    /// <remarks>
    /// <para>This handle should be destroyed with mpv_destroy() if no longer</para>
    /// <para>needed. The core will live as long as there is at least 1 handle referencing</para>
    /// <para>it. Any handle can make the core quit, which will result in every handle</para>
    /// <para>receiving MPV_EVENT_SHUTDOWN.</para>
    /// <para>This function can not be called before the main handle was initialized with</para>
    /// <para>mpv_initialize(). The new handle is always initialized, unless ctx=NULL was</para>
    /// <para>passed.</para>
    /// </remarks>
    [LibraryImport(LibraryName, EntryPoint = "mpv_create_client", StringMarshalling = StringMarshalling.Utf8)]
    public static partial MPVHandle MPVCreateClient(MPVHandle ctx, string name);

    /// <summary>
    /// <para>This is the same as mpv_create_client(), but the created mpv_handle is</para>
    /// <para>treated as a weak reference. If all mpv_handles referencing a core are</para>
    /// <para>weak references, the core is automatically destroyed. (This still goes</para>
    /// <para>through normal uninit of course. Effectively, if the last non-weak mpv_handle</para>
    /// <para>is destroyed, then the weak mpv_handles receive MPV_EVENT_SHUTDOWN and are</para>
    /// <para>asked to terminate as well.)</para>
    /// </summary>
    /// <remarks>
    /// <para>Note if you want to use this like refcounting: you have to be aware that</para>
    /// <para>mpv_terminate_destroy() _and_ mpv_destroy() for the last non-weak</para>
    /// <para>mpv_handle will block until all weak mpv_handles are destroyed.</para>
    /// </remarks>
    [LibraryImport(LibraryName, EntryPoint = "mpv_create_weak_client", StringMarshalling = StringMarshalling.Utf8)]
    public static partial MPVHandle MPVCreateWeakClient(MPVHandle ctx, string name);

    /// <summary>
    /// <para>Load a config file. This loads and parses the file, and sets every entry in</para>
    /// <para>the config file's default section as if mpv_set_option_string() is called.</para>
    /// </summary>
    /// <param name="ctx">the mpv handle</param>
    /// <param name="filename">absolute path to the config file on the local filesystem</param>
    /// <returns>error code</returns>
    /// <remarks>
    /// <para>The filename should be an absolute path. If it isn't, the actual path used</para>
    /// <para>is unspecified. (Note: an absolute path starts with '/' on UNIX.) If the</para>
    /// <para>file wasn't found, MPV_ERROR_INVALID_PARAMETER is returned.</para>
    /// <para>If a fatal error happens when parsing a config file, MPV_ERROR_OPTION_ERROR</para>
    /// <para>is returned. Errors when setting options as well as other types or errors</para>
    /// <para>are ignored (even if options do not exist). You can still try to capture</para>
    /// <para>the resulting error messages with mpv_request_log_messages(). Note that it's</para>
    /// <para>possible that some options were successfully set even if any of these errors</para>
    /// <para>happen.</para>
    /// </remarks>
    [LibraryImport(LibraryName, EntryPoint = "mpv_load_config_file", StringMarshalling = StringMarshalling.Utf8)]
    public static partial MPVError MPVLoadConfigFile(MPVHandle ctx, string filename);

    /// <summary>
    /// <para>Return the internal time in microseconds. This has an arbitrary start offset,</para>
    /// <para>but will never wrap or go backwards.</para>
    /// </summary>
    /// <remarks>
    /// <para>Note that this is always the real time, and doesn't necessarily have to do</para>
    /// <para>with playback time. For example, playback could go faster or slower due to</para>
    /// <para>playback speed, or due to playback being paused. Use the &quot;time-pos&quot; property</para>
    /// <para>instead to get the playback status.</para>
    /// <para>Unlike other libmpv APIs, this can be called at absolutely any time (even</para>
    /// <para>within wakeup callbacks), as long as the context is valid.</para>
    /// <para>Safe to be called from mpv render API threads.</para>
    /// </remarks>
    [LibraryImport(LibraryName, EntryPoint = "mpv_get_time_us")]
    public static partial long MPVGetTimeUs(MPVHandle ctx);

    /// <summary>
    /// <para>Frees any data referenced by the node. It doesn't free the node itself.</para>
    /// <para>Call this only if the mpv client API set the node. If you constructed the</para>
    /// <para>node yourself (manually), you have to free it yourself.</para>
    /// </summary>
    /// <remarks>
    /// <para>If node-&gt;format is MPV_FORMAT_NONE, this call does nothing. Likewise, if</para>
    /// <para>the client API sets a node with this format, this function doesn't need to</para>
    /// <para>be called. (This is just a clarification that there's no danger of anything</para>
    /// <para>strange happening in these cases.)</para>
    /// </remarks>
    [LibraryImport(LibraryName, EntryPoint = "mpv_free_node_contents", StringMarshalling = StringMarshalling.Utf8)]
    public static partial void MPVFreeNodeContents(ref MPVNode baseNode); //Maybe ref?

    #region MPVSetOption

    /// <summary>
    /// <para>Set an option. Note that you can't normally set options during runtime. It</para>
    /// <para>works in uninitialized state (see mpv_create()), and in some cases in at</para>
    /// <para>runtime.</para>
    /// </summary>
    /// <param name="ctx">MPV client.</param>
    /// <param name="name">
    /// <para>Option name. This is the same as on the mpv command line, but</para>
    /// <para>without the leading &quot;--&quot;.</para>
    /// </param>
    /// <param name="format">see enum mpv_format.</param>
    /// <param name="data">Option value (according to the format).</param>
    /// <returns>error code</returns>
    /// <remarks>
    /// <para>Using a format other than MPV_FORMAT_NODE is equivalent to constructing a</para>
    /// <para>mpv_node with the given format and data, and passing the mpv_node to this</para>
    /// <para>function.</para>
    /// <para>Note: this is semi-deprecated. For most purposes, this is not needed anymore.</para>
    /// <para>Starting with mpv version 0.21.0 (version 1.23) most options can be set</para>
    /// <para>with mpv_set_property() (and related functions), and even before</para>
    /// <para>mpv_initialize(). In some obscure corner cases, using this function</para>
    /// <para>to set options might still be required (see</para>
    /// <para>&quot;Inconsistencies between options and properties&quot; in the manpage). Once</para>
    /// <para>these are resolved, the option setting functions might be fully</para>
    /// <para>deprecated.</para>
    /// </remarks>
    [LibraryImport(LibraryName, EntryPoint = "mpv_set_option", StringMarshalling = StringMarshalling.Utf8)]
    public static partial MPVError MPVSetOption(MPVHandle ctx, string name, MPVFormat format,
        [MarshalAs(UnmanagedType.I4)] ref bool data);

    /// <summary>
    /// <para>Set an option. Note that you can't normally set options during runtime. It</para>
    /// <para>works in uninitialized state (see mpv_create()), and in some cases in at</para>
    /// <para>runtime.</para>
    /// </summary>
    /// <param name="ctx">MPV client.</param>
    /// <param name="name">
    /// <para>Option name. This is the same as on the mpv command line, but</para>
    /// <para>without the leading &quot;--&quot;.</para>
    /// </param>
    /// <param name="format">see enum mpv_format.</param>
    /// <param name="data">Option value (according to the format).</param>
    /// <returns>error code</returns>
    /// <remarks>
    /// <para>Using a format other than MPV_FORMAT_NODE is equivalent to constructing a</para>
    /// <para>mpv_node with the given format and data, and passing the mpv_node to this</para>
    /// <para>function.</para>
    /// <para>Note: this is semi-deprecated. For most purposes, this is not needed anymore.</para>
    /// <para>Starting with mpv version 0.21.0 (version 1.23) most options can be set</para>
    /// <para>with mpv_set_property() (and related functions), and even before</para>
    /// <para>mpv_initialize(). In some obscure corner cases, using this function</para>
    /// <para>to set options might still be required (see</para>
    /// <para>&quot;Inconsistencies between options and properties&quot; in the manpage). Once</para>
    /// <para>these are resolved, the option setting functions might be fully</para>
    /// <para>deprecated.</para>
    /// </remarks>
    [LibraryImport(LibraryName, EntryPoint = "mpv_set_option", StringMarshalling = StringMarshalling.Utf8)]
    public static partial MPVError MPVSetOption(MPVHandle ctx, string name, MPVFormat format, ref long data);

    /// <summary>
    /// <para>Set an option. Note that you can't normally set options during runtime. It</para>
    /// <para>works in uninitialized state (see mpv_create()), and in some cases in at</para>
    /// <para>runtime.</para>
    /// </summary>
    /// <param name="ctx">MPV client.</param>
    /// <param name="name">
    /// <para>Option name. This is the same as on the mpv command line, but</para>
    /// <para>without the leading &quot;--&quot;.</para>
    /// </param>
    /// <param name="format">see enum mpv_format.</param>
    /// <param name="data">Option value (according to the format).</param>
    /// <returns>error code</returns>
    /// <remarks>
    /// <para>Using a format other than MPV_FORMAT_NODE is equivalent to constructing a</para>
    /// <para>mpv_node with the given format and data, and passing the mpv_node to this</para>
    /// <para>function.</para>
    /// <para>Note: this is semi-deprecated. For most purposes, this is not needed anymore.</para>
    /// <para>Starting with mpv version 0.21.0 (version 1.23) most options can be set</para>
    /// <para>with mpv_set_property() (and related functions), and even before</para>
    /// <para>mpv_initialize(). In some obscure corner cases, using this function</para>
    /// <para>to set options might still be required (see</para>
    /// <para>&quot;Inconsistencies between options and properties&quot; in the manpage). Once</para>
    /// <para>these are resolved, the option setting functions might be fully</para>
    /// <para>deprecated.</para>
    /// </remarks>
    [LibraryImport(LibraryName, EntryPoint = "mpv_set_option", StringMarshalling = StringMarshalling.Utf8)]
    public static partial MPVError MPVSetOption(MPVHandle ctx, string name, MPVFormat format, ref double data);

    /// <summary>
    /// <para>Set an option. Note that you can't normally set options during runtime. It</para>
    /// <para>works in uninitialized state (see mpv_create()), and in some cases in at</para>
    /// <para>runtime.</para>
    /// </summary>
    /// <param name="ctx">MPV client.</param>
    /// <param name="name">
    /// <para>Option name. This is the same as on the mpv command line, but</para>
    /// <para>without the leading &quot;--&quot;.</para>
    /// </param>
    /// <param name="format">see enum mpv_format.</param>
    /// <param name="data">Option value (according to the format).</param>
    /// <returns>error code</returns>
    /// <remarks>
    /// <para>Using a format other than MPV_FORMAT_NODE is equivalent to constructing a</para>
    /// <para>mpv_node with the given format and data, and passing the mpv_node to this</para>
    /// <para>function.</para>
    /// <para>Note: this is semi-deprecated. For most purposes, this is not needed anymore.</para>
    /// <para>Starting with mpv version 0.21.0 (version 1.23) most options can be set</para>
    /// <para>with mpv_set_property() (and related functions), and even before</para>
    /// <para>mpv_initialize(). In some obscure corner cases, using this function</para>
    /// <para>to set options might still be required (see</para>
    /// <para>&quot;Inconsistencies between options and properties&quot; in the manpage). Once</para>
    /// <para>these are resolved, the option setting functions might be fully</para>
    /// <para>deprecated.</para>
    /// </remarks>
    [LibraryImport(LibraryName, EntryPoint = "mpv_set_option", StringMarshalling = StringMarshalling.Utf8)]
    public static partial MPVError MPVSetOption(MPVHandle ctx, string name, MPVFormat format, ref string data);

    /// <summary>
    /// <para>Set an option. Note that you can't normally set options during runtime. It</para>
    /// <para>works in uninitialized state (see mpv_create()), and in some cases in at</para>
    /// <para>runtime.</para>
    /// </summary>
    /// <param name="ctx">MPV client.</param>
    /// <param name="name">
    /// <para>Option name. This is the same as on the mpv command line, but</para>
    /// <para>without the leading &quot;--&quot;.</para>
    /// </param>
    /// <param name="format">see enum mpv_format.</param>
    /// <param name="data">Option value (according to the format).</param>
    /// <returns>error code</returns>
    /// <remarks>
    /// <para>Using a format other than MPV_FORMAT_NODE is equivalent to constructing a</para>
    /// <para>mpv_node with the given format and data, and passing the mpv_node to this</para>
    /// <para>function.</para>
    /// <para>Note: this is semi-deprecated. For most purposes, this is not needed anymore.</para>
    /// <para>Starting with mpv version 0.21.0 (version 1.23) most options can be set</para>
    /// <para>with mpv_set_property() (and related functions), and even before</para>
    /// <para>mpv_initialize(). In some obscure corner cases, using this function</para>
    /// <para>to set options might still be required (see</para>
    /// <para>&quot;Inconsistencies between options and properties&quot; in the manpage). Once</para>
    /// <para>these are resolved, the option setting functions might be fully</para>
    /// <para>deprecated.</para>
    /// </remarks>
    [LibraryImport(LibraryName, EntryPoint = "mpv_set_option", StringMarshalling = StringMarshalling.Utf8)]
    public static partial MPVError MPVSetOption(MPVHandle ctx, string name, MPVFormat format, ref MPVNode data);

    #endregion

    /// <summary>
    /// <para>Convenience function to set an option to a string value. This is like</para>
    /// <para>calling mpv_set_option() with MPV_FORMAT_STRING.</para>
    /// </summary>
    /// <returns>error code</returns>
    [LibraryImport(LibraryName, EntryPoint = "mpv_set_option_string", StringMarshalling = StringMarshalling.Utf8)]
    public static partial MPVError MPVSetOptionString(MPVHandle ctx, string name, ref string data);

    /// <summary>
    /// <para>Send a command to the player. Commands are the same as those used in</para>
    /// <para>input.conf, except that this function takes parameters in a pre-split</para>
    /// <para>form.</para>
    /// </summary>
    /// <param name="ctx">The mpv handle.</param>
    /// <param name="args">
    /// <para>NULL-terminated list of strings. Usually, the first item</para>
    /// <para>is the command, and the following items are arguments.</para>
    /// </param>
    /// <returns>error code</returns>
    /// <remarks>
    /// <para>The commands and their parameters are documented in input.rst.</para>
    /// <para>Does not use OSD and string expansion by default (unlike mpv_command_string()</para>
    /// <para>and input.conf).</para>
    /// </remarks>
    [LibraryImport(LibraryName, EntryPoint = "mpv_command", StringMarshalling = StringMarshalling.Utf8)]
    public static partial MPVError MPVCommand(MPVHandle ctx, string?[] args);

    /// <summary>
    /// <para>Same as mpv_command(), but allows passing structured data in any format.</para>
    /// <para>In particular, calling mpv_command() is exactly like calling</para>
    /// <para>mpv_command_node() with the format set to MPV_FORMAT_NODE_ARRAY, and</para>
    /// <para>every arg passed in order as MPV_FORMAT_STRING.</para>
    /// </summary>
    /// <param name="ctx">The mpv handle.</param>
    /// <param name="args">
    /// <para>mpv_node with format set to one of the values documented</para>
    /// <para>above (see there for details)</para>
    /// </param>
    /// <param name="result">
    /// <para>Optional, pass NULL if unused. If not NULL, and if the</para>
    /// <para>function succeeds, this is set to command-specific return</para>
    /// <para>data. You must call mpv_free_node_contents() to free it</para>
    /// <para>(again, only if the command actually succeeds).</para>
    /// <para>Not many commands actually use this at all.</para>
    /// </param>
    /// <returns>error code (the result parameter is not set on error)</returns>
    /// <remarks>
    /// <para>Does not use OSD and string expansion by default.</para>
    /// <para>The args argument can have one of the following formats:</para>
    /// <para>MPV_FORMAT_NODE_ARRAY:</para>
    /// <para>Positional arguments. Each entry is an argument using an arbitrary</para>
    /// <para>format (the format must be compatible to the used command). Usually,</para>
    /// <para>the first item is the command name (as MPV_FORMAT_STRING). The order</para>
    /// <para>of arguments is as documented in each command description.</para>
    /// <para>MPV_FORMAT_NODE_MAP:</para>
    /// <para>Named arguments. This requires at least an entry with the key &quot;name&quot;</para>
    /// <para>to be present, which must be a string, and contains the command name.</para>
    /// <para>The special entry &quot;_flags&quot; is optional, and if present, must be an</para>
    /// <para>array of strings, each being a command prefix to apply. All other</para>
    /// <para>entries are interpreted as arguments. They must use the argument names</para>
    /// <para>as documented in each command description. Some commands do not</para>
    /// <para>support named arguments at all, and must use MPV_FORMAT_NODE_ARRAY.</para>
    /// </remarks>
    [LibraryImport(LibraryName, EntryPoint = "mpv_command_node", StringMarshalling = StringMarshalling.Utf8)]
    public static partial MPVError MPVCommandNode(MPVHandle ctx, ref MPVNode args, out MPVNode result);

    /// <summary>This is essentially identical to mpv_command() but it also returns a result.</summary>
    /// <param name="ctx">The mpv handle.</param>
    /// <param name="args">
    /// <para>NULL-terminated list of strings. Usually, the first item</para>
    /// <para>is the command, and the following items are arguments.</para>
    /// </param>
    /// <param name="result">
    /// <para>Optional, pass NULL if unused. If not NULL, and if the</para>
    /// <para>function succeeds, this is set to command-specific return</para>
    /// <para>data. You must call mpv_free_node_contents() to free it</para>
    /// <para>(again, only if the command actually succeeds).</para>
    /// <para>Not many commands actually use this at all.</para>
    /// </param>
    /// <returns>error code (the result parameter is not set on error)</returns>
    /// <remarks>Does not use OSD and string expansion by default.</remarks>
    [LibraryImport(LibraryName, EntryPoint = "mpv_command_ret", StringMarshalling = StringMarshalling.Utf8)]
    public static partial MPVError MPVCommandRet(MPVHandle ctx, string?[] args, out MPVNode result);

    /// <summary>
    /// <para>Same as mpv_command, but use input.conf parsing for splitting arguments.</para>
    /// <para>This is slightly simpler, but also more error prone, since arguments may</para>
    /// <para>need quoting/escaping.</para>
    /// </summary>
    /// <remarks>This also has OSD and string expansion enabled by default.</remarks>
    [LibraryImport(LibraryName, EntryPoint = "mpv_command_string", StringMarshalling = StringMarshalling.Utf8)]
    public static partial MPVError MPVCommandString(MPVHandle ctx, string? args);

    /// <summary>Same as mpv_command, but run the command asynchronously.</summary>
    /// <param name="ctx">The mpv handle.</param>
    /// <param name="replyUserdata">
    /// <para>the value mpv_event.reply_userdata of the reply will</para>
    /// <para>be set to (see section about asynchronous calls)</para>
    /// </param>
    /// <param name="args">NULL-terminated list of strings (see mpv_command())</param>
    /// <returns>error code (if parsing or queuing the command fails)</returns>
    /// <remarks>
    /// <para>Commands are executed asynchronously. You will receive a</para>
    /// <para>MPV_EVENT_COMMAND_REPLY event. This event will also have an</para>
    /// <para>error code set if running the command failed. For commands that</para>
    /// <para>return data, the data is put into mpv_event_command.result.</para>
    /// <para>The only case when you do not receive an event is when the function call</para>
    /// <para>itself fails. This happens only if parsing the command itself (or otherwise</para>
    /// <para>validating it) fails, i.e. the return code of the API call is not 0 or</para>
    /// <para>positive.</para>
    /// <para>Safe to be called from mpv render API threads.</para>
    /// </remarks>
    [LibraryImport(LibraryName, EntryPoint = "mpv_command_async", StringMarshalling = StringMarshalling.Utf8)]
    public static partial MPVError MPVCommandAsync(MPVHandle ctx, ulong replyUserdata, string?[] args);

    /// <summary>
    /// <para>Same as mpv_command_node(), but run it asynchronously. Basically, this</para>
    /// <para>function is to mpv_command_node() what mpv_command_async() is to</para>
    /// <para>mpv_command().</para>
    /// </summary>
    /// <param name="ctx">The mpv handle.</param>
    /// <param name="replyUserdata">
    /// <para>the value mpv_event.reply_userdata of the reply will</para>
    /// <para>be set to (see section about asynchronous calls)</para>
    /// </param>
    /// <param name="args">as in mpv_command_node()</param>
    /// <returns>error code (if parsing or queuing the command fails)</returns>
    /// <remarks>
    /// <para>See mpv_command_async() for details.</para>
    /// <para>Safe to be called from mpv render API threads.</para>
    /// </remarks>
    [LibraryImport(LibraryName, EntryPoint = "mpv_command_node_async", StringMarshalling = StringMarshalling.Utf8)]
    public static partial MPVError MPVCommandNodeAsync(MPVHandle ctx, ulong replyUserdata, ref MPVNode args);

    /// <summary>
    /// <para>Signal to all async requests with the matching ID to abort. This affects</para>
    /// <para>the following API calls:</para>
    /// </summary>
    /// <param name="ctx">The mpv handle.</param>
    /// <param name="replyUserdata">ID of the request to be aborted (see above)</param>
    /// <remarks>
    /// <para>mpv_command_async</para>
    /// <para>mpv_command_node_async</para>
    /// <para>All of these functions take a reply_userdata parameter. This API function</para>
    /// <para>tells all requests with the matching reply_userdata value to try to return</para>
    /// <para>as soon as possible. If there are multiple requests with matching ID, it</para>
    /// <para>aborts all of them.</para>
    /// <para>This API function is mostly asynchronous itself. It will not wait until the</para>
    /// <para>command is aborted. Instead, the command will terminate as usual, but with</para>
    /// <para>some work not done. How this is signaled depends on the specific command (for</para>
    /// <para>example, the &quot;subprocess&quot; command will indicate it by &quot;killed_by_us&quot; set to</para>
    /// <para>true in the result). How long it takes also depends on the situation. The</para>
    /// <para>aborting process is completely asynchronous.</para>
    /// <para>Not all commands may support this functionality. In this case, this function</para>
    /// <para>will have no effect. The same is true if the request using the passed</para>
    /// <para>reply_userdata has already terminated, has not been started yet, or was</para>
    /// <para>never in use at all.</para>
    /// <para>You have to be careful of race conditions: the time during which the abort</para>
    /// <para>request will be effective is _after_ e.g. mpv_command_async() has returned,</para>
    /// <para>and before the command has signaled completion with MPV_EVENT_COMMAND_REPLY.</para>
    /// </remarks>
    [LibraryImport(LibraryName, EntryPoint = "mpv_abort_async_command", StringMarshalling = StringMarshalling.Utf8)]
    public static partial MPVError MPVAbortAsyncCommand(MPVHandle ctx, ulong replyUserdata);

    #region Set Property

    /// <summary>
    /// <para>Set a property to a given value. Properties are essentially variables which</para>
    /// <para>can be queried or set at runtime. For example, writing to the pause property</para>
    /// <para>will actually pause or unpause playback.</para>
    /// </summary>
    /// <param name="ctx">The mpv handle.</param>
    /// <param name="name">The property name. See input.rst for a list of properties.</param>
    /// <param name="format">see enum mpv_format.</param>
    /// <param name="data">Option value.</param>
    /// <returns>error code</returns>
    /// <remarks>
    /// <para>If the format doesn't match with the internal format of the property, access</para>
    /// <para>usually will fail with MPV_ERROR_PROPERTY_FORMAT. In some cases, the data</para>
    /// <para>is automatically converted and access succeeds. For example, MPV_FORMAT_INT64</para>
    /// <para>is always converted to MPV_FORMAT_DOUBLE, and access using MPV_FORMAT_STRING</para>
    /// <para>usually invokes a string parser. The same happens when calling this function</para>
    /// <para>with MPV_FORMAT_NODE: the underlying format may be converted to another</para>
    /// <para>type if possible.</para>
    /// <para>Using a format other than MPV_FORMAT_NODE is equivalent to constructing a</para>
    /// <para>mpv_node with the given format and data, and passing the mpv_node to this</para>
    /// <para>function. (Before API version 1.21, this was different.)</para>
    /// <para>Note: starting with mpv 0.21.0 (client API version 1.23), this can be used to</para>
    /// <para>set options in general. It even can be used before mpv_initialize()</para>
    /// <para>has been called. If called before mpv_initialize(), setting properties</para>
    /// <para>not backed by options will result in MPV_ERROR_PROPERTY_UNAVAILABLE.</para>
    /// <para>In some cases, properties and options still conflict. In these cases,</para>
    /// <para>mpv_set_property() accesses the options before mpv_initialize(), and</para>
    /// <para>the properties after mpv_initialize(). These conflicts will be removed</para>
    /// <para>in mpv 0.23.0. See mpv_set_option() for further remarks.</para>
    /// </remarks>
    [LibraryImport(LibraryName, EntryPoint = "mpv_set_property", StringMarshalling = StringMarshalling.Utf8)]
    public static partial MPVError MPVSetProperty(MPVHandle ctx, string name, MPVFormat format,
        [MarshalAs(UnmanagedType.I4)] ref bool data);

    /// <summary>
    /// <para>Set a property to a given value. Properties are essentially variables which</para>
    /// <para>can be queried or set at runtime. For example, writing to the pause property</para>
    /// <para>will actually pause or unpause playback.</para>
    /// </summary>
    /// <param name="ctx">The mpv handle.</param>
    /// <param name="name">The property name. See input.rst for a list of properties.</param>
    /// <param name="format">see enum mpv_format.</param>
    /// <param name="data">Option value.</param>
    /// <returns>error code</returns>
    /// <remarks>
    /// <para>If the format doesn't match with the internal format of the property, access</para>
    /// <para>usually will fail with MPV_ERROR_PROPERTY_FORMAT. In some cases, the data</para>
    /// <para>is automatically converted and access succeeds. For example, MPV_FORMAT_INT64</para>
    /// <para>is always converted to MPV_FORMAT_DOUBLE, and access using MPV_FORMAT_STRING</para>
    /// <para>usually invokes a string parser. The same happens when calling this function</para>
    /// <para>with MPV_FORMAT_NODE: the underlying format may be converted to another</para>
    /// <para>type if possible.</para>
    /// <para>Using a format other than MPV_FORMAT_NODE is equivalent to constructing a</para>
    /// <para>mpv_node with the given format and data, and passing the mpv_node to this</para>
    /// <para>function. (Before API version 1.21, this was different.)</para>
    /// <para>Note: starting with mpv 0.21.0 (client API version 1.23), this can be used to</para>
    /// <para>set options in general. It even can be used before mpv_initialize()</para>
    /// <para>has been called. If called before mpv_initialize(), setting properties</para>
    /// <para>not backed by options will result in MPV_ERROR_PROPERTY_UNAVAILABLE.</para>
    /// <para>In some cases, properties and options still conflict. In these cases,</para>
    /// <para>mpv_set_property() accesses the options before mpv_initialize(), and</para>
    /// <para>the properties after mpv_initialize(). These conflicts will be removed</para>
    /// <para>in mpv 0.23.0. See mpv_set_option() for further remarks.</para>
    /// </remarks>
    [LibraryImport(LibraryName, EntryPoint = "mpv_set_property", StringMarshalling = StringMarshalling.Utf8)]
    public static partial MPVError MPVSetProperty(MPVHandle ctx, string name, MPVFormat format, ref long data);

    /// <summary>
    /// <para>Set a property to a given value. Properties are essentially variables which</para>
    /// <para>can be queried or set at runtime. For example, writing to the pause property</para>
    /// <para>will actually pause or unpause playback.</para>
    /// </summary>
    /// <param name="ctx">The mpv handle.</param>
    /// <param name="name">The property name. See input.rst for a list of properties.</param>
    /// <param name="format">see enum mpv_format.</param>
    /// <param name="data">Option value.</param>
    /// <returns>error code</returns>
    /// <remarks>
    /// <para>If the format doesn't match with the internal format of the property, access</para>
    /// <para>usually will fail with MPV_ERROR_PROPERTY_FORMAT. In some cases, the data</para>
    /// <para>is automatically converted and access succeeds. For example, MPV_FORMAT_INT64</para>
    /// <para>is always converted to MPV_FORMAT_DOUBLE, and access using MPV_FORMAT_STRING</para>
    /// <para>usually invokes a string parser. The same happens when calling this function</para>
    /// <para>with MPV_FORMAT_NODE: the underlying format may be converted to another</para>
    /// <para>type if possible.</para>
    /// <para>Using a format other than MPV_FORMAT_NODE is equivalent to constructing a</para>
    /// <para>mpv_node with the given format and data, and passing the mpv_node to this</para>
    /// <para>function. (Before API version 1.21, this was different.)</para>
    /// <para>Note: starting with mpv 0.21.0 (client API version 1.23), this can be used to</para>
    /// <para>set options in general. It even can be used before mpv_initialize()</para>
    /// <para>has been called. If called before mpv_initialize(), setting properties</para>
    /// <para>not backed by options will result in MPV_ERROR_PROPERTY_UNAVAILABLE.</para>
    /// <para>In some cases, properties and options still conflict. In these cases,</para>
    /// <para>mpv_set_property() accesses the options before mpv_initialize(), and</para>
    /// <para>the properties after mpv_initialize(). These conflicts will be removed</para>
    /// <para>in mpv 0.23.0. See mpv_set_option() for further remarks.</para>
    /// </remarks>
    [LibraryImport(LibraryName, EntryPoint = "mpv_set_property", StringMarshalling = StringMarshalling.Utf8)]
    public static partial MPVError MPVSetProperty(MPVHandle ctx, string name, MPVFormat format, ref double data);

    /// <summary>
    /// <para>Set a property to a given value. Properties are essentially variables which</para>
    /// <para>can be queried or set at runtime. For example, writing to the pause property</para>
    /// <para>will actually pause or unpause playback.</para>
    /// </summary>
    /// <param name="ctx">The mpv handle.</param>
    /// <param name="name">The property name. See input.rst for a list of properties.</param>
    /// <param name="format">see enum mpv_format.</param>
    /// <param name="data">Option value.</param>
    /// <returns>error code</returns>
    /// <remarks>
    /// <para>If the format doesn't match with the internal format of the property, access</para>
    /// <para>usually will fail with MPV_ERROR_PROPERTY_FORMAT. In some cases, the data</para>
    /// <para>is automatically converted and access succeeds. For example, MPV_FORMAT_INT64</para>
    /// <para>is always converted to MPV_FORMAT_DOUBLE, and access using MPV_FORMAT_STRING</para>
    /// <para>usually invokes a string parser. The same happens when calling this function</para>
    /// <para>with MPV_FORMAT_NODE: the underlying format may be converted to another</para>
    /// <para>type if possible.</para>
    /// <para>Using a format other than MPV_FORMAT_NODE is equivalent to constructing a</para>
    /// <para>mpv_node with the given format and data, and passing the mpv_node to this</para>
    /// <para>function. (Before API version 1.21, this was different.)</para>
    /// <para>Note: starting with mpv 0.21.0 (client API version 1.23), this can be used to</para>
    /// <para>set options in general. It even can be used before mpv_initialize()</para>
    /// <para>has been called. If called before mpv_initialize(), setting properties</para>
    /// <para>not backed by options will result in MPV_ERROR_PROPERTY_UNAVAILABLE.</para>
    /// <para>In some cases, properties and options still conflict. In these cases,</para>
    /// <para>mpv_set_property() accesses the options before mpv_initialize(), and</para>
    /// <para>the properties after mpv_initialize(). These conflicts will be removed</para>
    /// <para>in mpv 0.23.0. See mpv_set_option() for further remarks.</para>
    /// </remarks>
    [LibraryImport(LibraryName, EntryPoint = "mpv_set_property", StringMarshalling = StringMarshalling.Utf8)]
    public static partial MPVError MPVSetProperty(MPVHandle ctx, string name, MPVFormat format, ref string data);

    /// <summary>
    /// <para>Set a property to a given value. Properties are essentially variables which</para>
    /// <para>can be queried or set at runtime. For example, writing to the pause property</para>
    /// <para>will actually pause or unpause playback.</para>
    /// </summary>
    /// <param name="ctx">The mpv handle.</param>
    /// <param name="name">The property name. See input.rst for a list of properties.</param>
    /// <param name="format">see enum mpv_format.</param>
    /// <param name="data">Option value.</param>
    /// <returns>error code</returns>
    /// <remarks>
    /// <para>If the format doesn't match with the internal format of the property, access</para>
    /// <para>usually will fail with MPV_ERROR_PROPERTY_FORMAT. In some cases, the data</para>
    /// <para>is automatically converted and access succeeds. For example, MPV_FORMAT_INT64</para>
    /// <para>is always converted to MPV_FORMAT_DOUBLE, and access using MPV_FORMAT_STRING</para>
    /// <para>usually invokes a string parser. The same happens when calling this function</para>
    /// <para>with MPV_FORMAT_NODE: the underlying format may be converted to another</para>
    /// <para>type if possible.</para>
    /// <para>Using a format other than MPV_FORMAT_NODE is equivalent to constructing a</para>
    /// <para>mpv_node with the given format and data, and passing the mpv_node to this</para>
    /// <para>function. (Before API version 1.21, this was different.)</para>
    /// <para>Note: starting with mpv 0.21.0 (client API version 1.23), this can be used to</para>
    /// <para>set options in general. It even can be used before mpv_initialize()</para>
    /// <para>has been called. If called before mpv_initialize(), setting properties</para>
    /// <para>not backed by options will result in MPV_ERROR_PROPERTY_UNAVAILABLE.</para>
    /// <para>In some cases, properties and options still conflict. In these cases,</para>
    /// <para>mpv_set_property() accesses the options before mpv_initialize(), and</para>
    /// <para>the properties after mpv_initialize(). These conflicts will be removed</para>
    /// <para>in mpv 0.23.0. See mpv_set_option() for further remarks.</para>
    /// </remarks>
    [LibraryImport(LibraryName, EntryPoint = "mpv_set_property", StringMarshalling = StringMarshalling.Utf8)]
    public static partial MPVError
        MPVSetProperty(MPVHandle ctx, string name, MPVFormat format, ref MPVNode data);

    #endregion

    /// <summary>Convenience function to set a property to a string value.</summary>
    /// <remarks>This is like calling mpv_set_property() with MPV_FORMAT_STRING.</remarks>
    [LibraryImport(LibraryName, EntryPoint = "mpv_set_property_string", StringMarshalling = StringMarshalling.Utf8)]
    public static partial MPVError MPVSetPropertyString(MPVHandle ctx, string name, ref string data);

    /// <summary>Convenience function to delete a property.</summary>
    /// <param name="ctx">The mpv handle.</param>
    /// <param name="name">The property name. See input.rst for a list of properties.</param>
    /// <returns>error code</returns>
    /// <remarks>This is equivalent to running the command &quot;del [name]&quot;.</remarks>
    [LibraryImport(LibraryName, EntryPoint = "mpv_del_property", StringMarshalling = StringMarshalling.Utf8)]
    public static partial MPVError MPVDelProperty(MPVHandle ctx, string name);

    #region SetPropertyAsync

    /// <summary>
    /// <para>Set a property asynchronously. You will receive the result of the operation</para>
    /// <para>as MPV_EVENT_SET_PROPERTY_REPLY event. The mpv_event.error field will contain</para>
    /// <para>the result status of the operation. Otherwise, this function is similar to</para>
    /// <para>mpv_set_property().</para>
    /// </summary>
    /// <param name="ctx">The MPVHandle.</param>
    /// <param name="replyUserData">see section about asynchronous calls</param>
    /// <param name="name">The property name.</param>
    /// <param name="format">see enum mpv_format.</param>
    /// <param name="data">
    /// <para>Option value. The value will be copied by the function. It</para>
    /// <para>will never be modified by the client API.</para>
    /// </param>
    /// <returns>error code if sending the request failed</returns>
    /// <remarks>Safe to be called from mpv render API threads.</remarks>
    [LibraryImport(LibraryName, EntryPoint = "mpv_set_property_async", StringMarshalling = StringMarshalling.Utf8)]
    public static partial MPVError MPVSetPropertyAsync(MPVHandle ctx, ulong replyUserData, string name,
        MPVFormat format, [MarshalAs(UnmanagedType.I4)] ref bool data);

    /// <summary>
    /// <para>Set a property asynchronously. You will receive the result of the operation</para>
    /// <para>as MPV_EVENT_SET_PROPERTY_REPLY event. The mpv_event.error field will contain</para>
    /// <para>the result status of the operation. Otherwise, this function is similar to</para>
    /// <para>mpv_set_property().</para>
    /// </summary>
    /// <param name="ctx">The MPVHandle.</param>
    /// <param name="replyUserData">see section about asynchronous calls</param>
    /// <param name="name">The property name.</param>
    /// <param name="format">see enum mpv_format.</param>
    /// <param name="data">
    /// <para>Option value. The value will be copied by the function. It</para>
    /// <para>will never be modified by the client API.</para>
    /// </param>
    /// <returns>error code if sending the request failed</returns>
    /// <remarks>Safe to be called from mpv render API threads.</remarks>
    [LibraryImport(LibraryName, EntryPoint = "mpv_set_property_async", StringMarshalling = StringMarshalling.Utf8)]
    public static partial MPVError MPVSetPropertyAsync(MPVHandle ctx, ulong replyUserData, string name,
        MPVFormat format, ref long data);

    /// <summary>
    /// <para>Set a property asynchronously. You will receive the result of the operation</para>
    /// <para>as MPV_EVENT_SET_PROPERTY_REPLY event. The mpv_event.error field will contain</para>
    /// <para>the result status of the operation. Otherwise, this function is similar to</para>
    /// <para>mpv_set_property().</para>
    /// </summary>
    /// <param name="ctx">The MPVHandle.</param>
    /// <param name="replyUserData">see section about asynchronous calls</param>
    /// <param name="name">The property name.</param>
    /// <param name="format">see enum mpv_format.</param>
    /// <param name="data">
    /// <para>Option value. The value will be copied by the function. It</para>
    /// <para>will never be modified by the client API.</para>
    /// </param>
    /// <returns>error code if sending the request failed</returns>
    /// <remarks>Safe to be called from mpv render API threads.</remarks>
    [LibraryImport(LibraryName, EntryPoint = "mpv_set_property_async", StringMarshalling = StringMarshalling.Utf8)]
    public static partial MPVError MPVSetPropertyAsync(MPVHandle ctx, ulong replyUserData, string name,
        MPVFormat format, ref double data);

    /// <summary>
    /// <para>Set a property asynchronously. You will receive the result of the operation</para>
    /// <para>as MPV_EVENT_SET_PROPERTY_REPLY event. The mpv_event.error field will contain</para>
    /// <para>the result status of the operation. Otherwise, this function is similar to</para>
    /// <para>mpv_set_property().</para>
    /// </summary>
    /// <param name="ctx">The MPVHandle.</param>
    /// <param name="replyUserData">see section about asynchronous calls</param>
    /// <param name="name">The property name.</param>
    /// <param name="format">see enum mpv_format.</param>
    /// <param name="data">
    /// <para>Option value. The value will be copied by the function. It</para>
    /// <para>will never be modified by the client API.</para>
    /// </param>
    /// <returns>error code if sending the request failed</returns>
    /// <remarks>Safe to be called from mpv render API threads.</remarks>
    [LibraryImport(LibraryName, EntryPoint = "mpv_set_property_async", StringMarshalling = StringMarshalling.Utf8)]
    public static partial MPVError MPVSetPropertyAsync(MPVHandle ctx, ulong replyUserData, string name,
        MPVFormat format, ref string data);

    /// <summary>
    /// <para>Set a property asynchronously. You will receive the result of the operation</para>
    /// <para>as MPV_EVENT_SET_PROPERTY_REPLY event. The mpv_event.error field will contain</para>
    /// <para>the result status of the operation. Otherwise, this function is similar to</para>
    /// <para>mpv_set_property().</para>
    /// </summary>
    /// <param name="ctx">The MPVHandle.</param>
    /// <param name="replyUserData">see section about asynchronous calls</param>
    /// <param name="name">The property name.</param>
    /// <param name="format">see enum mpv_format.</param>
    /// <param name="data">
    /// <para>Option value. The value will be copied by the function. It</para>
    /// <para>will never be modified by the client API.</para>
    /// </param>
    /// <returns>error code if sending the request failed</returns>
    /// <remarks>Safe to be called from mpv render API threads.</remarks>
    [LibraryImport(LibraryName, EntryPoint = "mpv_set_property_async", StringMarshalling = StringMarshalling.Utf8)]
    public static partial MPVError MPVSetPropertyAsync(MPVHandle ctx, ulong replyUserData, string name,
        MPVFormat format, ref MPVNode data);

    #endregion

    #region GetProperty

    /// <summary>Read the value of the given property.</summary>
    /// <param name="ctx">The mpv handle.</param>
    /// <param name="name">The property name.</param>
    /// <param name="format">see enum mpv_format.</param>
    /// <param name="data">
    /// <para>Pointer to the variable holding the option value. On</para>
    /// <para>success, the variable will be set to a copy of the option</para>
    /// <para>value. For formats that require dynamic memory allocation,</para>
    /// <para>you can free the value with mpv_free() (strings) or</para>
    /// <para>mpv_free_node_contents() (MPV_FORMAT_NODE).</para>
    /// </param>
    /// <returns>error code</returns>
    /// <remarks>
    /// <para>If the format doesn't match with the internal format of the property, access</para>
    /// <para>usually will fail with MPV_ERROR_PROPERTY_FORMAT. In some cases, the data</para>
    /// <para>is automatically converted and access succeeds. For example, MPV_FORMAT_INT64</para>
    /// <para>is always converted to MPV_FORMAT_DOUBLE, and access using MPV_FORMAT_STRING</para>
    /// <para>usually invokes a string formatter.</para>
    /// </remarks>
    [LibraryImport(LibraryName, EntryPoint = "mpv_get_property", StringMarshalling = StringMarshalling.Utf8)]
    public static partial MPVError MPVGetProperty(MPVHandle ctx, string name, MPVFormat format,
        [MarshalAs(UnmanagedType.I4)] out bool data);

    /// <summary>Read the value of the given property.</summary>
    /// <param name="ctx">The mpv handle.</param>
    /// <param name="name">The property name.</param>
    /// <param name="format">see enum mpv_format.</param>
    /// <param name="data">
    /// <para>Pointer to the variable holding the option value. On</para>
    /// <para>success, the variable will be set to a copy of the option</para>
    /// <para>value. For formats that require dynamic memory allocation,</para>
    /// <para>you can free the value with mpv_free() (strings) or</para>
    /// <para>mpv_free_node_contents() (MPV_FORMAT_NODE).</para>
    /// </param>
    /// <returns>error code</returns>
    /// <remarks>
    /// <para>If the format doesn't match with the internal format of the property, access</para>
    /// <para>usually will fail with MPV_ERROR_PROPERTY_FORMAT. In some cases, the data</para>
    /// <para>is automatically converted and access succeeds. For example, MPV_FORMAT_INT64</para>
    /// <para>is always converted to MPV_FORMAT_DOUBLE, and access using MPV_FORMAT_STRING</para>
    /// <para>usually invokes a string formatter.</para>
    /// </remarks>
    [LibraryImport(LibraryName, EntryPoint = "mpv_get_property", StringMarshalling = StringMarshalling.Utf8)]
    public static partial MPVError MPVGetProperty(MPVHandle ctx, string name, MPVFormat format, out long data);

    /// <summary>Read the value of the given property.</summary>
    /// <param name="ctx">The mpv handle.</param>
    /// <param name="name">The property name.</param>
    /// <param name="format">see enum mpv_format.</param>
    /// <param name="data">
    /// <para>Pointer to the variable holding the option value. On</para>
    /// <para>success, the variable will be set to a copy of the option</para>
    /// <para>value. For formats that require dynamic memory allocation,</para>
    /// <para>you can free the value with mpv_free() (strings) or</para>
    /// <para>mpv_free_node_contents() (MPV_FORMAT_NODE).</para>
    /// </param>
    /// <returns>error code</returns>
    /// <remarks>
    /// <para>If the format doesn't match with the internal format of the property, access</para>
    /// <para>usually will fail with MPV_ERROR_PROPERTY_FORMAT. In some cases, the data</para>
    /// <para>is automatically converted and access succeeds. For example, MPV_FORMAT_INT64</para>
    /// <para>is always converted to MPV_FORMAT_DOUBLE, and access using MPV_FORMAT_STRING</para>
    /// <para>usually invokes a string formatter.</para>
    /// </remarks>
    [LibraryImport(LibraryName, EntryPoint = "mpv_get_property", StringMarshalling = StringMarshalling.Utf8)]
    public static partial MPVError MPVGetProperty(MPVHandle ctx, string name, MPVFormat format, out double data);

    /// <summary>Read the value of the given property.</summary>
    /// <param name="ctx">The mpv handle.</param>
    /// <param name="name">The property name.</param>
    /// <param name="format">see enum mpv_format.</param>
    /// <param name="data">
    /// <para>Pointer to the variable holding the option value. On</para>
    /// <para>success, the variable will be set to a copy of the option</para>
    /// <para>value. For formats that require dynamic memory allocation,</para>
    /// <para>you can free the value with mpv_free() (strings) or</para>
    /// <para>mpv_free_node_contents() (MPV_FORMAT_NODE).</para>
    /// </param>
    /// <returns>error code</returns>
    /// <remarks>
    /// <para>If the format doesn't match with the internal format of the property, access</para>
    /// <para>usually will fail with MPV_ERROR_PROPERTY_FORMAT. In some cases, the data</para>
    /// <para>is automatically converted and access succeeds. For example, MPV_FORMAT_INT64</para>
    /// <para>is always converted to MPV_FORMAT_DOUBLE, and access using MPV_FORMAT_STRING</para>
    /// <para>usually invokes a string formatter.</para>
    /// </remarks>
    [LibraryImport(LibraryName, EntryPoint = "mpv_get_property", StringMarshalling = StringMarshalling.Utf8)]
    public static partial MPVError MPVGetProperty(MPVHandle ctx, string name, MPVFormat format, out string data);

    /// <summary>Read the value of the given property.</summary>
    /// <param name="ctx">The mpv handle.</param>
    /// <param name="name">The property name.</param>
    /// <param name="format">see enum mpv_format.</param>
    /// <param name="data">
    /// <para>Pointer to the variable holding the option value. On</para>
    /// <para>success, the variable will be set to a copy of the option</para>
    /// <para>value. For formats that require dynamic memory allocation,</para>
    /// <para>you can free the value with mpv_free() (strings) or</para>
    /// <para>mpv_free_node_contents() (MPV_FORMAT_NODE).</para>
    /// </param>
    /// <returns>error code</returns>
    /// <remarks>
    /// <para>If the format doesn't match with the internal format of the property, access</para>
    /// <para>usually will fail with MPV_ERROR_PROPERTY_FORMAT. In some cases, the data</para>
    /// <para>is automatically converted and access succeeds. For example, MPV_FORMAT_INT64</para>
    /// <para>is always converted to MPV_FORMAT_DOUBLE, and access using MPV_FORMAT_STRING</para>
    /// <para>usually invokes a string formatter.</para>
    /// </remarks>
    [LibraryImport(LibraryName, EntryPoint = "mpv_get_property", StringMarshalling = StringMarshalling.Utf8)]
    public static partial MPVError MPVGetProperty(MPVHandle ctx, string name, MPVFormat format, out MPVNode data);

    #endregion

    /// <summary>
    /// <para>Return the value of the property with the given name as string. This is</para>
    /// <para>equivalent to mpv_get_property() with MPV_FORMAT_STRING.</para>
    /// </summary>
    /// <param name="ctx">The mpv handle.</param>
    /// <param name="name">The property name.</param>
    /// <returns>
    /// <para>Property value, or NULL if the property can't be retrieved. Free</para>
    /// <para>the string with mpv_free().</para>
    /// </returns>
    /// <remarks>
    /// <para>See MPV_FORMAT_STRING for character encoding issues.</para>
    /// <para>On error, NULL is returned. Use mpv_get_property() if you want fine-grained</para>
    /// <para>error reporting.</para>
    /// </remarks>
    [LibraryImport(LibraryName, EntryPoint = "mpv_get_property_string", StringMarshalling = StringMarshalling.Custom,
        StringMarshallingCustomType = typeof(MPVStringMarshaller))]
    public static partial string
        MPVGetPropertyString(MPVHandle ctx,
            [MarshalAs(UnmanagedType.LPUTF8Str)] string name); //TODO: Verify if this actually works

    /// <summary>
    /// <para>Return the property as &quot;OSD&quot; formatted string. This is the same as</para>
    /// <para>mpv_get_property_string, but using MPV_FORMAT_OSD_STRING.</para>
    /// </summary>
    /// <returns>
    /// <para>Property value, or NULL if the property can't be retrieved. Free</para>
    /// <para>the string with mpv_free().</para>
    /// </returns>
    [LibraryImport(LibraryName, EntryPoint = "mpv_get_property_osd_string", StringMarshalling = StringMarshalling.Custom,
        StringMarshallingCustomType = typeof(MPVStringMarshaller))]
    public static partial string MPVGetPropertyOsdString(MPVHandle ctx,
        [MarshalAs(UnmanagedType.LPUTF8Str)] string name); //TODO: Verify if this actually works

    /// <summary>
    /// <para>Get a property asynchronously. You will receive the result of the operation</para>
    /// <para>as well as the property data with the MPV_EVENT_GET_PROPERTY_REPLY event.</para>
    /// <para>You should check the mpv_event.error field on the reply event.</para>
    /// </summary>
    /// <param name="ctx">The MPV handle.</param>
    /// <param name="replyUserData">see section about asynchronous calls</param>
    /// <param name="name">The property name.</param>
    /// <param name="format">see enum mpv_format.</param>
    /// <returns>error code if sending the request failed</returns>
    /// <remarks>Safe to be called from mpv render API threads.</remarks>
    [LibraryImport(LibraryName, EntryPoint = "mpv_get_property_async", StringMarshalling = StringMarshalling.Utf8)]
    public static partial MPVError MPVGetPropertyAsync(MPVHandle ctx, ulong replyUserData, string name,
        MPVFormat format);

    /// <summary>
    /// <para>Get a notification whenever the given property changes. You will receive</para>
    /// <para>updates as MPV_EVENT_PROPERTY_CHANGE. Note that this is not very precise:</para>
    /// <para>for some properties, it may not send updates even if the property changed.</para>
    /// <para>This depends on the property, and it's a valid feature request to ask for</para>
    /// <para>better update handling of a specific property. (For some properties, like</para>
    /// <para>``clock``, which shows the wall clock, this mechanism doesn't make too</para>
    /// <para>much sense anyway.)</para>
    /// </summary>
    /// <param name="ctx">The mpv handle.</param>
    /// <param name="replyUserData">
    /// <para>This will be used for the mpv_event.reply_userdata</para>
    /// <para>field for the received MPV_EVENT_PROPERTY_CHANGE</para>
    /// <para>events. (Also see section about asynchronous calls,</para>
    /// <para>although this function is somewhat different from</para>
    /// <para>actual asynchronous calls.)</para>
    /// <para>If you have no use for this, pass 0.</para>
    /// <para>Also see mpv_unobserve_property().</para>
    /// </param>
    /// <param name="name">The property name.</param>
    /// <param name="format">
    /// <para>see enum mpv_format. Can be MPV_FORMAT_NONE to omit values</para>
    /// <para>from the change events.</para>
    /// </param>
    /// <returns>error code (usually fails only on OOM or unsupported format)</returns>
    /// <remarks>
    /// <para>Property changes are coalesced: the change events are returned only once the</para>
    /// <para>event queue becomes empty (e.g. mpv_wait_event() would block or return</para>
    /// <para>MPV_EVENT_NONE), and then only one event per changed property is returned.</para>
    /// <para>You always get an initial change notification. This is meant to initialize</para>
    /// <para>the user's state to the current value of the property.</para>
    /// <para>Normally, change events are sent only if the property value changes according</para>
    /// <para>to the requested format. mpv_event_property will contain the property value</para>
    /// <para>as data member.</para>
    /// <para>Warning: if a property is unavailable or retrieving it caused an error,</para>
    /// <para>MPV_FORMAT_NONE will be set in mpv_event_property, even if the</para>
    /// <para>format parameter was set to a different value. In this case, the</para>
    /// <para>mpv_event_property.data field is invalid.</para>
    /// <para>If the property is observed with the format parameter set to MPV_FORMAT_NONE,</para>
    /// <para>you get low-level notifications whether the property _may_ have changed, and</para>
    /// <para>the data member in mpv_event_property will be unset. With this mode, you</para>
    /// <para>will have to determine yourself whether the property really changed. On the</para>
    /// <para>other hand, this mechanism can be faster and uses less resources.</para>
    /// <para>Observing a property that doesn't exist is allowed. (Although it may still</para>
    /// <para>cause some sporadic change events.)</para>
    /// <para>Keep in mind that you will get change notifications even if you change a</para>
    /// <para>property yourself. Try to avoid endless feedback loops, which could happen</para>
    /// <para>if you react to the change notifications triggered by your own change.</para>
    /// <para>Only the mpv_handle on which this was called will receive the property</para>
    /// <para>change events, or can unobserve them.</para>
    /// <para>Safe to be called from mpv render API threads.</para>
    /// </remarks>
    [LibraryImport(LibraryName, EntryPoint = "mpv_observe_property", StringMarshalling = StringMarshalling.Utf8)]
    public static partial MPVError
        MPVObserveProperty(MPVHandle ctx, ulong replyUserData, string name, MPVFormat format);

    /// <summary>
    /// <para>Undo mpv_observe_property(). This will remove all observed properties for</para>
    /// <para>which the given number was passed as reply_userdata to mpv_observe_property.</para>
    /// </summary>
    /// <param name="ctx">The mpv handle.</param>
    /// <param name="registeredReplyUserdata">ID that was passed to mpv_observe_property</param>
    /// <returns>
    /// <para>negative value is an error code, &gt;=0 is number of removed properties</para>
    /// <para>on success (includes the case when 0 were removed)</para>
    /// </returns>
    /// <remarks>Safe to be called from mpv render API threads.</remarks>
    [LibraryImport(LibraryName, EntryPoint = "mpv_unobserve_property")]
    public static partial MPVError MPVUnobserveProperty(MPVHandle ctx, ulong registeredReplyUserdata);

    /// <summary>Return a string describing the event. For unknown events, NULL is returned.</summary>
    /// <param name="eventId">event ID, see see enum mpv_event_id</param>
    /// <returns>
    /// <para>A static string giving a short symbolic name of the event. It</para>
    /// <para>consists of lower-case alphanumeric characters and can include &quot;-&quot;</para>
    /// <para>characters. This string is suitable for use in e.g. scripting</para>
    /// <para>interfaces.</para>
    /// <para>The string is completely static, i.e. doesn't need to be deallocated,</para>
    /// <para>and is valid forever.</para>
    /// </returns>
    /// <remarks>
    /// <para>Note that all events actually returned by the API will also yield a non-NULL</para>
    /// <para>string with this function.</para>
    /// </remarks>
    [LibraryImport(LibraryName, EntryPoint = "mpv_event_name", StringMarshalling = StringMarshalling.Custom,
        StringMarshallingCustomType = typeof(MPVStringMarshaller))]
    public static partial string MPVEventName(MPVEventId eventId);

    /// <summary>
    /// <para>Convert the given src event to a mpv_node, and set *dst to the result. *dst</para>
    /// <para>is set to a MPV_FORMAT_NODE_MAP, with fields for corresponding mpv_event and</para>
    /// <para>mpv_event.data/mpv_event_* fields.</para>
    /// </summary>
    /// <param name="dst">
    /// <para>Target. This is not read and fully overwritten. Must be released</para>
    /// <para>with mpv_free_node_contents(). Do not write to pointers returned</para>
    /// <para>by it. (On error, this may be left as an empty node.)</para>
    /// </param>
    /// <param name="src">
    /// <para>The source event. Not modified (it's not const due to the author's</para>
    /// <para>prejudice of the C version of const).</para>
    /// </param>
    /// <returns>error code (MPV_ERROR_NOMEM only, if at all)</returns>
    /// <remarks>
    /// <para>The exact details are not completely documented out of laziness. A start</para>
    /// <para>is located in the &quot;Events&quot; section of the manpage.</para>
    /// <para>*dst may point to newly allocated memory, or pointers in mpv_event. You must</para>
    /// <para>copy the entire mpv_node if you want to reference it after mpv_event becomes</para>
    /// <para>invalid (such as making a new mpv_wait_event() call, or destroying the</para>
    /// <para>mpv_handle from which it was returned). Call mpv_free_node_contents() to free</para>
    /// <para>any memory allocations made by this API function.</para>
    /// <para>Safe to be called from mpv render API threads.</para>
    /// </remarks>
    [LibraryImport(LibraryName, EntryPoint = "mpv_event_to_node", StringMarshalling = StringMarshalling.Utf8)]
    public static partial MPVError MPVEventToNode(out MPVNode dst, ref MPVEvent src);

    /// <summary>Enable or disable the given event.</summary>
    /// <param name="ctx">The mpv handle.</param>
    /// <param name="eventId">See enum mpv_event_id.</param>
    /// <param name="enable">1 to enable receiving this event, 0 to disable it.</param>
    /// <returns>error code</returns>
    /// <remarks>
    /// <para>Some events are enabled by default. Some events can't be disabled.</para>
    /// <para>(Informational note: currently, all events are enabled by default, except</para>
    /// <para>MPV_EVENT_TICK.)</para>
    /// <para>Safe to be called from mpv render API threads.</para>
    /// </remarks>
    [LibraryImport(LibraryName, EntryPoint = "mpv_request_event")]
    public static partial MPVError
        MPVRequestEvent(MPVHandle ctx, MPVEventId eventId,
            [MarshalAs(UnmanagedType.I4)] bool enable); //TODO: Check bool to int works

    /// <summary>
    /// <para>Enable or disable receiving of log messages. These are the messages the</para>
    /// <para>command line player prints to the terminal. This call sets the minimum</para>
    /// <para>required log level for a message to be received with MPV_EVENT_LOG_MESSAGE.</para>
    /// </summary>
    /// <param name="ctx">The mpv handle.</param>
    /// <param name="minLevel">
    /// <para>Minimal log level as string. Valid log levels:</para>
    /// <para>no fatal error warn info v debug trace</para>
    /// <para>The value &quot;no&quot; disables all messages. This is the default.</para>
    /// <para>An exception is the value &quot;terminal-default&quot;, which uses the</para>
    /// <para>log level as set by the &quot;--msg-level&quot; option. This works</para>
    /// <para>even if the terminal is disabled. (Since API version 1.19.)</para>
    /// <para>Also see mpv_log_level.</para>
    /// </param>
    /// <returns>error code</returns>
    [LibraryImport(LibraryName, EntryPoint = "mpv_request_log_messages", StringMarshalling = StringMarshalling.Utf8)]
    public static partial MPVError MPVRequestLogMessages(MPVHandle ctx, string minLevel);

    /// <summary>
    /// <para>Wait for the next event, or until the timeout expires, or if another thread</para>
    /// <para>makes a call to mpv_wakeup(). Passing 0 as timeout will never wait, and</para>
    /// <para>is suitable for polling.</para>
    /// </summary>
    /// <param name="ctx">The mpv handle.</param>
    /// <param name="timeout">
    /// <para>Timeout in seconds, after which the function returns even if</para>
    /// <para>no event was received. A MPV_EVENT_NONE is returned on</para>
    /// <para>timeout. A value of 0 will disable waiting. Negative values</para>
    /// <para>will wait with an infinite timeout.</para>
    /// </param>
    /// <returns>
    /// <para>A struct containing the event ID and other data. The pointer (and</para>
    /// <para>fields in the struct) stay valid until the next mpv_wait_event()</para>
    /// <para>call, or until the mpv_handle is destroyed. You must not write to</para>
    /// <para>the struct, and all memory referenced by it will be automatically</para>
    /// <para>released by the API on the next mpv_wait_event() call, or when the</para>
    /// <para>context is destroyed. The return value is never NULL.</para>
    /// </returns>
    /// <remarks>
    /// <para>The internal event queue has a limited size (per client handle). If you</para>
    /// <para>don't empty the event queue quickly enough with mpv_wait_event(), it will</para>
    /// <para>overflow and silently discard further events. If this happens, making</para>
    /// <para>asynchronous requests will fail as well (with MPV_ERROR_EVENT_QUEUE_FULL).</para>
    /// <para>Only one thread is allowed to call this on the same mpv_handle at a time.</para>
    /// <para>The API won't complain if more than one thread calls this, but it will cause</para>
    /// <para>race conditions in the client when accessing the shared mpv_event struct.</para>
    /// <para>Note that most other API functions are not restricted by this, and no API</para>
    /// <para>function internally calls mpv_wait_event(). Additionally, concurrent calls</para>
    /// <para>to different mpv_handles are always safe.</para>
    /// <para>As long as the timeout is 0, this is safe to be called from mpv render API</para>
    /// <para>threads.</para>
    /// </remarks>
    [LibraryImport(LibraryName, EntryPoint = "mpv_wait_event")]
    public static partial MPVEvent MPVWaitEvent(MPVHandle ctx, double timeout);

    /// <summary>
    /// <para>Interrupt the current mpv_wait_event() call. This will wake up the thread</para>
    /// <para>currently waiting in mpv_wait_event(). If no thread is waiting, the next</para>
    /// <para>mpv_wait_event() call will return immediately (this is to avoid lost</para>
    /// <para>wakeups).</para>
    /// </summary>
    /// <remarks>
    /// <para>mpv_wait_event() will receive a MPV_EVENT_NONE if it's woken up due to</para>
    /// <para>this call. But note that this dummy event might be skipped if there are</para>
    /// <para>already other events queued. All what counts is that the waiting thread</para>
    /// <para>is woken up at all.</para>
    /// <para>Safe to be called from mpv render API threads.</para>
    /// </remarks>
    [LibraryImport(LibraryName, EntryPoint = "mpv_wakeup")]
    public static partial void MPVWakeup(MPVHandle ctx);

    /// <summary>
    /// <para>Set a custom function that should be called when there are new events. Use</para>
    /// <para>this if blocking in mpv_wait_event() to wait for new events is not feasible.</para>
    /// </summary>
    /// <param name="ctx">The mpv handle.</param>
    /// <param name="cb">function that should be called if a wakeup is required</param>
    /// <param name="data">arbitrary userdata passed to cb</param>
    /// <remarks>
    /// <para>Keep in mind that the callback will be called from foreign threads. You</para>
    /// <para>must not make any assumptions of the environment, and you must return as</para>
    /// <para>soon as possible (i.e. no long blocking waits). Exiting the callback through</para>
    /// <para>any other means than a normal return is forbidden (no throwing exceptions,</para>
    /// <para>no longjmp() calls). You must not change any local thread state (such as</para>
    /// <para>the C floating point environment).</para>
    /// <para>You are not allowed to call any client API functions inside of the callback.</para>
    /// <para>In particular, you should not do any processing in the callback, but wake up</para>
    /// <para>another thread that does all the work. The callback is meant strictly for</para>
    /// <para>notification only, and is called from arbitrary core parts of the player,</para>
    /// <para>that make no considerations for reentrant API use or allowing the callee to</para>
    /// <para>spend a lot of time doing other things. Keep in mind that it's also possible</para>
    /// <para>that the callback is called from a thread while a mpv API function is called</para>
    /// <para>(i.e. it can be reentrant).</para>
    /// <para>In general, the client API expects you to call mpv_wait_event() to receive</para>
    /// <para>notifications, and the wakeup callback is merely a helper utility to make</para>
    /// <para>this easier in certain situations. Note that it's possible that there's</para>
    /// <para>only one wakeup callback invocation for multiple events. You should call</para>
    /// <para>mpv_wait_event() with no timeout until MPV_EVENT_NONE is reached, at which</para>
    /// <para>point the event queue is empty.</para>
    /// <para>If you actually want to do processing in a callback, spawn a thread that</para>
    /// <para>does nothing but call mpv_wait_event() in a loop and dispatches the result</para>
    /// <para>to a callback.</para>
    /// <para>Only one wakeup callback can be set.</para>
    /// </remarks>
    [LibraryImport(LibraryName, EntryPoint = "mpv_set_wakeup_callback")]
    public static partial void
        MPVSetWakeupCallback(MPVHandle ctx, MPVWakeupCallback<IntPtr> cb, IntPtr data); //TODO: More overloads?

    /// <summary>
    /// <para>Block until all asynchronous requests are done. This affects functions like</para>
    /// <para>mpv_command_async(), which return immediately and return their result as</para>
    /// <para>events.</para>
    /// </summary>
    /// <remarks>
    /// <para>This is a helper, and somewhat equivalent to calling mpv_wait_event() in a</para>
    /// <para>loop until all known asynchronous requests have sent their reply as event,</para>
    /// <para>except that the event queue is not emptied.</para>
    /// <para>In case you called mpv_suspend() before, this will also forcibly reset the</para>
    /// <para>suspend counter of the given handle.</para>
    /// </remarks>
    [LibraryImport(LibraryName, EntryPoint = "mpv_wait_async_requests")]
    public static partial MPVError MPVWaitAsyncRequests(MPVHandle ctx);

    /// <summary>
    /// <para>A hook is like a synchronous event that blocks the player. You register</para>
    /// <para>a hook handler with this function. You will get an event, which you need</para>
    /// <para>to handle, and once things are ready, you can let the player continue with</para>
    /// <para>mpv_hook_continue().</para>
    /// </summary>
    /// <param name="ctx">The mpv handle.</param>
    /// <param name="replyUserData">
    /// <para>This will be used for the mpv_event.reply_userdata</para>
    /// <para>field for the received MPV_EVENT_HOOK events.</para>
    /// <para>If you have no use for this, pass 0.</para>
    /// </param>
    /// <param name="name">
    /// <para>The hook name. This should be one of the documented names. But</para>
    /// <para>if the name is unknown, the hook event will simply be never</para>
    /// <para>raised.</para>
    /// </param>
    /// <param name="priority">See remarks above. Use 0 as a neutral default.</param>
    /// <returns>error code (usually fails only on OOM)</returns>
    /// <remarks>
    /// <para>Currently, hooks can't be removed explicitly. But they will be implicitly</para>
    /// <para>removed if the mpv_handle it was registered with is destroyed. This also</para>
    /// <para>continues the hook if it was being handled by the destroyed mpv_handle (but</para>
    /// <para>this should be avoided, as it might mess up order of hook execution).</para>
    /// <para>Hook handlers are ordered globally by priority and order of registration.</para>
    /// <para>Handlers for the same hook with same priority are invoked in order of</para>
    /// <para>registration (the handler registered first is run first). Handlers with</para>
    /// <para>lower priority are run first (which seems backward).</para>
    /// <para>See the &quot;Hooks&quot; section in the manpage to see which hooks are currently</para>
    /// <para>defined.</para>
    /// <para>Some hooks might be reentrant (so you get multiple MPV_EVENT_HOOK for the</para>
    /// <para>same hook). If this can happen for a specific hook type, it will be</para>
    /// <para>explicitly documented in the manpage.</para>
    /// <para>Only the mpv_handle on which this was called will receive the hook events,</para>
    /// <para>or can &quot;continue&quot; them.</para>
    /// </remarks>
    [LibraryImport(LibraryName, EntryPoint = "mpv_hook_add", StringMarshalling = StringMarshalling.Utf8)]
    public static partial MPVError MPVHookAdd(MPVHandle ctx, ulong replyUserData, string name, int priority);

    /// <summary>
    /// <para>Respond to a MPV_EVENT_HOOK event. You must call this after you have handled</para>
    /// <para>the event. There is no way to &quot;cancel&quot; or &quot;stop&quot; the hook.</para>
    /// </summary>
    /// <param name="ctx">The mpv handle.</param>
    /// <param name="id">
    /// <para>This must be the value of the mpv_event_hook.id field for the</para>
    /// <para>corresponding MPV_EVENT_HOOK.</para>
    /// </param>
    /// <returns>error code</returns>
    /// <remarks>
    /// <para>Calling this will will typically unblock the player for whatever the hook</para>
    /// <para>is responsible for (e.g. for the &quot;on_load&quot; hook it lets it continue</para>
    /// <para>playback).</para>
    /// <para>It is explicitly undefined behavior to call this more than once for each</para>
    /// <para>MPV_EVENT_HOOK, to pass an incorrect ID, or to call this on a mpv_handle</para>
    /// <para>different from the one that registered the handler and received the event.</para>
    /// </remarks>
    [LibraryImport(LibraryName, EntryPoint = "mpv_hook_continue")]
    public static partial MPVError MPVHookContinue(MPVHandle ctx, ulong id);

    #endregion

    #region render.h

    /// <summary>
    /// <para>Initialize the renderer state. Depending on the backend used, this will</para>
    /// <para>access the underlying GPU API and initialize its own objects.</para>
    /// </summary>
    /// <param name="res">
    /// <para>set to the context (on success) or NULL (on failure). The value</para>
    /// <para>is never read and always overwritten.</para>
    /// </param>
    /// <param name="mpv">
    /// <para>handle used to get the core (the mpv_render_context won't depend</para>
    /// <para>on this specific handle, only the core referenced by it)</para>
    /// </param>
    /// <param name="renderParams">
    /// <para>an array of parameters, terminated by type==0. It's left</para>
    /// <para>unspecified what happens with unknown parameters. At least</para>
    /// <para>MPV_RENDER_PARAM_API_TYPE is required, and most backends will</para>
    /// <para>require another backend-specific parameter.</para>
    /// </param>
    /// <returns>
    /// <para>error code, including but not limited to:</para>
    /// <para>MPV_ERROR_UNSUPPORTED: the OpenGL version is not supported</para>
    /// <para>(or required extensions are missing)</para>
    /// <para>MPV_ERROR_NOT_IMPLEMENTED: an unknown API type was provided, or</para>
    /// <para>support for the requested API was not</para>
    /// <para>built in the used libmpv binary.</para>
    /// <para>MPV_ERROR_INVALID_PARAMETER: at least one of the provided parameters was</para>
    /// <para>not valid.</para>
    /// </returns>
    /// <remarks>
    /// <para>You must free the context with mpv_render_context_free(). Not doing so before</para>
    /// <para>the mpv core is destroyed may result in memory leaks or crashes.</para>
    /// <para>Currently, only at most 1 context can exists per mpv core (it represents the</para>
    /// <para>main video output).</para>
    /// <para>You should pass the following parameters:</para>
    /// <para>- MPV_RENDER_PARAM_API_TYPE to select the underlying backend/GPU API.</para>
    /// <para>- Backend-specific init parameter, like MPV_RENDER_PARAM_OPENGL_INIT_PARAMS.</para>
    /// <para>- Setting MPV_RENDER_PARAM_ADVANCED_CONTROL and following its rules is</para>
    /// <para>strongly recommended.</para>
    /// <para>- If you want to use hwdec, possibly hwdec interop resources.</para>
    /// </remarks>
    [LibraryImport(LibraryName, EntryPoint = "mpv_render_context_create")]
    public static partial MPVError MPVRenderContextCreate(out MPVRenderContext res, MPVHandle mpv,
        MPVRenderParam renderParams);

    /// <summary>
    /// <para>Attempt to change a single parameter. Not all backends and parameter types</para>
    /// <para>support all kinds of changes.</para>
    /// </summary>
    /// <param name="ctx">a valid render context</param>
    /// <param name="param">the parameter type and data that should be set</param>
    /// <returns>
    /// <para>error code. If a parameter could actually be changed, this returns</para>
    /// <para>success, otherwise an error code depending on the parameter type</para>
    /// <para>and situation.</para>
    /// </returns>
    [LibraryImport(LibraryName, EntryPoint = "mpv_render_context_set_parameter")]
    public static partial MPVError MPVRenderContextSetParameter(MPVRenderContext ctx, MPVRenderParam param);

    /// <summary>
    /// <para>Retrieve information from the render context. This is NOT a counterpart to</para>
    /// <para>mpv_render_context_set_parameter(), because you generally can't read</para>
    /// <para>parameters set with it, and this function is not meant for this purpose.</para>
    /// <para>Instead, this is for communicating information from the renderer back to the</para>
    /// <para>user. See mpv_render_param_type; entries which support this function</para>
    /// <para>explicitly mention it, and for other entries you can assume it will fail.</para>
    /// </summary>
    /// <param name="ctx">a valid render context</param>
    /// <param name="param">the parameter type and data that should be retrieved</param>
    /// <returns>
    /// <para>error code. If a parameter could actually be retrieved, this returns</para>
    /// <para>success, otherwise an error code depending on the parameter type</para>
    /// <para>and situation. MPV_ERROR_NOT_IMPLEMENTED is used for unknown</para>
    /// <para>param.type, or if retrieving it is not supported.</para>
    /// </returns>
    /// <remarks>
    /// <para>You pass param with param.type set and param.data pointing to a variable</para>
    /// <para>of the required data type. The function will then overwrite that variable</para>
    /// <para>with the returned value (at least on success).</para>
    /// </remarks>
    [LibraryImport(LibraryName, EntryPoint = "mpv_render_context_get_info")]
    public static partial MPVError MPVRenderContextGetInfo(MPVRenderContext ctx, MPVRenderParam param);

    /// <summary>
    /// <para>Set the callback that notifies you when a new video frame is available, or</para>
    /// <para>if the video display configuration somehow changed and requires a redraw.</para>
    /// <para>Similar to mpv_set_wakeup_callback(), you must not call any mpv API from</para>
    /// <para>the callback, and all the other listed restrictions apply (such as not</para>
    /// <para>exiting the callback by throwing exceptions).</para>
    /// </summary>
    /// <param name="ctx">The MPVRenderContext.</param>
    /// <param name="callback">
    /// <para>callback(callback_ctx) is called if the frame should be</para>
    /// <para>redrawn</para>
    /// </param>
    /// <param name="callbackCtx">opaque argument to the callback</param>
    /// <remarks>
    /// <para>This can be called from any thread, except from an update callback. In case</para>
    /// <para>of the OpenGL backend, no OpenGL state or API is accessed.</para>
    /// <para>Calling this will raise an update callback immediately.</para>
    /// </remarks>
    [LibraryImport(LibraryName, EntryPoint = "mpv_render_context_set_update_callback")]
    public static partial void MPVRenderContextSetUpdateCallback(MPVRenderContext ctx,
        MPVRenderUpdateCallback callback, IntPtr callbackCtx);

    /// <summary>
    /// <para>The API user is supposed to call this when the update callback was invoked</para>
    /// <para>(like all mpv_render_* functions, this has to happen on the render thread,</para>
    /// <para>and _not_ from the update callback itself).</para>
    /// </summary>
    /// <returns>
    /// <para>a bitset of mpv_render_update_flag values (i.e. multiple flags are</para>
    /// <para>combined with bitwise or). Typically, this will tell the API user</para>
    /// <para>what should happen next. E.g. if the MPV_RENDER_UPDATE_FRAME flag is</para>
    /// <para>set, mpv_render_context_render() should be called. If flags unknown</para>
    /// <para>to the API user are set, or if the return value is 0, nothing needs</para>
    /// <para>to be done.</para>
    /// </returns>
    /// <remarks>
    /// <para>This is optional if MPV_RENDER_PARAM_ADVANCED_CONTROL was not set (default).</para>
    /// <para>Otherwise, it's a hard requirement that this is called after each update</para>
    /// <para>callback. If multiple update callback happened, and the function could not</para>
    /// <para>be called sooner, it's OK to call it once after the last callback.</para>
    /// <para>If an update callback happens during or after this function, the function</para>
    /// <para>must be called again at the soonest possible time.</para>
    /// <para>If MPV_RENDER_PARAM_ADVANCED_CONTROL was set, this will do additional work</para>
    /// <para>such as allocating textures for the video decoder.</para>
    /// </remarks>
    [LibraryImport(LibraryName, EntryPoint = "mpv_render_context_update")]
    public static partial MPVRenderUpdateFlag MPVRenderContextUpdate(MPVRenderContext ctx);

    /// <summary>Render video.</summary>
    /// <param name="ctx">a valid render context</param>
    /// <param name="param">
    /// <para>an array of parameters, terminated by type==0. Which parameters</para>
    /// <para>are required depends on the backend. It's left unspecified what</para>
    /// <para>happens with unknown parameters.</para>
    /// </param>
    /// <returns>error code</returns>
    /// <remarks>
    /// <para>Typically renders the video to a target surface provided via mpv_render_param</para>
    /// <para>(the details depend on the backend in use). Options like &quot;panscan&quot; are</para>
    /// <para>applied to determine which part of the video should be visible and how the</para>
    /// <para>video should be scaled. You can change these options at runtime by using the</para>
    /// <para>mpv property API.</para>
    /// <para>The renderer will reconfigure itself every time the target surface</para>
    /// <para>configuration (such as size) is changed.</para>
    /// <para>This function implicitly pulls a video frame from the internal queue and</para>
    /// <para>renders it. If no new frame is available, the previous frame is redrawn.</para>
    /// <para>The update callback set with mpv_render_context_set_update_callback()</para>
    /// <para>notifies you when a new frame was added. The details potentially depend on</para>
    /// <para>the backends and the provided parameters.</para>
    /// <para>Generally, libmpv will invoke your update callback some time before the video</para>
    /// <para>frame should be shown, and then lets this function block until the supposed</para>
    /// <para>display time. This will limit your rendering to video FPS. You can prevent</para>
    /// <para>this by setting the &quot;video-timing-offset&quot; global option to 0. (This applies</para>
    /// <para>only to &quot;audio&quot; video sync mode.)</para>
    /// <para>You should pass the following parameters:</para>
    /// <para>- Backend-specific target object, such as MPV_RENDER_PARAM_OPENGL_FBO.</para>
    /// <para>- Possibly transformations, such as MPV_RENDER_PARAM_FLIP_Y.</para>
    /// </remarks>
    [LibraryImport(LibraryName, EntryPoint = "mpv_render_context_render")]
    public static partial MPVError MPVRenderContextRender(MPVRenderContext ctx, MPVRenderParam param);

    /// <summary>
    /// <para>Tell the renderer that a frame was flipped at the given time. This is</para>
    /// <para>optional, but can help the player to achieve better timing.</para>
    /// </summary>
    /// <param name="ctx">a valid render context</param>
    /// <remarks>
    /// <para>Note that calling this at least once informs libmpv that you will use this</para>
    /// <para>function. If you use it inconsistently, expect bad video playback.</para>
    /// <para>If this is called while no video is initialized, it is ignored.</para>
    /// </remarks>
    [LibraryImport(LibraryName, EntryPoint = "mpv_render_context_report_swap")]
    public static partial MPVError MPVRenderContextReportSwap(MPVRenderContext ctx);

    /// <summary>Destroy the mpv renderer state.</summary>
    /// <param name="ctx">
    /// <para>a valid render context. After this function returns, this is not</para>
    /// <para>a valid pointer anymore. NULL is also allowed and does nothing.</para>
    /// </param>
    /// <remarks>
    /// <para>If video is still active (e.g. a file playing), video will be disabled</para>
    /// <para>forcefully.</para>
    /// </remarks>
    [LibraryImport(LibraryName, EntryPoint = "mpv_render_context_free")]
    public static partial void MPVRenderContextFree(MPVRenderContext ctx);

    #endregion

    #region stream_cb.h

    /// <summary>
    /// <para>Add a custom stream protocol. This will register a protocol handler under</para>
    /// <para>the given protocol prefix, and invoke the given callbacks if an URI with the</para>
    /// <para>matching protocol prefix is opened.</para>
    /// </summary>
    /// <param name="ctx">The MPV Handle.</param>
    /// <param name="protocol">protocol prefix, for example &quot;foo&quot; for &quot;foo://&quot; URIs</param>
    /// <param name="userData">
    /// <para>opaque pointer passed into the mpv_stream_cb_open_fn</para>
    /// <para>callback.</para>
    /// </param>
    /// <param name="openFn">StreamOpen callback function</param>
    /// <returns>error code</returns>
    /// <remarks>
    /// <para>The &quot;ro&quot; is for read-only - only read-only streams can be registered with</para>
    /// <para>this function.</para>
    /// <para>The callback remains registered until the mpv core is registered.</para>
    /// <para>If a custom stream with the same name is already registered, then the</para>
    /// <para>MPV_ERROR_INVALID_PARAMETER error is returned.</para>
    /// </remarks>
    [LibraryImport(LibraryName, EntryPoint = "mpv_stream_cb_add_ro", StringMarshalling = StringMarshalling.Utf8)]
    public static partial MPVError MPVStreamCBAddRO(MPVHandle ctx, string protocol, IntPtr userData,
        MPVStreamCBOpenROCallback openFn);

    #endregion
}

#region Delegates

#region client.h

public delegate void MPVWakeupCallback<in T>(T data);

#endregion

#region render.h

/// <summary>
/// <para>Parameters for mpv_render_param (which is used in a few places such as</para>
/// <para>mpv_render_context_create().</para>
/// </summary>
/// <remarks>Also see mpv_render_param for conventions and how to use it.</remarks>
/// <summary>
/// <para>Used to pass arbitrary parameters to some mpv_render_* functions. The</para>
/// <para>meaning of the data parameter is determined by the type, and each</para>
/// <para>MPV_RENDER_PARAM_* documents what type the value must point to.</para>
/// </summary>
/// <remarks>
/// <para>Each value documents the required data type as the pointer you cast to</para>
/// <para>void* and set on mpv_render_param.data. For example, if MPV_RENDER_PARAM_FOO</para>
/// <para>documents the type as Something* , then the code should look like this:</para>
/// <para>Something foo = {...};</para>
/// <para>mpv_render_param param;</para>
/// <para>param.type = MPV_RENDER_PARAM_FOO;</para>
/// <para>param.data =&amp;foo;</para>
/// <para>Normally, the data field points to exactly 1 object. If the type is char*,</para>
/// <para>it points to a 0-terminated string.</para>
/// <para>In all cases (unless documented otherwise) the pointers need to remain</para>
/// <para>valid during the call only. Unless otherwise documented, the API functions</para>
/// <para>will not write to the params array or any data pointed to it.</para>
/// <para>As a convention, parameter arrays are always terminated by type==0. There</para>
/// <para>is no specific order of the parameters required. The order of the 2 fields in</para>
/// <para>this struct is guaranteed (even after ABI changes).</para>
/// </remarks>
/// <summary>Flags used in mpv_render_frame_info.flags. Each value represents a bit in it.</summary>
/// <summary>
/// <para>Information about the next video frame that will be rendered. Can be</para>
/// <para>retrieved with MPV_RENDER_PARAM_NEXT_FRAME_INFO.</para>
/// </summary>
public delegate void MPVRenderUpdateCallback(IntPtr callbackCtx);

#endregion

#region stream_cb.h

/// <summary>
/// <para>Read callback used to implement a custom stream. The semantics of the</para>
/// <para>callback match read(2) in blocking mode. Short reads are allowed (you can</para>
/// <para>return less bytes than requested, and libmpv will retry reading the rest</para>
/// <para>with another call). If no data can be immediately read, the callback must</para>
/// <para>block until there is new data. A return of 0 will be interpreted as final</para>
/// <para>EOF, although libmpv might retry the read, or seek to a different position.</para>
/// </summary>
/// <param name="cookie">
/// <para>opaque cookie identifying the stream,</para>
/// <para>returned from mpv_stream_cb_open_fn</para>
/// </param>
/// <param name="buffer">buffer to read data into</param>
/// <param name="size">of the buffer</param>
/// <returns>number of bytes read into the buffer</returns>
/// <returns>0 on EOF</returns>
/// <returns>-1 on error</returns>
public delegate long MPVStreamCBReadCallback(IntPtr cookie, [MarshalAs(UnmanagedType.LPUTF8Str)] string buffer,
    ulong size);

/// <summary>Seek callback used to implement a custom stream.</summary>
/// <param name="cookie">
/// <para>opaque cookie identifying the stream,</para>
/// <para>returned from mpv_stream_cb_open_fn</para>
/// </param>
/// <param name="offset">target absolute stream position</param>
/// <returns>
/// <para>the resulting offset of the stream</para>
/// <para>MPV_ERROR_UNSUPPORTED or MPV_ERROR_GENERIC if the seek failed</para>
/// </returns>
/// <remarks>
/// <para>Note that mpv will issue a seek to position 0 immediately after opening. This</para>
/// <para>is used to test whether the stream is seekable (since seekability might</para>
/// <para>depend on the URI contents, not just the protocol). Return</para>
/// <para>MPV_ERROR_UNSUPPORTED if seeking is not implemented for this stream. This</para>
/// <para>seek also serves to establish the fact that streams start at position 0.</para>
/// <para>This callback can be NULL, in which it behaves as if always returning</para>
/// <para>MPV_ERROR_UNSUPPORTED.</para>
/// </remarks>
public delegate long MPVStreamCBSeekCallback(IntPtr cookie, long offset);

/// <summary>Size callback used to implement a custom stream.</summary>
/// <param name="cookie">
/// <para>opaque cookie identifying the stream,</para>
/// <para>returned from mpv_stream_cb_open_fn</para>
/// </param>
/// <returns>the total size in bytes of the stream</returns>
/// <remarks>
/// <para>Return MPV_ERROR_UNSUPPORTED if no size is known.</para>
/// <para>This callback can be NULL, in which it behaves as if always returning</para>
/// <para>MPV_ERROR_UNSUPPORTED.</para>
/// </remarks>
public delegate long MPVStreamCBSizeCallback(IntPtr cookie);

/// <summary>Close callback used to implement a custom stream.</summary>
/// <param name="cookie">
/// <para>opaque cookie identifying the stream,</para>
/// <para>returned from mpv_stream_cb_open_fn</para>
/// </param>
public delegate void MPVStreamCBCloseCallback(IntPtr cookie);

/// <summary>Cancel callback used to implement a custom stream.</summary>
/// <param name="cookie">
/// <para>opaque cookie identifying the stream,</para>
/// <para>returned from mpv_stream_cb_open_fn</para>
/// </param>
/// <remarks>
/// <para>This callback is used to interrupt any current or future read and seek</para>
/// <para>operations. It will be called from a separate thread than the demux</para>
/// <para>thread, and should not block.</para>
/// <para>This callback can be NULL.</para>
/// <para>Available since API 1.106.</para>
/// </remarks>
public delegate void MPVStreamCBCancelCallback(IntPtr cookie);

/// <summary>See mpv_stream_cb_open_ro_fn callback.</summary>
/// <summary>
/// <para>Open callback used to implement a custom read-only (ro) stream. The user</para>
/// <para>must set the callback fields in the passed info struct. The cookie field</para>
/// <para>also can be set to store state associated to the stream instance.</para>
/// </summary>
/// <param name="userData">opaque user data provided via mpv_stream_cb_add()</param>
/// <param name="uri">name of the stream to be opened (with protocol prefix)</param>
/// <param name="info">fields which the user should fill</param>
/// <returns>0 on success, MPV_ERROR_LOADING_FAILED if the URI cannot be opened.</returns>
/// <remarks>
/// <para>Note that the info struct is valid only for the duration of this callback.</para>
/// <para>You can't change the callbacks or the pointer to the cookie at a later point.</para>
/// <para>Each stream instance created by the open callback can have different</para>
/// <para>callbacks.</para>
/// <para>The close_fn callback will terminate the stream instance. The pointers to</para>
/// <para>your callbacks and cookie will be discarded, and the callbacks will not be</para>
/// <para>called again.</para>
/// </remarks>
public delegate MPVError MPVStreamCBOpenROCallback(IntPtr userData, [MarshalAs(UnmanagedType.LPUTF8Str)] string uri,
    MPVStreamCBInfo info);

#endregion

#endregion