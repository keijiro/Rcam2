using UnityEngine;
using System;
using System.Runtime.InteropServices;

namespace Rcam2 {

[StructLayout(LayoutKind.Sequential)]
struct Metadata
{
    public Vector3 CameraPosition;
    public Quaternion CameraRotation;
    public Vector2 DepthRange;
    public Matrix4x4 ProjectionMatrix;

    public string Serialize()
    {
        ReadOnlySpan<Metadata> data = stackalloc Metadata[] { this };
        var bytes = MemoryMarshal.AsBytes(data).ToArray();
        return "<![CDATA[" + System.Convert.ToBase64String(bytes) + "]] >";
    }

    public static Metadata Deserialize(string xml)
    {
        var base64 = xml.Substring(9, xml.Length - 9 - 4);
        var data = System.Convert.FromBase64String(base64);
        return MemoryMarshal.Read<Metadata>(new Span<byte>(data));
    }
}

} // namespace Rcam2
