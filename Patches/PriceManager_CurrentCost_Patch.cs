using HarmonyLib;

namespace CurrencyChanger2.Patches
{
    [HarmonyPatch(typeof(PriceManager), "CurrentCost")]
    public static class PriceManager_CurrentCost_Patch
    {
        public static void Postfix(ref float __result)
        {
            __result *= Plugin.CurrencyValueFactor.Value;
        }
    }
}
