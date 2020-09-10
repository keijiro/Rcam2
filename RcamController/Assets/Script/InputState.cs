using UnityEngine;

namespace Rcam2 {

sealed class InputState : MonoBehaviour
{
    #region Exposed accessor for use in GUI

    public bool Button1 { get; set; }
    public bool Button2 { get; set; }
    public bool Button3 { get; set; }
    public bool Button4 { get; set; }

    public bool Toggle1 { get; set; }
    public bool Toggle2 { get; set; }
    public bool Toggle3 { get; set; }
    public bool Toggle4 { get; set; }

    public float Knob1 { get; set; }
    public float Knob2 { get; set; }
    public float Knob3 { get; set; }
    public float Knob4 { get; set; }

    #endregion

    #region Public method

    public void LoadTo(ref Metadata metadata)
    {
        metadata.SetButtonData(0, (Button1 ? 0b0001 : 0) +
                                  (Button2 ? 0b0010 : 0) +
                                  (Button3 ? 0b0100 : 0) +
                                  (Button4 ? 0b1000 : 0));

        metadata.SetToggleData(0, (Toggle1 ? 0b0001 : 0) +
                                  (Toggle2 ? 0b0010 : 0) +
                                  (Toggle3 ? 0b0100 : 0) +
                                  (Toggle4 ? 0b1000 : 0));

        metadata.SetKnobData(0, Knob1);
        metadata.SetKnobData(1, Knob2);
        metadata.SetKnobData(2, Knob3);
        metadata.SetKnobData(3, Knob4);
    }

    #endregion
}

} // namespace Rcam2
