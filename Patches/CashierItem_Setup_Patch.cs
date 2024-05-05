using HarmonyLib;
using MyBox;
using System.Collections.Generic;
using System.Reflection;

namespace CurrencyChanger2.Patches
{
    [HarmonyPatch(typeof(CashierItem), "Setup")]
    public static class CashierItem_Setup_Patch
    {
        public static bool Done = false;
        public static void Postfix(CashierItem __instance)
        {
            if (!Done)
            {
                Done = true;
                Singleton<IDManager>.Instance.m_Cashiers.ForEach(x => { x.DailyWage *= Plugin.CurrencyValueFactor.Value; x.HiringCost *= Plugin.CurrencyValueFactor.Value; });
            }
            __instance.m_LocalizedDailyWageText.StringReference.Arguments = new object[]
            {
            __instance.m_CashierSetup.DailyWage.ToMoneyText(__instance.m_DailyWageText.fontSize)
            };
            __instance.m_LocalizedDailyWageText.RefreshString();
            __instance.m_LocalizedHiringCostText.StringReference.Arguments = new object[]
            {
            __instance.m_CashierSetup.HiringCost.ToMoneyText(__instance.m_HiringCostText.fontSize)
            };
            __instance.m_LocalizedHiringCostText.RefreshString();
        }
    }
    [HarmonyPatch(typeof(DayCycleManager), "StartNextDay")]
    public static class DayCycleManager_StartNextDay_Patch
    {
        public static void Postfix() => UpdatePlayerPricing();
        public static void UpdatePlayerPricing()
        {
            Singleton<PriceManager>.Instance.m_PricesSetByPlayer.ForEach(pricing =>
            {
                pricing.Price = Plugin.Rounder.Round(pricing.Price);
                List<DisplaySlot> displaySlots = Singleton<DisplayManager>.Instance.GetDisplaySlots(pricing.ProductID, false);
                if (displaySlots != null)
                {
                    foreach (DisplaySlot obj in displaySlots)
                    {
                        obj.PricingChanged(pricing.ProductID);
                    }
                }
            });
        }
    }
}
