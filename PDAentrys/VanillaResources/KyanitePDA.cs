using Nautilus.Handlers;
using UnityEngine;
using System.IO;
using System.Reflection;

namespace ArchitechAdddedPDAReso
{
    public static class KyanitePDA
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
            Texture2D kyaniteTexture = LoadTextureFromFile("KyanEntry.png");
            Sprite kyanitePopup = CreateSpriteFromTexture(kyaniteTexture);

            PDAHandler.AddEncyclopediaEntry(
                key: "Kyanite",
                path: "PlanetaryGeology",
                title: "Kyanite",
                desc: "A blue crystalline mineral found in extreme depth environments, prized for its unique thermal and electrical properties.\n\n" +
                      "1. High-Pressure Formation:\n" +
                      "Kyanite crystallizes under extreme pressure conditions found only in the deepest ocean trenches, where tectonic forces compress aluminum silicate into its distinctive blade-like crystal structure.\n\n" +
                      "2. Thermal Expansion Properties:\n" +
                      "Exhibits near-zero thermal expansion, making it invaluable for precision instruments that must maintain accuracy across extreme temperature variations encountered in deep-sea exploration.\n\n" +
                      "3. Electrical Insulation:\n" +
                      "Kyanite's exceptional dielectric properties provide superior electrical insulation, essential for high-voltage systems operating in conductive seawater environments.\n\n" +
                      "4. Metamorphic Indicator:\n" +
                      "The presence of kyanite indicates extreme metamorphic conditions, suggesting proximity to active geological processes and potential thermal energy sources.\n\n" +
                      "5. Crystallographic Anisotropy:\n" +
                      "Hardness varies dramatically along different crystal axes, from 4.5 to 7 on the Mohs scale, requiring specialized extraction and processing techniques.\n\n" +
                      "Assessment: Critical for deep-sea technology, thermal management systems, and high-precision instrumentation.",
                image: kyaniteTexture,
                popupImage: kyanitePopup,
                unlockSound: PDAHandler.UnlockBasic
            );

            PDAHandler.AddCustomScannerEntry(
                key: TechType.Kyanite,
                scanTime: 4f,
                destroyAfterScan: false,
                encyclopediaKey: "Kyanite"
            );
        }
    }
}