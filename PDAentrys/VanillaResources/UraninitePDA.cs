using Nautilus.Handlers;
using UnityEngine;
using System.IO;
using System.Reflection;

namespace ArchitechAdddedPDAReso
{
    public static class UraniniteGrystalPDA
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
            Texture2D uraniniteTexture = LoadTextureFromFile("UraniEntry.png");
            Sprite uraninitePopup = CreateSpriteFromTexture(uraniniteTexture);

            PDAHandler.AddEncyclopediaEntry(
                key: "UraniniteCrystal",
                path: "PlanetaryGeology",
                title: "Uraninite Crystal",
                desc: "A radioactive uranium oxide mineral, critical for advanced power generation and nuclear applications.\n\n" +
                      "1. Radioactive Properties:\n" +
                      "Uraninite crystals emit low-level radiation through natural uranium decay, requiring careful handling and specialized containment systems to prevent exposure during extraction and processing.\n\n" +
                      "2. Nuclear Energy Potential:\n" +
                      "The fissile isotopes within uraninite make it essential for nuclear reactor fuel fabrication, providing immense energy density for deep-sea power requirements.\n\n" +
                      "3. Crystal Structure Analysis:\n" +
                      "Exhibits a fluorite-type cubic crystal structure with uranium atoms surrounded by eight oxygen atoms, creating a stable yet energy-rich molecular arrangement.\n\n" +
                      "4. Geological Formation:\n" +
                      "Forms in oxygen-poor environments through hydrothermal processes, typically found near ancient volcanic systems where uranium-rich fluids crystallized over geological time scales.\n\n" +
                      "5. Safety Considerations:\n" +
                      "Prolonged exposure to uraninite can cause radiation poisoning. Automated extraction systems and lead-lined storage containers are mandatory for safe handling procedures.\n\n" +
                      "Assessment: Essential for nuclear power systems and advanced energy applications. Extreme caution required during all handling procedures.",
                image: uraniniteTexture,
                popupImage: uraninitePopup,
                unlockSound: PDAHandler.UnlockBasic
            );

            PDAHandler.AddCustomScannerEntry(
                key: TechType.UraniniteCrystal,
                scanTime: 5f,
                destroyAfterScan: false,
                encyclopediaKey: "UraniniteCrystal"
            );
        }
    }
}