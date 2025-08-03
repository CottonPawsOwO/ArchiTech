using Nautilus.Handlers;
using UnityEngine;
using System.IO;
using System.Reflection;

namespace ArchiTech
{
    public static class PlatinumPDA
    {
        // Load texture from the mod's Assets/PDA folder
        private static Texture2D LoadTextureFromFile(string fileName)
        {
            try
            {
                string pluginPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                string assetsPath = Path.Combine(pluginPath, "Assets", "PDA");
                string filePath = Path.Combine(assetsPath, fileName);

                Plugin.Logger.LogInfo($"[PlatinumPDA] Looking for texture in subfolder: {filePath}");

                if (!File.Exists(filePath))
                {
                    Plugin.Logger.LogWarning($"[PlatinumPDA] Texture file not found in Assets/PDA: {filePath}");
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
                    Plugin.Logger.LogInfo($"[PlatinumPDA] Successfully loaded texture: {fileName} ({tex.width}x{tex.height})");
                    return tex;
                }

                UnityEngine.Object.DestroyImmediate(tex);
                return null;
            }
            catch (System.Exception ex)
            {
                Plugin.Logger.LogError($"[PlatinumPDA] Exception loading texture: {ex}");
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

        public static void Register(TechType platinumTechType)
        {
            Plugin.Logger.LogInfo("[PlatinumPDA] Registering Platinum PDA entry...");

            // Load your custom image from the Assets/PDA folder
            Texture2D platinumTexture = LoadTextureFromFile("PlatEntry.png");
            Sprite platinumPopup = CreateSpriteFromTexture(platinumTexture);

            if (platinumTexture == null)
            {
                Plugin.Logger.LogWarning("[PlatinumPDA] Custom texture failed to load, entry will have no image");
            }

            // Register the encyclopedia PDA entry
            PDAHandler.AddEncyclopediaEntry(
    key: "Platinum",
    path: "PlanetaryGeology",
    title: "Platinum",
    desc: "A lustrous, silvery-white metallic ore exhibiting exceptional corrosion resistance and found primarily in high-temperature geological formations.\n\n" +
          "1. Atomic Density:\n" +
          "This platinum variant displays 15% higher atomic density than standard platinum group metals. Spectrographic analysis suggests the presence of ultra-heavy isotopes not found in typical stellar formation processes.\n\n" +
          "2. Catalytic Properties:\n" +
          "The metal demonstrates remarkable catalytic efficiency, capable of accelerating chemical reactions at rates 400% faster than conventional platinum. This property stems from an unusual surface electron configuration.\n\n" +
          "3. Magnetic Resistance:\n" +
          "Despite its metallic nature, the ore exhibits strong diamagnetic properties, actively repelling magnetic fields. This characteristic makes it ideal for precision instruments and electromagnetic shielding applications.\n\n" +
          "4. Geological Distribution:\n" +
          "Platinum deposits concentrate near hydrothermal vents and lava tube systems, suggesting formation through high-temperature mineralization processes. The metal's crystalline structure indicates cooling rates of approximately 0.001°C per millennium.\n\n" +
          "5. Surface Luminescence:\n" +
          "Under low-light conditions, the ore exhibits a faint blue-white phosphorescence lasting up to 3.2 hours after light exposure. This bioluminescent quality appears linked to trapped photonic energy within the crystal lattice.\n\n" +
          "The scarcity of large deposits in shallow waters supports the theory that platinum formation requires sustained high-pressure, high-temperature conditions found only in the planet's deeper geological layers.\n\n" +
          "Assessment: Critical component for manufacturing precision electronics, advanced propulsion systems, and corrosion-resistant equipment for extreme environment exploration.",
    image: platinumTexture,
    popupImage: platinumPopup,
    unlockSound: PDAHandler.UnlockBasic
            );

            // Register the scanner entry for Platinum
            PDAHandler.AddCustomScannerEntry(
                key: platinumTechType,
                scanTime: 4f,
                destroyAfterScan: false,
                encyclopediaKey: "Platinum"
            );

            Plugin.Logger.LogInfo("[PlatinumPDA] Platinum PDA entry registered successfully.");
        }
    }
}