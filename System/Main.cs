using ArchiTech.Items.BasicMaterials;
using ArchitechAdddedPDAReso;
using ArchiTech;
using BepInEx;
using BepInEx.Logging;
using Nautilus.Handlers;
using Nautilus.Utility;
using System;
using Items.PrecursorMaterials;

namespace ArchiTech
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    [BepInDependency("com.snmodding.nautilus")]
    public class Plugin : BaseUnityPlugin
    {
        public new static ManualLogSource Logger { get; private set; }

        private void Awake()
        {
            Logger = base.Logger;
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");

            try
            {
                RegisterItems();
                SetupCrafting();
                RegisterConsoleCommands();
                Logger.LogInfo("ArchiTech mod loaded successfully!");
            }
            catch (Exception ex)
            {
                Logger.LogError($"Error loading the mod: {ex}");
            }
        }

        private static void RegisterItems()
        {
            Logger.LogInfo("Registering basic resources...");
            PlatinumItem.Register();
            PlatinumDrillableItem.Register();
            MercuryItem.Register();
            MercuryDrillableItem.Register();
            IridiumItem.Register();
            GalliumItem.Register();
            PoziumItem.Register();
            PoziumDrillableItem.Register();
            

            // Register the PDA entry for Mercury
            MercuryPDA.Register(MercuryItem.Info.TechType);
            PlatinumPDA.Register(PlatinumItem.Info.TechType);
            IridiumPDA.Register(IridiumItem.Info.TechType);
            GalliumPDA.Register(GalliumItem.Info.TechType);
            PoziumPDA.Register(PoziumItem.Info.TechType);

            ReinforcementUnlock.ModifyReinforcementUnlock();

            CopperOrePDA.Register();
            TitaniumPDA.Register();
            SilverOrePDA.Register();
            GoldPDA.Register();
            LeadPDA.Register();
            NickelOrePDA.Register();
            LithiumPDA.Register();
            DiamondPDA.Register();
            QuartzPDA.Register();
            MagnetitePDA.Register();
            UraniniteGrystalPDA.Register();
            CrystallineSulfurPDA.Register();
            SaltDepositPDA.Register();
            RubyPDA.Register();
            KyanitePDA.Register();

            Logger.LogInfo("Basic resources registered successfully.");
        }

        private static void SetupCrafting()
        {
            Logger.LogInfo("Setting up crafting system...");
            // TODO: Implement crafting system when needed
            Logger.LogInfo("Crafting system setup completed.");
        }

        private static void RegisterConsoleCommands()
        {
            // Explicitly cast the methods to the correct delegate type (Action)
            // This resolves the compiler ambiguity.
            ConsoleCommandsHandler.RegisterConsoleCommand("giveitem", (Action<string>)GiveItem);
            ConsoleCommandsHandler.RegisterConsoleCommand("giveallarchitechitems", (Action)GiveAllItems);
            Logger.LogInfo("Registered console commands 'giveitem' and 'giveallarchitechitems'.");
        }

        // Usage in console: giveitem mercury
        private static void GiveItem(string itemName)
        {
            if (string.IsNullOrEmpty(itemName))
            {
                ErrorMessage.AddMessage("Usage: giveitem <itemName>");
                return;
            }

            TechType techTypeToGive = TechType.None;

            // Add more custom items here
            switch (itemName.ToLower())
            {
                case "mercury":
                    techTypeToGive = MercuryItem.Info.TechType;
                    break;
                case "gallium":
                    techTypeToGive = GalliumItem.Info.TechType;
                    break;
                case "platinum":
                    techTypeToGive = PlatinumItem.Info.TechType;
                    break;
                case "iridium":
                    techTypeToGive = IridiumItem.Info.TechType;
                    break;
                case "pozium":
                    techTypeToGive = PoziumItem.Info.TechType;
                    break;
                default:
                    if (Enum.TryParse<TechType>(itemName, true, out var parsedTechType))
                    {
                        techTypeToGive = parsedTechType;
                    }
                    break;
            }

            if (techTypeToGive != TechType.None)
            {
                CraftData.AddToInventory(techTypeToGive, 1);
                ErrorMessage.AddMessage($"Added {techTypeToGive} to inventory.");
            }
            else
            {
                ErrorMessage.AddMessage($"Item '{itemName}' not found.");
            }
        }

        // Usage in console: giveallitems
        private static void GiveAllItems()
        {
            CraftData.AddToInventory(MercuryItem.Info.TechType, 1);
            CraftData.AddToInventory(PlatinumItem.Info.TechType, 1);
            CraftData.AddToInventory(IridiumItem.Info.TechType, 1);
            CraftData.AddToInventory(GalliumItem.Info.TechType, 1);
            CraftData.AddToInventory(PoziumItem.Info.TechType, 1);
            ErrorMessage.AddMessage("Added all ArchiTech items to inventory.");
        }
    }

    public static class PluginInfo
    {
        public const string PLUGIN_GUID = "com.architech.mod";
        public const string PLUGIN_NAME = "ArchiTech";
        public const string PLUGIN_VERSION = "1.0.0";
    }
}