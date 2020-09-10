using UnityEngine;
using UnityEngine.Events;

namespace Rcam2 {

sealed class RcamKnobEvent : MonoBehaviour
{
    [System.Serializable] class FloatEvent : UnityEvent<float> {}

    [SerializeField] InputState _input = null;
    [SerializeField] int _index = 0;
    [SerializeField] FloatEvent _event = null;

    float _value;

    void Update()
    {
        var newValue = _input.GetKnob(_index);
        if (newValue != _value)
        {
            _event.Invoke(newValue);
            _value = newValue;
        }
    }
}

} // namespace Rcam2
