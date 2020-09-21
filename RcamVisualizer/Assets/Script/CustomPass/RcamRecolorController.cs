using UnityEngine;
using Klak.Chromatics;

namespace Rcam2 {

//
// Controller and data provider for RcamRecolorPass
//
sealed class RcamRecolorController : MonoBehaviour
{
    #region Editable attributes

    [Space]
    [SerializeField] Gradient _backGradient = null;
    [SerializeField] Gradient _frontGradient = null;
    [SerializeField, Range(0, 1)] float _backOpacity = 0;
    [SerializeField, Range(0, 1)] float _frontOpacity = 0;
    [SerializeField, Range(0, 1)] float _dithering = 0.5f;
    [Space]
    [SerializeField] Color _lineColor = Color.black;
    [SerializeField] float _lineThreshold = 0.5f;
    [SerializeField] float _lineContrast = 1;

    #endregion

    #region Public properties

    public bool IsActive => _backOpacity > 0 || _frontOpacity > 0;

    public float BackOpacity
      { get => _backOpacity; set => _backOpacity = value; }

    public float FrontOpacity
      { get => _frontOpacity; set => _frontOpacity = value; }

    #endregion

    #region Material property block

    public MaterialPropertyBlock PropertyBlock => UpdatePropertyBlock();

    MaterialPropertyBlock _props;

    MaterialPropertyBlock UpdatePropertyBlock()
    {
        if (_props == null) _props = new MaterialPropertyBlock();

        var fillParams = new Vector3(_backOpacity, _frontOpacity, _dithering);
        var lineParams = new Vector2(_lineThreshold, _lineContrast);

        _props.SetLinearGradient("_BackGradient", _backGradient);
        _props.SetLinearGradient("_FrontGradient", _frontGradient);
        _props.SetVector("_FillParams", fillParams);

        _props.SetColor("_LineColor", _lineColor);
        _props.SetVector("_LineParams", lineParams);

        return _props;
    }

    #endregion
}

} // namespace Rcam2
