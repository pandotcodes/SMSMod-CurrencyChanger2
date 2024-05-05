using HarmonyLib;

namespace CurrencyChanger2.Patches
{
    [HarmonyPatch(typeof(SaveManager), "Clear")]
    public static class SaveManager_Clear_Patch
    {
        public static void Postfix(SaveManager __instance)
        {
            __instance.Progression.Money *= Plugin.CurrencyValueFactor.Value;
        }
    }
}
