using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

namespace Rcam2 {

[System.Serializable]
sealed class RcamBackgroundPass : CustomPass
{
    [SerializeField] RcamReceiver _receiver = null;

    Material _material;

    protected override void Setup
      (ScriptableRenderContext renderContext, CommandBuffer cmd)
    {
        var shader = Resources.Load<Shader>("RcamBackground");
        _material = new Material(shader);
        _material.hideFlags = HideFlags.DontSave;
    }

    protected override void Execute
      (ScriptableRenderContext renderContext, CommandBuffer cmd,
       HDCamera hdCamera, CullingResults cullingResult)
    {
        if (_receiver != null && _receiver.ColorTexture != null)
        {
            _material.SetTexture("_ColorTexture", _receiver.ColorTexture);
            _material.SetTexture("_DepthTexture", _receiver.DepthTexture);
            _material.SetTexture("_MaskTexture", _receiver.MaskTexture);
            CoreUtils.DrawFullScreen(cmd, _material, shaderPassId: 0);
        }
    }

    protected override void Cleanup()
    {
        if (Application.isPlaying)
            Object.Destroy(_material);
        else
            Object.DestroyImmediate(_material);
    }
}

} // namespace Rcam2
