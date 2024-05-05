using HarmonyLib;

namespace CurrencyChanger2.Patches
{
    [HarmonyPatch(typeof(PricingInteraction), "ClosePricingMenu")]
    public static class PricingInteraction_ClosePricingMenu_Patch
    {
        public static void Postfix() => DayCycleManager_StartNextDay_Patch.UpdatePlayerPricing();
    }
}
