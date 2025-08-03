using Nautilus.Handlers;
using UnityEngine;
using System.IO;
using System.Reflection;

namespace ArchitechAdddedPDAReso
{
    public static class RubyPDA
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
            Texture2D rubyTexture = LoadTextureFromFile("RubyEntry.png");
            Sprite rubyPopup = CreateSpriteFromTexture(rubyTexture);

            PDAHandler.AddEncyclopediaEntry(
                key: "AluminumOxide",
                path: "PlanetaryGeology",
                title: "Ruby",
                desc: "A precious red corundum crystal, valued for both its aesthetic appeal and advanced technological applications.\n\n" +
                      "1. Crystal Formation:\n" +
                      "Ruby's distinctive red coloration results from chromium impurities within the aluminum oxide crystal lattice, formed under extreme metamorphic conditions deep within 4546B's crust.\n\n" +
                      "2. Laser Applications:\n" +
                      "The crystal's optical properties make it ideal for laser focusing systems, where its ability to withstand high-energy light beams enables precision cutting and welding operations.\n\n" +
                      "3. Hardness and Durability:\n" +
                      "Ranking 9 on the Mohs hardness scale, ruby's exceptional durability makes it suitable for high-wear mechanical components and protective equipment in extreme environments.\n\n" +
                      "4. Thermal Conductivity:\n" +
                      "Despite its crystalline structure, ruby exhibits excellent thermal conductivity, making it valuable for heat sink applications in advanced electronic systems.\n\n" +
                      "5. Electromagnetic Properties:\n" +
                      "Ruby crystals can be precisely tuned to specific electromagnetic frequencies, essential for communication arrays and navigation equipment.\n\n" +
                      "Assessment: Essential for laser technology, precision instruments, and high-performance electronic systems.",
                image: rubyTexture,
                popupImage: rubyPopup,
                unlockSound: PDAHandler.UnlockBasic
            );

            PDAHandler.AddCustomScannerEntry(
                key: TechType.AluminumOxide,
                scanTime: 4f,
                destroyAfterScan: false,
                encyclopediaKey: "AluminumOxide"
            );
        }
    }
}