using BACKBONE.Application.Interfaces;
using BACKBONE.Core.Models;
using BACKBONE.Core.ResponseClasses;
using BACKBONE.DB;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using BACKBONE.Core.Dtos;
using static BACKBONE.Core.ApplicationConnectionString.ApplicationConnectionString;

namespace BACKBONE.Infrastructure
{
    public class CommonRepository : ICommon
    {
        public EQResponse<List<U_USER_TYPE_DTO>> GetAllUserTypeDataAsync ()
        {
            var response = new EQResponse<List<U_USER_TYPE_DTO>>();

            try
            {
                var connectionString = GetConnectionString(1);
                IDBHelper _db = new OracleDbHelper(connectionString);

                var result = _db.ExecuteStoredProcedureWithRefCursor("SP_GET_USER_TYPE_DATA", "o_cursor");

                var DataList = new List<U_USER_TYPE_DTO>();
                foreach (var row in result)
                {
                    DataList.Add(new U_USER_TYPE_DTO
                    {
                        USER_TYPE_ID = row["USER_TYPE_ID"]?.ToString() ?? string.Empty,
                        USER_TYPE_NAME = row["USER_TYPE_NAME"]?.ToString() ?? string.Empty
            
                    });
                }

                response.Success = true;
                response.Message = "All data retrieved successfully.";
                response.Data = new EQResponseData<List<U_USER_TYPE_DTO>>
                {
                    ListValue = DataList.Count > 0 ? new List<List<U_USER_TYPE_DTO>> { DataList } : new List<List<U_USER_TYPE_DTO>>()
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving all data: {ex.Message}");
                response.Success = false;
                response.Message = $"Error retrieving data: {ex.Message}";
            }

            return response;
        }
        public EQResponse<List<S_DIVISION_DTO>> GetAllDivitionDataAsync()
        {
            var response = new EQResponse<List<S_DIVISION_DTO>>();

            try
            {
                var connectionString = GetConnectionString(1);
                IDBHelper _db = new OracleDbHelper(connectionString);

                var result = _db.ExecuteStoredProcedureWithRefCursor("SP_GET_ALL_DIVISION_DATA", "o_cursor");

                var DataList = new List<S_DIVISION_DTO>();
                foreach (var row in result)
                {
                    DataList.Add(new S_DIVISION_DTO
                    {
                        DIVISION_ID = row["DIVISION_ID"]?.ToString() ?? string.Empty,
                        DIVISION_NAME = row["DIVISION_NAME"]?.ToString() ?? string.Empty

                    });
                }

                response.Success = true;
                response.Message = "All data retrieved successfully.";
                response.Data = new EQResponseData<List<S_DIVISION_DTO>>
                {
                    ListValue = DataList.Count > 0 ? new List<List<S_DIVISION_DTO>> { DataList } : new List<List<S_DIVISION_DTO>>()
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving all data: {ex.Message}");
                response.Success = false;
                response.Message = $"Error retrieving data: {ex.Message}";
            }

            return response;
        }
        public EQResponse<List<S_DISTRICT_DTO>> GetDistrictDataByDivisionIdAsync(string id)
        {
            var response = new EQResponse<List<S_DISTRICT_DTO>>();

            try
            {
                var connectionString = GetConnectionString(1);
                IDBHelper _db = new OracleDbHelper(connectionString);

                var parameters = new { p_division_id = id };
                var result = _db.ExecuteStoredProcedureWithRefCursor("SP_GET_DISTRICT_BY_DIVISION_ID", "o_cursor", parameters);

                var DataList = new List<S_DISTRICT_DTO>();
                foreach (var row in result)
                {
                    DataList.Add(new S_DISTRICT_DTO
                    {
                        DISTRICT_ID = row["DISTRICT_ID"]?.ToString() ?? string.Empty,
                        DISTRICT_NAME = row["DISTRICT_NAME"]?.ToString() ?? string.Empty,
                        DIVISION_ID = row["DIVISION_ID"]?.ToString() ?? string.Empty

                    });
                }

                if (DataList.Count > 0)
                {
                    response.Success = true;
                    response.Message = "District data retrieved successfully.";
                    response.Data = new EQResponseData<List<S_DISTRICT_DTO>>
                    {
                        //SingleValue = sampleDataList[0]
                        ListValue = DataList.Count > 0 ? new List<List<S_DISTRICT_DTO>> { DataList } : new List<List<S_DISTRICT_DTO>>()
                    };
                }
                else
                {
                    response.Success = false;
                    response.Message = "District data not found.";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving district data by ID: {ex.Message}");
                response.Success = false;
                response.Message = $"Error retrieving district data: {ex.Message}";
            }

            return response;
        }
        public EQResponse<List<S_THANA_DTO>> GetThanaDataByDistrictIdAsync(string id)
        {
            var response = new EQResponse<List<S_THANA_DTO>>();

            try
            {
                var connectionString = GetConnectionString(1);
                IDBHelper _db = new OracleDbHelper(connectionString);

                var parameters = new { p_division_id = id };
                var result = _db.ExecuteStoredProcedureWithRefCursor("SP_GET_THANA_BY_DISTRICT_ID", "o_cursor", parameters);

                var DataList = new List<S_THANA_DTO>();
                foreach (var row in result)
                {
                    DataList.Add(new S_THANA_DTO
                    {
                        THANA_ID = row["THANA_ID"]?.ToString() ?? string.Empty,
                        THANA_NAME = row["THANA_NAME"]?.ToString() ?? string.Empty,
                        DISTRICT_ID = row["DISTRICT_ID"]?.ToString() ?? string.Empty

                    });
                }

                if (DataList.Count > 0)
                {
                    response.Success = true;
                    response.Message = "Thana data retrieved successfully.";
                    response.Data = new EQResponseData<List<S_THANA_DTO>>
                    {
                        //SingleValue = sampleDataList[0]
                        ListValue = DataList.Count > 0 ? new List<List<S_THANA_DTO>> { DataList } : new List<List<S_THANA_DTO>>()
                    };
                }
                else
                {
                    response.Success = false;
                    response.Message = "Thana data not found.";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving thana data by ID: {ex.Message}");
                response.Success = false;
                response.Message = $"Error retrieving thana data: {ex.Message}";
            }

            return response;
        }
        public Task<EQResponse2<UserDivisionData>> GetUserDivisionDataAsync()
        {
            var response = new EQResponse2<UserDivisionData>();

            try
            {
                var connectionString = GetConnectionString(1);
                IDBHelper _db = new OracleDbHelper(connectionString);

                // ================== Get Divisions ==================
                var divisionResult = _db.ExecuteStoredProcedureWithRefCursor("SP_GET_ALL_DIVISION_DATA", "o_cursor");

                var divisions = new List<DivisionDto>();
                foreach (var row in divisionResult)
                {
                    divisions.Add(new DivisionDto
                    {
                        Id = row["DIVISION_ID"]?.ToString() ?? string.Empty,
                        Name = row["DIVISION_NAME"]?.ToString() ?? string.Empty
                    });
                }

                // ================== Get User Types ==================
                var userTypeResult = _db.ExecuteStoredProcedureWithRefCursor("SP_GET_USER_TYPE_DATA", "o_cursor");

                var userTypes = new List<UserTypeDto>();
                foreach (var row in userTypeResult)
                {
                    userTypes.Add(new UserTypeDto
                    {
                        Id = row["USER_TYPE_ID"]?.ToString() ?? string.Empty,
                        Name = row["USER_TYPE_NAME"]?.ToString() ?? string.Empty
                    });
                }

                // ================== Build Response ==================
                response.Success = true;
                response.Message = "Success";
                response.Data = new UserDivisionData
                {
                    Divisions = divisions,
                    UserTypes = userTypes
                };
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error retrieving user division data: {ex.Message}";
                response.Data = null;
            }

            return Task.FromResult(response);
        }
        public EQResponse<List<PJ_ITEM_MASTER_DTO>> GetItemDataByBarcodeNumberAsync(string barcode)
        {
            var response = new EQResponse<List<PJ_ITEM_MASTER_DTO>>();

            try
            {
                var connectionString = GetConnectionString(1);
                IDBHelper _db = new OracleDbHelper(connectionString);

                var parameters = new { p_barcode = barcode };
                var result = _db.ExecuteStoredProcedureWithRefCursor("SP_GET_ITEM_BY_BARCODE", "o_cursor", parameters);

                var DataList = new List<PJ_ITEM_MASTER_DTO>();
                foreach (var row in result)
                {
                    DataList.Add(new PJ_ITEM_MASTER_DTO
                    {
                        ITEM_NAME = row["ITEM_NAME"]?.ToString() ?? string.Empty,
                        ITEM_CODE = row["ITEM_CODE"]?.ToString() ?? string.Empty,
                        BARCODE = row["BARCODE"]?.ToString() ?? string.Empty,
                        PRICE = Convert.ToInt32(row["PRICE"]),
                        POINT = Convert.ToInt32(row["POINT"]),
                        SCAN_COUNT = Convert.ToInt32(row["SCAN_COUNT"]),
                        UNIT_CODE = row["UNIT_CODE"]?.ToString() ?? string.Empty

                    });
                }

                if (DataList.Count > 0)
                {
                    response.Success = true;
                    response.Message = "Item data retrieved successfully.";
                    response.Data = new EQResponseData<List<PJ_ITEM_MASTER_DTO>>
                    {
                        //SingleValue = sampleDataList[0]
                        ListValue = DataList.Count > 0 ? new List<List<PJ_ITEM_MASTER_DTO>> { DataList } : new List<List<PJ_ITEM_MASTER_DTO>>()
                    };
                }
                else
                {
                    response.Success = false;
                    response.Message = "Item data not found.";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving Item data by ID: {ex.Message}");
                response.Success = false;
                response.Message = $"Error retrieving Item data: {ex.Message}";
            }

            return response;
        }     
        public EQResponse<PJ_USER_POINT_DTO> GetPointByUserIDAsync(string userId)
        {
            var response = new EQResponse<PJ_USER_POINT_DTO>();

            try
            {
                var connectionString = GetConnectionString(1);
                IDBHelper _db = new OracleDbHelper(connectionString);

                var parameters = new { p_user_id = userId };
                var result = _db.ExecuteStoredProcedureWithRefCursor("SP_GET_POINT_BY_USER_ID", "o_cursor", parameters);

                var DataList = new List<PJ_USER_POINT_DTO>();
                foreach (var row in result)
                {
                    DataList.Add(new PJ_USER_POINT_DTO
                    {
                        POINT = Convert.ToInt32(row["POINT"])

                    });
                }

                if (DataList.Count > 0)
                {
                    response.Success = true;
                    response.Message = "Data retrieved successfully.";
                    response.Data = new EQResponseData<PJ_USER_POINT_DTO>
                    {
                        SingleValue = DataList[0]
                        //ListValue = DataList.Count > 0 ? new List<List<PJ_USER_POINT_DTO>> { DataList } : new List<List<PJ_USER_POINT_DTO>>()
                    };
                }
                else
                {
                    response.Success = false;
                    response.Message = "Data not found.";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving user point data by id: {ex.Message}");
                response.Success = false;
                response.Message = $"Error retrieving user point data: {ex.Message}";
            }

            return response;
        }
        public Task<EQResponse2<DashboardData>> GetDashboardDataAsync(string userId)
        {
            var response = new EQResponse2<DashboardData>();

            try
            {
                var connectionString = GetConnectionString(1);
                IDBHelper _db = new OracleDbHelper(connectionString);

                // ================== Get Dashboard Data ==================
                var parameters = new { p_user_id = userId };
                var result = _db.ExecuteStoredProcedureWithRefCursor("SP_GET_DASHBOARD_DATA", "o_cursor", parameters);
                
                var DataList = new List<DashboardData>();
                foreach (var row in result)
                {
                    DataList.Add(new DashboardData
                    {
                        TOTAL_USER = row["TOTAL_USER"]?.ToString() ?? string.Empty,
                        TOTAL_PENDING_USER = row["TOTAL_PENDING_USER"]?.ToString() ?? string.Empty,
                        TOTAL_INVOICE = row["TOTAL_INVOICE"]?.ToString() ?? string.Empty,
                        TOTAL_PENDING_INVOICE = row["TOTAL_PENDING_INVOICE"]?.ToString() ?? string.Empty

                    });
                }

                // ================== Get User Data ==================
                
                var pendingUserList = _db.ExecuteStoredProcedureWithRefCursor("SP_GET_PENDING_USER_DATA", "o_cursor", parameters);

                var Registrationusers = new List<RegistrationUserDto>();
                foreach (var row in pendingUserList)
                {
                    Registrationusers.Add(new RegistrationUserDto
                    {
                        USER_ID = row["USER_ID"]?.ToString() ?? string.Empty,
                        NAME = row["NAME"]?.ToString() ?? string.Empty,
                        MOBILE = row["MOBILE"]?.ToString() ?? string.Empty,
                        USER_IMAGE_PATH = row["USER_IMAGE_PATH"]?.ToString() ?? string.Empty,
                        CDT = (DateTime)(row["CDT"] ?? string.Empty),
                        DAYS_AGO = row["DAYS_AGO"]?.ToString() ?? string.Empty
                        
                    });
                }

                // ================== Get Invoice Types ==================
                var pendingInvoiceList = _db.ExecuteStoredProcedureWithRefCursor("SP_GET_PENDING_INVOICE_DATA", "o_cursor", parameters);

                var Invoices = new List<InvoiceDto>();
                foreach (var row in pendingInvoiceList)
                {
                    Invoices.Add(new InvoiceDto
                    {
                        INVOICE_NUMBER = row["INVOICE_NUMBER"]?.ToString() ?? string.Empty,
                        TOTAL_AMOUNT = row["TOTAL_AMOUNT"]?.ToString() ?? string.Empty,
                        CDT = (DateTime)(row["CDT"] ?? string.Empty),
                        DAYS_AGO = row["DAYS_AGO"]?.ToString() ?? string.Empty
                    });
                }

                // ================== Build Response ==================
                response.Success = true;
                response.Message = "Success";
                response.Data = new DashboardData
                {
                    TOTAL_USER = DataList[0].TOTAL_USER,
                    TOTAL_PENDING_USER = DataList[0].TOTAL_PENDING_USER,
                    TOTAL_INVOICE = DataList[0].TOTAL_INVOICE,
                    TOTAL_PENDING_INVOICE = DataList[0].TOTAL_PENDING_INVOICE,
                    UserData = Registrationusers,
                    InvoiceData = Invoices,
                };
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error retrieving user division data: {ex.Message}";
                response.Data = null;
            }

            return Task.FromResult(response);
        }
        public EQResponse<List<U_USER_DTO>> GetAllApprovalPendingUserDataAsync(string userId)
        {
            var response = new EQResponse<List<U_USER_DTO>>();

            try
            {
                var connectionString = GetConnectionString(1);
                IDBHelper _db = new OracleDbHelper(connectionString);

                var parameters = new { p_user_id = userId };
                var result = _db.ExecuteStoredProcedureWithRefCursor("SP_GET_ALL_PENDING_USER_DATA", "o_cursor", parameters);

                var DataList = new List<U_USER_DTO>();
                foreach (var row in result)
                {
                    DataList.Add(new U_USER_DTO
                    {
                        USER_ID = row["USER_ID"]?.ToString() ?? string.Empty,
                        NAME = row["NAME"]?.ToString() ?? string.Empty,
                        MOBILE = row["MOBILE"]?.ToString() ?? string.Empty,
                        ADDRESS = row["ADDRESS"]?.ToString() ?? string.Empty,
                        DIVISION_NAME = row["DIVISION_NAME"]?.ToString() ?? string.Empty,
                        DISTRICT_NAME = row["DISTRICT_NAME"]?.ToString() ?? string.Empty,
                        THANA_NAME = row["THANA_NAME"]?.ToString() ?? string.Empty,
                        NID = row["NID"]?.ToString() ?? string.Empty,
                        DATEOFBIRTH = row["DATEOFBIRTH"]?.ToString() ?? string.Empty,
                        USER_IMAGE_PATH = row["USER_IMAGE_PATH"]?.ToString() ?? string.Empty,
                        BU_ID = row["BU_ID"]?.ToString() ?? string.Empty,
                        BASE_NAME = row["BASE_NAME"]?.ToString() ?? string.Empty,
                        ZONE_NAME = row["ZONE_NAME"]?.ToString() ?? string.Empty,
                        DAYS_AGO = row["DAYS_AGO"]?.ToString() ?? string.Empty,
                        CDT = (DateTime)(row["CDT"] ?? string.Empty)

                    });
                }

                if (DataList.Count > 0)
                {
                    response.Success = true;
                    response.Message = "Data retrieved successfully.";
                    response.Data = new EQResponseData<List<U_USER_DTO>>
                    {
                        ListValue = DataList.Count > 0 ? new List<List<U_USER_DTO>> { DataList } : new List<List<U_USER_DTO>>()
                    };
                }
                else
                {
                    response.Success = false;
                    response.Message = "Data not found.";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving data by ID: {ex.Message}");
                response.Success = false;
                response.Message = $"Error retrieving data: {ex.Message}";
            }

            return response;
        }
        public EQResponse<List<PJ_All_PENDING_INVOICE_DTO>> GetAllApprovalPendingInvoiceDataAsync(string userId)
        {
            var response = new EQResponse<List<PJ_All_PENDING_INVOICE_DTO>>();

            try
            {
                var connectionString = GetConnectionString(1);
                IDBHelper _db = new OracleDbHelper(connectionString);

                var parameters = new { p_user_id = userId };
                var result = _db.ExecuteStoredProcedureWithRefCursor("SP_GET_ALL_PENDING_INVOICE", "o_cursor", parameters);

                var DataList = new List<PJ_All_PENDING_INVOICE_DTO>();
                foreach (var row in result)
                {
                    DataList.Add(new PJ_All_PENDING_INVOICE_DTO
                    {
                        INVOICE_NUMBER = row["INVOICE_NUMBER"]?.ToString() ?? string.Empty,
                        TOTAL_AMOUNT = row["TOTAL_AMOUNT"]?.ToString() ?? string.Empty,
                        IMAGE_PATH = row["IMAGE_PATH"]?.ToString() ?? string.Empty,
                        BASE_NAME = row["BASE_NAME"]?.ToString() ?? string.Empty,
                        ZONE_NAME = row["ZONE_NAME"]?.ToString() ?? string.Empty,
                        BU_ID = row["BU_ID"]?.ToString() ?? string.Empty,
                        CDT = (DateTime)(row["CDT"] ?? string.Empty),
                        DAYS_AGO = row["DAYS_AGO"]?.ToString() ?? string.Empty

                    });
                }

                if (DataList.Count > 0)
                {
                    response.Success = true;
                    response.Message = "Data retrieved successfully.";
                    response.Data = new EQResponseData<List<PJ_All_PENDING_INVOICE_DTO>>
                    {
                        ListValue = DataList.Count > 0 ? new List<List<PJ_All_PENDING_INVOICE_DTO>> { DataList } : new List<List<PJ_All_PENDING_INVOICE_DTO>>()
                    };
                }
                else
                {
                    response.Success = false;
                    response.Message = "Data not found.";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving data by ID: {ex.Message}");
                response.Success = false;
                response.Message = $"Error retrieving data: {ex.Message}";
            }

            return response;
        }

    }
}
