using BACKBONE.Application.Interfaces;
using BACKBONE.Core.Dtos;
using BACKBONE.Core.Models;
using BACKBONE.Core.ResponseClasses;
using BACKBONE.DB;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BACKBONE.Core.ApplicationConnectionString.ApplicationConnectionString;

namespace BACKBONE.Infrastructure
{
    public class ApprovalRepository : IApprovalRepository
    {
        //public async Task<EQResponse<APPROVAL_DTO>> InsertApprovalDataAsync(APPROVAL_DTO ApprovalData)
        //{
        //    var response = new EQResponse<APPROVAL_DTO>();

        //    try
        //    {
        //        var connectionString = GetConnectionString(1);
        //        IDBHelper _db = new OracleDbHelper(connectionString);

        //        // Insert master
        //        var parameters = new DynamicParameters();
        //        parameters.Add("p_approval_type", ApprovalData.APPROVAL_TYPE);
        //        parameters.Add("p_approval_status", ApprovalData.APPROVAL_STATUS);
        //        parameters.Add("p_user_type", ApprovalData.USER_TYPE);
        //        parameters.Add("p_approve_remark", ApprovalData.APPROVE_REMARK);
        //        parameters.Add("p_approve_by", ApprovalData.APPROVE_BY);
        //        parameters.Add("p_id", ApprovalData.ID);
        //        parameters.Add("p_out_id", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

        //        await _db.ExecuteAsync("SP_UPDATE_APPROVAL", parameters, commandType: CommandType.StoredProcedure);

        //        //int invoiceId = parameters.Get<int>("p_invoice_id");
        //        //invoiceData.INVOICE_ID = invoiceId.ToString();



        //        response.Success = true;
        //        response.Message = "Save Successful.";
        //        response.Data = new EQResponseData<APPROVAL_DTO>
        //        {
        //            SingleValue = ApprovalData
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        response.Success = false;
        //        response.Message = $"Error creating invoice data: {ex.Message}";
        //        response.Data = null;
        //    }

        //    return response;
        //}

        public async Task<EQResponse<APPROVAL_DTO>> InsertApprovalDataAsync(APPROVAL_DTO ApprovalData)
        {
            var response = new EQResponse<APPROVAL_DTO>();

            try
            {
                var connectionString = GetConnectionString(1);
                IDBHelper _db = new OracleDbHelper(connectionString);

                var parameters = new DynamicParameters();
                parameters.Add("p_approval_type", ApprovalData.APPROVAL_TYPE);
                parameters.Add("p_approval_status", ApprovalData.APPROVAL_STATUS);
                parameters.Add("p_user_type", ApprovalData.USER_TYPE);
                parameters.Add("p_approve_remark", ApprovalData.APPROVE_REMARK);
                parameters.Add("p_approve_by", ApprovalData.APPROVE_BY);
                parameters.Add("p_id", ApprovalData.ID);
                parameters.Add("p_out_id", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

                // Execute SP
                await _db.ExecuteAsync("SP_UPDATE_APPROVAL", parameters, commandType: CommandType.StoredProcedure);

                var outId = parameters.Get<string>("p_out_id");

                // FIRST CHECK → ALREADY APPROVED OR NOT FOUND
                if (outId == "NOT_FOUND_OR_ALREADY_APPROVED")
                {
                    response.Success = false;
                    response.Message = $"Approval failed. No record found OR already approved for ID: {ApprovalData.ID}";
                    response.Data = null;
                    return response;
                }

                //  Now safe to insert invoice details
                if ((ApprovalData.APPROVAL_TYPE == "INVOICE_FIRST" ||
                     ApprovalData.APPROVAL_TYPE == "INVOICE_SECOND")
                     && ApprovalData.INVOICE_DETAILS?.Count > 0)
                {
                    foreach (var detail in ApprovalData.INVOICE_DETAILS)
                    {
                        var detailParams = new DynamicParameters();
                        detailParams.Add("p_invoice_number", detail.INVOICE_NUMBER);
                        detailParams.Add("p_item_code", detail.ITEM_CODE);
                        detailParams.Add("p_barcode", detail.BARCODE);
                        detailParams.Add("p_qty", detail.QTY);
                        detailParams.Add("p_price", detail.PRICE);
                        detailParams.Add("p_unit_code", detail.UNIT_CODE);
                        detailParams.Add("p_point", detail.POINT);
                        detailParams.Add("p_cdu", detail.CDU);

                        await _db.ExecuteAsync("SP_INSERT_INVOICE_DETAIL_DATA",
                            detailParams,
                            commandType: CommandType.StoredProcedure);
                    }
                }

                // SUCCESS RESPONSE
                response.Success = true;
                response.Message = $"Approval Successful. Updated ID: {outId}";
                response.Data = new EQResponseData<APPROVAL_DTO>
                {
                    SingleValue = ApprovalData
                };
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error creating invoice data: {ex.Message}";
                response.Data = null;
            }

            return response;
        }



    }
}
