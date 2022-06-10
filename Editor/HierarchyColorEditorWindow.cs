using UnityEditor;
using UnityEngine;
using static Ryuu.HierarchyColor.Editor.HierarchyColorController;

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
            if (!Model)
            {
                ButtonCreateModel();
                EditorGUILayout.Space();
                NoModelHelpBox();
            }
            else
            {
                ObjectFieldModel();
                EditorGUILayout.Space();
                ButtonEnableDisable();
            }

            static void ButtonCreateModel()
            {
                if (!GUILayout.Button(nameof(CreateModel))) return;

                CreateModel();
                SetModel();
            }

            static void NoModelHelpBox()
            {
                EditorGUILayout.HelpBox(
                    $"There is no {nameof(HierarchyColorModel)}.\n" +
                    $" You can create an info file by clicking {nameof(ButtonCreateModel)}.",
                    MessageType.Warning
                );
            }

            static void ObjectFieldModel()
            {
                Model = (HierarchyColorModel) EditorGUILayout.ObjectField(
                    $"{nameof(HierarchyColorModel)}",
                    Model,
                    typeof(HierarchyColorModel),
                    false
                );
            }

            static void ButtonEnableDisable()
            {
                if (GUILayout.Button("Enable / Disable")) EnableOrDisable();
            }
        }
    }
}