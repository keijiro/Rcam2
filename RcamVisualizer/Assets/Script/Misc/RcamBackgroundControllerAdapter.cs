using UnityEngine;

namespace Rcam2 {

sealed class RcamBackgroundControllerAdapter : MonoBehaviour
{
    #region External object reference

    [SerializeField] RcamBackgroundController _target = null;

    #endregion

    #region Public properties

    public bool EnableBackRender
      { set => _enableBack = value; }

    public int EffectNumber
      { set => _target.FrontEffect =
                 (RcamBackgroundController.EffectType)value; }

    public float EffectParameter
      { set => _target.EffectParameter = value; }

    public float EffectIntensity
      { set => _target.EffectIntensity = value; }

    #endregion

    #region Back opacity animation

    bool _enableBack = true;

    void Update()
    {
        var delta = (_enableBack ? 1 : -1) * 10 * Time.deltaTime;
        _target.BackOpacity = Mathf.Clamp01(_target.BackOpacity + delta);
    }

    #endregion
}

} // namespace Rcam2
