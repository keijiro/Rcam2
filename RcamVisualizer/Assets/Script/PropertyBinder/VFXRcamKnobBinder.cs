using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.VFX.Utility;

namespace Rcam2 {

[AddComponentMenu("VFX/Property Binders/Rcam/Knob Binder")]
[VFXBinder("Rcam/Knob")]
class VFXRcamKnobBinder : VFXBinderBase
{
    public string Property
      { get => (string)_property;
        set => _property = value; }

    [VFXPropertyBinding("System.Single"), SerializeField]
    ExposedProperty _property = "Knob";

    public InputState Input = null;
    public int Index = 0;

    public override bool IsValid(VisualEffect component)
      => Input != null && component.HasFloat(_property);

    public override void UpdateBinding(VisualEffect component)
    {
        component.SetFloat(_property, Input.GetKnob(Index));
    }

    public override string ToString()
      => $"Rcan : '{_property}' -> Konb {Index}";
}

} // namespace Rcam2
