using Arcade.UI;
using HarmonyLib;
using MelonLoader;
using MelonLoader.Utils;
using System.Drawing;
using System.Globalization;
using UnityEngine;
using static MelonLoader.MelonLogger;
using Color = UnityEngine.Color;

[assembly: MelonInfo(typeof(UBCustomPaletteLoader.Core), "UBCustomPaletteLoader", "1.0.0", "CosmicDreams", null)]
[assembly: MelonGame("D-CELL GAMES", "UNBEATABLE")]

namespace UBCustomPaletteLoader
{
    public class Core : MelonMod
    {
        public override void OnInitializeMelon()
        {
            LoggerInstance.Msg("Initialized UBCustomPaletteLoader");
        }



        public static Color[] GetColors()
        {
            string[] file = File.ReadAllLines(Path.Combine(MelonEnvironment.ModsDirectory, "UBCustomPalette.txt"));
            Color[] colors = new Color[10];

            for (int i = 0; i < colors.Length; i++)
            {
                string argb = file[i].Substring(0, 8); //take just the hex code
                var col = System.Drawing.Color.FromArgb(int.Parse(argb, NumberStyles.HexNumber)); //convert it to a color

                colors[i] = new Color((float)col.R / 255, (float)col.G / 255, (float)col.B / 255, (float)col.A / 255); //convert color to unity color
            }

            return colors;
        }

        [HarmonyPatch(typeof(MenuPaletteIndex), "AddPalettes", new Type[] { typeof(List<MenuPaletteIndex.Palette>) })]
        private static class Patch
        {
            private static void Prefix(ref List<MenuPaletteIndex.Palette> newPalette)
            {
                UIColorPalette customPalette = new UIColorPalette();
                customPalette.name = "MenuPalette_Custom";
                customPalette.colors = GetColors();
                newPalette.Add(new MenuPaletteIndex.Palette { name = "Custom", palette = customPalette });
            }
        }
    }
}