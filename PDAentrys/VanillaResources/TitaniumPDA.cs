using Nautilus.Handlers;
using UnityEngine;
using System.IO;
using System.Reflection;

namespace ArchitechAdddedPDAReso
{
    public static class TitaniumPDA
    {
        private static Texture2D LoadTextureFromFile(string fileName)
        {
            try
            {
                string pluginPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                string assetsPath = Path.Combine(pluginPath, "Assets", "PDA");
                string filePath = Path.Combine(assetsPath, fileName);

                if (!File.Exists(filePath))
                {
                    return null;
                }

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
            Texture2D titaniumTexture = LoadTextureFromFile("TitaEntry.png");
            Sprite titaniumPopup = CreateSpriteFromTexture(titaniumTexture);

            PDAHandler.AddEncyclopediaEntry(
                key: "Titanium",
                path: "PlanetaryGeology",
                title: "Titanium",
                desc: "A robust metal extracted from sandstone outcroppings and metal salvage, used in numerous construction applications.\n\n" +
                      "1. Durability and Usage:\n" +
                      "Titanium is celebrated for its strength-to-weight ratio, making it a favored material in the construction of underwater habitats and vehicles.\n\n" +
                      "2. Mineral Characteristics:\n" +
                      "The ore's significant corrosion resistance ensures long-term durability, even in the harsh saltwater conditions of 4546B's sloping trenches and mesas.\n\n" +
                      "3. Alloy Potential:\n" +
                      "Due to its natural abundance, titanium serves as an excellent alloy base for crafting advanced tools and components. Research into superalloys reveals its potential to enhance thermal resistance and structural integrity.\n\n" +
                      "4. Extraction and Harvesting:\n" +
                      "Once processed, titanium inherits characteristics similar to refined metals on Earth, with applications extending from basic construction to retrofitting escape pods and submersibles.\n\n" +
                      "Assessment: Vital for structural integrity and crafting comprehensive underwater bases.",
                image: titaniumTexture,
                popupImage: titaniumPopup,
                unlockSound: PDAHandler.UnlockBasic
            );

            PDAHandler.AddCustomScannerEntry(
                key: TechType.Titanium,
                scanTime: 2f,
                destroyAfterScan: false,
                encyclopediaKey: "Titanium"
            );
        }
    }
}
