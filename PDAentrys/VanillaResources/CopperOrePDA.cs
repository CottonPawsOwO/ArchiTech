using Nautilus.Handlers;
using UnityEngine;
using System.IO;
using System.Reflection;

namespace ArchitechAdddedPDAReso
{
    public static class CopperOrePDA
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
            Texture2D copperTexture = LoadTextureFromFile("CoppEntry.png");
            Sprite copperPopup = CreateSpriteFromTexture(copperTexture);

            PDAHandler.AddEncyclopediaEntry(
                key: "CopperOre",
                path: "PlanetaryGeology",
                title: "Copper Ore",
                desc: "A reddish-brown metallic ore, vital for electrical systems on Planet 4546B.\n\n" +
                      "1. Structure and Formation:\n" +
                      "Copper Ore is found predominantly within limestone outcrops scattered across the seabed, particularly in shallow biomes. Its conductive properties make it essential for developing electronic devices.\n\n" +
                      "2. Chemical Composition:\n" +
                      "Composed primarily of copper carbonate and copper sulfate, the ore exhibits high ductility and malleability. Traces of native copper often form metallic veins visible within the stone.\n\n" +
                      "3. Environmental Adaptation:\n" +
                      "This adaptable ore has managed to thrive on the planet, forming extensive colonies due to its minimal impact on the environment. It plays a critical role in underwater ecosystems, functioning as both producer and consumer within its niche.\n\n" +
                      "4. Practical Applications:\n" +
                      "Used extensively in wiring and electronics, Copper Ore is crucial for crafting tools, power modules, and communication devices. Its high conductivity makes it an irreplaceable resource in technology development.\n\n" +
                      "Assessment: Essential for electronics, wiring, and communication systems.",
                image: copperTexture,
                popupImage: copperPopup,
                unlockSound: PDAHandler.UnlockBasic
            );

            PDAHandler.AddCustomScannerEntry(
                key: TechType.Copper,
                scanTime: 2f,
                destroyAfterScan: false,
                encyclopediaKey: "CopperOre"
            );
        }
    }
}