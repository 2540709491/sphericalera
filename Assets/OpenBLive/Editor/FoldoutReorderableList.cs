using System;
using System.Reflection;
using OpenBLive.Runtime.Data;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace OpenBLive.Editor
{
    public class FoldoutReorderableList
    {
        private readonly ReorderableList m_List;
        
        public FoldoutReorderableList(SerializedObject serializedObject,
            SerializedProperty property,
           int fieldsCount,
            string displayName = "", bool draggable = true, bool displayHeader = true, bool displayAddButton = true,
            bool displayRemoveButton = true)
        {
            m_List = new ReorderableList(serializedObject, property, draggable, displayHeader, displayAddButton,
                displayRemoveButton);
            var clearCache = (Action) Delegate.CreateDelegate(typeof(Action), m_List, k_ClearCacheMethod);

            m_List.drawHeaderCallback = (Rect rect) =>
            {
                
                var newRect = new Rect(rect.x + 10, rect.y, rect.width - 10, rect.height);
                var newValue = EditorGUI.Foldout(newRect, property.isExpanded, displayName);

                if (property.isExpanded == newValue)
                    return;

                property.isExpanded = newValue;
                clearCache();
            };
            m_List.drawElementCallback =
                (Rect rect, int index, bool isActive, bool isFocused) =>
                {
                    if (!property.isExpanded)
                    {
                        //GUI.enabled = index == m_List.count;
                        return;
                    }

                    var element = m_List.serializedProperty.GetArrayElementAtIndex(index);
                    rect.y += 2;
                    EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight),
                        element, GUIContent.none);
                };
            m_List.elementHeightCallback = (int indexer) =>
            {
                if (!property.isExpanded)
                    return 0;
                return EditorGUIUtility.singleLineHeight * fieldsCount;
            };
        }

        public void DoLayoutList()
        {
            m_List.DoLayoutList();
        }


        private static readonly MethodInfo k_ClearCacheMethod = typeof(ReorderableList)
            .GetMethod("ClearCache", BindingFlags.Instance | BindingFlags.NonPublic);
    }
}