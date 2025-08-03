using Nautilus.Handlers;
using UnityEngine;
using System.IO;
using System.Reflection;

namespace ArchitechAdddedPDAReso
{
    public static class NickelOrePDA
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
            Texture2D nickelTexture = LoadTextureFromFile("NickeEntry.png");
            Sprite nickelPopup = CreateSpriteFromTexture(nickelTexture);

            PDAHandler.AddEncyclopediaEntry(
                key: "NickelOre",
                path: "PlanetaryGeology",
                title: "Nickel Ore",
                desc: "A robust alloying agent, often found in the lower regions of 4546B, renowned for its corrosion resistance.\n\n" +
                      "1. Composition and Mining:\n" +
                      "Nickel exhibits excellent resistance to oxidation, making it a preferred choice in alloy creation. It is extracted from deep-sea vents and ridges.\n\n" +
                      "2. Industrial Utility:\n" +
                      "Nickel improves the mechanical properties of materials it alloyed with, like steel, enhancing durability, especially in pressured environments.\n\n" +
                      "3. Thermal Regulation:\n" +
                      "Applied in the production of heat exchangers, nickel assists in regulating thermal energy, crucial for maintaining temperature stability in atmospheric systems.\n\n" +
                      "4. Biological Neutrality:\n" +
                      "Its neutral electrochemical properties hint that it may be involved in regulatory thermal interactions of marine fauna, although further study is required.\n\n" +
                      "Assessment: Ideal for crafting high-strength alloys and thermal regulation technology.",
                image: nickelTexture,
                popupImage: nickelPopup,
                unlockSound: PDAHandler.UnlockBasic
            );

            PDAHandler.AddCustomScannerEntry(
                key: TechType.Nickel,
                scanTime: 3f,
                destroyAfterScan: false,
                encyclopediaKey: "NickelOre"
            );
        }
    }
}