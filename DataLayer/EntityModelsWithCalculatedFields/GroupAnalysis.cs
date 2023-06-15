using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.EntityModels
{
    public partial class GroupAnalysis
    {


        public Nullable<decimal> TotalAmount
        {
            get
            {
                var total = 0.00M;
                if (GroupAnalysisItems.Count != 0)
                    total = total + GroupAnalysisItems.Sum(x => x.AnalysisItem.Amount ?? 0);
                if (GroupAnalysisAnalyses.Count != 0)
                    total = total + GroupAnalysisAnalyses.Sum(x => x.Analysis.TotalAmountWithDiscount ?? 0);
                return total;
            }
        }

        public Nullable<decimal> TotalAmountWithDiscount
        {
            get
            {
                return TotalAmount - Discount;
            }
        }



    }
}
