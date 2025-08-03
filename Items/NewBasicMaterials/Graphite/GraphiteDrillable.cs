using ArchiTech;
using ArchiTech.Items.BasicMaterials;
using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Assets.PrefabTemplates;
using Nautilus.Crafting;
using UnityEngine;
using UWE;

namespace ArchiTech.Items.BasicMaterials
{
    public static class GraphiteDrillableItem
    {
        public static PrefabInfo Info { get; } = PrefabInfo.WithTechType("GraphiteDrillable", "Graphite Deposit", "A large crystalline carbon deposit with visible graphite veins. This high-grade carbon formation can be extracted using a drill for advanced electronics and industrial applications.")
            .WithIcon(SpriteManager.Get(TechType.Sulphur)); // Use sulfur icon as placeholder

        public static void Register()
        {
            var prefab = new CustomPrefab(Info);

            // Use sulfur drillable as base since graphite is crystalline like sulfur
            var cloneTemplate = new CloneTemplate(Info, TechType.DrillableSulphur)
            {
                ModifyPrefab = ModifyGraphiteDrillablePrefab
            };

            prefab.SetGameObject(cloneTemplate);

            // Set up the gadget for drillable behavior
            // Set correct group and category for drillable
            prefab.SetPdaGroupCategory(TechGroup.Resources, TechCategory.RawMaterials);

            // Configure as drillable resource (not craftable)
            prefab.SetRecipe(new RecipeData())
                .WithFabricatorType(CraftTree.Type.None);

            // Set up spawning - biomes will be configured by user
            prefab.SetSpawns(new WorldEntityInfo
            {
                cellLevel = LargeWorldEntity.CellLevel.Medium,
                classId = Info.ClassID,
                localScale = Vector3.one * 1.4f, // Larger deposits for carbon formations
                slotType = EntitySlot.Type.Medium,
                techType = Info.TechType
            }, BiomesToSpawnIn);

            // Register the prefab
            prefab.Register();

            Plugin.Logger.LogInfo("Graphite Drillable deposit registered successfully");
        }

        private static LootDistributionData.BiomeData[] BiomesToSpawnIn
        {
            get
            {
                return new LootDistributionData.BiomeData[]
                {
                    // TEST BIOMES - Higher probability for testing
                    new LootDistributionData.BiomeData
                    {
                        biome = BiomeType.SafeShallows_Rock,
                        count = 1,
                        probability = 0.8f // High probability for testing
                    },
                    new LootDistributionData.BiomeData
                    {
                        biome = BiomeType.KelpForest_Rock,
                        count = 1,
                        probability = 0.8f // High probability for testing
                    },
                    new LootDistributionData.BiomeData
                    {
                        biome = BiomeType.Mountains_Rock,
                        count = 1,
                        probability = 0.6f // Medium probability
                    }
                    // User can modify these biomes as needed
                };
            }
        }

        private static void ModifyGraphiteDrillablePrefab(GameObject prefab)
        {
            // Get the drillable component and configure it
            var drillable = prefab.GetComponent<Drillable>();
            if (drillable != null)
            {
                // Configure tooltips
                drillable.primaryTooltip = "Graphite Deposit";
                drillable.secondaryTooltip = "High-grade crystalline carbon formation";

                // Set the resources that can be obtained from drilling
                drillable.resources = new Drillable.ResourceType[]
                {
                    new Drillable.ResourceType()
                    {
                        techType = GraphiteItem.Info.TechType,
                        chance = 1.0f // Always drops Graphite
                    }
                };

                // Configure health and drilling behavior
                drillable.health = new float[] { 25f }; // Moderate health - carbon is hard but brittle
                drillable.minResourcesToSpawn = 2; // Good yield
                drillable.maxResourcesToSpawn = 4;

                // Configure the breakFX - use default crystal breaking effects
                drillable.breakFX = null; // Use default break effects
            }

            // Modify visual appearance for graphite crystal deposits
            var renderer = prefab.GetComponentInChildren<Renderer>();
            if (renderer != null)
            {
                // Apply graphite crystal appearance - darker than regular sulfur
                renderer.material.color = new Color(0.15f, 0.15f, 0.2f, 1f); // Very dark charcoal
                renderer.material.SetColor("_Color", new Color(0.15f, 0.15f, 0.2f, 1f));

                // Crystal-like properties with carbon characteristics
                renderer.material.SetFloat("_SpecInt", 10f);        // Good crystal specularity
                renderer.material.SetFloat("_Shininess", 14f);      // Sharp crystal reflections  
                renderer.material.SetFloat("_Fresnel", 0.3f);       // Crystal fresnel
                renderer.material.SetColor("_SpecColor", new Color(0.4f, 0.4f, 0.5f)); // Cool gray specular

                // Subtle conductivity glow - hint at electrical properties
                renderer.material.SetColor("_GlowColor", new Color(0.1f, 0.15f, 0.25f, 0.3f)); // Cool blue glow
                renderer.material.SetFloat("_GlowStrength", 0.1f);      // Subtle glow
                renderer.material.SetFloat("_GlowStrengthNight", 0.2f); // More visible at night

                // Carbon crystal rim lighting
                renderer.material.SetFloat("_RimPower", 1.8f);
                renderer.material.SetColor("_RimColor", new Color(0.2f, 0.25f, 0.35f, 0.5f)); // Steel blue rim
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

            Plugin.Logger.LogInfo("Graphite Drillable deposit prefab modified successfully with dark crystalline carbon effects");
        }
    }
}
