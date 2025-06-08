using HarmonyLib;
using System.Linq;

namespace CurrencyChanger2.Patches
{
    [HarmonyPatch(typeof(PriceManager), "ChangeCurrentCost")]
    public static class PriceManager_ChangeCurrentCost_Patch
    {
        public static void Postfix(int productID, PriceManager __instance)
        {
            if(PriceChanger.Override.Value) PriceChanger.ConfigEntries[productID].Value = __instance.m_CurrentCosts.FirstOrDefault((Pricing i) => i.ProductID == productID).Price;
        }
    }
}
