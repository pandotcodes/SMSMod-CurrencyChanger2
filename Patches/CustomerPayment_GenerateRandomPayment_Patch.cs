using HarmonyLib;

namespace CurrencyChanger2.Patches
{
    [HarmonyPatch(typeof(CustomerPayment), "GenerateRandomPayment")]
    public static class CustomerPayment_GenerateRandomPayment_Patch
    {
        public static void Postfix(ref float __result)
        {
            if(Plugin.Rounder == null)
            {
                return;
            }
            __result = Plugin.Rounder.Round(__result);
        }
    }
}
