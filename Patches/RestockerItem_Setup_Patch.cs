using HarmonyLib;
using MyBox;

namespace CurrencyChanger2.Patches
{
    [HarmonyPatch(typeof(RestockerItem), "Setup")]
    public static class RestockerItem_Setup_Patch
    {
        public static bool Done = false;
        public static void Postfix(RestockerItem __instance)
        {
            if (!Done)
            {
                Done = true;
                Singleton<IDManager>.Instance.m_Restockers.ForEach(x => { x.DailyWage *= Plugin.CurrencyValueFactor.Value; x.HiringCost *= Plugin.CurrencyValueFactor.Value; });
            }
            __instance.m_LocalizedDailyWageText.StringReference.Arguments = new object[]
            {
            __instance.m_RestockerSetup.DailyWage.ToMoneyText(__instance.m_DailyWageText.fontSize)
            };
            __instance.m_LocalizedDailyWageText.RefreshString();
            __instance.m_LocalizedHiringCostText.StringReference.Arguments = new object[]
            {
            __instance.m_RestockerSetup.HiringCost.ToMoneyText(__instance.m_HiringCostText.fontSize)
            };
            __instance.m_LocalizedHiringCostText.RefreshString();
        }
    }
}
