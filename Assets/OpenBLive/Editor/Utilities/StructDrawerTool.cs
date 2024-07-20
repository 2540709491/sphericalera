using UnityEditor;
using UnityEngine;

namespace OpenBLive.Editor.Utilities
{
    public static class StructDrawerTool
    {
        public static void Draw(GUIContent[] contents, Rect[] rects, string[] propertyNames, Rect position,
            SerializedProperty property)
        {
            for (int i = 0; i < rects.Length; i++)
            {
                if (i == 0)
                {
                    rects[i] = new Rect(position)
                    {
                        position = new Vector2(position.x, position.y)
                    };
                }
                else
                {
                    var perRect = rects[i - 1];
                    rects[i] = new Rect(perRect)
                    {
                        position = new Vector2(position.x, perRect.y + perRect.height)
                    };
                }
                
                EditorGUI.PropertyField(rects[i], property.FindPropertyRelative(propertyNames[i]),
                    contents[i]);
            }
        }

    }
}