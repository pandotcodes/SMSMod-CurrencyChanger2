using HarmonyLib;

namespace CurrencyChanger2.Patches
{
    [HarmonyPatch(typeof(IDManager), "ProductLicenseSO")]
    public static class IDManager_ProductLicenseSO_Patch
    {
        public static bool Done = false;
        public static void Prefix(IDManager __instance)
        {
            if (Done) return;
            Done = true;
            __instance.m_ProductLicenses.ForEach(x => x.PurchasingCost *= Plugin.CurrencyValueFactor.Value);
        }
    }
}
