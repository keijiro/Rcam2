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
    [SerializeField] AROcclusionManager _occlusionManager = null;
    [Space]
    [SerializeField] int _width = 1920;
    [SerializeField] int _height = 1080;
    [SerializeField] int _frameRate = 60;
    [Space]
    [SerializeField] Text _statusText = null;
    [SerializeField] RawImage _monitorImage = null;

    #endregion

    #region Hidden external asset reference

    [SerializeField, HideInInspector] NdiResources _ndiResources = null;
    [SerializeField, HideInInspector] Shader _monitorShader = null;

    #endregion

    #region Internal-use properties

    string StatusText => MakeStatusText();

    string MakeStatusText()
    {
        var pos = _cameraTransform.position;
        var rot = _cameraTransform.rotation.eulerAngles;

        var text = $"Position: ({pos.x}, {pos.y}, {pos.z})\n";
        text += $"Rotation: ({rot.x}, {rot.y}, {rot.z})";

        return text;
    }

    #endregion

    #region Shader IDs

    static class ShaderID
    {
        public static int Y = Shader.PropertyToID("_textureY");
        public static int CbCr = Shader.PropertyToID("_textureCbCr");
        public static int Stencil = Shader.PropertyToID("_HumanStencil");
        public static int Depth = Shader.PropertyToID("_EnvironmentDepth");
    }

    #endregion

    #region Camera events

    (Texture2D y, Texture2D cbcr, Texture2D stencil, Texture2D depth) _textures;

    void OnCameraFrameReceived(ARCameraFrameEventArgs args)
    {
        for (var i = 0; i < args.textures.Count; i++)
        {
            var id = args.propertyNameIds[i];
            if (id == ShaderID.Y   ) _textures.y    = args.textures[i];
            if (id == ShaderID.CbCr) _textures.cbcr = args.textures[i];
        }
    }

    void OnOcclusionFrameReceived(AROcclusionFrameEventArgs args)
    {
        for (var i = 0; i < args.textures.Count; i++)
        {
            var id = args.propertyNameIds[i];
            if (id == ShaderID.Stencil) _textures.stencil = args.textures[i];
            if (id == ShaderID.Depth  ) _textures.depth   = args.textures[i];
        }
    }

    #endregion

    #region Configurator

    bool _configured;

    void ApplyConfiguration()
    {
        using (var configs = _cameraManager.GetConfigurations(Allocator.Temp))
        {
            if (!configs.IsCreated || configs.Length <= 0) return;

            foreach (var config in configs)
            {
                if (config.width != _width) continue;
                if (config.height != _height) continue;
                if (config.framerate.Value != _frameRate) continue;
                _cameraManager.currentConfiguration = config;
                break;
            }

            _configured = true;
        }
    }

    #endregion

    #region MonoBehaviour implementation

    RenderTexture _monitorRT;
    Material _monitorMaterial;

    void Start()
    {
        _monitorRT = new RenderTexture(_width, _height, 0);
        _monitorRT.Create();

        _monitorMaterial = new Material(_monitorShader);
        _monitorImage.material = _monitorMaterial;

        var sender = gameObject.AddComponent<NdiSender>();
        sender.SetResources(_ndiResources);
        sender.ndiName = "Rcam";
        sender.captureMethod = CaptureMethod.Texture;
        sender.sourceTexture = _monitorRT;
    }

    void OnDestroy()
    {
        Destroy(_monitorRT);
        Destroy(_monitorMaterial);
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
        if (!_configured)
        {
            ApplyConfiguration();
        }
        else
        {
            _statusText.text = StatusText;

            _monitorMaterial.SetTexture(ShaderID.Y      , _textures.y      );
            _monitorMaterial.SetTexture(ShaderID.CbCr   , _textures.cbcr   );
            _monitorMaterial.SetTexture(ShaderID.Stencil, _textures.stencil);
            _monitorMaterial.SetTexture(ShaderID.Depth  , _textures.depth  );

            Graphics.Blit(null, _monitorRT, _monitorMaterial, 0);
        }
    }

    #endregion
}

} // namespace Rcam2
