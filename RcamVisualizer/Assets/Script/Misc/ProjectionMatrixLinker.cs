using UnityEngine;

namespace Rcam2 {

sealed class ProjectionMatrixLinker : MonoBehaviour
{
    Camera _camera;

    void Start()
      => _camera = GetComponent<Camera>();

    void Update()
      => _camera.projectionMatrix = Singletons.Receiver.ProjectionMatrix;
}

} // namespace Rcam2
