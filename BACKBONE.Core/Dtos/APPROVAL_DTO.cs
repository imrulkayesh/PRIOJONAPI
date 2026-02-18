using BACKBONE.Core.Dtos.BACKBONE.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACKBONE.Core.Dtos
{
    public class APPROVAL_DTO
    {
        public string ID { get; set; }
        public string APPROVAL_TYPE { get; set; }
        public string APPROVAL_STATUS { get; set; }
        public string USER_TYPE { get; set; }
        public string APPROVE_REMARK { get; set; }
        public string APPROVE_BY { get; set; }
        public List<PJ_INVOICE_DETAILS_DTO> INVOICE_DETAILS { get; set; }
    }
}
