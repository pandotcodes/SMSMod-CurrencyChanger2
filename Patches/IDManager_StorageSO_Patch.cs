using HarmonyLib;

namespace CurrencyChanger2.Patches
{
    [HarmonyPatch(typeof(IDManager), "StorageSO")]
    public static class IDManager_StorageSO_Patch
    {
        public static bool Done = false;
        public static void Prefix(IDManager __instance)
        {
            if (Done) return;
            Done = true;
            __instance.StorageSections.ForEach(x => x.Cost *= Plugin.CurrencyValueFactor.Value);
        }
    }
}
