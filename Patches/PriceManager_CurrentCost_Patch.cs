using BepInEx.Configuration;
using HarmonyLib;
using MyBox;
using System.Collections.Generic;
using UnityEngine;

namespace CurrencyChanger2.Patches
{
    [HarmonyPatch(typeof(PriceManager), "CurrentCost")]
    public static class PriceManager_CurrentCost_Patch
    {
        public static void Postfix(int productID, ref float __result)
        {
            if (PriceChanger.ConfigEntries == null)
            {
                PriceChanger.ConfigEntries = new();
                Singleton<IDManager>.Instance.Products.ForEach(product =>
                {
                    PriceChanger.Log.LogInfo("Registering Config Entry for " + product.ProductBrand + " " + product.ProductName);
                    PriceChanger.ConfigEntries[product.ID] = PriceChanger.Config.Bind("Product Base Prices", "Product " + product.ID, product.BasePrice, "The base price of " + product.ProductBrand + " " + product.ProductName + " (ID: " + product.ID + ")");
                });
            }
            __result = PriceChanger.ConfigEntries[productID].Value;
            __result *= Plugin.CurrencyValueFactor.Value;
        }
    }
    [HarmonyPatch(typeof(PriceManager), "PreviousCost")]
    public static class PriceManager_PreviousCost_Patch
    {
        public static void Postfix(int productID, ref float __result)
        {
            if (Plugin.ApplyValueFactorRetroactively.Value && !Mathf.Approximately(__result, -1f))
                __result *= Plugin.CurrencyValueFactor.Value;
        }
    }
    [HarmonyPatch(typeof(PricingItem), "Setup")]
    public static class PricingItem_Setup_Patch
    {
        public static void Prefix(Pricing data)
        {
            data.Price = Singleton<PriceManager>.Instance.CurrentCost(data.ProductID);
        }
    }
    //[HarmonyPatch(typeof(PriceManager), "AverageCost")]
    //public static class PriceManager_AverageCost_Patch
    //{
    //    public static void Postfix(int productID, ref float __result)
    //    {
    //        if (Plugin.ApplyValueFactorRetroactively.Value)
    //            __result *= Plugin.CurrencyValueFactor.Value;
    //    }
    //}
}