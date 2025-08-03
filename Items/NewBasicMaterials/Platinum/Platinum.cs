using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Assets.PrefabTemplates;
using Nautilus.Crafting;
using Nautilus.Utility;
using UnityEngine;
using UWE;

namespace ArchiTech.Items.BasicMaterials
{
    public static class PlatinumItem
    {
        public static PrefabInfo Info { get; } = PrefabInfo.WithTechType("Platinum", "Platinum", "A dense, silvery-white metallic ore, highly resistant to thermal and chemical corrosion. Essential for advanced fabrication.")
            .WithIcon(ImageUtils.LoadSpriteFromFile(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Assets", "Minerals", "Platinum.png")));

        public static void Register()
        {
            var prefab = new CustomPrefab(Info);

            // Use the Nickel Ore prefab as the base model
            var cloneTemplate = new CloneTemplate(Info, TechType.Nickel)
            {
                ModifyPrefab = ModifyPlatinumPrefab
            };

            prefab.SetGameObject(cloneTemplate);

            // Set the pickup sound to be like lithium
            CraftData.pickupSoundList.Add(Info.TechType, "event:/loot/pickup_lithium");

            // Set up spawning in the specified biomes
            prefab.SetSpawns(new WorldEntityInfo
            {
                cellLevel = LargeWorldEntity.CellLevel.Near,
                classId = Info.ClassID,
                localScale = Vector3.one,
                slotType = EntitySlot.Type.Small,
                techType = Info.TechType
            }, BiomesToSpawnIn);

            // Register the prefab
            prefab.Register();

            Plugin.Logger.LogInfo("Platinum item registered successfully.");
        }

        private static LootDistributionData.BiomeData[] BiomesToSpawnIn
        {
            get
            {
                return new LootDistributionData.BiomeData[]
                {
                    new LootDistributionData.BiomeData
                    {
                        biome = BiomeType.KooshZone_Geyser,
                        count = 1,
                        probability = 0.4f
                    },
                    new LootDistributionData.BiomeData
                    {
                        biome = BiomeType.JellyshroomCaves_Geyser,
                        count = 1,
                        probability = 0.4f
                    },
                    new LootDistributionData.BiomeData
                    {
                        biome = BiomeType.UnderwaterIslands_Geyser,
                        count = 1,
                        probability = 1.2f
                    },
                    new LootDistributionData.BiomeData
                    {
                        biome = BiomeType.Mountains_Rock,
                        count = 1,
                        probability = 0.08f
                    },
                    new LootDistributionData.BiomeData
                    {
                        biome = BiomeType.TreeCove_Ground,
                        count = 1,
                        probability = 0.6f
                    },
                    new LootDistributionData.BiomeData
                    {
                        biome = BiomeType.TreeCove_Wall,
                        count = 1,
                        probability = 0.6f
                    },
                    new LootDistributionData.BiomeData
                    {
                        biome = BiomeType.InactiveLavaZone_Corridor_Ceiling,
                        count = 1,
                        probability = 1.8f
                    },
                    new LootDistributionData.BiomeData
                    {
                        biome = BiomeType.InactiveLavaZone_Corridor_Wall,
                        count = 1,
                        probability = 1.8f
                    }
                };
            }
        }

        private static void ModifyPlatinumPrefab(GameObject prefab)
        {
            // Add a resource tracker to make it show up on the scanner room HUD
            PrefabUtils.AddResourceTracker(prefab, Info.TechType);

            // Modify the appearance to look like platinum with enhanced glow/shine
            var renderer = prefab.GetComponentInChildren<Renderer>();
            if (renderer != null)
            {
                renderer.material.SetTexture("_MainTex", ImageUtils.LoadTextureFromFile(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Assets", "Platinum.png")));

                // Enhanced shine and glow properties
                renderer.material.SetFloat("_SpecInt", 8f);        // Higher specular intensity
                renderer.material.SetFloat("_Shininess", 12f);     // Higher shininess
                renderer.material.SetFloat("_Fresnel", 0.4f);      // Fresnel effect for more metallic look
                renderer.material.SetColor("_SpecColor", new Color(1.2f, 1.2f, 1.4f)); // Brighter silvery-white specular

                // Add a subtle glow effect
                renderer.material.SetColor("_GlowColor", new Color(0.8f, 0.8f, 1f, 0.3f)); // Subtle blue-white glow
                renderer.material.SetFloat("_GlowStrength", 0.2f); // Mild glow strength
                renderer.material.SetFloat("_GlowStrengthNight", 0.4f); // Stronger glow at night
            }

            // Ensure it has the correct TechTag
            var techTag = prefab.GetComponent<TechTag>() ?? prefab.AddComponent<TechTag>();
            techTag.type = Info.TechType;

            Plugin.Logger.LogInfo("Platinum prefab modified with enhanced shine and glow.");
        }
    }
}