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

    public static Metadata Deserialize(string base64)
    {
        var data = System.Convert.FromBase64String(base64);
        return MemoryMarshal.Read<Metadata>(new Span<byte>(data));
    }
}

} // namespace Rcam2
