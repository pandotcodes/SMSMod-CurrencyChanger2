using HarmonyLib;
using TMPro;

namespace CurrencyChanger2.Patches
{
    [HarmonyPatch(typeof(FurnitureSalesItem), "Setup")]
    public static class FurnitureSalesItem_Setup_Patch
    {
        public static void Postfix(FurnitureSalesItem __instance)
        {
            __instance.transform.GetChild(2).GetChild(4).GetComponent<TextMeshProUGUI>().enableWordWrapping = false;
            __instance.transform.GetChild(2).GetChild(4).gameObject.SetActive(false);
            __instance.transform.GetChild(2).GetChild(4).gameObject.SetActive(true);
        }
    }
}
