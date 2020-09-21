using UnityEngine;
using Kino;

namespace Rcam2 {

sealed class FeedbackEffectControllerAdapter : MonoBehaviour
{
    #region External object reference

    [SerializeField] FeedbackEffectController _target = null;
    [SerializeField] Gradient _tintGradient = null;
    [SerializeField] AnimationCurve _scaleCurve = null;
    [SerializeField] AnimationCurve _rotationCurve = null;
    [SerializeField] AnimationCurve _offsetCurve = null;

    #endregion

    #region Public properties

    public float Throttle { set => SetThrottle(value); }
    public float Rotation { set => SetRotation(value); }
    public float Offset { set => SetOffset(value); }

    #endregion

    #region Accessor implementation

    void SetThrottle(float value)
    {
        _target.scale = _scaleCurve.Evaluate(value);
        _target.tint = _tintGradient.Evaluate(value);
        _target.enabled = value > 0.01f;
    }

    void SetRotation(float value)
      => _target.rotation = _rotationCurve.Evaluate(value);

    void SetOffset(float value)
      => _target.offsetX = _offsetCurve.Evaluate(value);

    #endregion
}

} // namespace Rcam2
