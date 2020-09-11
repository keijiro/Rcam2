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

    bool [] _buttons = new bool [8];
    bool [] _toggles = new bool [8];
    float [] _knobs = new float [8];

    #endregion

    #region Accessing by properties

    public bool Button0 { get => _buttons[0]; set => _buttons[0] = value; }
    public bool Button1 { get => _buttons[1]; set => _buttons[1] = value; }
    public bool Button2 { get => _buttons[2]; set => _buttons[2] = value; }
    public bool Button3 { get => _buttons[3]; set => _buttons[3] = value; }
    public bool Button4 { get => _buttons[4]; set => _buttons[4] = value; }
    public bool Button5 { get => _buttons[5]; set => _buttons[5] = value; }
    public bool Button6 { get => _buttons[6]; set => _buttons[6] = value; }
    public bool Button7 { get => _buttons[7]; set => _buttons[7] = value; }

    public bool Toggle0 { get => _toggles[0]; set => _toggles[0] = value; }
    public bool Toggle1 { get => _toggles[1]; set => _toggles[1] = value; }
    public bool Toggle2 { get => _toggles[2]; set => _toggles[2] = value; }
    public bool Toggle3 { get => _toggles[3]; set => _toggles[3] = value; }
    public bool Toggle4 { get => _toggles[4]; set => _toggles[4] = value; }
    public bool Toggle5 { get => _toggles[5]; set => _toggles[5] = value; }
    public bool Toggle6 { get => _toggles[6]; set => _toggles[6] = value; }
    public bool Toggle7 { get => _toggles[7]; set => _toggles[7] = value; }

    public float Knob0 { get => _knobs[0]; set => _knobs[0] = value; }
    public float Knob1 { get => _knobs[1]; set => _knobs[1] = value; }
    public float Knob2 { get => _knobs[2]; set => _knobs[2] = value; }
    public float Knob3 { get => _knobs[3]; set => _knobs[3] = value; }
    public float Knob4 { get => _knobs[4]; set => _knobs[4] = value; }
    public float Knob5 { get => _knobs[5]; set => _knobs[5] = value; }
    public float Knob6 { get => _knobs[6]; set => _knobs[6] = value; }
    public float Knob7 { get => _knobs[7]; set => _knobs[7] = value; }

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

        state.SetButtonData(0, (_buttons[0] ? 0b00000001 : 0) +
                               (_buttons[1] ? 0b00000010 : 0) +
                               (_buttons[2] ? 0b00000100 : 0) +
                               (_buttons[3] ? 0b00001000 : 0) +
                               (_buttons[4] ? 0b00010000 : 0) +
                               (_buttons[5] ? 0b00100000 : 0) +
                               (_buttons[6] ? 0b01000000 : 0) +
                               (_buttons[7] ? 0b10000000 : 0));

        state.SetToggleData(0, (_toggles[0] ? 0b00000001 : 0) +
                               (_toggles[1] ? 0b00000010 : 0) +
                               (_toggles[2] ? 0b00000100 : 0) +
                               (_toggles[3] ? 0b00001000 : 0) +
                               (_toggles[4] ? 0b00010000 : 0) +
                               (_toggles[5] ? 0b00100000 : 0) +
                               (_toggles[6] ? 0b01000000 : 0) +
                               (_toggles[7] ? 0b10000000 : 0));

        state.SetKnobData(0, (int)(_knobs[0] * 255));
        state.SetKnobData(1, (int)(_knobs[1] * 255));
        state.SetKnobData(2, (int)(_knobs[2] * 255));
        state.SetKnobData(3, (int)(_knobs[3] * 255));
        state.SetKnobData(4, (int)(_knobs[4] * 255));
        state.SetKnobData(5, (int)(_knobs[5] * 255));
        state.SetKnobData(6, (int)(_knobs[6] * 255));
        state.SetKnobData(7, (int)(_knobs[7] * 255));

        return state;
    }

    public void UpdateState(in InputState state)
    {
        var bf = state.GetButtonData(0);
        _buttons[0] = (bf & 0b00000001) != 0;
        _buttons[1] = (bf & 0b00000010) != 0;
        _buttons[2] = (bf & 0b00000100) != 0;
        _buttons[3] = (bf & 0b00001000) != 0;
        _buttons[4] = (bf & 0b00010000) != 0;
        _buttons[5] = (bf & 0b00100000) != 0;
        _buttons[6] = (bf & 0b01000000) != 0;
        _buttons[7] = (bf & 0b10000000) != 0;

        bf = state.GetToggleData(0);
        _toggles[0] = (bf & 0b00000001) != 0;
        _toggles[1] = (bf & 0b00000010) != 0;
        _toggles[2] = (bf & 0b00000100) != 0;
        _toggles[3] = (bf & 0b00001000) != 0;
        _toggles[4] = (bf & 0b00010000) != 0;
        _toggles[5] = (bf & 0b00100000) != 0;
        _toggles[6] = (bf & 0b01000000) != 0;
        _toggles[7] = (bf & 0b10000000) != 0;

        _knobs[0] = state.GetKnobData(0) / 255.0f;
        _knobs[1] = state.GetKnobData(1) / 255.0f;
        _knobs[2] = state.GetKnobData(2) / 255.0f;
        _knobs[3] = state.GetKnobData(3) / 255.0f;
        _knobs[4] = state.GetKnobData(4) / 255.0f;
        _knobs[5] = state.GetKnobData(5) / 255.0f;
        _knobs[6] = state.GetKnobData(6) / 255.0f;
        _knobs[7] = state.GetKnobData(7) / 255.0f;
    }

    #endregion
}

} // namespace Rcam2
