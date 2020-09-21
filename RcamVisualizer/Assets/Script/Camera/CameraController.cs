using UnityEngine;

namespace Rcam2 {

sealed class CameraController : MonoBehaviour
{
    #region Editable attributes

    [SerializeField] Transform _originalRoot = null;
    [SerializeField] Transform _followerRoot = null;
    [SerializeField] Transform _randomPivot = null;
    [SerializeField] GameObject _pointRenderer = null;
    [SerializeField] GameObject _bgRenderer = null;

    #endregion

    #region Public methods

    public void JumpRandomly()
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

            // Switch from the AR renderer to the point renderer.
            _pointRenderer.SetActive(true);
            _bgRenderer.SetActive(false);
        }

        _kickCount++;
    }

    public void ReturnToOriginalRoot()
    {
        _randomPivot.localRotation = Quaternion.identity;
        _kickCount = 0;
        _resetDelay = 0.5f;
    }

    #endregion

    #region Private fields

    int _kickCount;
    float _resetDelay;

    Quaternion RandomRotation
      => Quaternion.Slerp(Quaternion.identity, Random.rotation, 0.5f);

    #endregion

    #region MonoBehaviour implementation

    void Update()
    {
        if (_kickCount == 0 && _resetDelay > 0)
        {
            _resetDelay -= Time.deltaTime;

            if (_resetDelay <= 0)
            {
                // Return the camera under the original root.
                transform.parent = _originalRoot;
                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.identity;

                // Return to the AR renderer.
                _pointRenderer.SetActive(false);
                _bgRenderer.SetActive(true);
            }
        }
    }

    #endregion
}

} // namespace Rcam2
