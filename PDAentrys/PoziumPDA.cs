using Nautilus.Handlers;
using UnityEngine;
using System.IO;
using System.Reflection;

namespace ArchiTech
{
    public static class PoziumPDA
    {
        // Load texture from the mod's Assets/PDA folder
        private static Texture2D LoadTextureFromFile(string fileName)
        {
            try
            {
                string pluginPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                string assetsPath = Path.Combine(pluginPath, "Assets", "PDA");
                string filePath = Path.Combine(assetsPath, fileName);

                Plugin.Logger.LogInfo($"[PoziumPDA] Looking for texture in subfolder: {filePath}");

                if (!File.Exists(filePath))
                {
                    Plugin.Logger.LogWarning($"[PoziumPDA] Texture file not found in Assets/PDA: {filePath}");
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
                    Plugin.Logger.LogInfo($"[PoziumPDA] Successfully loaded texture: {fileName} ({tex.width}x{tex.height})");
                    return tex;
                }

                UnityEngine.Object.DestroyImmediate(tex);
                return null;
            }
            catch (System.Exception ex)
            {
                Plugin.Logger.LogError($"[PoziumPDA] Exception loading texture: {ex}");
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

        public static void Register(TechType poziumTechType)
        {
            Plugin.Logger.LogInfo("[PoziumPDA] Registering Pozium PDA entry...");

            // Load your custom image from the Assets/PDA folder
            Texture2D poziumTexture = LoadTextureFromFile("PoziEntry.png");
            Sprite poziumPopup = CreateSpriteFromTexture(poziumTexture);

            if (poziumTexture == null)
            {
                Plugin.Logger.LogWarning("[PoziumPDA] Custom texture failed to load, entry will have no image");
            }

            // Register the encyclopedia PDA entry
            PDAHandler.AddEncyclopediaEntry(
                key: "Pozium",
                path: "PlanetaryGeology",
                title: "Pozium",
                desc: "A rare crystalline ore containing plasma gases trapped within its lattice structure. Found in kelp-rich areas, this dangerous material can cause electrotrauma if handled without proper isolation equipment.\n\n" +
                      "ALTERRA XENOGEOLOGY DIVISION\n" +
                      "MATERIAL ANALYSIS REPORT: POZIUM\n\n" +
                      "CLASSIFICATION: Rare Crystalline Ore\n" +
                      "DANGER LEVEL: HIGH - Electrotrauma Risk\n" +
                      "CONTAINMENT PROTOCOL: Class-3 Isolation Required\n\n" +
                      "1. Geological Composition:\n" +
                      "Pozium represents a unique crystalline formation discovered exclusively in kelp-rich oceanic environments. The mineral exhibits a distinctive whitish-pink coloration with characteristic sky-blue luminescent inclusions concentrated at crystal termination points.\n\n" +
                      "2. Formation Process:\n" +
                      "Pozium crystals form through a complex biochemical process involving organic compounds from decomposing kelp biomatter, hydrothermal mineral precipitation from seabed vents, and electromagnetic field interactions with deep-ocean current systems. The presence of blood kelp appears to accelerate formation, leading to significantly larger deposit concentrations in these biomes.\n\n" +
                      "3. Plasma Gas Content:\n" +
                      "Analysis reveals trapped plasma gases within the crystal matrix, consisting primarily of ionized hydrogen and helium isotopes, trace amounts of xenon and argon, electrically charged metallic vapor particles, and exotic quantum-entangled particle pairs. These gases exhibit remarkable energy density, making Pozium invaluable for advanced power cell applications.\n\n" +
                      "4. Electrical Properties:\n" +
                      "WARNING: Pozium crystals naturally conduct electrical current through their surface layer. Direct contact without proper isolation equipment will result in immediate electrotrauma, nervous system disruption, potential cardiac arrhythmia, and temporary or permanent neurological damage.\n\n" +
                      "5. Safety Protocols:\n" +
                      "Mandatory equipment includes reinforced isolation gloves (minimum 10kV rating), electrical hazard suits when handling large quantities, grounding equipment for extended exposure, and emergency medical kits with cardiac stimulants. Never approach deposits without proper protection.\n\n" +
                      "Assessment: Essential for advanced power cell manufacturing, high-capacity battery production, and plasma energy research. The energy potential of Pozium plasma makes it critical for advanced colonial operations, but safety must remain the primary concern.\n\n" +
                      "Remember: Respect the power of Pozium, and it will power your future.",
                image: poziumTexture,
                popupImage: poziumPopup,
                unlockSound: PDAHandler.UnlockBasic
            );

            // Register the scanner entry for Pozium
            PDAHandler.AddCustomScannerEntry(
                key: poziumTechType,
                scanTime: 5f, // Longer scan time due to danger
                destroyAfterScan: false,
                encyclopediaKey: "Pozium"
            );

            Plugin.Logger.LogInfo("[PoziumPDA] Pozium PDA entry registered successfully.");
        }
    }
}
