using UnityEngine;
using Klak.Chromatics;

namespace Rcam2 {

//
// Controller and data provider for RcamRecolorPass
//
sealed class RcamRecolorController : MonoBehaviour
{
    #region Editable attributes

    [SerializeField, Range(0, 1)] float _dithering = 0.5f;
    [SerializeField] float _lineThreshold = 0.5f;
    [SerializeField] float _lineContrast = 1;

    #endregion

    #region Public members

    public bool IsActive => _backOpacity > 0 || _frontOpacity > 0;
    public bool BackFill { get; set; }
    public bool FrontFill { get; set; }
    public void ShuffleColors() => RandomizeGradientsAndColors();

    #endregion

    #region Private variables

    float _backOpacity;
    float _frontOpacity;

    Gradient _backGradient = new Gradient();
    Gradient _frontGradient = new Gradient();
    Color _lineColor;

    #endregion

    #region Random color scheme

    GradientColorKey [] _colorKeys = new GradientColorKey[3];
    GradientAlphaKey [] _alphaKeys =
      new GradientAlphaKey[] { new GradientAlphaKey(1, 0) };

    void RandomizeGradientsAndColors()
    {
        var h1 = Random.value;
        var h2 = (h1 + 0.333f) % 1;

        var h3 = Random.value;
        var h4 = (h3 + 0.333f) % 1;

        var bg1 = Color.black;
        var bg2 = Color.HSVToRGB(h1, 1, 0.5f);
        var bg3 = Color.HSVToRGB(h2, 1, 0.8f);

        var fg1 = Color.HSVToRGB(h3, 1, 0.3f);
        var fg2 = Color.HSVToRGB(h4, 1, 1.0f);
        var fg3 = Color.white;

        _colorKeys[0] = new GradientColorKey(bg1, 0.5f);
        _colorKeys[1] = new GradientColorKey(bg2, 0.75f);
        _colorKeys[2] = new GradientColorKey(bg3, 1);
        _backGradient.SetKeys(_colorKeys, _alphaKeys);
        _backGradient.mode = GradientMode.Fixed;

        _colorKeys[0] = new GradientColorKey(fg1, 0.5f);
        _colorKeys[1] = new GradientColorKey(fg2, 0.75f);
        _colorKeys[2] = new GradientColorKey(fg3, 1);
        _frontGradient.SetKeys(_colorKeys, _alphaKeys);
        _frontGradient.mode = GradientMode.Fixed;

        _lineColor = Color.white;
    }

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

    #region MonoBehaviour implementation

    void Start()
      => RandomizeGradientsAndColors();

    void Update()
    {
        var delta = Time.deltaTime * 10;
         _backOpacity = Mathf.Clamp01( _backOpacity + ( BackFill ? 1 : -1) * delta);
        _frontOpacity = Mathf.Clamp01(_frontOpacity + (FrontFill ? 1 : -1) * delta);
    }

    #endregion
}

} // namespace Rcam2
