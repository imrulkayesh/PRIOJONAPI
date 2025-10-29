using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACKBONE.Core.Models
{
    public class PJ_INVOICE_DETAILS
    {
        public string INVOICE_NUMBER { get; set; }
        public string ITEM_CODE { get; set; }
        public int QTY { get; set; }
        public int PRICE { get; set; }
        public int POINT { get; set; }
        public string BARCODE { get; set; }
        public DateTime CDT { get; set; }
        public string CDU { get; set; }
        public int ACT { get; set; }
        public string UNIT_CODE { get; set; }
    }
}
