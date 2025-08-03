using Nautilus.Handlers;
using UnityEngine;
using System.IO;
using System.Reflection;

namespace ArchiTech
{
    public static class IridiumPDA
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

                Plugin.Logger.LogInfo($"[IridiumPDA] Looking for texture in subfolder: {filePath}");

                if (!File.Exists(filePath))
                {
                    Plugin.Logger.LogWarning($"[IridiumPDA] Texture file not found in Assets/PDA: {filePath}");
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
                    Plugin.Logger.LogInfo($"[IridiumPDA] Successfully loaded texture: {fileName} ({tex.width}x{tex.height})");
                    return tex;
                }

                UnityEngine.Object.DestroyImmediate(tex);
                return null;
            }
            catch (System.Exception ex)
            {
                Plugin.Logger.LogError($"[IridiumPDA] Exception loading texture: {ex}");
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

        public static void Register(TechType iridiumTechType)
        {
            Plugin.Logger.LogInfo("[IridiumPDA] Registering Iridium PDA entry...");

            // Load your custom image from the Assets/PDA folder
            Texture2D iridiumTexture = LoadTextureFromFile("IridEntry.png");
            Sprite iridiumPopup = CreateSpriteFromTexture(iridiumTexture);

            if (iridiumTexture == null)
            {
                Plugin.Logger.LogWarning("[IridiumPDA] Custom texture failed to load, entry will have no image");
            }

            // Register the encyclopedia PDA entry
            PDAHandler.AddEncyclopediaEntry(
                key: "Iridium",
                path: "PlanetaryGeology",
                title: "Iridium",
                desc: "An exceptionally rare, dense metallic ore with extraordinary corrosion resistance found exclusively in 4546B's crash zone regions.\n\n" +
                      "1. Extreme Density:\n" +
                      "With a density of 22.56 g/cm³, iridium ranks as the second-densest naturally occurring element. This extreme density creates unique gravitational micro-signatures detectable by advanced scanning equipment, though such readings are often masked by the electromagnetic interference from nearby wreckage.\n\n" +
                      "2. Corrosion Immunity:\n" +
                      "Spectral analysis confirms absolute resistance to all known corrosive agents, including the highly acidic compounds found in deep-sea thermal vents. This property stems from iridium's complete electron shell stability, making it invaluable for extreme-environment applications.\n\n" +
                      "3. Impact Origin:\n" +
                      "Isotope ratios indicate extraterrestrial origin - these deposits likely formed from asteroid impacts during 4546B's bombardment period approximately 2.8 billion years ago. The metal's current surface accessibility suggests recent geological disruption has exposed previously buried impact sites.\n\n" +
                      "4. Crash Zone Concentration:\n" +
                      "Paradoxically, iridium deposits show highest concentration near the Aurora crash site. Seismic analysis suggests the ship's impact reactivated dormant fault lines, fracturing deep geological layers and bringing ancient impact deposits within accessible range.\n\n" +
                      "The crash zone's extreme biological hazards appear correlated with iridium's electromagnetic properties. Local apex predators may be attracted to the metal's subtle energy signature, creating a high-risk extraction environment.\n\n" +
                      "Assessment: Critical component for advanced fabrication systems requiring absolute material stability. Extreme extraction hazards necessitate comprehensive safety protocols.",
                image: iridiumTexture,
                popupImage: iridiumPopup,
                unlockSound: PDAHandler.UnlockBasic
            );

            // Register the scanner entry for Iridium
            PDAHandler.AddCustomScannerEntry(
                key: iridiumTechType,
                scanTime: 3f,
                destroyAfterScan: false,
                encyclopediaKey: "Iridium"
            );

            Plugin.Logger.LogInfo("[IridiumPDA] Iridium PDA entry registered successfully.");
        }
    }
}