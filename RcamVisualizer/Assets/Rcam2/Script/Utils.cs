using UnityEngine;

namespace Rcam2 {

static class ShaderID
{
    public static readonly int DepthRange = Shader.PropertyToID("_DepthRange");
    public static readonly int ProjectionMatrix = Shader.PropertyToID("_ProjectionMatrix");
}

static class MatrixUtil
{
    // Fix the aspect ration difference between iPad Pro and 16:9
    public static Matrix4x4 FixProjectionAspectRatio(Matrix4x4 matrix)
    {
        matrix[1, 1] /= (2388.0f * 9) / (16 * 1668);
        return matrix;
    }
}

} // namespace Rcam2
