using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Assets.PrefabTemplates;
using Nautilus.Crafting;
using Nautilus.Utility;
using UnityEngine;
using UWE;

namespace ArchiTech.Items.BasicMaterials
{
    public static class IridiumItem
    {
        public static PrefabInfo Info { get; } = PrefabInfo.WithTechType("Iridium", "Iridium", "An extremely dense, black metallic ore with a mirror-like finish. One of the rarest and most corrosion-resistant metals in the universe.")
            .WithIcon(ImageUtils.LoadSpriteFromFile(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Assets", "Minerals", "Iridium.png")));

        public static void Register()
        {
            var prefab = new CustomPrefab(Info);

            // Use the Nickel Ore prefab as the base model
            var cloneTemplate = new CloneTemplate(Info, TechType.Nickel)
            {
                ModifyPrefab = ModifyIridiumPrefab
            };

            prefab.SetGameObject(cloneTemplate);

            // Set the pickup sound to be like ion cube (mysterious and rare)
            CraftData.pickupSoundList.Add(Info.TechType, "event:/loot/pickup_precursorioncrystal");

            // Set up spawning in the most extreme biomes (very rare)
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

            Plugin.Logger.LogInfo("Iridium item registered successfully.");
        }

        private static LootDistributionData.BiomeData[] BiomesToSpawnIn
        {
            get
            {
                return new LootDistributionData.BiomeData[]
                {
                    // Extremely rare - only in the most dangerous biomes
                    new LootDistributionData.BiomeData
                    {
                        biome = BiomeType.CrashZone_TrenchSand,
                        count = 1,
                        probability = 0.05f
                    },
                    new LootDistributionData.BiomeData
                    {
                        biome = BiomeType.CrashZone_TrenchRock,
                        count = 1,
                        probability = 0.05f
                    },
                    new LootDistributionData.BiomeData
                    {
                        biome = BiomeType.CrashZone_Rock,
                        count = 1,
                        probability = 0.05f
                    },
                    new LootDistributionData.BiomeData
                    {
                        biome = BiomeType.CrashZone_Sand,
                        count = 1,
                        probability = 0.05f
                    }
                };
            }
        }

        private static void ModifyIridiumPrefab(GameObject prefab)
        {
            // Add a resource tracker to make it show up on the scanner room HUD
            PrefabUtils.AddResourceTracker(prefab, Info.TechType);

            // Modify the appearance to look like black void metallic iridium
            var renderer = prefab.GetComponentInChildren<Renderer>();
            if (renderer != null)
            {
                renderer.material.SetTexture("_MainTex", ImageUtils.LoadTextureFromFile(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Assets", "Iridium.png")));

                renderer.material.SetFloat("_GlowStrength", 0.15f);     // Increase for more glow
                renderer.material.SetFloat("_GlowStrengthNight", 0.35f); // More visible at night
                renderer.material.color = new Color(0.1f, 0.1f, 0.1f, 1f); // Slightly lighter dark gray
                renderer.material.SetFloat("_SpecInt", 8f);        // Increase for more shine
                renderer.material.SetFloat("_Shininess", 15f);     // Increase for sharper reflections
                renderer.material.SetColor("_SpecColor", new Color(0.5f, 0.5f, 0.6f)); // Slightly brighter specular
            }

            // Ensure it has the correct TechTag
            var techTag = prefab.GetComponent<TechTag>() ?? prefab.AddComponent<TechTag>();
            techTag.type = Info.TechType;

            Plugin.Logger.LogInfo("Iridium prefab modified with black void metallic appearance.");
        }
    }
}