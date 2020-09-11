using UnityEngine;
using UnityEngine.VFX;

namespace Rcam2 {

public sealed class VfxActivator : MonoBehaviour
{
    enum ControlType { Button, Knob, Toggle }

    [SerializeField] ControlType _controlType = ControlType.Button;
    [SerializeField] int _controlNumber = 0;
    [SerializeField] float _delayTillOff = 1;

    bool CheckActive()
    {
        if (_controlType == ControlType.Button)
            return Singletons.InputHandle.GetButton(_controlNumber);
        if (_controlType == ControlType.Toggle)
            return Singletons.InputHandle.GetToggle(_controlNumber);
        // _controlType == ControlType.Knob
            return Singletons.InputHandle.GetKnob(_controlNumber) > 0.01f;
    }

    VisualEffect _vfx;
    float _delay;

    void Start()
      => _vfx = GetComponent<VisualEffect>();

    void Update()
    {
        _delay = CheckActive() ? 0 : _delay + Time.deltaTime;
        _vfx.enabled = (_delay <= _delayTillOff);
    }
}

} // namespace Rcam2
