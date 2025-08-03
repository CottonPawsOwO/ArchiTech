using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Assets.PrefabTemplates;
using Nautilus.Crafting;
using Nautilus.Utility;
using UnityEngine;
using UWE;

namespace ArchiTech.Items.BasicMaterials
{
    public static class GraphiteItem
    {
        public static PrefabInfo Info { get; } = PrefabInfo.WithTechType("Graphite", "Graphite", "A crystalline form of carbon with exceptional electrical conductivity and thermal resistance. Essential for advanced electronics, battery components, and high-temperature applications.")
            .WithIcon(ImageUtils.LoadSpriteFromFile(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Assets", "Minerals", "Graphite.png")));

        public static void Register()
        {
            var prefab = new CustomPrefab(Info);

            // Use Lithium as the base model (more reliable prefab)
            var cloneTemplate = new CloneTemplate(Info, TechType.Lithium)
            {
                ModifyPrefab = ModifyGraphitePrefab
            };

            prefab.SetGameObject(cloneTemplate);

            // Carbon/crystal pickup sound
            CraftData.pickupSoundList.Add(Info.TechType, "event:/loot/pickup_glass");

            // NO NATURAL SPAWNING - only obtained from drilling GraphiteDrillable deposits
            // Biome spawning will be configured by user

            prefab.Register();
            Plugin.Logger.LogInfo("Graphite item registered successfully (drillable-only).");
        }

        private static void ModifyGraphitePrefab(GameObject prefab)
        {
            PrefabUtils.AddResourceTracker(prefab, Info.TechType);

            var renderer = prefab.GetComponentInChildren<Renderer>();
            if (renderer != null)
            {
                // Use the Graphite.png texture
                var graphiteTexture = ImageUtils.LoadTextureFromFile(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Assets", "Minerals", "Graphite.png"));

                if (graphiteTexture != null)
                {
                    renderer.material.SetTexture("_MainTex", graphiteTexture);
                }

                // Mostly black graphite appearance
                renderer.material.color = new Color(0.08f, 0.08f, 0.1f, 1f); // Almost black with tiny blue tint
                renderer.material.SetColor("_Color", new Color(0.08f, 0.08f, 0.1f, 1f));

                // Crystalline carbon properties - some shine but not metallic
                renderer.material.SetFloat("_SpecInt", 8f);         // Moderate specularity for crystal surfaces
                renderer.material.SetFloat("_Shininess", 12f);      // Sharp crystal reflections
                renderer.material.SetFloat("_Fresnel", 0.25f);      // Crystal-like fresnel
                renderer.material.SetColor("_SpecColor", new Color(0.5f, 0.5f, 0.6f)); // Cool gray specular

                // Subtle electrical conductivity glow
                renderer.material.SetColor("_GlowColor", new Color(0.15f, 0.2f, 0.3f, 0.2f)); // Cool blue-gray glow
                renderer.material.SetFloat("_GlowStrength", 0.08f);     // Subtle glow for conductivity
                renderer.material.SetFloat("_GlowStrengthNight", 0.15f); // More visible at night

                // Carbon crystal structure rim lighting
                renderer.material.SetFloat("_RimPower", 1.5f);
                renderer.material.SetColor("_RimColor", new Color(0.25f, 0.3f, 0.4f, 0.4f)); // Steel blue rim
            }

            var techTag = prefab.GetComponent<TechTag>() ?? prefab.AddComponent<TechTag>();
            techTag.type = Info.TechType;

            Plugin.Logger.LogInfo("Graphite prefab modified with dark crystalline carbon appearance.");
        }
    }
}
