using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Quest)), /*CustomPropertyDrawer(typeof(Quest)), */CanEditMultipleObjects]
public class QuestEditor : Editor/*, PropertyDrawer*/
{
    public SerializedProperty
        Name_Prop,
        Type_Prop,
        FromCharacter_Prop,
        ToCharacter_Prop,
        Item_Prop,
        Num_Prop,
        Reqard_Prop;

    void OnEnable()
    {
        // Setup the SerializedProperties.
        //Name_Prop = serializedObject.FindProperty("Name");
        Type_Prop = serializedObject.FindProperty("Type");
        FromCharacter_Prop = serializedObject.FindProperty("FromCharacter");
        ToCharacter_Prop = serializedObject.FindProperty("ToCharacter");
        Item_Prop = serializedObject.FindProperty("Item");
        Num_Prop = serializedObject.FindProperty("Num");
        Reqard_Prop = serializedObject.FindProperty("Reward");
    }

    public override void OnInspectorGUI()
    {
        // Update the serializedProperty - always do this in the beginning of OnInspectorGUI.
        serializedObject.Update();

        // Show the custom GUI controls.
        //EditorGUILayout.PropertyField(Name_Prop, new GUIContent("Name"));
        EditorGUILayout.PropertyField(Type_Prop, new GUIContent("Type"));

        Quest.QuestType type = (Quest.QuestType)Type_Prop.enumValueIndex;

        switch (type)
        {
            case Quest.QuestType.Talk:
                //EditorGUILayout.PropertyField(FromCharacter_Prop, new GUIContent("From Character"));
                EditorGUILayout.PropertyField(ToCharacter_Prop, new GUIContent("To Character"));
                break;

            case Quest.QuestType.Collect:
                //EditorGUILayout.PropertyField(FromCharacter_Prop, new GUIContent("From Character"));
                EditorGUILayout.PropertyField(Item_Prop, new GUIContent("Item"));
                EditorGUILayout.IntSlider(Num_Prop, 1, 10, new GUIContent("Num"));
                break;
        }

        EditorGUILayout.IntSlider(Reqard_Prop, 1, 100, new GUIContent("Reward"));

        serializedObject.ApplyModifiedProperties();
    }

    // Draw the property inside the given rect
    //public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    //{
    //    // Using BeginProperty / EndProperty on the parent property means that
    //    // prefab override logic works on the entire property.
    //    EditorGUI.BeginProperty(position, label, property);

    //    // Draw label
    //    position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

    //    // Don't make child fields be indented
    //    var indent = EditorGUI.indentLevel;
    //    EditorGUI.indentLevel = 0;

    //    // Calculate rects
    //    var nameRect = new Rect(position.x, position.y, position.width, position.height);

    //    // Draw fields - passs GUIContent.none to each so they are drawn without labels
    //    EditorGUI.PropertyField(nameRect, property.FindPropertyRelative("Name"), EditorGUI.BeginProperty(position, new GUIContent("Name"), property));
    //    EditorGUI.EndProperty();

    //    // Set indent back to what it was
    //    EditorGUI.indentLevel = indent;

    //    EditorGUI.EndProperty();
    //}

}
