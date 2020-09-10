using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.VFX.Utility;

namespace Rcam2 {

[AddComponentMenu("VFX/Property Binders/Rcam/Toggle Binder")]
[VFXBinder("Rcam/Toggle")]
class VFXRcamToggleBinder : VFXBinderBase
{
    public string Property
      { get => (string)_property;
        set => _property = value; }

    [VFXPropertyBinding("System.Boolean"), SerializeField]
    ExposedProperty _property = "Toggle";

    public InputState Input = null;
    public int Index = 0;

    public override bool IsValid(VisualEffect component)
      => Input != null && component.HasBool(_property);

    public override void UpdateBinding(VisualEffect component)
    {
        component.SetBool(_property, Input.GetToggle(Index));
    }

    public override string ToString()
      => $"Rcan : '{_property}' -> Toggle {Index}";
}

} // namespace Rcam2
