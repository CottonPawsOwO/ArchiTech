using Nautilus.Handlers;
using UnityEngine;
using System.IO;
using System.Reflection;

namespace ArchitechAdddedPDAReso
{
    public static class SilverOrePDA
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
            Texture2D silverTexture = LoadTextureFromFile("SilvEntry.png");
            Sprite silverPopup = CreateSpriteFromTexture(silverTexture);

            PDAHandler.AddEncyclopediaEntry(
                key: "SilverOre",
                path: "PlanetaryGeology",
                title: "Silver Ore",
                desc: "A reflective metal valuable in both electronic and ornamental applications, found within sandstone deposits.\n\n" +
                      "1. Conductive Properties:\n" +
                      "Silver possesses superior conductivity, essential for crafting advanced electronic circuits and enhancing data transmission.\n\n" +
                      "2. Geological Distribution:\n" +
                      "Typically found in deeper seabeds, silver ore forms through hydrothermal mineralization, where mineral-rich fluids permeate limestone beds.\n\n" +
                      "3. Anti-Bacterial Characteristics:\n" +
                      "Silver ions disrupt the metabolic processes of bacteria, explaining its natural prevalence as a sterilization agent within geothermal vent ecosystems.\n\n" +
                      "4. Engineering Applications:\n" +
                      "Beyond electronics, silver's lustrous finish makes it ideal for decorative enhancements to constructed bases, providing both form and function.\n\n" +
                      "Assessment: Critical for high-efficiency circuitry and decorative purposes within habitats.",
                image: silverTexture,
                popupImage: silverPopup,
                unlockSound: PDAHandler.UnlockBasic
            );

            PDAHandler.AddCustomScannerEntry(
                key: TechType.Silver,
                scanTime: 2f,
                destroyAfterScan: false,
                encyclopediaKey: "SilverOre"
            );
        }
    }
}