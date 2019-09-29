using XeroTechnicalTest.Domain.Constants;

namespace XeroTechnicalTest.Domain.Models
{
    public abstract class Product : BaseModel
    {
        public string Code => $"{ProductType.ToString().ToUpper()}-{Name.ToUpper()}";

        // cost in cents, rather than a decimal dollars
        // including tax
        public long CostInc { get; set; }
        // excluding tax
        public long CostEx { get; set; }
        public string Name { get; set; }
        public abstract ProductType ProductType { get; set; }
    }
}