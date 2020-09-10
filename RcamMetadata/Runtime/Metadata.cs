using UnityEngine;
using System;
using System.Runtime.InteropServices;

namespace Rcam2 {

[StructLayout(LayoutKind.Sequential)]
public unsafe struct Metadata
{
    #region Data members

    // Camera pose
    public Vector3 CameraPosition;
    public Quaternion CameraRotation;

    // Camera parameters
    public Matrix4x4 ProjectionMatrix;
    public Vector2 DepthRange;

    // Controls
    public fixed byte Buttons[1];
    public fixed byte Toggles[1];
    public fixed byte Knobs[4];

    public void SetButtonData(int offset, int data)
      => Buttons[offset] = (byte)data;

    public void SetToggleData(int offset, int data)
      => Toggles[offset] = (byte)data;

    public void SetKnobData(int offset, float value)
      => Knobs[offset] = (byte)(value * 255);

    #endregion

    #region Serialization/deserialization

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

    #endregion
}

} // namespace Rcam2
