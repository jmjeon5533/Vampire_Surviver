using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;

namespace TechTree
{
    [CustomEditor(typeof(TechTreeNode))]
    public class TechTreeNodeEditor : Editor
    {
        ReorderableList list;

        private void OnEnable()
        {
            var nexts = serializedObject.FindProperty("nexts");

            for (int i = nexts.arraySize - 1; i >= 0; i--)
                if (nexts.GetArrayElementAtIndex(i).objectReferenceValue == null)
                {
                    nexts.DeleteArrayElementAtIndex(i);
                    serializedObject.ApplyModifiedProperties();
                }


            list = new ReorderableList(serializedObject, nexts, true, true, true, true);

            list.drawHeaderCallback = rect => EditorGUI.LabelField(rect, "Nexts");
            list.drawElementCallback = (rect, index, isActive, isFocused) =>
            {
                var i = index;

                var table = serializedObject.FindProperty("table");
                var element = nexts.GetArrayElementAtIndex(i);

                rect.y += 2;

                EditorGUI.LabelField(new Rect(rect.x, rect.y, rect.width * 0.4f, EditorGUIUtility.singleLineHeight), $"Element {i}:");

                if (GUI.Button(
                    new Rect(rect.x + rect.width * 0.4f, rect.y, rect.width * 0.6f, EditorGUIUtility.singleLineHeight), 
                    (element.objectReferenceValue as TechTreeNode).Id.ToString(), 
                    new GUIStyle(EditorStyles.popup) { alignment = TextAnchor.MiddleRight }
                ))
                {
                    var menu = new GenericMenu();
                    var selectList = (table.objectReferenceValue as TechTreeTable).Nodes.Select(item => item.Id).ToArray();

                    for (int k = 0; k < selectList.Length; k++)
                    {
                        int tk = k;
                        menu.AddItem(new GUIContent(selectList[k].ToString()), false, () =>
                        {
                            element.objectReferenceValue = (table.objectReferenceValue as TechTreeTable).Nodes[tk];
                            serializedObject.ApplyModifiedProperties();
                        });
                    }

                    menu.ShowAsContext();
                }
            };

            list.onAddCallback = list =>
            {
                nexts.arraySize++;
                nexts.GetArrayElementAtIndex(nexts.arraySize - 1).objectReferenceValue = (serializedObject.FindProperty("table").objectReferenceValue as TechTreeTable).Nodes[0];

                serializedObject.ApplyModifiedProperties();
            };
        }

        public override void OnInspectorGUI()
        {
            var id = serializedObject.FindProperty("id");
            var position = serializedObject.FindProperty("position");
            var command = serializedObject.FindProperty("command");
            var nexts = serializedObject.FindProperty("nexts");

            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.IntField("id", id.intValue);
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.Space(10);

            EditorGUILayout.PropertyField(position);
            EditorGUILayout.LabelField("Command");
            command.stringValue = EditorGUILayout.TextArea(command.stringValue, GUILayout.Height(EditorGUIUtility.singleLineHeight * 3));
            list.DoLayoutList();

            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif