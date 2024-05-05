using HarmonyLib;

namespace CurrencyChanger2.Patches
{
    [HarmonyPatch(typeof(PriceTag), "EnableTag")]
    public static class PriceTag_EnableTag_Patch
    {
        public static void Postfix(PriceTag __instance)
        {
            if(__instance.m_PriceText.text == "$-")
            {
                __instance.m_PriceText.text = Plugin.CurrencyPrefix.Value + "-" + Plugin.CurrencySuffix.Value;
            }
        }
    }
}
