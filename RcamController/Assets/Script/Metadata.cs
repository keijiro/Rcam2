using UnityEngine;
using System.Runtime.InteropServices;
using System;

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
}

} // namespace Rcam2
