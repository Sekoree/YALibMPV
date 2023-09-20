namespace YALibMPV.PInvoke.Enums.Client;

/// <summary>
/// <para>Numeric log levels. The lower the number, the more important the message is.</para>
/// <para>MPV_LOG_LEVEL_NONE is never used when receiving messages. The string in</para>
/// <para>the comment after the value is the name of the log level as used for the</para>
/// <para>mpv_request_log_messages() function.</para>
/// <para>Unused numeric values are unused, but reserved for future use.</para>
/// </summary>
public enum MPVLogLevel
{
    None = 0,
    /// <summary>&quot;no&quot;    - disable absolutely all messages</summary>
    Fatal = 10,
    /// <summary>&quot;fatal&quot; - critical/aborting errors</summary>
    Error = 20,
    /// <summary>&quot;error&quot; - simple errors</summary>
    Warn = 30,
    /// <summary>&quot;warn&quot;  - possible problems</summary>
    Info = 40,
    /// <summary>&quot;info&quot;  - informational message</summary>
    V = 50,
    /// <summary>&quot;v&quot;     - noisy informational message</summary>
    Debug = 60,
    /// <summary>&quot;debug&quot; - very noisy technical information</summary>
    Trace = 70
}