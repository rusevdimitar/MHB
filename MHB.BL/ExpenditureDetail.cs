using System.Linq;

namespace MHB.BL
{
    public class ExpenditureDetail : ExpenditureDetailBase
    {
        private readonly ExpenditureManager _expenditureManager;

        public ExpenditureDetail()
        {
        }

        public ExpenditureDetail(ExpenditureManager expenditureManager)
        {
            this._expenditureManager = expenditureManager;
        }

        public int Add()
        {
            int result = -1;

            result = this._expenditureManager.AddNewChildExpense(this);

            return result;
        }

        public decimal UnitPrice
        {
            get
            {
                return this.GetUnitPrice();
            }
        }

        private bool _forceUpdate = false;

        public bool ForceUpdate
        {
            get
            {
                return this._forceUpdate;
            }
            set
            {
                this._forceUpdate = value;
            }
        }

        private bool _isSurplus = false;

        public bool IsSurplus
        {
            get
            {
                return this._isSurplus;
            }
            set
            {
                this._isSurplus = value;
            }
        }

        public bool IsOcrScanned { get; set; }

        public decimal AverageConsumption
        {
            get
            {
                decimal averageConsumption = 0M;

                if (this.Product != null)
                {
                    ProductParameter milageParameter = this.Product.Parameters.FirstOrDefault(p => p.ProductParameterTypeID == ProductParameterType.Mileage);

                    decimal milage = 0M;

                    if (milageParameter != null && decimal.TryParse(milageParameter.Value, out milage))
                    {
                        if (milage > 0)
                            averageConsumption = (this.Amount / milage) * 100;
                    }
                }
                return averageConsumption;
            }
        }

        private decimal GetUnitPrice()
        {
            decimal unitPrice = 0;

            switch (this.MeasureType)
            {
                case Enums.MeasureType.Volume:
                case Enums.MeasureType.Weight:
                    unitPrice = this.DetailValue / this.Amount;
                    break;

                case Enums.MeasureType.Count:
                    unitPrice = this.DetailValue;
                    break;

                default:
                    unitPrice = this.DetailValue;
                    break;
            }

            return unitPrice;
        }
    }
}