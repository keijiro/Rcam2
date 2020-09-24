using UnityEngine;
using Klak.Chromatics;

namespace Rcam2 {

//
// Controller and data provider for RcamBackgroundPass
//
sealed class RcamBackgroundController : MonoBehaviour
{
    #region Public properties

    public bool IsActive => true;
    public int PassNumber => _currentEffect;
    public bool BackFill { get; set; } = true;
    public int EffectNumber { get; set; }
    public float EffectDirection { get; set; }
    public float EffectParameter { get; set; }
    public float EffectIntensity { get; set; }

    #endregion

    #region Private variables

    float _backOpacity;
    float _effectOpacity;
    int _currentEffect;

    #endregion

    #region Material property block

    public MaterialPropertyBlock PropertyBlock => UpdatePropertyBlock();

    MaterialPropertyBlock _props;

    MaterialPropertyBlock UpdatePropertyBlock()
    {
        if (_props == null) _props = new MaterialPropertyBlock();

        var oparams = new Vector2(_backOpacity, _effectOpacity);
        var phi = EffectDirection * Mathf.PI * 2;
        var eparams = new Vector4
          (EffectParameter, EffectIntensity, Mathf.Sin(phi), Mathf.Cos(phi));

        _props.SetVector("_Opacity", oparams);
        _props.SetVector("_EffectParams", eparams);

        return _props;
    }

    #endregion

    #region MonoBehaviour implementation

    void Update()
    {
        var delta = Time.deltaTime * 10;

        // BG opacity animation
        var dir = BackFill ? 1 : -1;
        _backOpacity = Mathf.Clamp01(_backOpacity + dir * delta);

        // Effect opacity animation
        dir = _currentEffect == EffectNumber ? 1 : -1;
        _effectOpacity = Mathf.Clamp01(_effectOpacity + dir * delta);

        // We can switch the effect when the opacity becomes zero.
        if (_currentEffect != EffectNumber && _effectOpacity == 0)
            _currentEffect = EffectNumber;
    }

    #endregion
}

} // namespace Rcam2
