using UnityEditor;
using UnityEngine;
using static Ryuu.HierarchyColor.Editor.HierarchyColorCore;

namespace Ryuu.HierarchyColor.Editor
{
    internal class HierarchyColorEditorWindow : EditorWindow
    {
        public const string MENU_ITEM_PATH = "Tools/Hierarchy Color";
        public HierarchyColorEditorWindow() => titleContent = new GUIContent(nameof(HierarchyColorEditorWindow));

        [MenuItem(MENU_ITEM_PATH)]
        public static void ShowWindow() => GetWindow<HierarchyColorEditorWindow>();

        private void OnEnable() => ColorfulHierarchyInitialize();

        private void OnGUI()
        {
            if (!Info)
            {
                BtnCreateInfo();
            }
            else if (Info)
            {
                ObjFldInfo();
                EditorGUILayout.Space(2);
                BtnEnableDisable();
            }

            static void BtnCreateInfo()
            {
                if (GUILayout.Button(nameof(CreateInfo)))
                {
                    CreateInfo();
                    SetInfo();
                }

                EditorGUILayout.HelpBox(
                    $"There is no {nameof(HierarchyColorInfo)}.\n" +
                    $" You can create an info file by clicking {nameof(CreateInfo)} button.",
                    MessageType.Warning
                );
            }

            static void ObjFldInfo()
            {
                Info = (HierarchyColorInfo) EditorGUILayout.ObjectField(
                    $"{nameof(HierarchyColorInfo)}",
                    Info,
                    typeof(HierarchyColorInfo),
                    false
                );
            }

            static void BtnEnableDisable()
            {
                if (GUILayout.Button("Enable / Disable")) EnableOrDisable();
            }
        }
    }
}