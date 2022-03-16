using System.ComponentModel;

namespace BusinessObjectLibrary
{
    public enum Status
    {
        [Description("Available")]
        AVAILABLE = 1,
        [Description("Out of stock")]
        OUT_OF_STOCK = 2,
        [Description("Almost out of stock")]
        ALMOST_OUT_OF_STOCK = 3,
        [Description("Best Seller")]
        BEST_SELLER = 4
    }
}
