using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.VFX.Utility;

namespace Rcam2 {

[AddComponentMenu("VFX/Property Binders/Rcam/Button Binder")]
[VFXBinder("Rcam/Button")]
class VFXRcamButtonBinder : VFXBinderBase
{
    public string Property
      { get => (string)_property;
        set => _property = value; }

    [VFXPropertyBinding("System.Boolean"), SerializeField]
    ExposedProperty _property = "Button";

    public InputHandle Input = null;
    public int Index = 0;

    public override bool IsValid(VisualEffect component)
      => Input != null && component.HasBool(_property);

    public override void UpdateBinding(VisualEffect component)
    {
        component.SetBool(_property, Input.GetButton(Index));
    }

    public override string ToString()
      => $"Rcan : '{_property}' -> Button {Index}";
}

} // namespace Rcam2
