using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomPropertyDrawer(typeof(SoundClip))]
public class SoundClipSerializedDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        // Calculate the width for each field (x and y)
        float halfWidth = position.width * 0.5f;

        // Draw the x and y fields side by side
        EditorGUI.PrefixLabel(new Rect(position.x, position.y, halfWidth, position.height), label);
        EditorGUI.PropertyField(new Rect(position.x + halfWidth + halfWidth * 0.25f, position.y, halfWidth * 0.75f, position.height), property.FindPropertyRelative("clip"), GUIContent.none);
        EditorGUI.PropertyField(new Rect(position.x + halfWidth * 0.9f, position.y, halfWidth * 0.275f, position.height), property.FindPropertyRelative("volume"), GUIContent.none);

        EditorGUI.EndProperty();
    }
}
