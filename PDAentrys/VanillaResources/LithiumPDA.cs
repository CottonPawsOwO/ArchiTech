using Nautilus.Handlers;
using UnityEngine;
using System.IO;
using System.Reflection;

namespace ArchitechAdddedPDAReso
{
    public static class LithiumPDA
    {
        private static Texture2D LoadTextureFromFile(string fileName)
        {
            try
            {
                string pluginPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                string assetsPath = Path.Combine(pluginPath, "Assets", "PDA");
                string filePath = Path.Combine(assetsPath, fileName);

                if (!File.Exists(filePath))
                    return null;

                byte[] fileData = File.ReadAllBytes(filePath);
                Texture2D tex = new Texture2D(2, 2, TextureFormat.RGBA32, false);
                tex.filterMode = FilterMode.Bilinear;
                tex.wrapMode = TextureWrapMode.Clamp;

                if (tex.LoadImage(fileData))
                {
                    tex.Apply();
                    return tex;
                }
                UnityEngine.Object.DestroyImmediate(tex);
                return null;
            }
            catch
            {
                return null;
            }
        }

        private static Sprite CreateSpriteFromTexture(Texture2D tex)
        {
            if (tex == null) return null;
            return Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
        }

        public static void Register()
        {
            Texture2D lithiumTexture = LoadTextureFromFile("LithEntry.png");
            Sprite lithiumPopup = CreateSpriteFromTexture(lithiumTexture);

            PDAHandler.AddEncyclopediaEntry(
                key: "Lithium",
                path: "PlanetaryGeology",
                title: "Lithium",
                desc: "A lightweight element favored for its application in high-strength alloys and battery production.\n\n" +
                      "1. Thermal Resistance:\n" +
                      "Lithium's incorporation into alloy fabrication processes is attributed to its high melting point and low thermal expansion, making it ideal for thermal shields.\n\n" +
                      "2. Energy Storage:\n" +
                      "This alkali metal is crucial for compact energy systems; its ions move swiftly in a battery cell, ensuring high energy density with rapid charge-discharge cycles.\n\n" +
                      "3. Geological Origin:\n" +
                      "Naturally occurs in mineral-rich seabeds, its deposits are often layered within the seabed near hydrothermal vents, often in clusters with rare earth elements.\n\n" +
                      "4. Biological Integration:\n" +
                      "Speculation suggests that certain marine organisms utilize lithium to regulate cellular energy pathways; however, this is yet unconfirmed.\n\n" +
                      "Assessment: Highly valued for energy systems, structural components, and its capability to withstand extreme pressures.",
                image: lithiumTexture,
                popupImage: lithiumPopup,
                unlockSound: PDAHandler.UnlockBasic
            );

            PDAHandler.AddCustomScannerEntry(
                key: TechType.Lithium,
                scanTime: 3f,
                destroyAfterScan: false,
                encyclopediaKey: "Lithium"
            );
        }
    }
}