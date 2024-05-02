using BepInEx.Configuration;
using System.IO;
using CoinTextureType = CurrencyChanger2.Plugin.CoinTextureType;
using BillTextureType = CurrencyChanger2.Plugin.BillTextureType;
using System.Collections.Generic;

namespace CurrencyChanger2.ConfigMigration
{
    public partial class V1ConfigResult
    {
        public ConfigEntry<string> CurrencyPrefix;
        public ConfigEntry<string> CurrencySuffix;
        public ConfigEntry<string> CurrencyDecimalSeperator;

        public ConfigEntry<float> CoinValue1ct;
        public ConfigEntry<float> CoinValue5ct;
        public ConfigEntry<float> CoinValue10ct;
        public ConfigEntry<float> CoinValue25ct;
        public ConfigEntry<float> CoinValue50ct;

        public ConfigEntry<float> BillValue1d;
        public ConfigEntry<float> BillValue5d;
        public ConfigEntry<float> BillValue10d;
        public ConfigEntry<float> BillValue20d;
        public ConfigEntry<float> BillValue50d;

        public ConfigEntry<float> LowestBill;
        public ConfigEntry<string> TerminalSymbol;

        public ConfigEntry<string> ValueText1ct;
        public ConfigEntry<string> ValueText5ct;
        public ConfigEntry<string> ValueText10ct;
        public ConfigEntry<string> ValueText25ct;
        public ConfigEntry<string> ValueText50ct;

        public ConfigEntry<string> ValueText1d;
        public ConfigEntry<string> ValueText5d;
        public ConfigEntry<string> ValueText10d;
        public ConfigEntry<string> ValueText20d;
        public ConfigEntry<string> ValueText50d;

        public ConfigEntry<TextureType> Texture1ct;
        public ConfigEntry<TextureType> Texture5ct;
        public ConfigEntry<TextureType> Texture10ct;
        public ConfigEntry<TextureType> Texture25ct;
        public ConfigEntry<TextureType> Texture50ct;

        public ConfigEntry<TextureType> Texture1d;
        public ConfigEntry<TextureType> Texture5d;
        public ConfigEntry<TextureType> Texture10d;
        public ConfigEntry<TextureType> Texture20d;
        public ConfigEntry<TextureType> Texture50d;
        public bool ConfigWasV1 { get; set; }
        public void RemoveV1ConfigEntries(ConfigFile Config)
        {
            new List<ConfigEntryBase>()
            {
                CurrencyPrefix, CurrencySuffix, CurrencyDecimalSeperator, 
                CoinValue1ct, CoinValue5ct, CoinValue10ct, CoinValue25ct, CoinValue50ct, 
                BillValue1d, BillValue5d, BillValue10d, BillValue20d, BillValue50d, 
                LowestBill, TerminalSymbol,
                ValueText1ct, ValueText5ct, ValueText10ct, ValueText25ct, ValueText50ct,
                ValueText1d, ValueText5d, ValueText10d, ValueText20d, ValueText50d,
                Texture1ct, Texture5ct, Texture10ct, Texture25ct, Texture50ct,
                Texture1d, Texture5d, Texture10d, Texture20d, Texture50d
            }.ForEach(entry =>
            {
                Config.Remove(entry.Definition);
            });
            Config.Save();
        }
        public V1ConfigResult(bool success)
        {
            ConfigWasV1 = success;
        }
        public static V1ConfigResult Check(ConfigFile Config)
        {
            var v1DecimalSeperator = Config.Bind<string>("Money Text", "Decimal Seperator", null);

            if (v1DecimalSeperator.Value == null)
            {
                Config.Remove(v1DecimalSeperator.Definition);
                return new V1ConfigResult(false);
            }
            else
            {
                V1ConfigResult result = new V1ConfigResult(true);
                result.Parse(Config);
                return result;
            }
        }
        public void Parse(ConfigFile Config)
        {
            CurrencyPrefix = Config.Bind("Money Text", "Prefix", "$", "The currency symbol (or arbitrary string) to use in front of the value, where the dollar sign would normally be.");
            CurrencySuffix = Config.Bind("Money Text", "Suffix", "", "The currency symbol (or arbitrary string) to use after the value, where a euro sign might for example be.");
            CurrencyDecimalSeperator = Config.Bind("Money Text", "Decimal Seperator", ".", "What symbol to use to seperate the whole number part from the fractional part.");

            CoinValue1ct = Config.Bind("Cash Values", "1 Cent Coin", 0.01f, "What cash value the 1 cent coin should represent.");
            CoinValue5ct = Config.Bind("Cash Values", "5 Cent Coin", 0.05f, "What cash value the 5 cent coin should represent.");
            CoinValue10ct = Config.Bind("Cash Values", "10 Cent Coin", 0.1f, "What cash value the 10 cent coin should represent.");
            CoinValue25ct = Config.Bind("Cash Values", "25 Cent Coin", 0.25f, "What cash value the 25 cent coin should represent.");
            CoinValue50ct = Config.Bind("Cash Values", "50 Cent Coin", 0.5f, "What cash value the 50 cent coin should represent.");

            BillValue1d = Config.Bind("Cash Values", "1 Dollar Bill", 1f, "What cash value the 1 dollar bill should represent.");
            BillValue5d = Config.Bind("Cash Values", "5 Dollar Bill", 5f, "What cash value the 5 dollar bill should represent.");
            BillValue10d = Config.Bind("Cash Values", "10 Dollar Bill", 10f, "What cash value the 10 dollar bill should represent.");
            BillValue20d = Config.Bind("Cash Values", "20 Dollar Bill", 20f, "What cash value the 20 dollar bill should represent.");
            BillValue50d = Config.Bind("Cash Values", "50 Dollar Bill", 50f, "What cash value the 50 dollar bill should represent.");

            ValueText1ct = Config.Bind("Value Texts", "1 Cent Coin", "1¢", "What value text to display below the 1 cent coin.");
            ValueText5ct = Config.Bind("Value Texts", "5 Cent Coin", "5¢", "What value text to display below the 5 cent coin.");
            ValueText10ct = Config.Bind("Value Texts", "10 Cent Coin", "10¢", "What value text to display below the 10 cent coin.");
            ValueText25ct = Config.Bind("Value Texts", "25 Cent Coin", "25¢", "What value text to display below the 25 cent coin.");
            ValueText50ct = Config.Bind("Value Texts", "50 Cent Coin", "50¢", "What value text to display below the 50 cent coin.");

            ValueText1d = Config.Bind("Value Texts", "1 Dollar Bill", "$1", "What value text to display above the 1 dollar bill.");
            ValueText5d = Config.Bind("Value Texts", "5 Dollar Bill", "$5", "What value text to display above the 5 dollar bill.");
            ValueText10d = Config.Bind("Value Texts", "10 Dollar Bill", "$10", "What value text to display above the 10 dollar bill.");
            ValueText20d = Config.Bind("Value Texts", "20 Dollar Bill", "$20", "What value text to display above the 20 dollar bill.");
            ValueText50d = Config.Bind("Value Texts", "50 Dollar Bill", "$50", "What value text to display above the 50 dollar bill.");

            Texture1ct = Config.Bind("Texture Types", "1 Cent Coin", TextureType.USD, "What visual style this coin should have.");
            Texture5ct = Config.Bind("Texture Types", "5 Cent Coin", TextureType.USD, "What visual style this coin should have.");
            Texture10ct = Config.Bind("Texture Types", "10 Cent Coin", TextureType.USD, "What visual style this coin should have.");
            Texture25ct = Config.Bind("Texture Types", "25 Cent Coin", TextureType.USD, "What visual style this coin should have.");
            Texture50ct = Config.Bind("Texture Types", "50 Cent Coin", TextureType.USD, "What visual style this coin should have.");

            Texture1d = Config.Bind("Texture Types", "1 Dollar Bill", TextureType.USD, "What visual style this bill should have.");
            Texture5d = Config.Bind("Texture Types", "5 Dollar Bill", TextureType.USD, "What visual style this bill should have.");
            Texture10d = Config.Bind("Texture Types", "10 Dollar Bill", TextureType.USD, "What visual style this bill should have.");
            Texture20d = Config.Bind("Texture Types", "20 Dollar Bill", TextureType.USD, "What visual style this bill should have.");
            Texture50d = Config.Bind("Texture Types", "50 Dollar Bill", TextureType.USD, "What visual style this bill should have.");

            LowestBill = Config.Bind("Miscellaneous", "Lowest Bill", 1f, "The lowest value of money that should be considered a bill by the game.");
            TerminalSymbol = Config.Bind("Miscellaneous", "Terminal Symbol", "$", "What symbol to display on the credit card terminal.");
        }
        public void Apply()
        {
            Plugin.StaticLogger.LogWarning("Migrating V1 Config File...");

            Plugin.EnableAdditionalCoinCompartments.Value = false;

            Plugin.CurrencyPrefix.Value = CurrencyPrefix.Value;
            Plugin.CurrencySuffix.Value = CurrencySuffix.Value;
            Plugin.CurrencyDecimalSeperator.Value = CurrencyDecimalSeperator.Value;
            Plugin.LowestBill.Value = LowestBill.Value;
            Plugin.TerminalSymbol.Value = TerminalSymbol.Value;

            Plugin.Coin1.Value.Value = CoinValue1ct.Value;
            Plugin.Coin1.Texture.Value = ConvertCoinTextureType(MoneyType.CENT_1);
            Plugin.Coin1.Text.Value = ValueText1ct.Value;

            Plugin.Coin2.Value.Value = CoinValue5ct.Value;
            Plugin.Coin2.Texture.Value = ConvertCoinTextureType(MoneyType.CENT_5);
            Plugin.Coin2.Text.Value = ValueText5ct.Value;

            Plugin.Coin3.Value.Value = CoinValue10ct.Value;
            Plugin.Coin3.Texture.Value = ConvertCoinTextureType(MoneyType.CENT_10);
            Plugin.Coin3.Text.Value = ValueText10ct.Value;

            Plugin.Coin4.Value.Value = CoinValue25ct.Value;
            Plugin.Coin4.Texture.Value = ConvertCoinTextureType(MoneyType.CENT_25);
            Plugin.Coin4.Text.Value = ValueText25ct.Value;

            Plugin.Coin5.Value.Value = CoinValue50ct.Value;
            Plugin.Coin5.Texture.Value = ConvertCoinTextureType(MoneyType.CENT_50);
            Plugin.Coin5.Text.Value = ValueText50ct.Value;

            Plugin.Bill1.Value.Value = BillValue1d.Value;
            Plugin.Bill1.Texture.Value = ConvertBillTextureType(MoneyType.DOLLAR_1);
            Plugin.Bill1.Text.Value = ValueText1d.Value;

            Plugin.Bill2.Value.Value = BillValue5d.Value;
            Plugin.Bill2.Texture.Value = ConvertBillTextureType(MoneyType.DOLLAR_5);
            Plugin.Bill2.Text.Value = ValueText5d.Value;

            Plugin.Bill3.Value.Value = BillValue10d.Value;
            Plugin.Bill3.Texture.Value = ConvertBillTextureType(MoneyType.DOLLAR_10);
            Plugin.Bill3.Text.Value = ValueText10d.Value;

            Plugin.Bill4.Value.Value = BillValue20d.Value;
            Plugin.Bill4.Texture.Value = ConvertBillTextureType(MoneyType.DOLLAR_20);
            Plugin.Bill4.Text.Value = ValueText20d.Value;

            Plugin.Bill5.Value.Value = BillValue50d.Value;
            Plugin.Bill5.Texture.Value = ConvertBillTextureType(MoneyType.DOLLAR_50);
            Plugin.Bill5.Text.Value = ValueText50d.Value;
        }
        public CoinTextureType ConvertCoinTextureType(MoneyType moneyType)
        {
            switch (moneyType)
            {
                default:
                case MoneyType.CENT_1:
                    switch (Texture1ct.Value)
                    {
                        case TextureType.EUR:
                            return CoinTextureType.EUR_1ct_COIN;
                        case TextureType.GBP:
                            return CoinTextureType.GBP_1ct_COIN;
                        case TextureType.SGD:
                            return CoinTextureType.SGD_1ct_COIN;
                        case TextureType.CAD:
                            return CoinTextureType.CAD_1ct_COIN;
                        case TextureType.USD:
                        default:
                            return CoinTextureType.USD_1ct_COIN;
                    }
                case MoneyType.CENT_5:
                    switch (Texture5ct.Value)
                    {
                        case TextureType.EUR:
                            return CoinTextureType.EUR_5ct_COIN;
                        case TextureType.GBP:
                            return CoinTextureType.GBP_5ct_COIN;
                        case TextureType.SGD:
                            return CoinTextureType.SGD_5ct_COIN;
                        case TextureType.CAD:
                            return CoinTextureType.CAD_5ct_COIN;
                        case TextureType.USD:
                        default:
                            return CoinTextureType.USD_5ct_COIN;
                    }
                case MoneyType.CENT_10:
                    switch (Texture10ct.Value)
                    {
                        case TextureType.EUR:
                            return CoinTextureType.EUR_10ct_COIN;
                        case TextureType.GBP:
                            return CoinTextureType.GBP_10ct_COIN;
                        case TextureType.SGD:
                            return CoinTextureType.SGD_20ct_COIN;
                        case TextureType.CAD:
                            return CoinTextureType.CAD_10ct_COIN;
                        case TextureType.USD:
                        default:
                            return CoinTextureType.USD_10ct_COIN;
                    }
                case MoneyType.CENT_25:
                    switch (Texture25ct.Value)
                    {
                        case TextureType.EUR:
                            return CoinTextureType.EUR_20ct_COIN;
                        case TextureType.GBP:
                            return CoinTextureType.GBP_20ct_COIN;
                        case TextureType.SGD:
                            return CoinTextureType.SGD_50ct_COIN;
                        case TextureType.CAD:
                            return CoinTextureType.CAD_25ct_COIN;
                        case TextureType.USD:
                        default:
                            return CoinTextureType.USD_25ct_COIN;
                    }
                case MoneyType.CENT_50:
                    switch (Texture50ct.Value)
                    {
                        case TextureType.EUR:
                            return CoinTextureType.EUR_50ct_COIN;
                        case TextureType.GBP:
                            return CoinTextureType.GBP_50ct_COIN;
                        case TextureType.SGD:
                            return CoinTextureType.SGD_1_COIN;
                        case TextureType.CAD:
                            return CoinTextureType.CAD_50ct_COIN;
                        case TextureType.USD:
                        default:
                            return CoinTextureType.USD_50ct_COIN;
                    }
            }
        }
        public BillTextureType ConvertBillTextureType(MoneyType moneyType)
        {
            switch (moneyType)
            {
                default:
                case MoneyType.DOLLAR_1:
                    switch (Texture1d.Value)
                    {
                        case TextureType.EUR:
                            return BillTextureType.EUR_1_BILL;
                        case TextureType.GBP:
                            return BillTextureType.GBP_1_BILL;
                        case TextureType.SGD:
                            return BillTextureType.SGD_2_BILL;
                        case TextureType.CAD:
                            return BillTextureType.CAD_1_BILL;
                        case TextureType.USD:
                        default:
                            return BillTextureType.USD_1_BILL;
                    }
                case MoneyType.DOLLAR_5:
                    switch (Texture5d.Value)
                    {
                        case TextureType.EUR:
                            return BillTextureType.EUR_5_BILL;
                        case TextureType.GBP:
                            return BillTextureType.GBP_5_BILL;
                        case TextureType.SGD:
                            return BillTextureType.SGD_5_BILL;
                        case TextureType.CAD:
                            return BillTextureType.CAD_5_BILL;
                        case TextureType.USD:
                        default:
                            return BillTextureType.USD_5_BILL;
                    }
                case MoneyType.DOLLAR_10:
                    switch (Texture10d.Value)
                    {
                        case TextureType.EUR:
                            return BillTextureType.EUR_10_BILL;
                        case TextureType.GBP:
                            return BillTextureType.GBP_10_BILL;
                        case TextureType.SGD:
                            return BillTextureType.SGD_10_BILL;
                        case TextureType.CAD:
                            return BillTextureType.CAD_10_BILL;
                        case TextureType.USD:
                        default:
                            return BillTextureType.USD_10_BILL;
                    }
                case MoneyType.DOLLAR_20:
                    switch (Texture20d.Value)
                    {
                        case TextureType.EUR:
                            return BillTextureType.EUR_20_BILL;
                        case TextureType.GBP:
                            return BillTextureType.GBP_20_BILL;
                        case TextureType.SGD:
                            return BillTextureType.SGD_20_BILL;
                        case TextureType.CAD:
                            return BillTextureType.CAD_20_BILL;
                        case TextureType.USD:
                        default:
                            return BillTextureType.USD_20_BILL;
                    }
                case MoneyType.DOLLAR_50:
                    switch (Texture50d.Value)
                    {
                        case TextureType.EUR:
                            return BillTextureType.EUR_50_BILL;
                        case TextureType.GBP:
                            return BillTextureType.GBP_50_BILL;
                        case TextureType.SGD:
                            return BillTextureType.SGD_50_BILL;
                        case TextureType.CAD:
                            return BillTextureType.CAD_50_BILL;
                        case TextureType.USD:
                        default:
                            return BillTextureType.USD_50_BILL;
                    }
            }
        }
    }
}
