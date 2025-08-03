using Nautilus.Handlers;
using UnityEngine;
using System.IO;
using System.Reflection;

namespace ArchitechAdddedPDAReso
{
    public static class CrystallineSulfurPDA
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
            Texture2D sulfurTexture = LoadTextureFromFile("SulfEntry.png");
            Sprite sulfurPopup = CreateSpriteFromTexture(sulfurTexture);

            PDAHandler.AddEncyclopediaEntry(
                key: "CrystallineSulfur",
                path: "PlanetaryGeology",
                title: "Crystalline Sulfur",
                desc: "A bright yellow crystalline formation, commonly found near volcanic activity and essential for chemical synthesis.\n\n" +
                      "1. Volcanic Origin:\n" +
                      "Crystalline sulfur deposits form through sublimation processes near hydrothermal vents, where sulfur vapor condenses into pure crystalline structures under specific temperature and pressure conditions.\n\n" +
                      "2. Chemical Reactivity:\n" +
                      "Highly reactive with metals and organic compounds, sulfur serves as a key component in battery acid production and various chemical catalysts essential for advanced manufacturing.\n\n" +
                      "3. Structural Properties:\n" +
                      "The orthorhombic crystal structure exhibits remarkable stability at standard temperatures but becomes highly volatile when heated, releasing toxic gases that require careful handling.\n\n" +
                      "4. Biological Implications:\n" +
                      "Certain extremophile bacteria thrive in sulfur-rich environments, suggesting potential applications in biofuel production and waste processing systems.\n\n" +
                      "Assessment: Critical for battery production, chemical synthesis, and advanced material processing. Handle with appropriate safety measures.",
                image: sulfurTexture,
                popupImage: sulfurPopup,
                unlockSound: PDAHandler.UnlockBasic
            );

            PDAHandler.AddCustomScannerEntry(
                key: TechType.Sulphur,
                scanTime: 3f,
                destroyAfterScan: false,
                encyclopediaKey: "CrystallineSulfur"
            );
        }
    }
}