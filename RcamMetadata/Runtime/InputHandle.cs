using UnityEngine;

namespace Rcam2 {

//
// Rcam2 Input Handle class
//
// This is a wrapper class for accessing the control (UI input) data members in
// the Metadata class. We can use it to construct an input system with Events
// and Property Binders.
//
public sealed class InputHandle : MonoBehaviour
{
    #region Internal state data

    bool [] _buttons = new bool [4];
    bool [] _toggles = new bool [4];
    float [] _knobs = new float [4];

    #endregion

    #region Accessing by properties

    public bool Button0 { get => _buttons[0]; set => _buttons[0] = value; }
    public bool Button1 { get => _buttons[1]; set => _buttons[1] = value; }
    public bool Button2 { get => _buttons[2]; set => _buttons[2] = value; }
    public bool Button3 { get => _buttons[3]; set => _buttons[3] = value; }

    public bool Toggle0 { get => _toggles[0]; set => _toggles[0] = value; }
    public bool Toggle1 { get => _toggles[1]; set => _toggles[1] = value; }
    public bool Toggle2 { get => _toggles[2]; set => _toggles[2] = value; }
    public bool Toggle3 { get => _toggles[3]; set => _toggles[3] = value; }

    public float Knob0 { get => _knobs[0]; set => _knobs[0] = value; }
    public float Knob1 { get => _knobs[1]; set => _knobs[1] = value; }
    public float Knob2 { get => _knobs[2]; set => _knobs[2] = value; }
    public float Knob3 { get => _knobs[3]; set => _knobs[3] = value; }

    #endregion

    #region Accessing by methods

    public bool GetButton(int index) => _buttons[index];
    public void SetButton(int index, bool value) => _buttons[index] = value;

    public bool GetToggle(int index) => _toggles[index];
    public void SetToggle(int index, bool value) => _toggles[index] = value;

    public float GetKnob(int index) => _knobs[index];
    public void SetKnob(int index, float value) => _knobs[index] = value;

    #endregion

    #region Input State interface

    public InputState InputState
      { get => MakeInputState(); set => UpdateState(value); }

    InputState MakeInputState()
    {
        var state = new InputState();

        state.SetButtonData(0, (_buttons[0] ? 0b0001 : 0) +
                               (_buttons[1] ? 0b0010 : 0) +
                               (_buttons[2] ? 0b0100 : 0) +
                               (_buttons[3] ? 0b1000 : 0));

        state.SetToggleData(0, (_toggles[0] ? 0b0001 : 0) +
                               (_toggles[1] ? 0b0010 : 0) +
                               (_toggles[2] ? 0b0100 : 0) +
                               (_toggles[3] ? 0b1000 : 0));

        state.SetKnobData(0, (int)(_knobs[0] * 255));
        state.SetKnobData(1, (int)(_knobs[1] * 255));
        state.SetKnobData(2, (int)(_knobs[2] * 255));
        state.SetKnobData(3, (int)(_knobs[3] * 255));

        return state;
    }

    public void UpdateState(in InputState state)
    {
        var bf = state.GetButtonData(0);
        _buttons[0] = (bf & 0b0001) != 0;
        _buttons[1] = (bf & 0b0010) != 0;
        _buttons[2] = (bf & 0b0100) != 0;
        _buttons[3] = (bf & 0b1000) != 0;

        bf = state.GetToggleData(0);
        _toggles[0] = (bf & 0b0001) != 0;
        _toggles[1] = (bf & 0b0010) != 0;
        _toggles[2] = (bf & 0b0100) != 0;
        _toggles[3] = (bf & 0b1000) != 0;

        _knobs[0] = state.GetKnobData(0) / 255.0f;
        _knobs[1] = state.GetKnobData(1) / 255.0f;
        _knobs[2] = state.GetKnobData(2) / 255.0f;
        _knobs[3] = state.GetKnobData(3) / 255.0f;
    }

    #endregion
}

} // namespace Rcam2
