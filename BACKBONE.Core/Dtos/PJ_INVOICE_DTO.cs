using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BACKBONE.Core.Dtos
{
    public class PJ_INVOICE_DTO
    {
        public string INVOICE_NUMBER { get; set; }
        public int TOTAL_AMOUNT { get; set; }
        public string BU_ID { get; set; }
        public string CDU { get; set; }
        public IFormFile? INVOICEIMAGE { get; set; }
      
    }
    public class PJ_All_PENDING_INVOICE_DTO
    {
        public string INVOICE_NUMBER { get; set; }
        public string TOTAL_AMOUNT { get; set; }
        public string IMAGE_PATH { get; set; }
        public string BU_ID { get; set; }
        public string BASE_NAME { get; set; }
        public string ZONE_NAME { get; set; }
        public DateTime? CDT { get; set; }
        public string DAYS_AGO { get; set; }

    }
    public class PJ_All_INVOICE_DTO
    {
        public string USER_ID { get; set; }
        public string FIRST_APPROVAL_STSTUS { get; set; }
        public string FINAL_APPROVAL_STSTUS { get; set; }
        public string USER_TYPE_ID { get; set; }

        public string BU_ID { get; set; }

    }
    public class PJ_All_INVOICE_DATA_DTO
    {
        public string INVOICE_NUMBER { get; set; }
        public string TOTAL_AMOUNT { get; set; }
        public string IMAGE_PATH { get; set; }
        public string BU_ID { get; set; }
        public string BASE_NAME { get; set; }
        public string ZONE_NAME { get; set; }
        public string FST_APPRV_STATUS { get; set; }
        public string FNL_APPRV_STATUS { get; set; }
        public DateTime? CDT { get; set; }
        public string DAYS_AGO { get; set; }

        // ✅ Add this property
        public List<PJ_INVOICE_ITEM_DTO> ITEMS { get; set; } = new List<PJ_INVOICE_ITEM_DTO>();
    }

    public class PJ_INVOICE_ITEM_DTO
    {
        public string INVOICE_NUMBER { get; set; }
        public string ITEM_NAME { get; set; }
        public string QTY { get; set; }
        public string PRICE { get; set; }
        public string POINT { get; set; }
        public string BARCODE { get; set; }
        public string UNITNAME { get; set; }
    }

}
