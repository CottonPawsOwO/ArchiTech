using Nautilus.Handlers;
using UnityEngine;
using System.IO;
using System.Reflection;

namespace ArchitechAdddedPDAReso
{
    public static class DiamondPDA
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
            Texture2D diamondTexture = LoadTextureFromFile("DiamEntry.png");
            Sprite diamondPopup = CreateSpriteFromTexture(diamondTexture);

            PDAHandler.AddEncyclopediaEntry(
                key: "Diamond",
                path: "PlanetaryGeology",
                title: "Diamond",
                desc: "A crystal lattice composed primarily of carbon, known for its unparalleled hardness and light refraction.\n\n" +
                      "1. Pressure Formed:\n" +
                      "Originating from the depths of 4546B, diamonds are birthed from intense geothermal and tectonic pressure, giving rise to flawless crystalline forms.\n\n" +
                      "2. Industrial Use:\n" +
                      "Leveraged in the production of precision tools and laser technology, diamonds are essential for cutting instruments and high-power laser fabrication.\n\n" +
                      "3. Unmatched Durability:\n" +
                      "The crystalline arrangement exhibits extreme resilience against stress and impact, suitable for protective casing materials in high-risk equipment.\n\n" +
                      "4. Geological Setting:\n" +
                      "Commonly associated with lava fields and volcanic fissures, the retrieval of diamonds requires careful navigation of inhospitable environments.\n\n" +
                      "Assessment: Indispensable for cutting-edge technology and essential for crafting advanced equipment.",
                image: diamondTexture,
                popupImage: diamondPopup,
                unlockSound: PDAHandler.UnlockBasic
            );

            PDAHandler.AddCustomScannerEntry(
                key: TechType.Diamond,
                scanTime: 3f,
                destroyAfterScan: false,
                encyclopediaKey: "Diamond"
            );
        }
    }
}