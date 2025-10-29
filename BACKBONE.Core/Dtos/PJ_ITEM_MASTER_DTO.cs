using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACKBONE.Core.Dtos
{
    public class PJ_ITEM_MASTER_DTO
    {
        public string ITEM_CODE { get; set; }
        public string ITEM_NAME { get; set; }
        public string BARCODE { get; set; }
        public int PRICE { get; set; }
        public int POINT { get; set; }
        public int SCAN_COUNT { get; set; }
        public string BU_ID { get; set; }
        public string UNIT_CODE { get; set; }
    }
}
