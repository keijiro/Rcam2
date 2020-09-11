using UnityEngine;

namespace Rcam2 {

static class ShaderID
{
    public static readonly int DepthRange = Shader.PropertyToID("_DepthRange");
    public static readonly int ProjectionMatrix = Shader.PropertyToID("_ProjectionMatrix");
}

static class Singletons
{
    static Camera _mainCamera;

    public static Camera MainCamera
      => _mainCamera != null ? _mainCamera : (_mainCamera = Camera.main);

    static RcamReceiver _receiver;

    public static RcamReceiver Receiver
      => _receiver != null ? _receiver :
           (_receiver = Object.FindObjectOfType<RcamReceiver>());

    static InputHandle _inputHandle;

    public static InputHandle InputHandle
      => _inputHandle != null ? _inputHandle :
           (_inputHandle = Object.FindObjectOfType<InputHandle>());
}

} // namespace Rcam2
