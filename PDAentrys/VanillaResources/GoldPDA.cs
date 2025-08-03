using Nautilus.Handlers;
using UnityEngine;
using System.IO;
using System.Reflection;

namespace ArchitechAdddedPDAReso
{
    public static class GoldPDA
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
            Texture2D goldTexture = LoadTextureFromFile("GoldEntry.png");
            Sprite goldPopup = CreateSpriteFromTexture(goldTexture);

            PDAHandler.AddEncyclopediaEntry(
                key: "Gold",
                path: "PlanetaryGeology",
                title: "Gold",
                desc: "A precious metal known for its luster and conductivity, mined from sandstone and shale outcroppings.\n\n" +
                      "1. Molecular Density:\n" +
                      "Gold's dense molecular structure grants it weight and malleability, allowing it to be easily shaped into wiring for electronic applications.\n\n" +
                      "2. Corrosion Resistance:\n" +
                      "Impressive resistance to oxidation and corrosion makes gold suitable for long-term electronic stability, especially crucial in high-pressure ocean depths.\n\n" +
                      "3. Radiant Conductivity:\n" +
                      "When exposed to certain radiation frequencies, gold gains a temporary radiance which enhances the efficiency of solar panels and other energy devices.\n\n" +
                      "4. Cultural Significance:\n" +
                      "Serving as a universal medium of exchange, gold remains valuable for trade, crafting, and ornamental purposes.\n\n" +
                      "Assessment: Highly conductive and valuable for advanced tech solutions and bartering.",
                image: goldTexture,
                popupImage: goldPopup,
                unlockSound: PDAHandler.UnlockBasic
            );

            PDAHandler.AddCustomScannerEntry(
                key: TechType.Gold,
                scanTime:3f,
                destroyAfterScan: false,
                encyclopediaKey: "Gold"
            );
        }
    }
}