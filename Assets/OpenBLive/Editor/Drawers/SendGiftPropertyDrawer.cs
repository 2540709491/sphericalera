using OpenBLive.Editor.Utilities;
using OpenBLive.Runtime.Data;
using OpenBLive.Runtime.Utilities;
using UnityEditor;
using UnityEngine;

namespace OpenBLive.Editor.Drawers
{
    [CustomPropertyDrawer(typeof(SendGift))]
    public class SendGiftPropertyDrawer : StructPropertyDrawer
    {

        public override void OnGUI(Rect position, SerializedProperty property,
            GUIContent label)
        {
            if (!DrawBase(position, property, label)) return;
            var sendGift = (SendGift) property.GetValue();
            sendGift.Mock();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label);
        }

        public SendGiftPropertyDrawer()
        {
            contents = new[]
            {
                new GUIContent(EditorConstants.kRoomID),
                new GUIContent(EditorConstants.kUid),
                new GUIContent(EditorConstants.kUserName),
                new GUIContent(EditorConstants.kUserFace),
                new GUIContent(EditorConstants.kGiftID),
                new GUIContent(EditorConstants.kGiftName),
                new GUIContent(EditorConstants.kGiftNum),
                new GUIContent(EditorConstants.kPrice),
                new GUIContent(EditorConstants.kPaid),
                new GUIContent(EditorConstants.kFansMedalLevel),
                new GUIContent(EditorConstants.kFansMedalName),
                new GUIContent(EditorConstants.kFansMedalWearingStatus),
                new GUIContent(EditorConstants.kGuardLevel),
                new GUIContent(EditorConstants.kAuId),
                new GUIContent(EditorConstants.kAuName),
                new GUIContent(EditorConstants.kAuFace)
            };
            rects = new Rect[contents.Length];
            propertyNames = new[]
            {
                nameof(SendGift.roomId),
                nameof(SendGift.uid),
                nameof(SendGift.userName),
                nameof(SendGift.userFace),
                nameof(SendGift.giftId),
                nameof(SendGift.giftName),
                nameof(SendGift.giftNum),
                nameof(SendGift.price),
                nameof(SendGift.paid),
                nameof(SendGift.fansMedalLevel),
                nameof(SendGift.fansMedalName),
                nameof(SendGift.fansMedalWearingStatus),
                nameof(SendGift.guardLevel),
                $"{nameof(SendGift.anchorInfo)}.{nameof(SendGift.anchorInfo.uid)}",
                $"{nameof(SendGift.anchorInfo)}.{nameof(SendGift.anchorInfo.userName)}",
                $"{nameof(SendGift.anchorInfo)}.{nameof(SendGift.anchorInfo.userFace)}",
            };
        }
    }
}