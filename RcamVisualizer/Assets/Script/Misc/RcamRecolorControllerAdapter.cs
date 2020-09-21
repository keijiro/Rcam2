using UnityEngine;

namespace Rcam2 {

sealed class RcamRecolorControllerAdapter : MonoBehaviour
{
    #region External object reference

    [SerializeField] RcamRecolorController _target = null;

    #endregion

    #region Public properties

    public bool EnableBackEffect { set => _enableBack = value; }
    public bool EnableFrontEffect { set => _enableFront = value; }

    #endregion

    #region Back opacity animation

    bool _enableBack;
    bool _enableFront;

    void Update()
    {
        var dt = Time.deltaTime;

        {
            var delta = (_enableBack ? 1 : -1) * 10 * dt;
            _target.BackOpacity = Mathf.Clamp01(_target.BackOpacity + delta);
        }

        {
            var delta = (_enableFront ? 1 : -1) * 10 * dt;
            _target.FrontOpacity = Mathf.Clamp01(_target.FrontOpacity + delta);
        }
    }

    #endregion
}

} // namespace Rcam2
