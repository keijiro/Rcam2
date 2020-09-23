using UnityEngine;
using Klak.Chromatics;

namespace Rcam2 {

//
// Controller and data provider for RcamBackgroundPass
//
sealed class RcamBackgroundController : MonoBehaviour
{
    #region Editable attributes

    [SerializeField] Gradient _effectGradient = null;

    #endregion

    #region Public properties

    public bool IsActive => true;
    public int PassNumber => EffectNumber;
    public int EffectNumber { get; set; }
    public bool BackFill { get; set; }
    public float EffectParameter { get; set; }
    public float EffectIntensity { get; set; }

    #endregion

    #region Private variables

    float _backOpacity;

    #endregion

    #region Material property block

    public MaterialPropertyBlock PropertyBlock => UpdatePropertyBlock();

    MaterialPropertyBlock _props;

    MaterialPropertyBlock UpdatePropertyBlock()
    {
        if (_props == null) _props = new MaterialPropertyBlock();

        var eparams = new Vector2(EffectParameter, EffectIntensity);
        _props.SetFloat("_BGOpacity", _backOpacity);
        _props.SetVector("_EffectParams", eparams);
        _props.SetLinearGradient("_EffectGradient", _effectGradient);

        return _props;
    }

    #endregion

    #region MonoBehaviour implementation

    void Update()
      => _backOpacity = Mathf.Clamp01
           (_backOpacity + (BackFill ? 1 : -1) * 10 * Time.deltaTime);

    #endregion
}

} // namespace Rcam2
