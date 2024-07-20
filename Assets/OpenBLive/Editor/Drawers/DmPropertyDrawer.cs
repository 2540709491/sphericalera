using OpenBLive.Editor.Utilities;
using OpenBLive.Runtime;
using OpenBLive.Runtime.Data;
using OpenBLive.Runtime.Utilities;
using UnityEditor;
using UnityEngine;

namespace OpenBLive.Editor.Drawers
{
    [CustomPropertyDrawer(typeof(Dm))]
    public class DmPropertyDrawer : StructPropertyDrawer
    {
        public DmPropertyDrawer()
        {
            contents = new[]
            {
                new GUIContent(EditorConstants.kUid),
                new GUIContent(EditorConstants.kUserName),
                new GUIContent(EditorConstants.kUserFace),
                new GUIContent(EditorConstants.kMsg),
                new GUIContent(EditorConstants.kFansMedalLevel),
                new GUIContent(EditorConstants.kFansMedalName),
                new GUIContent(EditorConstants.kFansMedalWearingStatus),
                new GUIContent(EditorConstants.kGuardLevel),
                new GUIContent(EditorConstants.kRoomID),
            };
            rects = new Rect[contents.Length];
            propertyNames = new[]
            {
                nameof(Dm.uid),
                nameof(Dm.userName),
                nameof(Dm.userFace),
                nameof(Dm.msg),
                nameof(Dm.fansMedalLevel),
                nameof(Dm.fansMedalName),
                nameof(Dm.fansMedalWearingStatus),
                nameof(Dm.guardLevel),
                nameof(Dm.roomId),
            };
        }

        public override void OnGUI(Rect position, SerializedProperty property,
            GUIContent label)
        {
            if (!DrawBase(position, property, label)) return;
            var dm = (Dm) property.GetValue();
            dm.Mock();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label);
        }
    }
}