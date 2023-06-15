using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.EntityModels
{
    public partial class Analysis
    {


        public Nullable<decimal> TotalAmount
        {
            get
            {
                if (AnalysisAnalysisItems.Count != 0)
                {

                    return AnalysisAnalysisItems.Sum(x => x.AnalysisItem.Amount);
                }

                else
                    return 0;
                //return TotalAmount;
            }
            //set
            //{
            //    if (AnalysisAnalysisItems.Count != 0)
            //    {

            //        AnalysisAnalysisItems.Sum(x => x.AnalysisItem.Amount);
            //    }
            //}
        }

        public Nullable<decimal> TotalAmountWithDiscount
        {
            get
            {
                if (AnalysisAnalysisItems.Count != 0)
                    return TotalAmount - Discount;
                else
                    return 0;
                //return TotalAmountWithDiscount;
            }
            //set
            //{
            //    if (AnalysisAnalysisItems.Count != 0)
            //    {
            //        AnalysisAnalysisItems.Sum(x => x.AnalysisItem.Amount);
            //    }
            //}
        }



    }
}
