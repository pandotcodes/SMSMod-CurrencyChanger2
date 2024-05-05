using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using CurrencyChanger2.ConfigMigration;
using CurrencyChanger2.GoofyAhhCustomComponents;
using HarmonyLib;
using MyBox;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEngineInternal;
using Activator = CurrencyChanger2.GoofyAhhCustomComponents.Activator;

namespace CurrencyChanger2
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public partial class Plugin : BaseUnityPlugin
    {
        internal static bool MoneyGeneratorDone = false;
        static Money coin6Prefab;
        static Money coin7Prefab;

        public static ManualLogSource StaticLogger;

        public static ConfigEntry<bool> EnableAdditionalCoinCompartments { get; set; }

        public static ConfigEntry<float> LowestBill { get; set; }
        public static ConfigEntry<string> TerminalSymbol { get; set; }
        public static ConfigEntry<string> CurrencyPrefix { get; set; }
        public static ConfigEntry<string> CurrencySuffix { get; set; }
        public static ConfigEntry<string> CurrencyDecimalSeperator { get; set; }
        public static ConfigEntry<float> CurrencyValueFactor { get; set; }


        public static ConfigEntry<CardTextureType> CreditCardFront { get; set; }
        public static ConfigEntry<CardTextureType> CreditCardBack { get; set; }

        public static ConfigEntry<string> VersionNumber { get; set; }

        public static ConfigComp<BillTextureType> Bill1 { get; set; }
        public static ConfigComp<BillTextureType> Bill2 { get; set; }
        public static ConfigComp<BillTextureType> Bill3 { get; set; }
        public static ConfigComp<BillTextureType> Bill4 { get; set; }
        public static ConfigComp<BillTextureType> Bill5 { get; set; }

        public static ConfigComp<CoinTextureType> Coin1 { get; set; }
        public static ConfigComp<CoinTextureType> Coin2 { get; set; }
        public static ConfigComp<CoinTextureType> Coin3 { get; set; }
        public static ConfigComp<CoinTextureType> Coin4 { get; set; }
        public static ConfigComp<CoinTextureType> Coin5 { get; set; }
        public static ConfigComp<CoinTextureType> Coin6 { get; set; }
        public static ConfigComp<CoinTextureType> Coin7 { get; set; }

        public static PriceRounder Rounder { get; set; }
        private void Awake()
        {
            StaticLogger = Logger;

            V1ConfigResult v1Config = V1ConfigResult.Check(Config);

            InitConfig();

            if (v1Config.ConfigWasV1)
            {
                v1Config.Apply();
                v1Config.RemoveV1ConfigEntries(Config);
            }

            string customImagePath = Path.Combine("BepInEx", "config", "ChangeCurrency");
            if (!Directory.Exists(customImagePath)) Directory.CreateDirectory(customImagePath);

            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded! Applying patch...");
            Logger.LogInfo($"Working directory: {Environment.CurrentDirectory}");
            Logger.LogInfo($"Currency String: " + System.Globalization.NumberFormatInfo.CurrentInfo.CurrencyPositivePattern);
            Harmony harmony = new Harmony(PluginInfo.PLUGIN_GUID);
            harmony.PatchAll();

            //HarmonyMethod eCommerce = new HarmonyMethod(typeof(Plugin).GetMethod("eCommercePatch"));
            //if(Assembly.Load("eCommerce") is Assembly assembly)
            //{
            //    if(assembly.GetType("eCommerce", false) is Type eCommerceType)
            //    {
            //        if(eCommerceType.GetMethod("OnLateUpdate") is MethodInfo method)
            //        {
            //            Logger.LogWarning("eCommerce is present - applying patch");
            //            harmony.Patch(method, postfix: eCommerce);
            //        }
            //    }
            //}

            SceneManager.sceneLoaded += (a, b) =>
            {
                if (SceneManager.GetActiveScene().name == "Main Menu")
                {
                    coin6Prefab = null;
                    coin7Prefab = null;
                    MoneyGeneratorDone = false;
                }
            };

            Rounder = new PriceRounder(Config);
        }
        //public static Dictionary<object, List<int>> updatedPrices = new();
        //public void eCommercePatch(object __instance)
        //{
        //    object order = __instance.GetType().GetField("currentOrder").GetValue(__instance);
        //    //Logger.LogInfo(order == null ? "Order is missing" : "Order was found");
        //    Dictionary<int, float> ProductsPrices = (Dictionary<int, float>)order.GetType().GetField("ProductsPrices").GetValue(order);
        //    //Logger.LogInfo(ProductsPrices == null ? "Dictionary is missing" : "Dictionary was found");

        //    foreach(var kvp in ProductsPrices)
        //    {
        //        if(updatedPrices.ContainsKey(order))
        //        {
        //            if (updatedPrices[order].Contains(kvp.Key))
        //            {
        //                continue;
        //            }
        //        } else
        //        {
        //            updatedPrices.Add(order, new());
        //        }
        //        ProductsPrices[kvp.Key] *= CurrencyValueFactor.Value;
        //        updatedPrices[order].Add(kvp.Key);
        //    }
        //}
        private void InitConfig()
        {
            EnableAdditionalCoinCompartments = Config.Bind("Currency Changer", "Enable Additional Coin Compartments", false, "Turns the $1 bill compartment into two coin compartments, 6 and 7, for additional coins.\nThis disables Bill Compartment 1 and enables Coin Compartments 6 and 7.");

            LowestBill = Config.Bind("Currency Settings", "Lowest Bill", 1f, "The lowest value of money that should be considered a bill by the game.");
            TerminalSymbol = Config.Bind("Currency Settings", "Terminal Symbol", "$", "What symbol to display on the credit card terminal.");
            CurrencyPrefix = Config.Bind("Currency Settings", "Prefix", "$", "The currency symbol (or arbitrary string) to use in front of the value, where the dollar sign would normally be.");
            CurrencySuffix = Config.Bind("Currency Settings", "Suffix", "", "The currency symbol (or arbitrary string) to use after the value, where a euro sign might for example be.");
            CurrencyDecimalSeperator = Config.Bind("Currency Settings", "Decimal Seperator", ".", "What symbol to use to seperate the whole number part from the fractional part.");
            CurrencyValueFactor = Config.Bind("Currency Settings", "Value Factor", 1f, "Multiplies all product costs and market prices by the defined number.");

            VersionNumber = Config.Bind("Info", "Version", "2.0.0", "What version of the plugin this config file was created under.\nDo NOT change this value, or stuff WILL break.\nIf this value is missing, v1.2.1 will be assumed.");

            CreditCardFront = Config.Bind("Credit Card", "Front Texture", CardTextureType.DEFAULT, "The texture type to place on the front of the credit card.\nIf you choose \"CUSTOM\", the texture file will be loaded from BepInEx/config/ChangeCurrency/Card_front.png");
            CreditCardBack = Config.Bind("Credit Card", "Back Texture", CardTextureType.DEFAULT, "The texture type to place on the back of the credit card.\nIf you choose \"CUSTOM\", the texture file will be loaded from BepInEx/config/ChangeCurrency/Card_back.png");

            Bill1 = new(Config, "Bill", 1, 01.00f, "$1", BillTextureType.USD_1_BILL);
            Bill2 = new(Config, "Bill", 2, 05.00f, "$5", BillTextureType.USD_5_BILL);
            Bill3 = new(Config, "Bill", 3, 10.00f, "$10", BillTextureType.USD_10_BILL);
            Bill4 = new(Config, "Bill", 4, 20.00f, "$20", BillTextureType.USD_20_BILL);
            Bill5 = new(Config, "Bill", 5, 50.00f, "$50", BillTextureType.USD_50_BILL);

            Coin1 = new(Config, "Coin", 1, 00.01f, "1¢", CoinTextureType.USD_1ct_COIN);
            Coin2 = new(Config, "Coin", 2, 00.05f, "5¢", CoinTextureType.USD_5ct_COIN);
            Coin3 = new(Config, "Coin", 3, 00.10f, "10¢", CoinTextureType.USD_10ct_COIN);
            Coin4 = new(Config, "Coin", 4, 00.25f, "25¢", CoinTextureType.USD_25ct_COIN);
            Coin5 = new(Config, "Coin", 5, 00.50f, "50¢", CoinTextureType.USD_50ct_COIN);
            Coin6 = new(Config, "Coin", 6, 01.00f, "$1", CoinTextureType.EUR_1_COIN);
            Coin7 = new(Config, "Coin", 7, 02.00f, "$2", CoinTextureType.EUR_2_COIN);
        }
        public static void InitExtraCompartments(CheckoutDrawer __instance, GameObject Bill1, GameObject Case, GameObject Coin5, out GameObject Coin6, out GameObject Coin7, out Money Coin6Prefab, out Money Coin7Prefab, out TextMeshProUGUI Coin6Text, out TextMeshProUGUI Coin7Text)
        {
            float rot = __instance.transform.parent.parent.eulerAngles.y;

            //MoneyGeneratorDone = false;
            var divider = GameObject.CreatePrimitive(PrimitiveType.Cube);
            divider.transform.localScale = new Vector3(0.072f, 0.03f, 0.01f);
            divider.transform.SetParent(__instance.gameObject.transform);
            divider.transform.localPosition = new Vector3(0.1591f, 0.0360f, 0.1529f);
            divider.transform.eulerAngles = new Vector3(0, -rot, 0);

            var mat = new Material(Case.transform.GetChild(0).GetComponent<MeshRenderer>().material);
            mat.color = Color.black;
            divider.GetComponent<MeshRenderer>().material = mat;

            Bill1.transform.localPosition = new Vector3(0, -10, 0);
            CheckoutChangeManager ccm = __instance.transform.parent.parent.GetComponent<CheckoutChangeManager>();
            List<MoneyPack> MoneyPacks = ccm.m_MoneyPacks == null ? new() : ccm.m_MoneyPacks.ToList();
            MoneyPacks.RemoveAll(x => x && x.gameObject.name == "1 Dollar Pack");
            Bill1 = null;

            Money prefab;
            if (MoneyGeneratorDone)
            {
                prefab = coin6Prefab;
            }
            else
            {
                prefab = Singleton<MoneyGenerator>.Instance.m_MoneyPrefabs[4];
            }

            void ApplyNewRenderers(MeshRenderer currentRenderer)
            {
                //currentRenderer.CopyPropertiesOf(prefab.GetComponent<MeshRenderer>());
                currentRenderer.materials = currentRenderer.materials.Select(x => Material.Instantiate(x)).ToArray();
            }

            Coin6 = GameObject.Instantiate(Coin5);
            Coin6.transform.SetParent(__instance.gameObject.transform);
            Coin6.transform.localPosition = new Vector3(divider.transform.localPosition.x, 0.04f, divider.transform.localPosition.z - 0.035f);
            Coin6.transform.GetChild(0).GetChild(0).localPosition = Vector3.zero;
            Coin6.SetActive(true);
            Coin6.AddComponent<Activator>();
            Coin6.GetComponentsInChildren<MeshRenderer>().ForEach(ApplyNewRenderers);
            Coin7 = GameObject.Instantiate(Coin5);
            Coin7.transform.SetParent(__instance.gameObject.transform);
            Coin7.transform.localPosition = new Vector3(divider.transform.localPosition.x, 0.04f, divider.transform.localPosition.z + 0.035f);
            Coin7.transform.GetChild(0).GetChild(0).localPosition = Vector3.zero;
            Coin7.SetActive(true);
            Coin7.AddComponent<Activator>();
            Coin7.GetComponentsInChildren<MeshRenderer>().ForEach(ApplyNewRenderers);

            Coin6Text = __instance.gameObject.transform.GetChild(11).GetChild(0).GetComponent<TextMeshProUGUI>();
            Coin7Text = GameObject.Instantiate(__instance.gameObject.transform.GetChild(11).GetChild(0).GetComponent<TextMeshProUGUI>().gameObject).GetComponent<TextMeshProUGUI>();
            TransformDefiner.AddToGameObject(Coin7Text.gameObject, new Vector3(-2.72f, 0.9f, 0.39f), new Vector3(-132.3f, 38f, 0f), new Vector3(-1f, 1f, 1f), new Vector3(Coin7Text.transform.eulerAngles.x, -rot, Coin7Text.transform.eulerAngles.z));
            Coin7Text.transform.SetParent(Coin6Text.transform.parent);

            MoneyPacks.Add(Coin6.GetComponent<MoneyPack>());
            MoneyPacks.Add(Coin7.GetComponent<MoneyPack>());
            ccm.m_MoneyPacks = MoneyPacks.Where(x => x).ToArray();
            if (!MoneyGeneratorDone)
            {
                List<Money> MoneyPrefabs = Singleton<MoneyGenerator>.Instance.m_MoneyPrefabs == null ? new() : Singleton<MoneyGenerator>.Instance.m_MoneyPrefabs.ToList();
                MoneyPrefabs.RemoveAll(x => x && x.gameObject.name == "1 Dollar Variant");
                Coin6Prefab = GameObject.Instantiate(Singleton<MoneyGenerator>.Instance.m_MoneyPrefabs[4].gameObject).GetComponent<Money>();
                Coin7Prefab = GameObject.Instantiate(Singleton<MoneyGenerator>.Instance.m_MoneyPrefabs[4].gameObject).GetComponent<Money>();
                ApplyNewRenderers(Coin6Prefab.GetComponent<MeshRenderer>());
                ApplyNewRenderers(Coin7Prefab.GetComponent<MeshRenderer>());
                coin6Prefab = Coin6Prefab;
                coin7Prefab = Coin7Prefab;
                MoneyPrefabs.Add(Coin6Prefab);
                MoneyPrefabs.Add(Coin7Prefab);
                Singleton<MoneyGenerator>.Instance.m_MoneyPrefabs = MoneyPrefabs.Where(x => x).ToArray();

                MoneyGeneratorDone = true;
            }
            else
            {
                Coin6Prefab = coin6Prefab;
                Coin7Prefab = coin7Prefab;
            }
        }
        public static void ApplyConfig(CheckoutDrawer __instance, GameObject Bill1, GameObject Bill2, GameObject Bill3, GameObject Bill4, GameObject Bill5, GameObject Coin1, GameObject Coin2, GameObject Coin3, GameObject Coin4, GameObject Coin5, GameObject Coin6, GameObject Coin7, TextMeshProUGUI Coin6Text, TextMeshProUGUI Coin7Text, Money Coin6Prefab, Money Coin7Prefab)
        {
            if (Bill1 != null) Plugin.Bill1.Apply(Bill1.GetComponent<MoneyPack>()).Apply(__instance.gameObject.transform.GetChild(11).GetChild(0).GetComponent<TextMeshProUGUI>());
            if (Bill2 != null) Plugin.Bill2.Apply(Bill2.GetComponent<MoneyPack>()).Apply(__instance.gameObject.transform.GetChild(11).GetChild(1).GetComponent<TextMeshProUGUI>());
            if (Bill3 != null) Plugin.Bill3.Apply(Bill3.GetComponent<MoneyPack>()).Apply(__instance.gameObject.transform.GetChild(11).GetChild(2).GetComponent<TextMeshProUGUI>());
            if (Bill4 != null) Plugin.Bill4.Apply(Bill4.GetComponent<MoneyPack>()).Apply(__instance.gameObject.transform.GetChild(11).GetChild(3).GetComponent<TextMeshProUGUI>());
            if (Bill5 != null) Plugin.Bill5.Apply(Bill5.GetComponent<MoneyPack>()).Apply(__instance.gameObject.transform.GetChild(11).GetChild(4).GetComponent<TextMeshProUGUI>());

            if (Coin1 != null) Plugin.Coin1.Apply(Coin1.GetComponent<MoneyPack>()).Apply(__instance.gameObject.transform.GetChild(11).GetChild(5).GetComponent<TextMeshProUGUI>());
            if (Coin2 != null) Plugin.Coin2.Apply(Coin2.GetComponent<MoneyPack>()).Apply(__instance.gameObject.transform.GetChild(11).GetChild(6).GetComponent<TextMeshProUGUI>());
            if (Coin3 != null) Plugin.Coin3.Apply(Coin3.GetComponent<MoneyPack>()).Apply(__instance.gameObject.transform.GetChild(11).GetChild(7).GetComponent<TextMeshProUGUI>());
            if (Coin4 != null) Plugin.Coin4.Apply(Coin4.GetComponent<MoneyPack>()).Apply(__instance.gameObject.transform.GetChild(11).GetChild(8).GetComponent<TextMeshProUGUI>());

            if (Coin6 != null) Plugin.Coin6.Apply(Coin6.GetComponent<MoneyPack>()).Apply(Coin6Text).Apply(Coin6Prefab);
            if (Coin7 != null) Plugin.Coin7.Apply(Coin7.GetComponent<MoneyPack>()).Apply(Coin7Text).Apply(Coin7Prefab);

            if (Coin5 != null) Plugin.Coin5.Apply(Coin5.GetComponent<MoneyPack>()).Apply(__instance.gameObject.transform.GetChild(11).GetChild(9).GetComponent<TextMeshProUGUI>());
        }
        public static (Texture2D front, Texture2D back) GetCreditCardTextures()
        {
            return (GetCreditCardFrontTexture(), GetCreditCardBackTexture());
        }
        static Texture2D storedFrontTex;
        static Texture2D storedBackTex;
        public static Texture2D GetCreditCardFrontTexture()
        {
            if (Convert.ToInt32(CreditCardFront.Value) == Convert.ToInt32(CreditCardFront.DefaultValue)) return null;
            if (storedFrontTex != null) return storedFrontTex;
            string name = Enum.GetName(typeof(CardTextureType), CreditCardFront.Value);
            byte[] data;
            if (name == "CUSTOM")
            {
                data = File.ReadAllBytes(Path.Combine("BepInEx", "config", "ChangeCurrency", "Card_front.png"));
            }
            else
            {
                data = (byte[])Properties.Resources.ResourceManager.GetObject(name);
            }
            Texture2D tex = new Texture2D(1024, 1024);
            tex.LoadImage(data);
            return tex;
        }
        public static Texture2D GetCreditCardBackTexture()
        {
            if (Convert.ToInt32(CreditCardBack.Value) == Convert.ToInt32(CreditCardBack.DefaultValue)) return null;
            if (storedBackTex != null) return storedBackTex;
            string name = Enum.GetName(typeof(CardTextureType), CreditCardBack.Value);
            byte[] data;
            if (name == "CUSTOM")
            {
                data = File.ReadAllBytes(Path.Combine("BepInEx", "config", "ChangeCurrency", "Card_back.png"));
            }
            else
            {
                data = (byte[])Properties.Resources.ResourceManager.GetObject(name);
            }
            Texture2D tex = new Texture2D(1024, 1024);
            tex.LoadImage(data);
            return tex;
        }
    }
}
