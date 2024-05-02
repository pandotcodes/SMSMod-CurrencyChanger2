using HarmonyLib;
using UnityEngine;

namespace CurrencyChanger2.Patches
{
    [HarmonyPatch(typeof(MoneyGenerator), "SpawnCreditCard")]
    public static class MoneyGenerator_SpawnCreditCard_Patch
    {
        public static void Prefix(MoneyGenerator __instance)
        {
            var card = __instance.m_CustomerCreditCard;
            var tex = Plugin.GetCreditCardTextures();

            if (Plugin.CreditCardFront.Value != Plugin.CardTextureType.DEFAULT) card.transform.GetChild(0).GetComponent<MeshRenderer>().material.mainTexture = tex.front;
            if (Plugin.CreditCardBack.Value != Plugin.CardTextureType.DEFAULT) card.transform.GetChild(1).GetComponent<MeshRenderer>().material.mainTexture = tex.back;
        }
    }
}
