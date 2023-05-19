using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;

namespace TechTree
{
    [CustomEditor(typeof(TechTreeTable))]
    public class TechTreeTableEditor : Editor
    {
        ReorderableList list;

        private void OnEnable()
        {
            list = new ReorderableList(serializedObject, serializedObject.FindProperty("nodes"), true, true, true, true);

            list.drawHeaderCallback = rect => EditorGUI.LabelField(rect, "Nodes");
            list.drawElementCallback = (rect, index, isActive, isFocused) =>
            {
                var i = index;
                var element = serializedObject.FindProperty("nodes").GetArrayElementAtIndex(i);

                (element.objectReferenceValue as TechTreeNode).ChangeID(i);
                (element.objectReferenceValue as TechTreeNode).SetTable(target as TechTreeTable);

                rect.y += 2;

                EditorGUI.LabelField(
                    new Rect(rect.x, rect.y, rect.width * 0.1f, EditorGUIUtility.singleLineHeight),
                    $"id: {i} "
                );

                EditorGUI.BeginDisabledGroup(true);
                element.objectReferenceValue = EditorGUI.ObjectField
                (
                    new Rect(rect.x + rect.width * 0.1f, rect.y, rect.width * 0.8f - 5, EditorGUIUtility.singleLineHeight),
                    element.objectReferenceValue,
                    typeof(TechTreeNode),
                    false
                );
                EditorGUI.EndDisabledGroup();

                if(GUI.Button(new Rect(rect.x + rect.width * 0.9f - 25, rect.y, 20, EditorGUIUtility.singleLineHeight), ">"))
                {
                    Selection.activeObject = element.objectReferenceValue;
                }

                (element.objectReferenceValue as TechTreeNode).Active = EditorGUI.Toggle
                (
                    new Rect(rect.x + rect.width - EditorGUIUtility.singleLineHeight, rect.y, EditorGUIUtility.singleLineHeight, EditorGUIUtility.singleLineHeight),
                    (element.objectReferenceValue as TechTreeNode).Active
                );
            };

            list.onAddCallback = list =>
            {
                var elements = serializedObject.FindProperty("nodes");

                elements.arraySize++;

                var node = ScriptableObject.CreateInstance<TechTreeNode>();
                node.hideFlags = HideFlags.HideInHierarchy;
                AssetDatabase.AddObjectToAsset(node, target);
                EditorUtility.SetDirty(target);
                AssetDatabase.Refresh();

                elements.GetArrayElementAtIndex(elements.arraySize - 1).objectReferenceValue = node;

                serializedObject.ApplyModifiedProperties();
            };

            list.onRemoveCallback = list => 
            {
                var elements = serializedObject.FindProperty("nodes");

                for(int i = 0; i < list.selectedIndices.Count; i++)
                {
                    DestroyImmediate(elements.GetArrayElementAtIndex(list.selectedIndices[i]).objectReferenceValue as Object, true);
                    elements.DeleteArrayElementAtIndex(list.selectedIndices[i]);

                    EditorUtility.SetDirty(target);
                    AssetDatabase.Refresh();
                }
            };
        }

        public override void OnInspectorGUI()
        {
            list.DoLayoutList();
            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif