using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

namespace Rcam2 {

//
// HDRP custom fullscreen pass for postprocessing recolor effects
//
[System.Serializable]
sealed class RcamRecolorPass : CustomPass
{
    #region Editable attributes

    public RcamRecolorController _controller = null;

    #endregion

    #region Runtime objects

    Material _material;

    #endregion

    #region CustomPass implementation

    protected override void Setup
      (ScriptableRenderContext renderContext, CommandBuffer cmd)
    {
        var shader = Resources.Load<Shader>("RcamRecolor");
        _material = new Material(shader);
        _material.hideFlags = HideFlags.DontSave;
    }

    protected override void Execute
      (ScriptableRenderContext renderContext, CommandBuffer cmd,
       HDCamera hdCamera, CullingResults cullingResult)
    {
        if (_controller == null) return;
        CoreUtils.DrawFullScreen(cmd, _material, _controller.PropertyBlock, 0);
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
