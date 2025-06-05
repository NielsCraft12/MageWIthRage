using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        // Calculate the height needed for the property (same as default)
        return EditorGUI.GetPropertyHeight(property, label, true);
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Save the current GUI enabled state
        var previousGUIState = GUI.enabled;

        // Disable GUI for the property
        GUI.enabled = false;

        // Draw the property field as usual
        EditorGUI.PropertyField(position, property, label, true);

        // Restore the previous GUI enabled state
        GUI.enabled = previousGUIState;
    }
}