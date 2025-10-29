using BACKBONE.Application.Interfaces;
using BACKBONE.Core.Dtos;
using BACKBONE.Core.Dtos.BACKBONE.Core.Dtos;
using BACKBONE.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BACKBONE.API.Controllers.v1.Invoice
{
    [Route("api/v1/data/invoice")]
    [ApiController]
    [Authorize]
    public class InvoiceController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public InvoiceController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost("invoice")]
        [Authorize]
        public async Task<IActionResult> InsertInvoiceData([FromForm] PJ_INVOICE_DTO invoiceData)
        {
            string? imagePath = null;
            string? fileName = null;


            if (invoiceData.INVOICEIMAGE != null && invoiceData.INVOICEIMAGE.Length > 0)
            {
                // Generate unique file name
                 fileName = $"{Guid.NewGuid()}{Path.GetExtension(invoiceData.INVOICEIMAGE.FileName)}";

                // Folder to save images
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "InvoiceImages");

                // Create folder if not exists
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                // Full path
                var fullPath = Path.Combine(folderPath, fileName);

                // Save file
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await invoiceData.INVOICEIMAGE.CopyToAsync(stream);
                }

                // Save relative path to DB
                imagePath = Path.Combine("InvoiceImages", fileName);
            }

            var invoice = new PJ_INVOICE
            {
                INVOICE_NUMBER = invoiceData.INVOICE_NUMBER,
                TOTAL_AMOUNT = invoiceData.TOTAL_AMOUNT,
                BU_ID = invoiceData.BU_ID,
                CDU = invoiceData.CDU,
                IMAGE_PATH = fileName,
               

            };


            var response = await _unitOfWork.InvoiceRepository.InsertInvoiceDataAsync(invoice); 

            if (response.Success == true)
            {
                Console.WriteLine($"Successfully created invoice data with ID: {response.Data?.SingleValue?.INVOICE_ID}");
                return Ok(response);
            }
            Console.WriteLine($"Failed to create invoice data: {response.Message}");
            return BadRequest(response);

        }

        [HttpPost("invoice-barcode")]
        [Authorize]
        public async Task<IActionResult> InsertInvoiceByBarcodeData([FromBody] PJ_INVOICE_BARCODE_DTO invoiceData)
        {
            try
            {
                var invoice = new PJ_INVOICE
                {
                    INVOICE_NUMBER = invoiceData.INVOICE_NUMBER,
                    TOTAL_AMOUNT = invoiceData.TOTAL_AMOUNT,
                    TOTAL_POINT = invoiceData.TOTAL_POINT,
                    BU_ID = invoiceData.BU_ID,
                    CDU = invoiceData.CDU
                };

                foreach (var detailDto in invoiceData.INVOICE_DETAILS)
                {
                    var detail = new PJ_INVOICE_DETAILS
                    {
                        INVOICE_NUMBER = detailDto.INVOICE_NUMBER,
                        ITEM_CODE = detailDto.ITEM_CODE,
                        BARCODE = detailDto.BARCODE,
                        QTY = detailDto.QTY,
                        PRICE = detailDto.PRICE,
                        UNIT_CODE = detailDto.UNIT_CODE,
                        POINT = detailDto.POINT,
                        CDU = invoiceData.CDU
                    };
                    invoice.INVOICE_DETAILS.Add(detail);
                }

                var response = await _unitOfWork.InvoiceRepository.InsertInvoiceByBarcodeDataAsync(invoice);

                if (response.Success == true)
                {
                    return Ok(new
                    {
                        message = response.Message,
                        success = true,
                        data = (object?)null
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        message = response.Message,
                        success = false,
                        data = (object?)null
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = $"Error creating invoice data: {ex.Message}",
                    success = false,
                    data = (object?)null
                });
            }
        }

        [HttpPost("all-invoices")]
        [Authorize]
        public async Task<IActionResult> GetAllInvoiceData([FromForm] PJ_All_INVOICE_DTO invoiceData)
        {
            var invoice = new PJ_INVOICE
            {
                CDU = invoiceData.USER_ID,
                FST_APPRV_STATUS = invoiceData.FIRST_APPROVAL_STSTUS,
                FNL_APPRV_STATUS = invoiceData.FINAL_APPROVAL_STSTUS,
                USER_TYPE_ID = invoiceData.USER_TYPE_ID,
                BU_ID = invoiceData.BU_ID
            };

            var response = await _unitOfWork.InvoiceRepository.GetAllInvoiceDataAsync(invoice);

            if (response.Success == true)
            {
                var formattedResponse = new
                {
                    message = response.Message,
                    success = response.Success,
                    data = new
                    {
                        singleValue = response.Data.SingleValue,
                        listValue = new List<object> { response.Data.ListValue },
                        dataTableValue = response.Data.DataTableValue
                    }
                };
                return Ok(formattedResponse);
            }

            return BadRequest(response);
        }






    }
}
