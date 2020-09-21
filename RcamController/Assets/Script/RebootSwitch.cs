using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Rcam2 {

sealed class RebootSwitch : MonoBehaviour
{
    [SerializeField] float _delay = 3;
    [SerializeField] Text _text = null;

    public bool Hold { get; set; }

    float _time;

    void Update()
    {
        _time = Hold ? _time - Time.deltaTime : _delay;

        _text.text = Hold ? $"Hold To Reboot {_time:F2}" : "";

        if (_time <= 0) SceneManager.LoadScene(0);
    }
}

} // namespace Rcam2
