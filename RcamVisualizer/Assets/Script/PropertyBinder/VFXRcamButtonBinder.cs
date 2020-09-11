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

    public int ControlNumber = 0;

    public override bool IsValid(VisualEffect component)
      => component.HasBool(_property);

    public override void UpdateBinding(VisualEffect component)
    {
        component.SetBool
          (_property, Singletons.InputHandle.GetButton(ControlNumber));
    }

    public override string ToString()
      => $"Rcan : '{_property}' -> Button {ControlNumber}";
}

} // namespace Rcam2
