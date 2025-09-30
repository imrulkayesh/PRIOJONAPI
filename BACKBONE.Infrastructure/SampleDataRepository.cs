using BACKBONE.Application.Interfaces;
using BACKBONE.Core.Models;
using BACKBONE.Core.ResponseClasses;
using BACKBONE.DB;
using Dapper;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BACKBONE.Core.ApplicationConnectionString.ApplicationConnectionString;

namespace BACKBONE.Infrastructure
{
    public class SampleDataRepository : ISampleDataRepository
    {
        public EQResponse<List<SAMPLE_DATA>> GetAllSampleDataAsync()
        {
            var response = new EQResponse<List<SAMPLE_DATA>>();
            
            try
            {
                var connectionString = GetConnectionString(1);
                IDBHelper _db = new OracleDbHelper(connectionString);
                
                var result = _db.ExecuteStoredProcedureWithRefCursor("SP_GET_ALL_SAMPLE_DATA", "o_cursor");
                
                var sampleDataList = new List<SAMPLE_DATA>();
                foreach (var row in result)
                {
                    sampleDataList.Add(new SAMPLE_DATA
                    {
                        ID = Convert.ToInt32(row["ID"]),
                        NAME = row["NAME"]?.ToString() ?? string.Empty,
                        DESCRIPTION = row["DESCRIPTION"]?.ToString() ?? string.Empty,
                        CREATED_DATE = Convert.ToDateTime(row["CREATED_DATE"]),
                        UPDATED_DATE = Convert.ToDateTime(row["UPDATED_DATE"])
                    });
                }
                
                response.Success = true;
                response.Message = "All sample data retrieved successfully.";
                response.Data = new EQResponseData<List<SAMPLE_DATA>>
                {
                    ListValue = sampleDataList.Count > 0 ? new List<List<SAMPLE_DATA>> { sampleDataList } : new List<List<SAMPLE_DATA>>()
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving all sample data: {ex.Message}");
                response.Success = false;
                response.Message = $"Error retrieving sample data: {ex.Message}";
            }
            
            return response;
        }

        public EQResponse<SAMPLE_DATA> GetSampleDataByIdAsync(int id)
        {
            var response = new EQResponse<SAMPLE_DATA>();
            
            try
            {
                var connectionString = GetConnectionString(1);
                IDBHelper _db = new OracleDbHelper(connectionString);
                
                var parameters = new { p_id = id };
                var result = _db.ExecuteStoredProcedureWithRefCursor("SP_GET_SAMPLE_DATA_BY_ID", "o_cursor", parameters);
                
                var sampleDataList = new List<SAMPLE_DATA>();
                foreach (var row in result)
                {
                    sampleDataList.Add(new SAMPLE_DATA
                    {
                        ID = Convert.ToInt32(row["ID"]),
                        NAME = row["NAME"]?.ToString() ?? string.Empty,
                        DESCRIPTION = row["DESCRIPTION"]?.ToString() ?? string.Empty,
                        CREATED_DATE = Convert.ToDateTime(row["CREATED_DATE"]),
                        UPDATED_DATE = Convert.ToDateTime(row["UPDATED_DATE"])
                    });
                }
                
                if (sampleDataList.Count > 0)
                {
                    response.Success = true;
                    response.Message = "Sample data retrieved successfully.";
                    response.Data = new EQResponseData<SAMPLE_DATA>
                    {
                        SingleValue = sampleDataList[0]
                    };
                }
                else
                {
                    response.Success = false;
                    response.Message = "Sample data not found.";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving sample data by ID: {ex.Message}");
                response.Success = false;
                response.Message = $"Error retrieving sample data: {ex.Message}";
            }
            
            return response;
        }

        public async Task<EQResponse<SAMPLE_DATA>> CreateSampleDataAsync(SAMPLE_DATA sampleData)
        {
            var response = new EQResponse<SAMPLE_DATA>();
            
            try
            {
                var connectionString = GetConnectionString(1);
                using (var connection = new OracleConnection(connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new OracleCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = "SP_INSERT_SAMPLE_DATA";
                        command.CommandType = CommandType.StoredProcedure;
                        
                        command.Parameters.Add(new OracleParameter("p_name", OracleDbType.Varchar2) { Value = sampleData.NAME });
                        command.Parameters.Add(new OracleParameter("p_description", OracleDbType.Varchar2) { Value = sampleData.DESCRIPTION });
                        command.Parameters.Add(new OracleParameter("p_id", OracleDbType.Int32) { Direction = ParameterDirection.Output });
                        
                        await command.ExecuteNonQueryAsync();

                        // Get the generated ID
                        //var newId = Convert.ToInt32(command.Parameters["p_id"].Value);
                        var oracleDecimal = (OracleDecimal)command.Parameters["p_id"].Value;
                        var newId = oracleDecimal.ToInt32();

                        // Retrieve the created record
                        var createdData = GetSampleDataByIdAsync(newId);
                        
                        if (createdData.Success == true)
                        {
                            response.Success = true;
                            response.Message = "Sample data created successfully.";
                            response.Data = createdData.Data;
                        }
                        else
                        {
                            response.Success = false;
                            response.Message = "Sample data created but could not be retrieved.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating sample data: {ex.Message}");
                response.Success = false;
                response.Message = $"Error creating sample data: {ex.Message}";
            }
            
            return response;
        }

        public async Task<EQResponse<SAMPLE_DATA>> UpdateSampleDataAsync(int id, SAMPLE_DATA sampleData)
        {
            var response = new EQResponse<SAMPLE_DATA>();
            
            try
            {
                var connectionString = GetConnectionString(1);
                using (var connection = new OracleConnection(connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new OracleCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = "SP_UPDATE_SAMPLE_DATA";
                        command.CommandType = CommandType.StoredProcedure;
                        
                        command.Parameters.Add(new OracleParameter("p_id", OracleDbType.Int32) { Value = id });
                        command.Parameters.Add(new OracleParameter("p_name", OracleDbType.Varchar2) { Value = sampleData.NAME });
                        command.Parameters.Add(new OracleParameter("p_description", OracleDbType.Varchar2) { Value = sampleData.DESCRIPTION });
                        
                        await command.ExecuteNonQueryAsync();
                        
                        // Retrieve the updated record
                        var updatedData = GetSampleDataByIdAsync(id);
                        
                        if (updatedData.Success == true)
                        {
                            response.Success = true;
                            response.Message = "Sample data updated successfully.";
                            response.Data = updatedData.Data;
                        }
                        else
                        {
                            response.Success = false;
                            response.Message = "Sample data updated but could not be retrieved.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating sample data: {ex.Message}");
                response.Success = false;
                response.Message = $"Error updating sample data: {ex.Message}";
            }
            
            return response;
        }

        public async Task<EQResponse<bool>> DeleteSampleDataAsync(int id)
        {
            var response = new EQResponse<bool>();
            
            try
            {
                var connectionString = GetConnectionString(1);
                using (var connection = new OracleConnection(connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new OracleCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = "SP_DELETE_SAMPLE_DATA";
                        command.CommandType = CommandType.StoredProcedure;
                        
                        command.Parameters.Add(new OracleParameter("p_id", OracleDbType.Int32) { Value = id });
                        
                        await command.ExecuteNonQueryAsync();
                        
                        response.Success = true;
                        response.Message = "Sample data deleted successfully.";
                        response.Data = new EQResponseData<bool>
                        {
                            SingleValue = true
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting sample data: {ex.Message}");
                response.Success = false;
                response.Message = $"Error deleting sample data: {ex.Message}";
                response.Data = new EQResponseData<bool>
                {
                    SingleValue = false
                };
            }
            
            return response;
        }
    }
}