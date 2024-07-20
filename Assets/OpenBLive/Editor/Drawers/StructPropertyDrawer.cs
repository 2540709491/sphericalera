using OpenBLive.Editor.Utilities;
using UnityEditor;
using UnityEngine;

namespace OpenBLive.Editor.Drawers
{
    public abstract class StructPropertyDrawer : PropertyDrawer
    {
        protected GUIContent[] contents;
        protected Rect[] rects;
        protected string[] propertyNames;



        protected bool DrawBase(Rect position, SerializedProperty property, GUIContent label)
        {
            StructDrawerTool.Draw(contents, rects, propertyNames, position, property);

            var lastRect = rects[^1];

            var buttonRect = new Rect()
            {
                position = new Vector2(lastRect.x, lastRect.y + position.height),
                width = position.width / 4,
                height = EditorGUIUtility.singleLineHeight
            };
            return GUI.Button(buttonRect, EditorConstants.kTestButton);
        }
        
    }
}