using HarmonyLib;
using MyBox;
using TMPro;
using UnityEngine;

namespace CurrencyChanger2.Patches
{
    [HarmonyPatch(typeof(CheckoutDrawer), "Awake")]
    public static class CheckoutDrawer_Awake_Patch
    {
        public static void Postfix(CheckoutDrawer __instance)
        {
            Plugin.StaticLogger.LogInfo("CheckoutDrawer_Awake_Patch");
            Plugin.StaticLogger.LogInfo(Plugin.EnableAdditionalCoinCompartments.Value);

            var Case = __instance.gameObject.transform.GetChild(0).gameObject;

            var Bill1 = __instance.gameObject.transform.GetChild(1).gameObject;
            var Bill2 = __instance.gameObject.transform.GetChild(2).gameObject;
            var Bill3 = __instance.gameObject.transform.GetChild(3).gameObject;
            var Bill4 = __instance.gameObject.transform.GetChild(4).gameObject;
            var Bill5 = __instance.gameObject.transform.GetChild(5).gameObject;

            var Coin1 = __instance.gameObject.transform.GetChild(6).gameObject;
            var Coin2 = __instance.gameObject.transform.GetChild(7).gameObject;
            var Coin3 = __instance.gameObject.transform.GetChild(8).gameObject;
            var Coin4 = __instance.gameObject.transform.GetChild(9).gameObject;
            var Coin5 = __instance.gameObject.transform.GetChild(10).gameObject;

            GameObject Coin6 = null;
            GameObject Coin7 = null;

            Money Coin6Prefab = null;
            Money Coin7Prefab = null;

            TextMeshProUGUI Coin6Text = null;
            TextMeshProUGUI Coin7Text = null;
            if (!Plugin.MoneyGeneratorDone)
            {
                Plugin.Bill1.Apply(Singleton<MoneyGenerator>.Instance.m_MoneyPrefabs[5]);
                Plugin.Bill2.Apply(Singleton<MoneyGenerator>.Instance.m_MoneyPrefabs[6]);
                Plugin.Bill3.Apply(Singleton<MoneyGenerator>.Instance.m_MoneyPrefabs[7]);
                Plugin.Bill4.Apply(Singleton<MoneyGenerator>.Instance.m_MoneyPrefabs[8]);
                Plugin.Bill5.Apply(Singleton<MoneyGenerator>.Instance.m_MoneyPrefabs[9]);

                Plugin.Coin1.Apply(Singleton<MoneyGenerator>.Instance.m_MoneyPrefabs[0]);
                Plugin.Coin2.Apply(Singleton<MoneyGenerator>.Instance.m_MoneyPrefabs[1]);
                Plugin.Coin3.Apply(Singleton<MoneyGenerator>.Instance.m_MoneyPrefabs[2]);
                Plugin.Coin4.Apply(Singleton<MoneyGenerator>.Instance.m_MoneyPrefabs[3]);
                Plugin.Coin5.Apply(Singleton<MoneyGenerator>.Instance.m_MoneyPrefabs[4]);
            }

            if (Plugin.EnableAdditionalCoinCompartments.Value)
            {
                Plugin.InitExtraCompartments(__instance, Bill1, Case, Coin5, out Coin6, out Coin7, out Coin6Prefab, out Coin7Prefab, out Coin6Text, out Coin7Text);
            }
            Plugin.ApplyConfig(__instance, Bill1, Bill2, Bill3, Bill4, Bill5, Coin1, Coin2, Coin3, Coin4, Coin5, Coin6, Coin7, Coin6Text, Coin7Text, Coin6Prefab, Coin7Prefab);
        }
    }
}
