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

    }
}
