using BACKBONE.Application.Interfaces;
using BACKBONE.Core.Dtos;
using BACKBONE.Core.Dtos.BACKBONE.Core.Dtos;
using BACKBONE.Core.Models;
using BACKBONE.Core.ResponseClasses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BACKBONE.API.Controllers.v1.Approval
{
    [Route("api/v1/data/approval")]
    [ApiController]
    [Authorize]
    public class ApprovalController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ApprovalController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost("approval")]
        [Authorize]
        //[AllowAnonymous]
        public async Task<IActionResult> Approval([FromBody] APPROVAL_DTO ApprovalData)
        {
            try
            {
                var approval = new APPROVAL_DTO
                {
                    APPROVAL_TYPE = ApprovalData.APPROVAL_TYPE,
                    APPROVAL_STATUS = ApprovalData.APPROVAL_STATUS,
                    USER_TYPE = ApprovalData.USER_TYPE,
                    APPROVE_REMARK = ApprovalData.APPROVE_REMARK,
                    APPROVE_BY = ApprovalData.APPROVE_BY,
                    ID = ApprovalData.ID,
                    INVOICE_DETAILS = new List<PJ_INVOICE_DETAILS_DTO>()
                };

                if (ApprovalData.INVOICE_DETAILS?.Count > 0)
                {
                    foreach (var d in ApprovalData.INVOICE_DETAILS)
                    {
                        approval.INVOICE_DETAILS.Add(new PJ_INVOICE_DETAILS_DTO
                        {
                            INVOICE_NUMBER = d.INVOICE_NUMBER,
                            ITEM_CODE = d.ITEM_CODE,
                            BARCODE = d.BARCODE,
                            QTY = d.QTY,
                            PRICE = d.PRICE,
                            UNIT_CODE = d.UNIT_CODE,
                            POINT = d.POINT,
                            CDU = ApprovalData.APPROVE_BY
                        });
                    }
                }

                var response = await _unitOfWork.ApprovalRepository.InsertApprovalDataAsync(approval);

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
                    message = $"Error creating : {ex.Message}",
                    success = false,
                    data = (object?)null
                });
            }
        }
    }
}
