using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Assets.PrefabTemplates;
using Nautilus.Crafting;
using Nautilus.Utility;
using UnityEngine;
using UWE;

namespace ArchiTech.Items.BasicMaterials
{
    public static class PoziumItem
    {
        public static PrefabInfo Info { get; } = PrefabInfo.WithTechType("Pozium", "Pozium", "A rare crystalline ore containing plasma gases trapped within its lattice structure. Found in kelp-rich areas, this dangerous material can cause electrotrauma if handled without proper isolation equipment. Essential for advanced power cell manufacturing.")
            .WithIcon(ImageUtils.LoadSpriteFromFile(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Assets", "Minerals", "Pozium.png")));

        public static void Register()
        {
            var prefab = new CustomPrefab(Info);

            // Use Magnetite as the base model (crystalline structure)
            var cloneTemplate = new CloneTemplate(Info, TechType.Magnetite)
            {
                ModifyPrefab = ModifyPoziumPrefab
            };

            prefab.SetGameObject(cloneTemplate);

            // Electrical/dangerous pickup sound
            CraftData.pickupSoundList.Add(Info.TechType, "event:/loot/pickup_lithium");

            // Spawn in kelp areas with large deposits
            prefab.SetSpawns(new WorldEntityInfo
            {
                cellLevel = LargeWorldEntity.CellLevel.Near,
                classId = Info.ClassID,
                localScale = Vector3.one * 1.2f, // Slightly larger deposits
                slotType = EntitySlot.Type.Small,
                techType = Info.TechType
            }, BiomesToSpawnIn);

            // Register the prefab
            prefab.Register();

            Plugin.Logger.LogInfo("Pozium ore registered successfully.");
        }

        private static LootDistributionData.BiomeData[] BiomesToSpawnIn
        {
            get
            {
                return new LootDistributionData.BiomeData[]
                {
                    
                    new LootDistributionData.BiomeData
                    {
                        biome = BiomeType.Kelp_CaveSpecial,
                        count = 2, 
                        probability = 0.6f
                    },
                    new LootDistributionData.BiomeData
                    {
                        biome = BiomeType.Kelp_CaveFloor,
                        count = 1,
                        probability = 0.6f
                    },
                    
                    new LootDistributionData.BiomeData
                    {
                        biome = BiomeType.Kelp_CaveWall,
                        count = 3, 
                        probability = 0.6f
                    },
                    new LootDistributionData.BiomeData
                    {
                        biome = BiomeType.Kelp_ShellTunnel,
                        count = 2,
                        probability = 0.6f
                    },
                    
                    new LootDistributionData.BiomeData
                    {
                        biome = BiomeType.Kelp_CaveSpecial,
                        count = 1,
                        probability = 0.6f
                    }
                };
            }
        }




        private static void ModifyPoziumPrefab(GameObject prefab)
        {
            // Add a resource tracker
            PrefabUtils.AddResourceTracker(prefab, Info.TechType);

            var renderer = prefab.GetComponentInChildren<Renderer>();
            if (renderer != null)
            {
                // Use the Pozium.png texture
                renderer.material.SetTexture("_MainTex", ImageUtils.LoadTextureFromFile(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Assets", "Minerals", "Pozium.png")));

                // Whitish-pink base color
                renderer.material.color = new Color(0.95f, 0.85f, 0.90f, 1f); // Light pinkish-white
                renderer.material.SetColor("_Color", new Color(0.95f, 0.85f, 0.90f, 1f));

                // Crystal-like properties with electrical shimmer
                renderer.material.SetFloat("_SpecInt", 15f);        // High specularity for crystal look
                renderer.material.SetFloat("_Shininess", 18f);      // Very shiny
                renderer.material.SetFloat("_Fresnel", 0.45f);      // Crystal-like fresnel
                renderer.material.SetColor("_SpecColor", new Color(0.7f, 0.9f, 1.0f)); // Sky blue specular highlights

                // Sky blue electrical glow at crystal tips
                renderer.material.SetColor("_GlowColor", new Color(0.4f, 0.7f, 1.0f, 0.6f)); // Sky blue glow
                renderer.material.SetFloat("_GlowStrength", 0.8f);     // Strong glow for plasma effect
                renderer.material.SetFloat("_GlowStrengthNight", 1.2f); // Even stronger at night

                // Add electrical sparkle effect
                renderer.material.SetFloat("_RimPower", 2.0f);
                renderer.material.SetColor("_RimColor", new Color(0.3f, 0.6f, 1.0f, 0.8f)); // Blue rim lighting
            }

            // Ensure it has the correct TechTag
            var techTag = prefab.GetComponent<TechTag>() ?? prefab.AddComponent<TechTag>();
            techTag.type = Info.TechType;

            Plugin.Logger.LogInfo("Pozium prefab modified with crystalline plasma appearance and electrical effects.");
        }
    }
}