using System.Runtime.InteropServices;

namespace Rcam2 {

//
// Rcam2 Input State struct
//
// This is a blittable struct for storing control (UI input) data.
//
[StructLayout(LayoutKind.Sequential)]
public unsafe struct InputState
{
    #region Data members

    fixed byte Buttons[1];
    fixed byte Toggles[1];
    fixed byte Knobs[4];

    #endregion

    #region Public accessor methods

    public byte GetButtonData(int offset)
      => Buttons[offset];

    public void SetButtonData(int offset, int data)
      => Buttons[offset] = (byte)data;

    public byte GetToggleData(int offset)
      => Toggles[offset];

    public void SetToggleData(int offset, int data)
      => Toggles[offset] = (byte)data;

    public float GetKnobData(int offset)
      => Knobs[offset];

    public void SetKnobData(int offset, float value)
      => Knobs[offset] = (byte)value;

    #endregion
}

} // namespace Rcam2
