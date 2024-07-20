using OpenBLive.Runtime.Data;
using OpenBLive.Runtime.Utilities;
using UnityEditor;
using UnityEngine;

namespace OpenBLive.Editor.Drawers
{
    [CustomPropertyDrawer(typeof(Guard))]
    public class GuardPropertyDrawer : StructPropertyDrawer
    {

        public override void OnGUI(Rect position, SerializedProperty property,
            GUIContent label)
        {
            if (!DrawBase(position, property, label)) return;
            var guard = (Guard) property.GetValue();
            guard.Mock();
        }

        public GuardPropertyDrawer()
        {
            contents = new[]
            {
                new GUIContent(EditorConstants.kGuardLevel),
                new GUIContent(EditorConstants.kGuardNum),
                new GUIContent(EditorConstants.kGuardUnit),
                new GUIContent(EditorConstants.kFansMedalLevel),
                new GUIContent(EditorConstants.kFansMedalName),
                new GUIContent(EditorConstants.kFansMedalWearingStatus),
                new GUIContent(EditorConstants.kRoomID),
                
                new GUIContent(EditorConstants.kUid),
                new GUIContent(EditorConstants.kUserName),
                new GUIContent(EditorConstants.kUserFace),
            };
            rects = new Rect[contents.Length];
            propertyNames = new[]
            {
                 nameof(Guard.guardLevel),
                 nameof(Guard.guardNum),
                 nameof(Guard.guardUnit),
                 nameof(Guard.fansMedalLevel),
                 nameof(Guard.fansMedalName),
                 nameof(Guard.fansMedalWearingStatus),
                 nameof(Guard.roomID),
                 
                 $"{nameof(Guard.userInfo)}.{nameof(Guard.userInfo.uid)}",
                 $"{nameof(Guard.userInfo)}.{nameof(Guard.userInfo.userName)}",
                 $"{nameof(Guard.userInfo)}.{nameof(Guard.userInfo.userFace)}",
            };
        }
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label);
        }
    }
}