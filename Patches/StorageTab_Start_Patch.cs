using HarmonyLib;

namespace CurrencyChanger2.Patches
{
    [HarmonyPatch(typeof(StorageTab), "Start")]
    public static class StorageTab_Start_Patch
    {
        public static void Prefix(StorageTab __instance)
        {
            __instance.m_Cost *= Plugin.CurrencyValueFactor.Value;
        }
    }
}
