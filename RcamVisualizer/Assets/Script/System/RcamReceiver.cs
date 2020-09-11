using UnityEngine;
using Klak.Ndi;

namespace Rcam2 {

//
// Receiver class that extracts image components and
// control data from NDI frames
//
sealed class RcamReceiver : MonoBehaviour
{
    #region External scene object references

    [SerializeField] NdiReceiver _ndiReceiver = null;

    #endregion

    #region Embedded asset refnerence

    [SerializeField, HideInInspector] Shader _demuxShader = null;

    #endregion

    #region Public accessor properties

    public RenderTexture ColorTexture => _textures.color;
    public RenderTexture DepthTexture => _textures.depth;
    public Matrix4x4 ProjectionMatrix => _metadata.ProjectionMatrix;

    #endregion

    #region Runtime objects

    (RenderTexture color, RenderTexture depth) _textures;
    Metadata _metadata;
    Material _demuxMaterial;

    #endregion

    #region MonoBehaviour implementation

    void Start()
      => _demuxMaterial = new Material(_demuxShader);

    void OnDestroy()
    {
        Destroy(_textures.color);
        Destroy(_textures.depth);
        Destroy(_demuxMaterial);
    }

    void Update()
    {
        RetrieveAndApplyMetadata();
        ExtractTextures();
    }

    #endregion

    #region Metadata extraction

    void RetrieveAndApplyMetadata()
    {
        // Deserialization
        var xml = _ndiReceiver.metadata;
        if (xml == null || xml.Length == 0) return;
        _metadata = Metadata.Deserialize(xml);

        // Camera update with the metadata
        var camera = Singletons.MainCamera;
        camera.projectionMatrix = _metadata.ProjectionMatrix;
        camera.transform.position = _metadata.CameraPosition;
        camera.transform.rotation = _metadata.CameraRotation;

        // Input state update with the metadata
        Singletons.InputHandle.InputState = _metadata.InputState;
    }

    #endregion

    #region Image extraction

    void ExtractTextures()
    {
        var source = _ndiReceiver.texture;
        if (source == null) return;

        // Lazy initialization
        if (_textures.color == null) InitializeTextures(source);

        // Parameters from metadata
        _demuxMaterial.SetVector(ShaderID.DepthRange, _metadata.DepthRange);

        // Blit (color/depth)
        Graphics.Blit(source, _textures.color, _demuxMaterial, 0);
        Graphics.Blit(source, _textures.depth, _demuxMaterial, 1);
    }

    void InitializeTextures(RenderTexture source)
    {
        var w = source.width / 2;
        var h = source.height / 2;
        _textures.color = new RenderTexture(w, h * 2, 0);
        _textures.depth = new RenderTexture(w, h, 0, RenderTextureFormat.RHalf);
    }

    #endregion
}

} // namespace Rcam2
