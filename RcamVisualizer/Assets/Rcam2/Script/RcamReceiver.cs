using UnityEngine;
using Klak.Ndi;

namespace Rcam2 {

//
// Receiver class that extracts image components and
// control data from NDI frames
//
sealed class RcamReceiver : MonoBehaviour
{
    #region External object references

    [SerializeField] NdiReceiver _ndiReceiver = null;
    [SerializeField] Camera _mainCamera = null;

    #endregion

    #region Embedded asset refnerence

    [SerializeField, HideInInspector] Shader _demuxShader = null;

    #endregion

    #region Extracted image components

    (RenderTexture color, RenderTexture depth, RenderTexture mask) _textures;

    public RenderTexture ColorTexture => _textures.color;
    public RenderTexture DepthTexture => _textures.depth;
    public RenderTexture MaskTexture => _textures.mask;

    #endregion

    #region Private objects

    RenderBuffer[] _mrt = new RenderBuffer[2];
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
        Destroy(_textures.mask);
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
        var xml = _ndiReceiver.metadata;
        if (xml == null || xml.Length == 0) return;

        _metadata = Metadata.Deserialize(xml);

        var proj = _metadata.ProjectionMatrix;
        proj[1, 1] /= (2388.0f * 9) / (16 * 1668);

        _mainCamera.projectionMatrix = proj;
        _mainCamera.transform.position = _metadata.CameraPosition;
        _mainCamera.transform.rotation = _metadata.CameraRotation;
    }

    #endregion

    #region Image extraction

    void ExtractTextures()
    {
        var source = _ndiReceiver.texture;
        if (source == null) return;

        if (_textures.color == null) InitializeTextures(source);

        _demuxMaterial.SetVector("_DepthRange", _metadata.DepthRange);

        Graphics.Blit(source, _textures.color, _demuxMaterial, 0);

        _mrt[0] = _textures.depth.colorBuffer;
        _mrt[1] = _textures.mask.colorBuffer;

        Graphics.SetRenderTarget(_mrt, _textures.depth.depthBuffer);
        Graphics.Blit(source, _demuxMaterial, 1);
    }

    void InitializeTextures(RenderTexture source)
    {
        var w = source.width;
        var h = source.height;

        _textures.color = new RenderTexture(w / 2, h, 0);

        _textures.depth =
          new RenderTexture(w / 2, h / 2, 0, RenderTextureFormat.RHalf);

        _textures.mask =
          new RenderTexture(w / 2, h / 2, 0, RenderTextureFormat.R8);
    }

    #endregion
}

} // namespace Rcam2
