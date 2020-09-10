using UnityEngine;

namespace Rcam2 {

sealed class InputState : MonoBehaviour
{
    #region Internal data

    bool [] _buttons = new bool [4];
    bool [] _toggles = new bool [4];
    float [] _knobs = new float [4];

    #endregion

    #region Public accessor

    public bool GetButton(int index) => _buttons[index];
    public bool GetToggle(int index) => _toggles[index];
    public float GetKnob(int index) => _knobs[index];

    #endregion

    #region Public method

    public void LoadFrom(in Metadata metadata)
    {
        var data = metadata.GetButtonData(0);
        _buttons[0] = (data & 0b0001) != 0;
        _buttons[1] = (data & 0b0010) != 0;
        _buttons[2] = (data & 0b0100) != 0;
        _buttons[3] = (data & 0b1000) != 0;

        data = metadata.GetToggleData(0);
        _toggles[0] = (data & 0b0001) != 0;
        _toggles[1] = (data & 0b0010) != 0;
        _toggles[2] = (data & 0b0100) != 0;
        _toggles[3] = (data & 0b1000) != 0;

        _knobs[0] = metadata.GetKnobData(0);
        _knobs[1] = metadata.GetKnobData(1);
        _knobs[2] = metadata.GetKnobData(2);
        _knobs[3] = metadata.GetKnobData(3);
    }

    #endregion
}

} // namespace Rcam2
