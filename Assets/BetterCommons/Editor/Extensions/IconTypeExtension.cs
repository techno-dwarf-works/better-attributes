using Better.Commons.EditorAddons.Enums;
using Better.Commons.EditorAddons.Utility;
using UnityEngine;

namespace Better.Commons.EditorAddons.Extensions
{
    public static class IconTypeExtension
    {
        /// <summary>
        /// Getting Icon Name from Unity Inspector
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static Texture GetIcon(this IconType self)
        {
            var icon = GetIconName(self);
            return UnityEditor.EditorGUIUtility.IconContent(icon).image;
        }

        /// <summary>
        /// Getting Icon Name from Unity Inspector
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static string GetIconName(this IconType self)
        {
            var icon = self switch
            {
                IconType.InfoMessage => "console.infoicon",
                IconType.WarningMessage => "console.warnicon",
                IconType.ErrorMessage => "console.erroricon",
                IconType.Info => "d__Help@2x",
                IconType.View => "d_scenevis_visible_hover@2x",
                IconType.Close => "d_winbtn_win_close_a@2x",
                IconType.Search => "d_Search Icon",
                IconType.WhiteLine => "d_animationanimated",
                IconType.GrayLine => "d_animationnocurve",
                IconType.WhiteDropdown => "d_icon dropdown",
                IconType.GrayDropdown => "icon dropdown@2x",
                IconType.Checkmark => "d_Valid@2x",
                IconType.GrayPlayButton => "d_PlayButton",
                IconType.PlusMore => "d_Toolbar Plus More@2x",
                IconType.Minus => "d_Toolbar Minus@2x",
                _ => ""
            };
            return icon;
        }

        /// <summary>
        /// Getting Icon Name from Unity Inspector
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static GUIContent GetIconGUIContent(this IconType self)
        {
            var icon = GetIconName(self);
            return UnityEditor.EditorGUIUtility.IconContent(icon);
        }
    }
}