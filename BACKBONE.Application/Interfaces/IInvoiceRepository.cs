using BACKBONE.Core.Models;
using BACKBONE.Core.ResponseClasses;
using BACKBONE.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACKBONE.Application.Interfaces
{
    public interface IInvoiceRepository
    {
        Task<EQResponse<PJ_INVOICE>> InsertInvoiceDataAsync(PJ_INVOICE invoiceData);
        Task<EQResponse<PJ_INVOICE>> InsertInvoiceByBarcodeDataAsync(PJ_INVOICE invoiceData);
        Task<EQResponse<PJ_All_INVOICE_DATA_DTO>> GetAllInvoiceDataAsync(PJ_INVOICE invoiceData);
    }
}
