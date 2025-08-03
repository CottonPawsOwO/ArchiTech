using Nautilus.Handlers;
using UnityEngine;
using System.IO;
using System.Reflection;

namespace ArchiTech
{
    public static class MercuryPDA
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

                Plugin.Logger.LogInfo($"[MercuryPDA] Looking for texture in subfolder: {filePath}");

                if (!File.Exists(filePath))
                {
                    Plugin.Logger.LogWarning($"[MercuryPDA] Texture file not found in Assets/PDA: {filePath}");
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
                    Plugin.Logger.LogInfo($"[MercuryPDA] Successfully loaded texture: {fileName} ({tex.width}x{tex.height})");
                    return tex;
                }

                UnityEngine.Object.DestroyImmediate(tex);
                return null;
            }
            catch (System.Exception ex)
            {
                Plugin.Logger.LogError($"[MercuryPDA] Exception loading texture: {ex}");
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

        public static void Register(TechType mercuryTechType)
        {
            Plugin.Logger.LogInfo("[MercuryPDA] Registering Mercury PDA entry...");

            // Load your custom image from the Assets/PDA folder
            Texture2D mercuryTexture = LoadTextureFromFile("MercEntry.png");
            Sprite mercuryPopup = CreateSpriteFromTexture(mercuryTexture);

            if (mercuryTexture == null)
            {
                Plugin.Logger.LogWarning("[MercuryPDA] Custom texture failed to load, entry will have no image");
            }

            // Register the encyclopedia PDA entry
            PDAHandler.AddEncyclopediaEntry(
    key: "Mercury",
    path: "PlanetaryGeology",
    title: "Mercury",
    desc: "A dense metallic ore found in bright red cinnabar deposits throughout 4546B's mid-depth biomes.\n\n" +
          "1. Crystalline Structure:\n" +
          "Unlike Earth-based mercury ores, this variant exhibits a unique hexagonal crystal lattice that remains stable at ambient temperatures. The structure suggests formation under extreme pressure conditions, likely during the planet's early geological development.\n\n" +
          "2. Thermal Properties:\n" +
          "Spectral analysis reveals unusual thermal conductivity - approximately 340% higher than standard mercury compounds. This property makes it invaluable for heat dissipation in advanced electronics and power systems.\n\n" +
          "3. Electromagnetic Signature:\n" +
          "The ore generates a weak but consistent electromagnetic field, detectable at ranges up to 15 meters. This phenomenon appears linked to trace amounts of an unknown isotope embedded within the crystal matrix.\n\n" +
          "4. Formation Theory:\n" +
          "Geological surveys indicate these deposits formed through hydrothermal venting during the planet's volcanic period. The distinctive red coloration results from iron oxide inclusions, suggesting the ore crystallized in oxygen-rich underwater environments.\n\n" +
          "While mercury is typically liquid at standard temperatures, 4546B's variant remains solid due to unique atmospheric pressure and mineral composition. Its abundance in deeper biomes correlates with ancient volcanic activity patterns.\n\n" +
          "Assessment: Essential for fabricating thermal management systems, conductive components required for deep-sea operations.",
    image: mercuryTexture,
    popupImage: mercuryPopup,
    unlockSound: PDAHandler.UnlockBasic
);

            // Register the scanner entry for Mercury
            PDAHandler.AddCustomScannerEntry(
                key: mercuryTechType,
                scanTime: 2f,
                destroyAfterScan: false,
                encyclopediaKey: "Mercury"
            );

            Plugin.Logger.LogInfo("[MercuryPDA] Mercury PDA entry registered successfully.");
        }
    }
}