using System;
using System.Collections.Generic;
using ClientCore;
using DTAConfig.Entity;
using Localization;
using Microsoft.Xna.Framework;
using Rampastring.Tools;
using ClientCore.Entity;
namespace Ra2Client.Domain.Multiplayer
{
    /// <summary>
    /// A color for the multiplayer game lobby.
    /// </summary>
    public class MultiplayerColor
    {
        public int GameColorIndex { get; private set; }

        public string Name { get; private set; }

        public Color XnaColor { get; private set; }

        private static List<MultiplayerColor> colorList;

        private static string randomColorLabel;

        private static string RandomColorDefaultLabel => "Random".L10N("UI:Main:Random");

        /// <summary>
        /// Creates a new multiplayer color from data in a string array.
        /// </summary>
        /// <param name="name">The name of the color.</param>
        /// <param name="data">The input data. Needs to be in the format R,G,B,(game color index).</param>
        /// <returns>A new multiplayer color created from the given string array.</returns>
        public static MultiplayerColor CreateFromStringArray(int nIndex, string name, string[] data)
        {
            return new MultiplayerColor()
            {
                Name = name,
                XnaColor = new Color(Math.Min(255, Int32.Parse(data[0])),
                Math.Min(255, Int32.Parse(data[1])),
                Math.Min(255, Int32.Parse(data[2])), 255),
                GameColorIndex = nIndex,
            };
        }

        /// <summary>
        /// Returns the available multiplayer colors.
        /// </summary>
        public static List<MultiplayerColor> LoadColors(List<string> newColors = null)
        {
            //if (colorList != null && newColors == null)
            //    return new List<MultiplayerColor>(colorList);

            IniFile gameOptionsIni = new IniFile(SafePath.CombineFilePath(ProgramConstants.GetBaseResourcePath(), "GameOptions.ini"));

            List<MultiplayerColor> mpColors = new List<MultiplayerColor>();

            List<string> colorKeys = gameOptionsIni.GetSectionKeys("MPColors");

            if (colorKeys == null)
                throw new ClientConfigurationException("[MPColors] not found in GameOptions.ini!");

            int nIndex = 0;
            if (newColors != null)
            {
                foreach (var color in newColors)
                {
                    string[] values = color.Split(',');

                    try
                    {
                        MultiplayerColor mpColor = MultiplayerColor.CreateFromStringArray(nIndex, ""/*key.L10N("UI:Color:" + key)*/, values);
                        mpColors.Add(mpColor);
                        nIndex++;
                    }
                    catch
                    {
                        throw new ClientConfigurationException("Invalid MPColor specified in GameOptions.ini: " + color);
                    }
                }
            }
            else
                foreach (string key in colorKeys)
                {
                    string[] values = gameOptionsIni.GetStringValue("MPColors", key, "255,255,255").Split(',');

                    try
                    {
                        MultiplayerColor mpColor = MultiplayerColor.CreateFromStringArray(nIndex, key/*key.L10N("UI:Color:" + key)*/, values);
                        mpColors.Add(mpColor);
                        nIndex++;
                    }
                    catch
                    {
                        throw new ClientConfigurationException("Invalid MPColor specified in GameOptions.ini: " + key);
                    }
                }

            var randomColorSection = gameOptionsIni.GetSection("MPColorsRandomLabel");
            if (randomColorSection != null)
                randomColorLabel = randomColorSection.GetStringValue("Text", null);

            colorList = mpColors;
            return new List<MultiplayerColor>(colorList);
        }


        /// <summary>
        /// 添加Mod自定义的颜色
        /// </summary>
        /// <param name="newColors"></param>
        /// <returns></returns>
        /// <exception cref="ClientConfigurationException"></exception>
        public static List<MultiplayerColor> LoadColorsFormMod(String ModID,List<string> newColors = null)
        {
            //if (colorList != null && newColors == null)
            //    return new List<MultiplayerColor>(colorList);

            IniFile gameOptionsIni = new IniFile(SafePath.CombineFilePath(ProgramConstants.GamePath, "Mod&AI", $"Mod&AI{ModID}.ini"));

            List<MultiplayerColor> mpColors = new List<MultiplayerColor>();

            List<string> colorKeys = gameOptionsIni.GetSectionKeys("MPColors");
            if (colorKeys == null)
                throw new ClientConfigurationException($"[MPColors] not found in {ModID}.ini! \n" + gameOptionsIni.FileName);

            int nIndex = 0;
            if (newColors != null)
            {
                foreach (var color in newColors)
                {
                    string[] values = color.Split(',');

                    try
                    {
                        MultiplayerColor mpColor = MultiplayerColor.CreateFromStringArray(nIndex, ""/*key.L10N("UI:Color:" + key)*/, values);
                        mpColors.Add(mpColor);
                        nIndex++;
                    }
                    catch
                    {
                        throw new ClientConfigurationException("Invalid MPColor specified in Mod.ini: " + color);
                    }
                }
            }
            else
                foreach (string key in colorKeys)
                {
                    string[] values = gameOptionsIni.GetStringValue("MPColors", key, "255,255,255").Split(',');

                    try
                    {
                        MultiplayerColor mpColor = MultiplayerColor.CreateFromStringArray(nIndex, key/*key.L10N("UI:Color:" + key)*/, values);
                        mpColors.Add(mpColor);
                        nIndex++;
                    }
                    catch
                    {
                        throw new ClientConfigurationException("Invalid MPColor specified in GameOptions.ini: " + key);
                    }
                }

            var randomColorSection = gameOptionsIni.GetSection("MPColorsRandomLabel");
            if (randomColorSection != null)
                randomColorLabel = randomColorSection.GetStringValue("Text", null);

            colorList = mpColors;
            return new List<MultiplayerColor>(colorList);
        }
        public static string GetRandomColorLabel()
            => string.IsNullOrEmpty(randomColorLabel) ? RandomColorDefaultLabel : randomColorLabel;
    }
}
