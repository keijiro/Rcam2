using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

namespace Rcam2 {

//
// HDRP custom fullscreen pass for drawing camera images
//
[System.Serializable]
sealed class RcamBackgroundPass : CustomPass
{
    #region External scene object references

    [SerializeField] RcamReceiver _receiver = null;

    #endregion

    #region Runtime objects

    Material _material;

    #endregion

    #region CustomPass implementation

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
        if (_receiver == null || _receiver.ColorTexture == null) return;
        _material.SetTexture("_ColorTexture", _receiver.ColorTexture);
        _material.SetTexture("_DepthTexture", _receiver.DepthTexture);
        CoreUtils.DrawFullScreen(cmd, _material, null, 0);
    }

    protected override void Cleanup()
    {
        if (Application.isPlaying)
            Object.Destroy(_material);
        else
            Object.DestroyImmediate(_material);
    }

    #endregion
}

} // namespace Rcam2
