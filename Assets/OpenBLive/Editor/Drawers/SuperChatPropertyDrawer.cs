using System.Collections.Generic;
using OpenBLive.Editor.Utilities;
using OpenBLive.Runtime.Data;
using OpenBLive.Runtime.Utilities;
using UnityEditor;
using UnityEngine;

namespace OpenBLive.Editor.Drawers
{
    [CustomPropertyDrawer(typeof(SuperChat))]
    public class SuperChatPropertyDrawer : StructPropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property,
            GUIContent label)
        {
            if (!DrawBase(position, property, label)) return;
            var superChat = (SuperChat) property.GetValue();
            superChat.Mock();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label);
        }

        public SuperChatPropertyDrawer()
        {
            contents = new[]
            {
                new GUIContent(EditorConstants.kUid),
                new GUIContent(EditorConstants.kUserName),
                new GUIContent(EditorConstants.kUserFace),
                new GUIContent(EditorConstants.kMessageId),
                new GUIContent(EditorConstants.kMessage),
                new GUIContent(EditorConstants.kRmb),
                new GUIContent(EditorConstants.kTimeStamp),
                new GUIContent(EditorConstants.kStartTime),
                new GUIContent(EditorConstants.kEndTime),
                new GUIContent(EditorConstants.kFansMedalLevel),
                new GUIContent(EditorConstants.kFansMedalName),
                new GUIContent(EditorConstants.kFansMedalWearingStatus),
                new GUIContent(EditorConstants.kGuardLevel),
                new GUIContent(EditorConstants.kRoomID),
            };
            rects = new Rect[contents.Length];
            propertyNames = new[]
            {
                nameof(SuperChat.uid),
                nameof(SuperChat.userName),
                nameof(SuperChat.userFace),
                nameof(SuperChat.messageId),
                nameof(SuperChat.message),
                nameof(SuperChat.rmb),
                nameof(SuperChat.timeStamp),
                nameof(SuperChat.startTime),
                nameof(SuperChat.endTime),
                nameof(SuperChat.fansMedalLevel),
                nameof(SuperChat.fansMedalName),
                nameof(SuperChat.fansMedalWearingStatus),
                nameof(SuperChat.guardLevel),
                nameof(SuperChat.roomId),
            };
        }
    }
}