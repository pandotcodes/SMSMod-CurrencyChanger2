using BepInEx.Configuration;
using System;
using System.IO;
using System.Reflection;
using TMPro;
using UnityEngine;

namespace CurrencyChanger2
{
    public partial class Plugin
    {
        public class ConfigComp<TTexture> where TTexture : Enum
        {
            public ConfigEntry<float> Value { get; set; }
            public ConfigEntry<string> Text { get; set; }
            public ConfigEntry<TTexture> Texture { get; set; }
            public string type;
            public int number;
            public string sectionName => "Register - " + type + " Slot " + number;

            public ConfigComp(ConfigFile Config, string type, int number, float value, string text, TTexture texture)
            {
                this.type = type;
                this.number = number;

                Value = Config.Bind(sectionName, "Value", value, "What value this slot's " + type.ToLower() + "s should have.");
                Text = Config.Bind(sectionName, "Text", text, "What text should be shown above this slot.");
                Texture = Config.Bind(sectionName, "Texture Type", texture, "What type of texture should be applied to this slot's " + type.ToLower() + "s. If you choose \"CUSTOM\", the texture file will be loaded from BepInEx/config/ChangeCurrency/" + type + number + ".png");
            }
            public ConfigComp<TTexture> Apply(TextMeshProUGUI text)
            {
                text.text = Text.Value;
                text.enableWordWrapping = false;
                return this;
            }
            public ConfigComp<TTexture> Apply(MoneyPack pack)
            {
                pack.Value = Value.Value;
                var texture = GetTexture();
                if (texture == null) return this;
                Plugin.StaticLogger.LogWarning("Applying " + Enum.GetName(typeof(TTexture), Texture.Value) + " texture to " + pack.gameObject.name + " (MoneyPack)");
                foreach (var item in pack.gameObject.transform.GetChild(0).GetChild(0).GetComponentsInChildren<MeshRenderer>())
                {
                    foreach (var item1 in item.sharedMaterials)
                    {
                        item1.mainTexture = texture;
                    }
                }
                return this;
            }
            public ConfigComp<TTexture> Apply(Money pack)
            {
                pack.Value = Value.Value;
                var texture = GetTexture();
                if (texture == null) return this;
                Plugin.StaticLogger.LogWarning("Applying " + Enum.GetName(typeof(TTexture), Texture.Value) + " texture to " + pack.gameObject.name + " (Money)");
                foreach (var item1 in pack.GetComponent<MeshRenderer>().sharedMaterials)
                {
                    item1.mainTexture = texture;
                }
                return this;
            }
            public Texture2D storedTex;
            public Texture2D GetTexture()
            {
                if (Convert.ToInt32(Texture.Value) == Convert.ToInt32(Texture.DefaultValue) && number != 6 && number != 7) return null;
                if (storedTex != null) return storedTex;
                string name = Enum.GetName(typeof(TTexture), Texture.Value);
                byte[] data;
                if (name == "CUSTOM")
                {
                    data = File.ReadAllBytes(Path.Combine("BepInEx", "config", "ChangeCurrency", type + number + ".png"));
                }
                else
                {
                    data = (byte[])Properties.Resources.ResourceManager.GetObject(name);
                }
                Texture2D tex = new Texture2D(1024, 1024);
                tex.LoadImage(data);
                return tex;
            }
        }
    }
}
