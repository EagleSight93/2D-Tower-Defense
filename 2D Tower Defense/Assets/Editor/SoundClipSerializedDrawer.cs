using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
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
        EditorGUI.PropertyField(new Rect(position.x + halfWidth + halfWidth * 0.5f, position.y, halfWidth * 0.5f, position.height), property.FindPropertyRelative("clip"), GUIContent.none);

        // Create a slider for the "volume" field
        float volumeValue = property.FindPropertyRelative("volume").floatValue;
        Rect sliderPosition = new Rect(position.x + halfWidth * 0.75f, position.y, halfWidth / 1.35f, position.height);
        volumeValue = EditorGUI.Slider(sliderPosition, volumeValue, 0.0f, 1.0f);

        property.FindPropertyRelative("volume").floatValue = volumeValue;

        EditorGUI.EndProperty();
    }
}