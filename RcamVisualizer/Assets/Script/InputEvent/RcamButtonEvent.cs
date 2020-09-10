using UnityEngine;
using UnityEngine.Events;

namespace Rcam2 {

sealed class RcamButtonEvent : MonoBehaviour
{
    [SerializeField] InputState _input = null;
    [SerializeField] int _index = 0;
    [SerializeField] UnityEvent _onEvent = null;
    [SerializeField] UnityEvent _offEvent = null;

    bool _state;

    void Update()
    {
        var newState = _input.GetToggle(_index);
        if (newState != _state)
        {
            if (newState) _onEvent.Invoke(); else _offEvent.Invoke();
            _state = newState;
        }
    }
}

} // namespace Rcam2
