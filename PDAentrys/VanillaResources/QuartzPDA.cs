using Nautilus.Handlers;
using UnityEngine;
using System.IO;
using System.Reflection;

namespace ArchitechAdddedPDAReso
{
    public static class QuartzPDA
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
            Texture2D quartzTexture = LoadTextureFromFile("QuartzEntry.png");
            Sprite quartzPopup = CreateSpriteFromTexture(quartzTexture);

            PDAHandler.AddEncyclopediaEntry(
                key: "Quartz",
                path: "PlanetaryGeology",
                title: "Quartz",
                desc: "A common crystalline mineral, integral to both structural and high-tech applications due to its reflective properties.\n\n" +
                      "1. Formation and Structure:\n" +
                      "Quartz forms from silicate mineralization, observed primarily in sandy reefs and caves, where silica-laden waters precipitate over time into a hexagonal crystal lattice.\n\n" +
                      "2. Mechanical Stability:\n" +
                      "Its hardness and resistance to weathering are ideal for long-lasting architectural and electronic components, including electronics housings and window ports.\n\n" +
                      "3. Optical Properties:\n" +
                      "Refractive qualities make quartz a natural choice for lenses and optics calibration. Its interaction with light allows it to be used in advanced optical computing technologies.\n\n" +
                      "4. Thermal Resilience:\n" +
                      "Resistant to thermal shock, quartz is suited for temperature control systems in power generation equipment, reducing risk of component failure.\n\n" +
                      "Assessment: Essential for crafting transparent structures and electronic interfaces, as well as for thermal management.",
                image: quartzTexture,
                popupImage: quartzPopup,
                unlockSound: PDAHandler.UnlockBasic
            );

            PDAHandler.AddCustomScannerEntry(
                key: TechType.Quartz,
                scanTime: 3f,
                destroyAfterScan: false,
                encyclopediaKey: "Quartz"
            );
        }
    }
}