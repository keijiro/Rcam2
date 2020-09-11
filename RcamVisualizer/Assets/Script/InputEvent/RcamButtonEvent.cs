using UnityEngine;
using UnityEngine.Events;

namespace Rcam2 {

sealed class RcamButtonEvent : MonoBehaviour
{
    [SerializeField] int _controlNumber = 0;
    [SerializeField] UnityEvent _onEvent = null;
    [SerializeField] UnityEvent _offEvent = null;

    bool _state;

    void Update()
    {
        var newState = Singletons.InputHandle.GetToggle(_controlNumber);
        if (newState != _state)
        {
            if (newState) _onEvent.Invoke(); else _offEvent.Invoke();
            _state = newState;
        }
    }
}

} // namespace Rcam2
