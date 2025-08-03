using Nautilus.Handlers;
using UnityEngine;
using System.IO;
using System.Reflection;

namespace ArchitechAdddedPDAReso
{
    public static class MagnetitePDA
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
            Texture2D magnetiteTexture = LoadTextureFromFile("MagnetEntry.png");
            Sprite magnetitePopup = CreateSpriteFromTexture(magnetiteTexture);

            PDAHandler.AddEncyclopediaEntry(
                key: "Magnetite",
                path: "PlanetaryGeology",
                title: "Magnetite",
                desc: "A naturally magnetic iron oxide mineral, essential for electromagnetic applications and navigation systems.\n\n" +
                      "1. Natural Magnetism:\n" +
                      "Magnetite exhibits strong ferromagnetic properties, naturally aligning with 4546B's magnetic field. This characteristic makes it invaluable for compass construction and magnetic field detection systems.\n\n" +
                      "2. Electromagnetic Shielding:\n" +
                      "The mineral's magnetic properties provide excellent electromagnetic interference (EMI) shielding, protecting sensitive electronic equipment from external magnetic disturbances.\n\n" +
                      "3. Biomagnetic Navigation:\n" +
                      "Many of 4546B's marine species appear to use magnetite deposits for navigation, suggesting an intricate relationship between geological formations and biological behavior patterns.\n\n" +
                      "4. Industrial Applications:\n" +
                      "Used in manufacturing magnetic storage devices, electromagnetic generators, and induction heating systems essential for advanced manufacturing processes.\n\n" +
                      "5. Geological Distribution:\n" +
                      "Magnetite concentrations indicate historical volcanic activity, often found in association with other iron-rich minerals in underwater mountain ranges.\n\n" +
                      "Assessment: Fundamental for navigation systems, electromagnetic equipment, and magnetic field manipulation technology.",
                image: magnetiteTexture,
                popupImage: magnetitePopup,
                unlockSound: PDAHandler.UnlockBasic
            );

            PDAHandler.AddCustomScannerEntry(
                key: TechType.Magnetite,
                scanTime: 3f,
                destroyAfterScan: false,
                encyclopediaKey: "Magnetite"
            );
        }
    }
}