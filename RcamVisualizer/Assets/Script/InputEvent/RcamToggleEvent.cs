using UnityEngine;
using UnityEngine.Events;

namespace Rcam2 {

sealed class RcamToggleEvent : MonoBehaviour
{
    [System.Serializable] class BoolEvent : UnityEvent<bool> {}

    [SerializeField] int _controlNumber = 0;
    [SerializeField] BoolEvent _event = null;

    bool _state;

    void Update()
    {
        var newState = Singletons.InputHandle.GetToggle(_controlNumber);
        if (newState != _state)
        {
            _event.Invoke(newState);
            _state = newState;
        }
    }
}

} // namespace Rcam2
