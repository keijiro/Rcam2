using UnityEngine;
using Klak.Chromatics;

namespace Rcam2 {

//
// Controller and data provider for RcamBackgroundPass
//
sealed class RcamBackgroundController : MonoBehaviour
{
    #region Enum declaration

    public enum EffectType { Off, Slit, Marble, Slice, Displace }

    #endregion

    #region Editable attributes

    [Space]
    [SerializeField, Range(0, 1)] float _bgOpacity = 1;
    [Space]
    [SerializeField] EffectType _effectType = EffectType.Off;
    [SerializeField, Range(0, 1)] float _effectParameter = 0;
    [SerializeField, Range(0, 1)] float _effectIntensity = 0;
    [SerializeField] Gradient _effectGradient = null;

    #endregion

    #region Public properties

    public bool IsActive => true;
    public int PassNumber => (int)_effectType;

    #endregion

    #region Material property block

    public MaterialPropertyBlock PropertyBlock => UpdatePropertyBlock();

    MaterialPropertyBlock _props;

    MaterialPropertyBlock UpdatePropertyBlock()
    {
        if (_props == null) _props = new MaterialPropertyBlock();

        var eparams = new Vector2(_effectParameter, _effectIntensity);
        _props.SetFloat("_BGOpacity", _bgOpacity);
        _props.SetVector("_EffectParams", eparams);
        _props.SetLinearGradient("_EffectGradient", _effectGradient);

        return _props;
    }

    #endregion
}

} // namespace Rcam2
