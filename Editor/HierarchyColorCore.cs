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
    internal static class HierarchyColorCore
    {
        public static HierarchyColorInfo Info;

        [InitializeOnLoadMethod]
        public static void ColorfulHierarchyInitialize()
        {
            // get hierarchy styles
            string[] guids = AssetDatabase.FindAssets($"t:{nameof(HierarchyColorInfo)}");

            // create hierarchy styles if no exist
            if (guids is not {Length: > 0})
            {
                CreateInfo();
            }

            SetInfo();
        }

        public static void CreateInfo()
        {
            var info = ScriptableObject.CreateInstance<HierarchyColorInfo>();

            if (!Directory.Exists("Assets/Plugins/Ryuu/Hierarchy Color/"))
            {
                Directory.CreateDirectory("Assets/Plugins/Ryuu/Hierarchy Color/");
            }

            AssetDatabase.CreateAsset(info,
                $"Assets/Plugins/Ryuu/Hierarchy Color/{nameof(HierarchyColorInfo)}.asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log(
                $"{nameof(HierarchyColorInfo)} created.\n Check {HierarchyColorEditorWindow.MENU_ITEM_PATH}."
            );
        }

        public static void SetInfo()
        {
            if (Info == null)
            {
                string[] guids = AssetDatabase.FindAssets($"t:{nameof(HierarchyColorInfo)}");
                string path = AssetDatabase.GUIDToAssetPath(guids[0]);
                Info = AssetDatabase.LoadAssetAtPath<HierarchyColorInfo>(path);
            }

            if (Info.enableWhenOpenEditor)
            {
                Enable();
            }
        }

        public static void EnableOrDisable()
        {
            if (EditorApplication.hierarchyWindowItemOnGUI.GetInvocationList().Any(@delegate =>
                    @delegate.Method.Name.Equals(nameof(HierarchyColorOnGUI))))
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
            EditorApplication.hierarchyWindowItemOnGUI -= HierarchyColorOnGUI;
            EditorApplication.hierarchyWindowItemOnGUI += HierarchyColorOnGUI;
        }

        private static void Disable()
        {
            EditorApplication.hierarchyWindowItemOnGUI -= HierarchyColorOnGUI;
        }

        private static void HierarchyColorOnGUI(int instanceID, Rect selectionRect)
        {
            // type check
            if (EditorUtility.InstanceIDToObject(instanceID) is not GameObject gameObject)
            {
                return;
            }

            // target style check
            var hierarchyStyle = Info.styles.SingleOrDefault(style => gameObject.name.StartsWith(style.prefix));
            if (hierarchyStyle == null)
            {
                return;
            }

            // upper
            string text = Info.enableUpperCase
                ? gameObject.name[hierarchyStyle.prefix.Length..].ToUpper()
                : gameObject.name[hierarchyStyle.prefix.Length..];

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
                backgroundRect.x += Info.hierarchyOffsetX;
                backgroundRect.width -= Info.hierarchyOffsetX;
            }

            backgroundRect.width += Info.hierarchyPaddingX;

            // draw
            EditorGUI.DrawRect(backgroundRect, hierarchyStyle.backgroundColor);
            EditorGUI.LabelField(selectionRect, text, guiStyle);
        }
    }
}