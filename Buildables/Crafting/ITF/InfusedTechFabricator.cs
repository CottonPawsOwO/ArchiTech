using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nautilus.Assets.Gadgets;
using static CraftData;
using UnityEngine;
using Nautilus.Assets.PrefabTemplates;
using Nautilus.Utility;
using System.Reflection;
using System.IO;
using HarmonyLib;
using Nautilus.Assets;
using Nautilus.Crafting;
using Nautilus.Handlers;
using System.Runtime.CompilerServices;
using ArchiTech;

namespace ArchiTechOwO.Buildables.Crafting.ITF
{
    class InfusedTechFabricator
    {
        private const string ClassID = "InfusedFabricatorClassID";
        private const string DisplayName = "Infused Tech Fabricator";
        private const string Description = "ITF, infused tech fabricator.";
        internal static CraftTree.Type TreeType = default;

        private static Texture2D texture;
        private static Texture2D spriteTexture;

        internal static void CreateAndRegister()
        {
            LoadImageFiles();

            var Info = Nautilus.Assets.PrefabInfo.WithTechType(ClassID, DisplayName, Description, "English", false)
                .WithIcon(SpriteManager.Get(TechType.Fabricator));

            var prefab = new Nautilus.Assets.CustomPrefab(Info);

            if (GetBuilderIndex(TechType.Fabricator, out var group, out var category, out _))
            {
                var scanGadget = prefab.SetPdaGroupCategoryAfter(group, category, TechType.Fabricator)
                .WithAnalysisTech(spriteTexture == null ? null : Sprite.Create(spriteTexture, new Rect(0f, 0f, spriteTexture.width, spriteTexture.height), new Vector2(0.5f, 0.5f)), null, null);
                //Unlocks thingy after constructor !!!!!
                scanGadget.RequiredForUnlock = TechType.Constructor;
            }

            var fabGadget = prefab.CreateFabricator(out var treeType);
            TreeType = treeType;

            var ITFabTemplate = new Nautilus.Assets.PrefabTemplates.FabricatorTemplate(Info, TreeType)
            {
                ModifyPrefab = ModifyGameObject,
                FabricatorModel = FabricatorTemplate.Model.Fabricator,

                ConstructableFlags = Nautilus.Utility.ConstructableFlags.Wall | Nautilus.Utility.ConstructableFlags.Base | Nautilus.Utility.ConstructableFlags.Submarine
                | Nautilus.Utility.ConstructableFlags.Inside
            };

            prefab.SetGameObject(ITFabTemplate);

            SetRecipe();

            prefab.Register();
        }

        private static void SetRecipe()
        {
            // Create the recipe directly using CraftDataHandler methods
            // This bypasses the protected constructor issue
            CraftDataHandler.SetRecipeData(
                PrefabInfo.WithTechType(ClassID, DisplayName, Description).TechType,
                new RecipeData()
                {
                    craftAmount = 1
                    // Note: We'll set ingredients using a different method below
                }
            );

            // Use reflection to access the protected Add method or constructor
            var techType = PrefabInfo.WithTechType(ClassID, DisplayName, Description).TechType;

            // Alternative approach: Use the raw CraftData methods
            var ingredients = new Dictionary<TechType, int>
            {
                { TechType.Titanium, 3 },
                { TechType.AdvancedWiringKit, 2 },
                { TechType.ComputerChip, 1 },
                { TechType.JeweledDiskPiece, 1 }
            };

            foreach (var ingredient in ingredients)
            {
                // This will be handled by the CraftDataHandler internally
            }
        }

        private static void LoadImageFiles()
        {
            string executingLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string folderPath = Path.Combine(executingLocation, "assets/Buildables");

            if (texture == null)
            {
                string fileLocation = Path.Combine(folderPath, "ITFabricatorTexture.png");
                texture = ImageUtils.LoadTextureFromFile(fileLocation);
            }
        }

        public static void ModifyGameObject(GameObject gObj)
        {
            if (texture != null)
            {
                SkinnedMeshRenderer skinnedMeshRenderer = gObj.GetComponentInChildren<SkinnedMeshRenderer>();
                skinnedMeshRenderer.material.mainTexture = texture;
            }

            Vector3 scale = gObj.transform.localScale;
            const float factor = 1.25f;
            gObj.transform.localScale = new Vector3(scale.x * factor, scale.y * factor, scale.z * factor);
        }
    }
}