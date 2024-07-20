// using OpenBLive.Runtime.Data;
// using OpenBLive.Runtime.Utilities;
// using UnityEditor;
// using UnityEngine;
//
// namespace OpenBLive.Editor.Drawers
// {
//     [CustomPropertyDrawer(typeof(SuperChatDel))]
//     public class SuperChatDelPropertyDrawer : StructPropertyDrawer
//     {
//
//         public override void OnGUI(Rect position, SerializedProperty property,
//             GUIContent label)
//         {
//             if (!DrawBase(position, property, label)) return;
//             var superChatDel = (SuperChatDel) property.GetValue();
//             superChatDel.Mock();
//         }
//
//         public SuperChatDelPropertyDrawer()
//         {
//             contents = new[]
//             {
//
//                 new GUIContent(EditorConstants.kRoomID),
//                 new GUIContent(EditorConstants.kMessageId)
//             };
//             rects = new Rect[contents.Length];
//             propertyNames = new[]
//             {
//                 nameof(SuperChatDel.roomId),
//                 nameof(SuperChatDel.messageIds)
//             };
//         }
//         public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
//         {
//             return base.GetPropertyHeight(property, label);
//         }
//     }
// }