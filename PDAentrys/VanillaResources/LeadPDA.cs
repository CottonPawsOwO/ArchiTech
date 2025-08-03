using Nautilus.Handlers;
using UnityEngine;
using System.IO;
using System.Reflection;

namespace ArchitechAdddedPDAReso
{
    public static class LeadPDA
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
            Texture2D leadTexture = LoadTextureFromFile("LeadEntry.png");
            Sprite leadPopup = CreateSpriteFromTexture(leadTexture);

            PDAHandler.AddEncyclopediaEntry(
                key: "Lead",
                path: "PlanetaryGeology",
                title: "Lead",
                desc: "A dense metal predominantly used for radiation shielding and found within sandstone deposits.\n\n" +
                      "1. Shielding Properties:\n" +
                      "Lead's high density blocks harmful radiation, making it vital for constructing safe habitats and nuclear facilities.\n\n" +
                      "2. Geological Distribution:\n" +
                      "Deposits are commonly associated with tectonic rifts and deep-sea hydrothermal activity, suggesting formation through mineral-rich fluid displacement.\n\n" +
                      "3. Toxicity Concerns:\n" +
                      "Prolonged exposure to seawater leads to gradual lead leaching, posing environmental and health risks. Proper handling and containment are essential for safety.\n\n" +
                      "4. Industrial Applications:\n" +
                      "Utilized in pressure-resistant pigments, lead enhances durability in paint coatings used on submersibles and base structures.\n\n" +
                      "Assessment: Essential for radiation protection and marine safety; handle with care.",
                image: leadTexture,
                popupImage: leadPopup,
                unlockSound: PDAHandler.UnlockBasic
            );

            PDAHandler.AddCustomScannerEntry(
                key: TechType.Lead,
                scanTime: 2f,
                destroyAfterScan: false,
                encyclopediaKey: "Lead"
            );
        }
    }
}