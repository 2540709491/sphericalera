using System;
using System.Reflection;
using OpenBLive.Runtime.Data;
using UnityEditor;
using UnityEditorInternal;

namespace OpenBLive.Editor.Inspectors
{
    [CustomEditor(typeof(MockData))]
    public class MockDataInspector : UnityEditor.Editor
    {
        private FoldoutReorderableList m_DmList;
        private FoldoutReorderableList m_SendGiftList;
        private FoldoutReorderableList m_GuardList;
        private FoldoutReorderableList m_SuperChatList;


        public override void OnInspectorGUI()
        {
            using var scope = new EditorGUI.ChangeCheckScope();
            if (m_DmList == null)
            {
                var dmFieldsCount = GetStructFieldsCount(typeof(Dm));
                m_DmList =
                    new FoldoutReorderableList(serializedObject,
                        serializedObject.FindProperty(nameof(MockData.dms)),
                        dmFieldsCount, EditorConstants.kMockDataDms);
            }

            if (m_SendGiftList == null)
            {
                var sgFieldsCount = GetStructFieldsCount(typeof(SendGift)) + GetStructFieldsCount(typeof(AnchorInfo))-1;
                m_SendGiftList = new FoldoutReorderableList(serializedObject,
                    serializedObject.FindProperty(nameof(MockData.sendGifts)),
                    sgFieldsCount, EditorConstants.kMockDataSendGifts);
            }

            if (m_GuardList == null)
            {
                var gFieldsCount = GetStructFieldsCount(typeof(Guard)) + GetStructFieldsCount(typeof(UserInfo));
                m_GuardList = new FoldoutReorderableList(serializedObject,
                    serializedObject.FindProperty(nameof(MockData.guards)), gFieldsCount,
                    EditorConstants.kMockDataGuards);
            }

            if (m_SuperChatList == null)
            {
                var scFieldsCount = GetStructFieldsCount(typeof(SuperChat)) + 1;
                m_SuperChatList = new FoldoutReorderableList(serializedObject,
                    serializedObject.FindProperty(nameof(MockData.superChats)), scFieldsCount,
                    EditorConstants.kMockDataSuperChats);
            }


            m_DmList.DoLayoutList();
            m_SendGiftList.DoLayoutList();
            m_GuardList.DoLayoutList();
            m_SuperChatList.DoLayoutList();

            if (scope.changed)
            {
                serializedObject.ApplyModifiedProperties();
            }
        }

        private int GetStructFieldsCount(Type type)
        {
            return type.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance).Length;
        }
    }
}