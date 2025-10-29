using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACKBONE.Core.Models
{
    public class PJ_INVOICE
    {
        public string INVOICE_ID { get; set; }
        public string INVOICE_NUMBER { get; set; }
        public int TOTAL_AMOUNT { get; set; }
        public string IMAGE_PATH { get; set; }  
        public int TOTAL_POINT { get;set; }
        public string FST_APPRV_STATUS { get; set; }
        public string FST_APPRV_ID { get; set; }
        public string FST_APPRV_DATE { get; set; }
        public string FST_APPRV_REMARK { get; set; }
        public string FNL_APPRV_STATUS { get; set; }
        public string FNL_APPRV_ID { get; set; }
        public string FNL_APPRV_DATE { get; set; }
        public string FNL_APPRV_REMARK { get; set; }
        public DateTime? CDT { get; set; }
        public DateTime? UDT { get; set; }
        public string CDU { get; set; }
        public string UDU { get; set; }
        public int ACT { get; set; }
        public string BU_ID { get; set; }

        public string USER_TYPE_ID { get; set; }

        // This property to hold the invoice detail items
        public List<PJ_INVOICE_DETAILS> INVOICE_DETAILS { get; set; } = new List<PJ_INVOICE_DETAILS>();

    }
}
