using BACKBONE.Application.Interfaces;
using BACKBONE.Core.Dtos;
using BACKBONE.Core.Models;
using BACKBONE.Core.ResponseClasses;
using BACKBONE.DB;
using Dapper;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BACKBONE.Core.ApplicationConnectionString.ApplicationConnectionString;


namespace BACKBONE.Infrastructure
{
    public class InvoiceRepository : IInvoiceRepository
    {
        public async Task<EQResponse<PJ_INVOICE>> InsertInvoiceDataAsync(PJ_INVOICE invoiceData)
        {
            var response = new EQResponse<PJ_INVOICE>();

            try
            {
                var connectionString = GetConnectionString(1);
                IDBHelper _db = new OracleDbHelper(connectionString);

                // Prepare parameters including OUTPUT
                var parameters = new DynamicParameters(); // Assuming you're using Dapper-based IDBHelper

                parameters.Add("p_invoice_number", invoiceData.INVOICE_NUMBER);
                parameters.Add("p_total_amount", invoiceData.TOTAL_AMOUNT);
                parameters.Add("p_bu_id", invoiceData.BU_ID);
                parameters.Add("p_cdu", invoiceData.CDU);
                parameters.Add("p_image_path", invoiceData.IMAGE_PATH);


                // Output parameter
                parameters.Add("p_invoice_id", dbType: DbType.Int32, direction: ParameterDirection.Output);

                // Execute the stored procedure
                await _db.ExecuteAsync("SP_INSERT_INVOICE_DATA", parameters, commandType: CommandType.StoredProcedure);

                int newId = parameters.Get<int>("p_invoice_id");

                response.Success = true;
                //response.Message = $"Invoice Save Successful. - {invoiceData.IMAGE_PATH}";
                response.Message = $"{invoiceData.IMAGE_PATH}";

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating invoice data: {ex.Message}");

                string userMessage = $"Error creating invoice data: {ex.Message}";

                response.Success = false;
                response.Message = userMessage;
            }

            return response;
        }

        public async Task<EQResponse<PJ_INVOICE>> InsertInvoiceByBarcodeDataAsync(PJ_INVOICE invoiceData)
        {
            var response = new EQResponse<PJ_INVOICE>();

            try
            {
                var connectionString = GetConnectionString(1);
                IDBHelper _db = new OracleDbHelper(connectionString);

                // Insert master
                var parameters = new DynamicParameters();
                parameters.Add("p_invoice_number", invoiceData.INVOICE_NUMBER);
                parameters.Add("p_total_amount", invoiceData.TOTAL_AMOUNT);
                parameters.Add("p_total_point", invoiceData.TOTAL_POINT);
                parameters.Add("p_bu_id", invoiceData.BU_ID);
                parameters.Add("p_cdu", invoiceData.CDU);
                parameters.Add("p_invoice_id", dbType: DbType.Int32, direction: ParameterDirection.Output);

                await _db.ExecuteAsync("SP_INSERT_INVOICE_MASTER_DATA", parameters, commandType: CommandType.StoredProcedure);

                int invoiceId = parameters.Get<int>("p_invoice_id");
                invoiceData.INVOICE_ID = invoiceId.ToString();

                // Insert details
                foreach (var detail in invoiceData.INVOICE_DETAILS)
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

                    await _db.ExecuteAsync("SP_INSERT_INVOICE_DETAIL_DATA", detailParams, commandType: CommandType.StoredProcedure);
                }

                response.Success = true;
                response.Message = "Invoice Save Successful.";
                response.Data = new EQResponseData<PJ_INVOICE>
                {
                    SingleValue = invoiceData
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

        //public async Task<EQResponse<PJ_All_INVOICE_DATA_DTO>> GetAllInvoiceDataAsync(PJ_INVOICE invoiceData)
        //{
        //    var response = new EQResponse<PJ_All_INVOICE_DATA_DTO>
        //    {
        //        Data = new EQResponseData<PJ_All_INVOICE_DATA_DTO>()
        //    };

        //    try
        //    {
        //        var connectionString = GetConnectionString(1);
        //        IDBHelper _db = new OracleDbHelper(connectionString);

        //        // 1️⃣ Get invoice headers
        //        var headerParams = new DynamicParameters();
        //        headerParams.Add("p_cdu", invoiceData.CDU, DbType.String, ParameterDirection.Input);
        //        headerParams.Add("p_fst_apprv_status", invoiceData.FST_APPRV_STATUS, DbType.String, ParameterDirection.Input);
        //        headerParams.Add("p_bu_id", invoiceData.BU_ID, DbType.String, ParameterDirection.Input);
        //        headerParams.Add("o_cursor", dbType: DbType.Object, direction: ParameterDirection.Output);

        //        var invoices = (await _db.QueryAsync<PJ_All_INVOICE_DATA_DTO>(
        //            "SP_GET_ALL_INVOICE",
        //            headerParams,
        //            commandType: CommandType.StoredProcedure
        //        )).ToList();

        //        // 2️⃣ Get invoice details for each invoice
        //        foreach (var inv in invoices)
        //        {
        //            var detailParams = new DynamicParameters();
        //            detailParams.Add("p_invoice_number", inv.INVOICE_NUMBER, DbType.String, ParameterDirection.Input);
        //            detailParams.Add("o_cursor", dbType: DbType.Object, direction: ParameterDirection.Output);

        //            var items = (await _db.QueryAsync<PJ_INVOICE_ITEM_DTO>(
        //                "SP_GET_INVOICE_DETAILS",
        //                detailParams,
        //                commandType: CommandType.StoredProcedure
        //            )).ToList();

        //            inv.ITEMS = items;
        //        }

        //        // 3️⃣ Prepare response
        //        response.Success = true;
        //        response.Message = "Invoice data retrieved successfully.";
        //        response.Data.ListValue = invoices;
        //    }
        //    catch (Exception ex)
        //    {
        //        response.Success = false;
        //        response.Message = $"Error retrieving invoice data: {ex.Message}";
        //        response.Data = null;
        //    }

        //    return response;
        //}

        public async Task<EQResponse<PJ_All_INVOICE_DATA_DTO>> GetAllInvoiceDataAsync(PJ_INVOICE invoiceData)
        {
            var response = new EQResponse<PJ_All_INVOICE_DATA_DTO>
            {
                Data = new EQResponseData<PJ_All_INVOICE_DATA_DTO>()
            };

            try
            {
                var connectionString = GetConnectionString(1);
                IDBHelper _db = new OracleDbHelper(connectionString);

                var parameters = new
                {
                    p_user_id = invoiceData.CDU,
                    p_fst_apprv_status = invoiceData.FST_APPRV_STATUS,
                    p_fnl_apprv_status = invoiceData.FNL_APPRV_STATUS,
                    p_user_type_id = invoiceData.USER_TYPE_ID,
                    p_bu_id = invoiceData.BU_ID
                };

                //  Execute SP to get all invoice headers
                var result = _db.ExecuteStoredProcedureWithRefCursor("SP_GET_ALL_INVOICE", "o_cursor", parameters);

                var invoiceList = new List<PJ_All_INVOICE_DATA_DTO>();
                foreach (var row in result)
                {
                    var invoice = new PJ_All_INVOICE_DATA_DTO
                    {
                        INVOICE_NUMBER = row["INVOICE_NUMBER"]?.ToString() ?? string.Empty,
                        TOTAL_AMOUNT = row["TOTAL_AMOUNT"]?.ToString() ?? string.Empty,
                        IMAGE_PATH = row["IMAGE_PATH"]?.ToString() ?? string.Empty,
                        BU_ID = row["BU_ID"]?.ToString() ?? string.Empty,
                        BASE_NAME = row["BASE_NAME"]?.ToString() ?? string.Empty,
                        ZONE_NAME = row["ZONE_NAME"]?.ToString() ?? string.Empty,
                        FST_APPRV_STATUS = row["FST_APPRV_STATUS"]?.ToString() ?? string.Empty,
                        FNL_APPRV_STATUS = row["FNL_APPRV_STATUS"]?.ToString() ?? string.Empty,
                        DAYS_AGO = row["DAYS_AGO"]?.ToString() ?? string.Empty,
                        CDT = row["CDT"] != null ? (DateTime?)Convert.ToDateTime(row["CDT"]) : null,
                        ITEMS = new List<PJ_INVOICE_ITEM_DTO>()
                    };

                    //  Execute SP to get invoice items
                    var itemParams = new { p_invoice_number = invoice.INVOICE_NUMBER };
                    var itemResult = _db.ExecuteStoredProcedureWithRefCursor("SP_GET_INVOICE_DETAILS", "o_cursor", itemParams);

                    foreach (var itemRow in itemResult)
                    {
                        invoice.ITEMS.Add(new PJ_INVOICE_ITEM_DTO
                        {
                            INVOICE_NUMBER = itemRow["INVOICE_NUMBER"]?.ToString() ?? string.Empty,
                            ITEM_NAME = itemRow["ITEM_NAME"]?.ToString() ?? string.Empty,
                            QTY = itemRow["QTY"]?.ToString() ?? string.Empty,
                            PRICE = itemRow["PRICE"]?.ToString() ?? string.Empty,
                            POINT = itemRow["POINT"]?.ToString() ?? string.Empty,
                            BARCODE = itemRow["BARCODE"]?.ToString() ?? string.Empty,
                            UNITNAME = itemRow["UNITNAME"]?.ToString() ?? string.Empty
                        });
                    }

                    invoiceList.Add(invoice);
                }

                // 3️⃣ Return response
                response.Success = true;
                response.Message = "Invoice data retrieved successfully.";
                response.Data.ListValue = invoiceList;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error retrieving invoice data: {ex.Message}";
                response.Data = null;
            }

            return response;
        }






    }
}
