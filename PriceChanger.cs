using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using MyBox;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace CurrencyChanger2
{
    [BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERSION), BepInDependency("ChangeCurrency")]
    public class PriceChanger : BaseUnityPlugin
    {
        public static Dictionary<int, ConfigEntry<float>> ConfigEntries;
        public static ConfigEntry<bool> Override;
        public const string PLUGIN_GUID = PluginInfo.PLUGIN_GUID + ".ProductPrices";
        public const string PLUGIN_NAME = PluginInfo.PLUGIN_NAME + " Product Price Changer";
        public const string PLUGIN_VERSION = PluginInfo.PLUGIN_VERSION;
        public static new ConfigFile Config { get; set; }
        public static ManualLogSource Log { get; set; }
        public void Awake()
        {
            Config = base.Config;
            Config.Bind("Info", ".", ".", "This config file allows you to independently define the base prices of every product.\nThis price will be what you end up buying them from the market for,\nbut it will also be what the market value (the recommended selling price) will be calculated based on.\nIf both are configured, the \"Value Factor\" multiplier from the main config will still be applied to these prices.");
            Override = Config.Bind("Info", "Allow Ingame Price Changes to override config", true, "By default, the in-game prices will change by up to 20% in either direction on a few randomly selected products every day.\nSince this feature overrides product prices, it causes that to stop working.\nBy setting this to true, the in-game price changes will instead override the ones in this config file.\nThis is irreversible, so make sure to backup the config file if you've put a lot of work into editing it.");
            Log = Logger;

            SceneManager.sceneLoaded += (a, b) => ConfigEntries = null;
        }
    }
}
