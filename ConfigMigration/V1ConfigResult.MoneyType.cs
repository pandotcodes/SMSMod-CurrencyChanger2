namespace CurrencyChanger2.ConfigMigration
{
    public partial class V1ConfigResult
    {
        public enum MoneyType
        {
            CENT_1 = 0b0001, CENT_5 = 0b0010, CENT_10 = 0b0011, CENT_25 = 0b0100, CENT_50 = 0b0101,
            DOLLAR_1 = 0b1001, DOLLAR_5 = 0b1010, DOLLAR_10 = 0b1011, DOLLAR_20 = 0b1100, DOLLAR_50 = 0b1101
        }
    }
}
