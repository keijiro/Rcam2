using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using Klak.Ndi;

namespace Rcam2 {

sealed class Controller : MonoBehaviour
{
    #region Editable attributes

    [Space]
    [SerializeField] Transform _cameraTransform = null;
    [SerializeField] ARCameraManager _cameraManager = null;
    [SerializeField] ARCameraBackground _cameraBackground = null;
    [SerializeField] AROcclusionManager _occlusionManager = null;
    [Space]
    [SerializeField] float _minDepth = 0.2f;
    [SerializeField] float _maxDepth = 3.2f;
    [Space]
    [SerializeField] Text _statusText = null;

    #endregion

    #region Hidden external asset reference

    [SerializeField, HideInInspector] NdiResources _ndiResources = null;
    [SerializeField, HideInInspector] Shader _shader = null;

    #endregion

    #region Shader IDs

    static class ShaderID
    {
        public static int Y = Shader.PropertyToID("_textureY");
        public static int CbCr = Shader.PropertyToID("_textureCbCr");
        public static int Mask = Shader.PropertyToID("_HumanStencil");
        public static int Depth = Shader.PropertyToID("_EnvironmentDepth");
        public static int Range = Shader.PropertyToID("_DepthRange");
    }

    #endregion

    #region Internal-use objects

    const int _width = 2048;
    const int _height = 1024;

    NdiSender _ndiSender;

    Matrix4x4 _projection;

    Material _bgMaterial;
    Material _muxMaterial;

    RenderTexture _senderRT;

    #endregion

    #region Internal-use properties and methods

    string StatusText => MakeStatusText();

    string MakeStatusText()
    {
        var pos = _cameraTransform.position;
        var rot = _cameraTransform.rotation.eulerAngles;

        var text = $"Position: ({pos.x}, {pos.y}, {pos.z})\n";
        text += $"Rotation: ({rot.x}, {rot.y}, {rot.z})\n";
        text += $"Projection: {_projection}";

        return text;
    }

    Metadata BuildMetadata()
      => new Metadata { CameraPosition = _cameraTransform.position,
                        CameraRotation = _cameraTransform.rotation,
                        DepthRange = new Vector2(_minDepth, _maxDepth),
                        ProjectionMatrix = _projection };

    #endregion

    #region Camera events

    void OnCameraFrameReceived(ARCameraFrameEventArgs args)
    {
        for (var i = 0; i < args.textures.Count; i++)
        {
            var id = args.propertyNameIds[i];
            var tex = args.textures[i];
            if (id == ShaderID.Y)
                _muxMaterial.SetTexture(ShaderID.Y, tex);
            else if (id == ShaderID.CbCr)
                _muxMaterial.SetTexture(ShaderID.CbCr, tex);
        }

        if (args.projectionMatrix.HasValue)
            _projection = args.projectionMatrix.Value;
    }

    void OnOcclusionFrameReceived(AROcclusionFrameEventArgs args)
    {
        for (var i = 0; i < args.textures.Count; i++)
        {
            var id = args.propertyNameIds[i];
            var tex = args.textures[i];
            if (id == ShaderID.Mask )
                _muxMaterial.SetTexture(ShaderID.Mask, tex);
            else if (id == ShaderID.Depth)
                _muxMaterial.SetTexture(ShaderID.Depth, tex);
        }
    }

    #endregion

    #region MonoBehaviour implementation

    void Start()
    {
        // Shader setup
        _bgMaterial = new Material(_shader);
        _bgMaterial.EnableKeyword("RCAM_MONITOR");

        _muxMaterial = new Material(_shader);
        _muxMaterial.EnableKeyword("RCAM_MULTIPLEXER");

        // Custom background material
        _cameraBackground.customMaterial = _bgMaterial;
        _cameraBackground.useCustomMaterial = true;

        // Render texture as NDI source
        _senderRT = new RenderTexture(_width, _height, 0);
        _senderRT.Create();

        // NDI sender instantiation
        _ndiSender = gameObject.AddComponent<NdiSender>();
        _ndiSender.SetResources(_ndiResources);
        _ndiSender.ndiName = "Rcam";
        _ndiSender.captureMethod = CaptureMethod.Texture;
        _ndiSender.sourceTexture = _senderRT;
    }

    void OnDestroy()
    {
        Destroy(_bgMaterial);
        Destroy(_muxMaterial);
        Destroy(_senderRT);
    }

    void OnEnable()
    {
        _cameraManager.frameReceived += OnCameraFrameReceived;
        _occlusionManager.frameReceived += OnOcclusionFrameReceived;
    }

    void OnDisable()
    {
        _cameraManager.frameReceived -= OnCameraFrameReceived;
        _occlusionManager.frameReceived -= OnOcclusionFrameReceived;
    }

    void Update()
    {
        _statusText.text = StatusText;

        // Parameter update
        var range = new Vector2(_minDepth, _maxDepth);
        _bgMaterial.SetVector(ShaderID.Range, range);
        _muxMaterial.SetVector(ShaderID.Range, range);

        // NDI sender RT update
        Graphics.Blit(null, _senderRT, _muxMaterial, 0);

        // Metadata
        _ndiSender.metadata = BuildMetadata().Serialize();
    }

    #endregion
}

} // namespace Rcam2
