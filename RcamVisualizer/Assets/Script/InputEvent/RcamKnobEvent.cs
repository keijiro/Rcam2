using UnityEngine;
using UnityEngine.Events;

namespace Rcam2 {

sealed class RcamKnobEvent : MonoBehaviour
{
    [System.Serializable] class FloatEvent : UnityEvent<float> {}

    [SerializeField] int _controlNumber = 0;
    [SerializeField] FloatEvent _event = null;

    float _value;

    void Update()
    {
        var newValue = Singletons.InputHandle.GetKnob(_controlNumber);
        if (newValue != _value)
        {
            _event.Invoke(newValue);
            _value = newValue;
        }
    }
}

} // namespace Rcam2
