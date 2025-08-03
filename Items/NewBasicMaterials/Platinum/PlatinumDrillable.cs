using ArchiTech;
using ArchiTech.Items.BasicMaterials;
using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Assets.PrefabTemplates;
using Nautilus.Crafting;
using Nautilus.Utility;
using UnityEngine;
using UWE;

namespace ArchiTech.Items.BasicMaterials
{
    public static class PlatinumDrillableItem
    {
        public static PrefabInfo Info { get; } = PrefabInfo.WithTechType("PlatinumDrillable", "Platinum Deposit", "A large deposit of platinum ore. This dense, silvery-white metal is highly resistant to corrosion and can be extracted using a drill.")
            .WithIcon(ImageUtils.LoadSpriteFromFile(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Assets", "Minerals", "Platinum.png")));

        public static void Register()
        {
            var prefab = new CustomPrefab(Info);

            // Use the existing nickel drillable prefab as the base (since platinum is similar to nickel)
            var cloneTemplate = new CloneTemplate(Info, TechType.DrillableNickel)
            {
                ModifyPrefab = ModifyPlatinumDrillablePrefab
            };

            prefab.SetGameObject(cloneTemplate);

            // Configure as drillable resource
            prefab.SetRecipe(new RecipeData())
                .WithFabricatorType(CraftTree.Type.None); // Not craftable, only drillable

            // Set up spawning using biome-based method for deeper areas
            prefab.SetSpawns(new WorldEntityInfo
            {
                cellLevel = LargeWorldEntity.CellLevel.Medium,
                classId = Info.ClassID,
                localScale = Vector3.one,
                slotType = EntitySlot.Type.Medium,
                techType = Info.TechType
            }, BiomesToSpawnIn);

            // Register the prefab
            prefab.Register();

            Plugin.Logger.LogInfo("Platinum Drillable item registered successfully");
        }

        private static LootDistributionData.BiomeData[] BiomesToSpawnIn
        {
            get
            {
                return new LootDistributionData.BiomeData[]
                {
                    // Lost River areas - rare spawns for end-game material
                    new LootDistributionData.BiomeData
                    {
                        biome = BiomeType.TreeCove_Ground,
                        count = 1,
                        probability = 0.15f
                    },
                    new LootDistributionData.BiomeData
                    {
                        biome = BiomeType.TreeCove_Wall,
                        count = 1,
                        probability = 0.12f
                    },
                    new LootDistributionData.BiomeData
                    {
                        biome = BiomeType.GhostTree_Ground,
                        count = 1,
                        probability = 0.1f
                    },
                    // Inactive Lava Zone - very rare but rich deposits
                    new LootDistributionData.BiomeData
                    {
                        biome = BiomeType.InactiveLavaZone_Corridor_Floor,
                        count = 1,
                        probability = 0.6f
                    },
                    new LootDistributionData.BiomeData
                    {
                        biome = BiomeType.InactiveLavaZone_Corridor_Wall,
                        count = 1,
                        probability = 0.8f
                    },
                    // Active Lava Zone - extremely rare but highest yield
                    new LootDistributionData.BiomeData
                    {
                        biome = BiomeType.ActiveLavaZone_Chamber_Wall,
                        count = 1,
                        probability = 1.2f
                    }
                };
            }
        }

        private static void ModifyPlatinumDrillablePrefab(GameObject prefab)
        {
            // Get the drillable component and configure it
            var drillable = prefab.GetComponent<Drillable>();
            if (drillable != null)
            {
                // Configure what resources this drillable gives
                drillable.primaryTooltip = "Platinum Deposit";
                drillable.secondaryTooltip = "Contains dense platinum ore";

                // Set the resources that can be obtained from drilling
                drillable.resources = new Drillable.ResourceType[]
                {
                    new Drillable.ResourceType()
                    {
                        techType = PlatinumItem.Info.TechType,
                        chance = 1.0f
                    }
                };

                // Configure health and drilling behavior - platinum is harder to drill
                drillable.health = new float[] { 30f }; // Harder to drill than mercury
                drillable.minResourcesToSpawn = 3; // More resources than mercury
                drillable.maxResourcesToSpawn = 6; // Higher max yield

                // Configure the breakFX and other properties
                drillable.breakFX = null; // Use default break effects
            }

            // Apply the same visual modifications as the platinum item
            var renderer = prefab.GetComponentInChildren<Renderer>();
            if (renderer != null)
            {
                renderer.material.SetTexture("_MainTex", ImageUtils.LoadTextureFromFile(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Assets", "Platinum.png")));

                // Enhanced shine and glow properties (same as PlatinumItem)
                renderer.material.SetFloat("_SpecInt", 8f);        // Higher specular intensity
                renderer.material.SetFloat("_Shininess", 12f);     // Higher shininess
                renderer.material.SetFloat("_Fresnel", 0.4f);      // Fresnel effect for more metallic look
                renderer.material.SetColor("_SpecColor", new Color(1.2f, 1.2f, 1.4f)); // Brighter silvery-white specular

                // Add a subtle glow effect
                renderer.material.SetColor("_GlowColor", new Color(0.8f, 0.8f, 1f, 0.3f)); // Subtle blue-white glow
                renderer.material.SetFloat("_GlowStrength", 0.2f); // Mild glow strength
                renderer.material.SetFloat("_GlowStrengthNight", 0.4f); // Stronger glow at night
            }

            // Ensure the prefab has the correct tech type
            var techTag = prefab.GetComponent<TechTag>();
            if (techTag != null)
            {
                techTag.type = Info.TechType;
            }

            // Configure the large world entity for proper spawning
            var lwe = prefab.GetComponent<LargeWorldEntity>();
            if (lwe != null)
            {
                lwe.cellLevel = LargeWorldEntity.CellLevel.Medium;
            }

            Plugin.Logger.LogInfo("Platinum Drillable prefab modified successfully with enhanced shine and glow");
        }
    }
}