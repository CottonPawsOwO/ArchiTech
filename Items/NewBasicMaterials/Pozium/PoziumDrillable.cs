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
    public static class PoziumDrillableItem
    {
        public static PrefabInfo Info { get; } = PrefabInfo.WithTechType("PoziumDrillable", "Pozium Deposit", "A large crystalline deposit of Pozium ore. This dangerous plasma-containing mineral can be extracted using a drill, but extreme caution is advised due to electrical hazards.")
            .WithIcon(SpriteManager.Get(TechType.Magnetite)); // Use magnetite icon as placeholder

        public static void Register()
        {
            var prefab = new CustomPrefab(Info);

            // Use magnetite drillable as base since Pozium is crystalline
            var cloneTemplate = new CloneTemplate(Info, TechType.DrillableMagnetite)
            {
                ModifyPrefab = ModifyPoziumDrillablePrefab
            };

            prefab.SetGameObject(cloneTemplate);

            // Configure as drillable resource (not craftable)
            prefab.SetRecipe(new RecipeData())
                .WithFabricatorType(CraftTree.Type.None);

            // Set up spawning in kelp-rich biomes with large deposits
            prefab.SetSpawns(new WorldEntityInfo
            {
                cellLevel = LargeWorldEntity.CellLevel.Medium,
                classId = Info.ClassID,
                localScale = Vector3.one * 1.3f, // Larger deposits
                slotType = EntitySlot.Type.Medium,
                techType = Info.TechType
            }, BiomesToSpawnIn);

            // Register the prefab
            prefab.Register();

            Plugin.Logger.LogInfo("Pozium Drillable deposit registered successfully");
        }

        private static LootDistributionData.BiomeData[] BiomesToSpawnIn
        {
            get
            {
                return new LootDistributionData.BiomeData[]
                {
                    // Primary kelp forest spawns - moderate depth
                    new LootDistributionData.BiomeData
                    {
                        biome = BiomeType.BloodKelp_TrenchFloor,
                        count = 1,
                        probability = 0.8f
                    },
                    
                    // Deep Grand Reef - kelp-adjacent areas
                    new LootDistributionData.BiomeData
                    {
                        biome = BiomeType.BloodKelp_TrenchFloor,
                        count = 1,
                        probability = 0.8f
                    },
                    new LootDistributionData.BiomeData
                    {
                        biome = BiomeType.BloodKelp_CaveFloor,
                        count = 1,
                        probability = 0.8f
                    },
                    
                    // Sparse Ocean areas near kelp
                    new LootDistributionData.BiomeData
                    {
                        biome = BiomeType.BloodKelp_Floor,
                        count = 1,
                        probability = 0.8f
                    }
                };
            }
        }

        private static void ModifyPoziumDrillablePrefab(GameObject prefab)
        {
            // Get the drillable component and configure it
            var drillable = prefab.GetComponent<Drillable>();
            if (drillable != null)
            {
                // Configure tooltips with danger warnings
                drillable.primaryTooltip = "Pozium Deposit";
                drillable.secondaryTooltip = "Dangerous plasma-charged crystals - Use protective equipment";

                // Set the resources that can be obtained from drilling
                drillable.resources = new Drillable.ResourceType[]
                {
                    new Drillable.ResourceType()
                    {
                        techType = PoziumItem.Info.TechType,
                        chance = 1.0f // Always drops Pozium
                    }
                };

                // Configure health and drilling behavior - harder to drill due to electrical resistance
                drillable.health = new float[] { 40f }; // More health than standard deposits
                drillable.minResourcesToSpawn = 2; // Generous yield due to danger
                drillable.maxResourcesToSpawn = 4;

                // Configure the breakFX for electrical effects
                drillable.breakFX = null; // Use default break effects (will be enhanced later if needed)
            }

            // Modify visual appearance for electrical effects
            var renderer = prefab.GetComponentInChildren<Renderer>();
            if (renderer != null)
            {
                // Apply Pozium crystal appearance
                renderer.material.color = new Color(0.95f, 0.85f, 0.90f, 1f); // Whitish-pink
                renderer.material.SetColor("_Color", new Color(0.95f, 0.85f, 0.90f, 1f));

                // Crystal-like properties with electrical shimmer
                renderer.material.SetFloat("_SpecInt", 12f);
                renderer.material.SetFloat("_Shininess", 15f);
                renderer.material.SetFloat("_Fresnel", 0.4f);
                renderer.material.SetColor("_SpecColor", new Color(0.7f, 0.9f, 1.0f)); // Sky blue highlights

                // Electrical glow
                renderer.material.SetColor("_GlowColor", new Color(0.4f, 0.7f, 1.0f, 0.5f));
                renderer.material.SetFloat("_GlowStrength", 0.6f);
                renderer.material.SetFloat("_GlowStrengthNight", 0.9f);

                // Electrical rim lighting
                renderer.material.SetFloat("_RimPower", 1.8f);
                renderer.material.SetColor("_RimColor", new Color(0.3f, 0.6f, 1.0f, 0.7f));
            }

            // Electrical effects handled through material properties only
            // Custom spark components can interfere with drillable functionality

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

            Plugin.Logger.LogInfo("Pozium Drillable deposit prefab modified successfully with electrical effects");
        }
    }
}