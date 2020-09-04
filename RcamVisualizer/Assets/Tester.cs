using UnityEngine;
using Klak.Ndi;

namespace Rcam2 {

class Tester : MonoBehaviour
{
    [SerializeField] NdiReceiver _ndiReceiver = null;

    void Update()
    {
        var xml = _ndiReceiver.metadata;
        if (xml == null || xml.Length == 0) return;

        var metadata = Metadata.Deserialize(xml);

        Debug.Log(metadata.CameraPosition);
    }
}

} // namespace Rcam2
