using BACKBONE.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACKBONE.Core.Dtos
{
    namespace BACKBONE.Core.Dtos
    {
        public class PJ_INVOICE_BARCODE_DTO
        {
            public string INVOICE_NUMBER { get; set; }
            public int TOTAL_AMOUNT { get; set; }
            public int TOTAL_POINT { get; set; }
            public string BU_ID { get; set; }
            public string CDU { get; set; }

            public List<PJ_INVOICE_DETAILS_DTO> INVOICE_DETAILS { get; set; }
        }

        public class PJ_INVOICE_DETAILS_DTO
        {
            public string INVOICE_NUMBER { get; set; }
            public string ITEM_CODE { get; set; }
            public string BARCODE { get; set; }
            public string UNIT_CODE { get; set; }
            public int QTY { get; set; }
            public int PRICE { get; set; }
            public int POINT { get; set; }
            public string CDU { get; set; }
        }
    }

}
