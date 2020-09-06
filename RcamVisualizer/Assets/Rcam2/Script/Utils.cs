using UnityEngine;

namespace Rcam2 {

static class ShaderID
{
    public static readonly int DepthRange = Shader.PropertyToID("_DepthRange");
    public static readonly int ProjectionMatrix = Shader.PropertyToID("_ProjectionMatrix");
}

} // namespace Rcam2
