using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace YALibMPV.PInvoke.Marshaller;

[CustomMarshaller(typeof(string), MarshalMode.ManagedToUnmanagedOut, typeof(MPVStringMarshaller))]
public static class MPVStringMarshaller
{
    public static string ConvertToManaged(IntPtr unmanaged)
    {
        return Marshal.PtrToStringUTF8(unmanaged) ?? string.Empty;
    }
}