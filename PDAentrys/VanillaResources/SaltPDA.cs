using Nautilus.Handlers;
using UnityEngine;
using System.IO;
using System.Reflection;

namespace ArchitechAdddedPDAReso
{
    public static class SaltDepositPDA
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
            Texture2D saltTexture = LoadTextureFromFile("SaltEntry.png");
            Sprite saltPopup = CreateSpriteFromTexture(saltTexture);

            PDAHandler.AddEncyclopediaEntry(
                key: "SaltDeposit",
                path: "PlanetaryGeology",
                title: "Salt Deposit",
                desc: "A mineral formed through the evaporation of ancient seawater, utilized in food preservation and chemical processing.\n\n" +
                      "1. Chemical Stability:\n" +
                      "Salt's crystalline structure ensures its stability, solubility, and functional use in chemical reactions, including chlorine production and metal purification cycles.\n\n" +
                      "2. Geological Formation:\n" +
                      "Sediment layers reveal salt deposits were formed during planetary desiccation periods, evident in the coloration gradient from surface to base segments of each deposit.\n\n" +
                      "3. Biological Interactions:\n" +
                      "Acts as a natural preservative, inhibiting bacterial colonization; its presence maintains marine biodiversity by fostering numerous specialized flora and fauna.\n\n" +
                      "4. Practical Applications:\n" +
                      "Used in food production through dehydration storage solutions and antiseptic treatments, effectively preventing spoilage in various consumables.\n\n" +
                      "Assessment: Essential for crafting antiseptic compounds and desalination technologies.",
                image: saltTexture,
                popupImage: saltPopup,
                unlockSound: PDAHandler.UnlockBasic
            );

            PDAHandler.AddCustomScannerEntry(
                key: TechType.Salt,
                scanTime: 3f,
                destroyAfterScan: false,
                encyclopediaKey: "SaltDeposit"
            );
        }
    }
}