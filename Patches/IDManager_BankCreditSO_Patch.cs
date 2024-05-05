using HarmonyLib;

namespace CurrencyChanger2.Patches
{
    [HarmonyPatch(typeof(IDManager), "BankCreditSO")]
    public static class IDManager_BankCreditSO_Patch
    {
        public static bool Done = false;
        public static void Prefix(IDManager __instance)
        {
            if (Done) return;
            Done = true;
            __instance.Loans.ForEach(x => x.Amount *= Plugin.CurrencyValueFactor.Value);
        }
    }
}
