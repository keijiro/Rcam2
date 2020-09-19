using UnityEngine;

namespace Rcam2 {

sealed class CameraController : MonoBehaviour
{
    [SerializeField] Transform _originalRoot = null;
    [SerializeField] Transform _followerRoot = null;
    [SerializeField] Transform _randomPivot = null;
    [SerializeField] GameObject _pointRenderer = null;
    [SerializeField] GameObject _bgRenderer = null;

    int _kickCount;
    float _resetDelay;

    Quaternion RandomRotation
      => Quaternion.Slerp(Quaternion.identity, Random.rotation, 0.5f);

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            // Randomize the pivot.
            _randomPivot.localRotation = RandomRotation;

            // Check if this is the first kick.
            if (_kickCount == 0)
            {
                // Reset the follower at the current point.
                _followerRoot.position = transform.position;
                _followerRoot.rotation = transform.rotation;

                // Put the camera under the follower.
                transform.parent = _followerRoot;
                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.identity;

                _pointRenderer.SetActive(true);
                _bgRenderer.SetActive(false);
            }

            _kickCount++;
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            _randomPivot.localRotation = Quaternion.identity;
            _kickCount = 0;
            _resetDelay = 0.5f;
        }

        if (_kickCount == 0 && _resetDelay > 0)
        {
            _resetDelay -= Time.deltaTime;
            if (_resetDelay <= 0)
            {
                transform.parent = _originalRoot;
                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.identity;

                _pointRenderer.SetActive(false);
                _bgRenderer.SetActive(true);
            }
        }
    }
}

} // namespace Rcam2
