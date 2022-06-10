using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

// ReSharper disable StringLiteralTypo
// ReSharper disable IdentifierTypo
// ReSharper disable LocalVariableHidesMember
// ReSharper disable CheckNamespace

namespace Ryuu.HierarchyColor.Editor
{
    internal static class HierarchyColorController
    {
        public static HierarchyColorModel Model;

        [InitializeOnLoadMethod]
        public static void ColorfulHierarchyInitialize()
        {
            string[] guids = AssetDatabase.FindAssets($"t:{nameof(HierarchyColorModel)}");

            if (guids == null || guids.Length == 0)
            {
                CreateModel();
            }

            SetModel();
        }

        public static void CreateModel()
        {
            const string modelPath = "Assets/Plugins/Ryuu/Hierarchy Color/";

            var info = ScriptableObject.CreateInstance<HierarchyColorModel>();

            if (!Directory.Exists(modelPath))
            {
                Directory.CreateDirectory(modelPath);
            }

            AssetDatabase.CreateAsset(info, $"{modelPath}{nameof(HierarchyColorModel)}.asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log(
                $"{nameof(HierarchyColorModel)} created at {modelPath}.\n" +
                $" Check {HierarchyColorEditorWindow.MENU_ITEM_PATH}."
            );
        }

        public static void SetModel()
        {
            if (Model == null)
            {
                string[] guids = AssetDatabase.FindAssets($"t:{nameof(HierarchyColorModel)}");
                string path = AssetDatabase.GUIDToAssetPath(guids[0]);
                Model = AssetDatabase.LoadAssetAtPath<HierarchyColorModel>(path);
            }

            if (Model.enableWhenOpenEditor)
            {
                Enable();
            }
        }

        public static void EnableOrDisable()
        {
            if (EditorApplication.hierarchyWindowItemOnGUI
                .GetInvocationList()
                .Any(
                    @delegate => @delegate.Method.Name.Equals(nameof(HierarchyColorOnGUI))
                )
               )
            {
                Disable();
            }
            else
            {
                Enable();
            }
        }

        private static void Enable()
        {
            EditorApplication.hierarchyWindowItemOnGUI += HierarchyColorOnGUI;
        }

        private static void Disable()
        {
            EditorApplication.hierarchyWindowItemOnGUI -= HierarchyColorOnGUI;
        }

        private static void HierarchyColorOnGUI(int instanceID, Rect selectionRect)
        {
            if (Model == null)
            {
                Debug.LogWarning(
                    $"There is no {nameof(HierarchyColorModel)}. Go to Tools -> Hierarchy Color.\n" +
                    $" Set the {nameof(HierarchyColorModel)}.\n" +
                    $"You can create info file by click {nameof(CreateModel)} button in {nameof(HierarchyColorModel)}."
                );
                return;
            }

            // type check
            if (EditorUtility.InstanceIDToObject(instanceID) is not GameObject gameObject)
            {
                return;
            }

            // target style check
            HierarchyColorModel.Style hierarchyStyle = Model.styles.SingleOrDefault(style => gameObject.name.StartsWith(style.prefix));
            if (hierarchyStyle == null)
            {
                return;
            }

            // set data
            var guiStyle = new GUIStyle
            {
                alignment = hierarchyStyle.textAnchor,
                fontStyle = hierarchyStyle.fontStyle,
                normal = new GUIStyleState {textColor = hierarchyStyle.textColor}
            };
            var backgroundRect = new Rect(selectionRect);
            if (gameObject.transform.childCount == 0 && gameObject.transform.parent == null)
            {
                backgroundRect.x += Model.hierarchyOffsetX;
                backgroundRect.width -= Model.hierarchyOffsetX;
            }

            backgroundRect.width += Model.hierarchyPaddingX;

            // draw
            EditorGUI.DrawRect(backgroundRect, hierarchyStyle.backgroundColor);
            string text = gameObject.name[hierarchyStyle.prefix.Length..];
            EditorGUI.LabelField(selectionRect, text, guiStyle);
        }
    }
}