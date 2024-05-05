using HarmonyLib;

namespace CurrencyChanger2.Patches
{
    [HarmonyPatch(typeof(IDManager), "FurnitureSO")]
    public static class IDManager_FurnitureSO_Patch
    {
        public static bool Done = false;
        public static void Prefix(IDManager __instance)
        {
            if (Done) return;
            Done = true;
            __instance.Furnitures.ForEach(x => x.Cost *= Plugin.CurrencyValueFactor.Value);
        }
    }
}
