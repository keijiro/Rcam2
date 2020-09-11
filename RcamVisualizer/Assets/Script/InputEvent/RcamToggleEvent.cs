using UnityEngine;
using UnityEngine.Events;

namespace Rcam2 {

sealed class RcamToggleEvent : MonoBehaviour
{
    [System.Serializable] class BoolEvent : UnityEvent<bool> {}

    [SerializeField] InputHandle _input = null;
    [SerializeField] int _index = 0;
    [SerializeField] BoolEvent _event = null;

    bool _state;

    void Update()
    {
        var newState = _input.GetToggle(_index);
        if (newState != _state)
        {
            _event.Invoke(newState);
            _state = newState;
        }
    }
}

} // namespace Rcam2
