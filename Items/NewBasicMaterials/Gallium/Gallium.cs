using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Assets.PrefabTemplates;
using Nautilus.Crafting;
using Nautilus.Utility;
using UnityEngine;
using UWE;

namespace ArchiTech.Items.BasicMaterials
{
    public static class GalliumItem
    {
        public static PrefabInfo Info { get; } = PrefabInfo.WithTechType("Gallium", "Gallium", "A silvery-metallic element used heavily in electronics and advanced alloys. Naturally occurs in trace concentrations within aluminum and zinc ores.")
            .WithIcon(ImageUtils.LoadSpriteFromFile(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Assets", "Minerals", "Gallium.png")));

        public static void Register()
        {
            var prefab = new CustomPrefab(Info);

            // Use Copper as the base model
            var cloneTemplate = new CloneTemplate(Info, TechType.Copper)
            {
                ModifyPrefab = ModifyGalliumPrefab
            };

            prefab.SetGameObject(cloneTemplate);

            // Electronics-like pickup sound
            CraftData.pickupSoundList.Add(Info.TechType, "event:/loot/pickup_copper");

            // Spawn in electronic-friendly biomes
            prefab.SetSpawns(new WorldEntityInfo
            {
                cellLevel = LargeWorldEntity.CellLevel.Near,
                classId = Info.ClassID,
                localScale = Vector3.one,
                slotType = EntitySlot.Type.Small,
                techType = Info.TechType
            }, BiomesToSpawnIn);

            prefab.Register();
            Plugin.Logger.LogInfo("Gallium item registered successfully.");
        }

        private static LootDistributionData.BiomeData[] BiomesToSpawnIn
        {
            get
            {
                return new LootDistributionData.BiomeData[]
                {
                    // Common in shallow areas (electronics manufacturing)
                    new LootDistributionData.BiomeData
                    {
                        biome = BiomeType.SafeShallows_CaveSpecial,
                        count = 1,
                        probability = 0.8f
                    },
                    new LootDistributionData.BiomeData
                    {
                        biome = BiomeType.SafeShallows_CaveWall,
                        count = 1,
                        probability = 0.8f
                    },
                    new LootDistributionData.BiomeData
                    {
                        biome = BiomeType.SafeShallows_CaveFloor,
                        count = 1,
                        probability = 0.8f
                    },
                    new LootDistributionData.BiomeData
                    {
                        biome = BiomeType.SafeShallows_ShellTunnelHuge,
                        count = 1,
                        probability = 1.6f
                    }
                };
            }
        }

        private static void ModifyGalliumPrefab(GameObject prefab)
        {
            PrefabUtils.AddResourceTracker(prefab, Info.TechType);

            var renderer = prefab.GetComponentInChildren<Renderer>();
            if (renderer != null)
            {
                // Use your Aluminum.png texture
                var galliumTexture = ImageUtils.LoadTextureFromFile(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Assets", "Minerals", "Gallium.png"));

                if (galliumTexture != null)
                {
                    renderer.material.SetTexture("_MainTex", galliumTexture);
                }

                // Silvery-metallic appearance, slightly darker than pure silver
                renderer.material.color = new Color(0.75f, 0.75f, 0.8f, 1f); // Slightly bluish silver
                renderer.material.SetColor("_Color", new Color(0.75f, 0.75f, 0.8f, 1f));

                // Shiny metallic properties
                renderer.material.SetFloat("_SpecInt", 12f);        // High shine
                renderer.material.SetFloat("_Shininess", 16f);     // Sharp reflections
                renderer.material.SetFloat("_Fresnel", 0.4f);      // Metallic fresnel
                renderer.material.SetColor("_SpecColor", new Color(0.9f, 0.9f, 0.95f)); // Bright silver specular

                // Subtle metallic glow
                renderer.material.SetColor("_GlowColor", new Color(0.4f, 0.4f, 0.45f, 0.25f));
                renderer.material.SetFloat("_GlowStrength", 0.12f);
                renderer.material.SetFloat("_GlowStrengthNight", 0.25f);
            }

            var techTag = prefab.GetComponent<TechTag>() ?? prefab.AddComponent<TechTag>();
            techTag.type = Info.TechType;

            Plugin.Logger.LogInfo("Gallium prefab modified with aluminum-based metallic appearance.");
        }
    }
}