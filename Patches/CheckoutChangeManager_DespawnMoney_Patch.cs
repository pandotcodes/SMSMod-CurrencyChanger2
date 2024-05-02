using HarmonyLib;
using MyBox;
using System.Linq;

namespace CurrencyChanger2.Patches
{
    [HarmonyPatch(typeof(CheckoutChangeManager), "SpawnMoney")]
    public static class CheckoutChangeManager_SpawnMoney_Patch
    {
        public static void Prefix(CheckoutChangeManager __instance, MoneyPack moneyPack)
        {
            var log = Plugin.StaticLogger.LogInfo;
            log("moneyPack");
            log(moneyPack);
        }
    }
    [HarmonyPatch(typeof(CheckoutChangeManager), "DespawnMoney")]
    public static class CheckoutChangeManager_DespawnMoney_Patch
    {
        public static void Prefix(CheckoutChangeManager __instance)
        {
            return;
            var log = Plugin.StaticLogger.LogInfo;
            log("m_SpawnedMoney");
            __instance.m_SpawnedMoney.ForEach(x => log(x.m_Value));
            log("m_SpawnedCoin");
            __instance.m_SpawnedCoin.ForEach(x => log(x.m_Value));
            log("m_MoneyPacks");
            __instance.m_MoneyPacks.ToList().ForEach(x => log(x.m_Value));
            log("m_MoneyPrefabs");
            log(Singleton<MoneyGenerator>.Instance.m_MoneyPrefabs);
            Singleton<MoneyGenerator>.Instance.m_MoneyPrefabs.ForEach(x => log(x.m_Value));
        }
    }
}
