using UnityEditor;

namespace Rcam2 {

[CustomEditor(typeof(VfxActivator)), CanEditMultipleObjects]
sealed class VfxActivatorEditor : Editor
{
    SerializedProperty _controlType;
    SerializedProperty _controlNumber;
    SerializedProperty _delayTillOff;

    void OnEnable()
    {
        _controlType = serializedObject.FindProperty("_controlType");
        _controlNumber = serializedObject.FindProperty("_controlNumber");
        _delayTillOff = serializedObject.FindProperty("_delayTillOff");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(_controlType);
        EditorGUILayout.PropertyField(_controlNumber);
        EditorGUILayout.PropertyField(_delayTillOff);
        serializedObject.ApplyModifiedProperties();
    }
}

} // namespace Rcam2
