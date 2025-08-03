using Nautilus.Handlers;
using UnityEngine;
using System.IO;
using System.Reflection;

namespace ArchiTech
{
    public static class GalliumPDA
    {
        // Load texture from the mod's Assets/PDA folder
        private static Texture2D LoadTextureFromFile(string fileName)
        {
            try
            {
                string pluginPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                // Correct path to include the 'PDA' subfolder
                string assetsPath = Path.Combine(pluginPath, "Assets", "PDA");
                string filePath = Path.Combine(assetsPath, fileName);

                Plugin.Logger.LogInfo($"[GalliumPDA] Looking for texture in subfolder: {filePath}");

                if (!File.Exists(filePath))
                {
                    Plugin.Logger.LogWarning($"[GalliumPDA] Texture file not found in Assets/PDA: {filePath}");
                    return null;
                }

                byte[] fileData = File.ReadAllBytes(filePath);

                // Create texture with proper format and settings
                Texture2D tex = new Texture2D(2, 2, TextureFormat.RGBA32, false);
                tex.filterMode = FilterMode.Bilinear;
                tex.wrapMode = TextureWrapMode.Clamp;

                if (tex.LoadImage(fileData))
                {
                    tex.Apply();
                    Plugin.Logger.LogInfo($"[GalliumPDA] Successfully loaded texture: {fileName} ({tex.width}x{tex.height})");
                    return tex;
                }

                UnityEngine.Object.DestroyImmediate(tex);
                return null;
            }
            catch (System.Exception ex)
            {
                Plugin.Logger.LogError($"[GalliumPDA] Exception loading texture: {ex}");
                return null;
            }
        }

        private static Sprite CreateSpriteFromTexture(Texture2D tex)
        {
            if (tex == null) return null;

            Sprite sprite = Sprite.Create(
                tex,
                new Rect(0, 0, tex.width, tex.height),
                new Vector2(0.5f, 0.5f),
                100.0f
            );

            return sprite;
        }

        public static void Register(TechType galliumTechType)
        {
            Plugin.Logger.LogInfo("[GalliumPDA] Registering Gallium PDA entry...");

            // Load your custom image from the Assets/PDA folder
            Texture2D galliumTexture = LoadTextureFromFile("GallEntry.png");
            Sprite galliumPopup = CreateSpriteFromTexture(galliumTexture);

            if (galliumTexture == null)
            {
                Plugin.Logger.LogWarning("[GalliumPDA] Custom texture failed to load, entry will have no image");
            }

            // Register the encyclopedia PDA entry
            PDAHandler.AddEncyclopediaEntry(
    key: "Gallium",
    path: "PlanetaryGeology",
    title: "Gallium",
    desc: "A versatile silvery-metallic element critical to advanced electronics and semiconductor applications. Found in aluminum and zinc ore deposits throughout 4546B's shallow biomes.\n\n" +
                      "1. Physical Properties:\n" +
                      "Gallium exhibits unusual behavior - liquid at 29.8°C on Earth, but remains solid on 4546B due to the planet's unique atmospheric pressure and temperature conditions. The metal maintains a distinctive blue-gray metallic luster and exceptional malleability at ambient temperatures.\n\n" +
                      "2. Geological Distribution:\n" +
                      "Occurs primarily as trace concentrations within bauxite (aluminum ore) and sphalerite (zinc ore) deposits. On 4546B, volcanic activity has concentrated gallium into accessible nodules within shallow rock formations, making extraction significantly more efficient than Earth-based mining.\n\n" +
                      "3. Electronic Applications:\n" +
                      "Essential for fabricating high-frequency semiconductors, particularly gallium arsenide compounds used in advanced communication systems and quantum processors. The element's unique electron mobility properties make it superior to silicon for specialized applications.\n\n" +
                      "4. Alterra Applications:\n" +
                      "Critical component in fabricator quantum matrices, seamoth sonar systems, and base communication arrays. Its low melting point allows for precision electronics manufacturing in standard fabrication units without specialized heating systems.\n\n" +
                      "The element's abundance in 4546B's shallow biomes suggests optimal conditions for electronics manufacturing, likely explaining Alterra's selection of this planet for research installations.\n\n" +
                      "Assessment: High-priority resource for maintaining and upgrading electronic systems. Essential for advanced technology fabrication.",

    image: galliumTexture,
    popupImage: galliumPopup,
    unlockSound: PDAHandler.UnlockBasic
);

            // Register the scanner entry for Gallium
            PDAHandler.AddCustomScannerEntry(
                key: galliumTechType,
                scanTime: 1f,
                destroyAfterScan: false,
                encyclopediaKey: "Gallium"
            );

            Plugin.Logger.LogInfo("[GalliumPDA] Gallium PDA entry registered successfully.");
        }
    }
}
