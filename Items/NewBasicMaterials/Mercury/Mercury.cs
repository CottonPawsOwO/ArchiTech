using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Assets.PrefabTemplates;
using Nautilus.Crafting;
using UnityEngine;
using UWE;

namespace ArchiTech.Items.BasicMaterials
{
    public static class MercuryItem
    {
        public static PrefabInfo Info { get; } = PrefabInfo.WithTechType("Mercury", "Mercury", "A bright red mineral ore containing mercury. Cinnabar is the primary source of mercury and essential for advanced technological applications")
            .WithIcon(SpriteManager.Get(TechType.MercuryOre));

        public static void Register()
        {
            var prefab = new CustomPrefab(Info);

            // Use the existing mercury ore prefab from the game
            var cloneTemplate = new CloneTemplate(Info, TechType.MercuryOre)
            {
                ModifyPrefab = ModifyMercuryPrefab
            };

            prefab.SetGameObject(cloneTemplate);

            // Set the pickup sound to be like lithium
            CraftData.pickupSoundList.Add(Info.TechType, "event:/loot/pickup_lithium");

            // Set up spawning using biome-based method
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

            Plugin.Logger.LogInfo("Mercury item registered successfully");
        }

        private static LootDistributionData.BiomeData[] BiomesToSpawnIn
        {
            get
            {
                return new LootDistributionData.BiomeData[]
                {
                    new LootDistributionData.BiomeData
                    {
                        biome = BiomeType.GrandReef_Ground,
                        count = 1,
                        probability = 0.65f
                    },
                    new LootDistributionData.BiomeData
                    {
                        biome = BiomeType.GrandReef_Wall,
                        count = 1,
                        probability = 0.075f
                    },
                    new LootDistributionData.BiomeData
                    {
                        biome = BiomeType.GrandReef_WhiteCoral,
                        count = 1,
                        probability = 0.3f
                    },
                    new LootDistributionData.BiomeData
                    {
                        biome = BiomeType.DeepGrandReef_Ceiling,
                        count = 1,
                        probability = 0.25f
                    },
                    new LootDistributionData.BiomeData
                    {
                        biome = BiomeType.DeepGrandReef_Ground,
                        count = 1,
                        probability = 0.6f
                    },
                    new LootDistributionData.BiomeData
                    {
                        biome = BiomeType.MushroomForest_Sand,
                        count = 1,
                        probability = 0.4f
                    },
                    new LootDistributionData.BiomeData
                    {
                        biome = BiomeType.MushroomForest_RockWall,
                        count = 1,
                        probability = 0.3f
                    },
                    new LootDistributionData.BiomeData
                    {
                        biome = BiomeType.UnderwaterIslands_IslandSides,
                        count = 1,
                        probability = 0.5f
                    },
                    new LootDistributionData.BiomeData
                    {
                        biome = BiomeType.UnderwaterIslands_IslandTop,
                        count = 1,
                        probability = 0.5f
                    },
                    new LootDistributionData.BiomeData
                    {
                        biome = BiomeType.KooshZone_Sand,
                        count = 1,
                        probability = 0.13f
                    },
                    new LootDistributionData.BiomeData
                    {
                        biome = BiomeType.KooshZone_RockWall,
                        count = 1,
                        probability = 0.3f
                    },
                    new LootDistributionData.BiomeData
                    {
                        biome = BiomeType.Dunes_SandDune,
                        count = 1,
                        probability = 0.2f
                    },
                    new LootDistributionData.BiomeData
                    {
                        biome = BiomeType.Dunes_SandPlateau,
                        count = 1,
                        probability = 0.2f
                    }
                };
            }
        }

        private static void ModifyMercuryPrefab(GameObject prefab)
        {
            // Get the pickupable component and modify it if needed
            var pickupable = prefab.GetComponent<Pickupable>();
            if (pickupable != null)
            {
                // Ensure it's properly configured as a resource
                pickupable.isPickupable = true;
            }

            // Get the world forces component for physics
            var worldForces = prefab.GetComponent<WorldForces>();
            if (worldForces != null)
            {
                worldForces.underwaterGravity = 9.81f;
                worldForces.underwaterDrag = 10f;
            }

            // Ensure the prefab has the correct tech type
            var techTag = prefab.GetComponent<TechTag>();
            if (techTag != null)
            {
                techTag.type = Info.TechType;
            }

            Plugin.Logger.LogInfo("Mercury prefab modified successfully");
        }
    }
}