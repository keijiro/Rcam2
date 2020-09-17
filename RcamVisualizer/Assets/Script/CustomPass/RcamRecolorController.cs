using UnityEngine;
using Klak.Chromatics;

namespace Rcam2 {

//
// Controller and data provider for RcamRecolorPass
//
sealed class RcamRecolorController : MonoBehaviour
{
    #region Public properties

    [Space]
    [SerializeField] Gradient _backGradient = null;
    [SerializeField] Gradient _frontGradient = null;
    [SerializeField, Range(0, 1)] float _dithering = 0.5f;
    [Space]
    [SerializeField] Color _lineColor = Color.black;
    [SerializeField] float _lineThreshold = 0.5f;
    [SerializeField] float _lineContrast = 1;

    #endregion

    #region Material property block

    public MaterialPropertyBlock PropertyBlock => UpdatePropertyBlock();

    MaterialPropertyBlock _props;

    MaterialPropertyBlock UpdatePropertyBlock()
    {
        if (_props == null) _props = new MaterialPropertyBlock();

        _props.SetLinearGradient("_BackGradient", _backGradient);
        _props.SetLinearGradient("_FrontGradient", _frontGradient);
        _props.SetFloat("_DitherStrength", _dithering);

        _props.SetColor("_LineColor", _lineColor);
        _props.SetVector("_LineParams", new Vector2(_lineThreshold, _lineContrast));

        return _props;
    }

    #endregion

}

} // namespace Rcam2
