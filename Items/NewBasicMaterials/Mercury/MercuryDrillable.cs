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
    public static class MercuryDrillableItem
    {
        public static PrefabInfo Info { get; } = PrefabInfo.WithTechType("MercuryDrillable", "Mercury Deposit", "A large deposit of cinnabar ore. This bright red mineral contains mercury and can be extracted using a drill.")
            .WithIcon(SpriteManager.Get(TechType.MercuryOre));

        public static void Register()
        {
            var prefab = new CustomPrefab(Info);

            // Use the existing mercury drillable prefab from the game
            var cloneTemplate = new CloneTemplate(Info, TechType.DrillableMercury)
            {
                ModifyPrefab = ModifyMercuryDrillablePrefab
            };

            prefab.SetGameObject(cloneTemplate);

            // Configure as drillable resource
            prefab.SetRecipe(new RecipeData())
                .WithFabricatorType(CraftTree.Type.None); // Not craftable, only drillable

            // Set up spawning using biome-based method for Lost River
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

            Plugin.Logger.LogInfo("Mercury Drillable item registered successfully");
        }

        private static LootDistributionData.BiomeData[] BiomesToSpawnIn
        {
            get
            {
                return new LootDistributionData.BiomeData[]
                                {
                    new LootDistributionData.BiomeData
                    {
                        biome = BiomeType.BloodKelp_CaveFloor,
                        count = 1,
                        probability = 0.2f
                    },
                    new LootDistributionData.BiomeData
                    {
                        biome = BiomeType.BloodKelp_TrenchFloor,
                        count = 1,
                        probability = 0.1f
                    },
                    new LootDistributionData.BiomeData
                    {
                        biome = BiomeType.BloodKelp_Floor,
                        count = 1,
                        probability = 0.05f
                    },
                    new LootDistributionData.BiomeData
                    {
                        biome = BiomeType.BonesField_Ceiling,
                        count = 1,
                        probability = 0.08f
                    },
                    new LootDistributionData.BiomeData
                    {
                        biome = BiomeType.BonesField_Ground,
                        count = 1,
                        probability = 0.045f
                    },
                    new LootDistributionData.BiomeData
                    {
                        biome = BiomeType.GhostTree_Grass,
                        count = 1,
                        probability = 0.1f
                    },
                    new LootDistributionData.BiomeData
                    {
                        biome = BiomeType.GhostTree_Ground,
                        count = 1,
                        probability = 0.06f
                    },
                    new LootDistributionData.BiomeData
                    {
                        biome = BiomeType.TreeCove_Ground,
                        count = 1,
                        probability = 0.09f
                    },
                    new LootDistributionData.BiomeData
                    {
                        biome = BiomeType.BonesField_Corridor_Ground,
                        count = 1,
                        probability = 0.075f
                    },
                    new LootDistributionData.BiomeData
                    {
                        biome = BiomeType.LostRiverCorridor_Ground,
                        count = 1,
                        probability = 0.09f
                    },
                    new LootDistributionData.BiomeData
                    {
                        biome = BiomeType.LostRiverJunction_Ground,
                        count = 1,
                        probability = 0.09f
                    }
                };
            }
        }

        private static void ModifyMercuryDrillablePrefab(GameObject prefab)
        {
            // Get the drillable component and configure it
            var drillable = prefab.GetComponent<Drillable>();
            if (drillable != null)
            {
                // Configure what resources this drillable gives
                drillable.primaryTooltip = "Mercury Deposit";
                drillable.secondaryTooltip = "Contains cinnabar ore";

                // Set the resources that can be obtained from drilling
                drillable.resources = new Drillable.ResourceType[]
                {
                    new Drillable.ResourceType()
                    {
                        techType = MercuryItem.Info.TechType,
                        chance = 1.0f
                    }
                };

                // Configure health and drilling behavior (health is an array)
                drillable.health = new float[] { 20f }; // Takes some time to drill
                drillable.minResourcesToSpawn = 2;
                drillable.maxResourcesToSpawn = 4;

                // Configure the breakFX and other properties
                drillable.breakFX = null; // Use default break effects
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

            Plugin.Logger.LogInfo("Mercury Drillable prefab modified successfully");
        }
    }
}