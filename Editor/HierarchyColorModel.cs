using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ryuu.HierarchyColor.Editor
{
    internal class HierarchyColorModel : ScriptableObject
    {

        [Serializable]
        internal class Style
        {
            public string prefix;
            public Color textColor;
            public Color backgroundColor;
            public TextAnchor textAnchor;
            public FontStyle fontStyle;

            public Style(string prefix, Color textColor, Color backgroundColor, TextAnchor textAnchor,
                FontStyle fontStyle)
            {
                this.prefix = prefix;
                this.textColor = textColor;
                this.backgroundColor = backgroundColor;
                this.textAnchor = textAnchor;
                this.fontStyle = fontStyle;
            }
        }

        public bool enableWhenOpenEditor;
        public int hierarchyPaddingX;
        public int hierarchyOffsetX;
        public List<Style> styles;

        public HierarchyColorModel()
        {
            enableWhenOpenEditor = true;
            hierarchyPaddingX = 20;
            hierarchyOffsetX = -28;
            Color orangeRed = new(1, 69 / 255f, 0, 1);
            Color vermilion = new(1, 77 / 255f, 0, 1);
            Color fireBrick = new(178 / 255f, 34 / 255f, 34 / 255f, 1);
            Color maroon = new(128 / 255f, 0, 0, 1);
            Color burgundy = new(71 / 255f, 0, 36 / 255f, 1);
            Color coconutBrown = new(77 / 255f, 31 / 255f, 0, 1);
            Color saddleBrown = new(139 / 255f, 69 / 255f, 19 / 255f, 1);
            Color darkOliveGreen = new(85 / 255f, 107 / 255f, 47 / 255f, 1);
            Color chromeGreen = new(18 / 255f, 116 / 255f, 54 / 255f, 1);
            Color seaGreen = new(46 / 255f, 139 / 255f, 87 / 255f, 1);
            Color navy = new(0, 0, 128 / 255f, 1);
            Color prussianBlue = new(0, 49 / 255f, 83 / 255f, 1);
            styles = new List<Style>
            {
                new($"{nameof(orangeRed)}", Color.white, orangeRed, TextAnchor.MiddleCenter, FontStyle.Bold),
                new($"{nameof(vermilion)}", Color.white, vermilion, TextAnchor.MiddleCenter, FontStyle.Bold),
                new($"{nameof(fireBrick)}", Color.white, fireBrick, TextAnchor.MiddleCenter, FontStyle.Bold),
                new($"{nameof(maroon)}", Color.white, maroon, TextAnchor.MiddleCenter, FontStyle.Bold),
                new($"{nameof(burgundy)}", Color.white, burgundy, TextAnchor.MiddleCenter, FontStyle.Bold),
                new($"{nameof(coconutBrown)}", Color.white, coconutBrown, TextAnchor.MiddleCenter, FontStyle.Bold),
                new($"{nameof(saddleBrown)}", Color.white, saddleBrown, TextAnchor.MiddleCenter, FontStyle.Bold),
                new($"{nameof(darkOliveGreen)}", Color.white, darkOliveGreen, TextAnchor.MiddleCenter, FontStyle.Bold),
                new($"{nameof(chromeGreen)}", Color.white, chromeGreen, TextAnchor.MiddleCenter, FontStyle.Bold),
                new($"{nameof(seaGreen)}", Color.white, seaGreen, TextAnchor.MiddleCenter, FontStyle.Bold),
                new($"{nameof(navy)}", Color.white, navy, TextAnchor.MiddleCenter, FontStyle.Bold),
                new($"{nameof(prussianBlue)}", Color.white, prussianBlue, TextAnchor.MiddleCenter, FontStyle.Bold),
                new($"{nameof(Color.red)}", Color.white, Color.red, TextAnchor.MiddleCenter, FontStyle.Bold),
                new($"{nameof(Color.blue)}", Color.white, Color.blue, TextAnchor.MiddleCenter, FontStyle.Bold),
                new($"{nameof(Color.yellow)}", Color.black, Color.yellow, TextAnchor.MiddleCenter, FontStyle.Bold)
            };
        }

        private void OnValidate()
        {
            foreach (Style style in styles)
            {
                style.backgroundColor.a = 1;
            }
        }
    }
}