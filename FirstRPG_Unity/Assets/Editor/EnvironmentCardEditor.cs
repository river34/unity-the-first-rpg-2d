using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnvironmentCard)), CanEditMultipleObjects]
public class EnvironmentCardEditor : Editor
{
    public SerializedProperty
        Width_Prop,
        CameraWidth_Prop,
        PlayerMoving_Prop,
        PlayerSpeed_Prop,
        SpeedDevider_Prop;

    void OnEnable()
    {
        // Setup the SerializedProperties.
        Width_Prop = serializedObject.FindProperty("Width");
        CameraWidth_Prop = serializedObject.FindProperty("CameraWidth");
        PlayerMoving_Prop = serializedObject.FindProperty("PlayerMoving");
        PlayerSpeed_Prop = serializedObject.FindProperty("PlayerSpeed");
        SpeedDevider_Prop = serializedObject.FindProperty("SpeedDevider");
    }

    public override void OnInspectorGUI()
    {
        // Update the serializedProperty - always do this in the beginning of OnInspectorGUI.
        serializedObject.Update();

        // Show the custom GUI controls.
        EditorGUILayout.PropertyField(Width_Prop, new GUIContent("Width"));
        EditorGUILayout.PropertyField(CameraWidth_Prop, new GUIContent("Camera Width"));
        EditorGUILayout.PropertyField(PlayerMoving_Prop, new GUIContent("Player Moving"));
        EditorGUILayout.PropertyField(PlayerSpeed_Prop, new GUIContent("Player Speed"));
        EditorGUILayout.IntSlider(SpeedDevider_Prop, 1, 32, new GUIContent("Speed Devider"));

        serializedObject.ApplyModifiedProperties();
    }
}
