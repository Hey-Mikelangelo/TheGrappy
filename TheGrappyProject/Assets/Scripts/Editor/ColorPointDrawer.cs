using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ColorPoint))]
public class ColorPointDrawer : PropertyDrawer
{
	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		
		return (label != GUIContent.none && Screen.width < 495) ? (16f + 18f) : 16f;
	}
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		label = EditorGUI.BeginProperty(position, label, property);
		Rect contentPosition = EditorGUI.PrefixLabel(position, label);
		int oldIndentLevel = EditorGUI.indentLevel;
		if (position.height > 16f)
		{
			position.height = 16f;
			EditorGUI.indentLevel += 1;
			contentPosition = EditorGUI.IndentedRect(position);
			contentPosition.y += 18f;
		}
		EditorGUI.indentLevel = 0;
		contentPosition.width *= 0.75f;
		
		EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("position"), GUIContent.none);
		contentPosition.x += contentPosition.width;
		contentPosition.width /= 3f;
		EditorGUIUtility.labelWidth = 14;
		EditorGUI.PropertyField(
			contentPosition, property.FindPropertyRelative("color"), new GUIContent("C"));
		EditorGUI.EndProperty();
		EditorGUI.indentLevel = oldIndentLevel;
	}
	
}