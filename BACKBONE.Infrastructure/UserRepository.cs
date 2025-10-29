
using BACKBONE.Application.Interfaces;
using BACKBONE.Core.Dtos;
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
    public class UserRepository : IUserRepository
    {
        public async Task<U_USER?> GetUserByEmailAsync(string email)
        {
            try
            {
                var connectionString = GetConnectionString(1);
                IDBHelper _db = new OracleDbHelper(connectionString);

                string sql = @"SELECT ID, USER_ID, PASSWORD, NAME, STAFF_ID, MOBILE, EMAIL, 
                              DIVISION_ID, DISTRICT_ID, THANA_ID, ADDRESS, NID, USER_TYPE_ID, 
                              USER_IMAGE_PATH, FB_TOKEN, APPROVE_STATUS, APPROVE_BY, CDT, UDT, 
                              CDU, UDU, GROUP_ID, ACT, BU_ID,BASE_ID, ZONE_ID,DATEOFBIRTH
                              FROM PRIOJON.U_USERS 
                              WHERE USER_ID=:EMAIL AND ACT=1";

                return await _db.QuerySingleOrDefaultAsync<U_USER>(sql, new { EMAIL = email });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving user by email: {ex.Message}");
                throw;
            }
        }

        public async Task<JWT_USER_DTO?> GetJwtUserByIdAsync(string userId)
        {
            try
            {
                var connectionString = GetConnectionString(1);
                IDBHelper _db = new OracleDbHelper(connectionString);
                
                string sql = @"SELECT USER_ID, EMAIL as USER_MAIL, NAME as USER_NAME 
                              FROM PRIOJON.U_USERS 
                              WHERE USER_ID=:USER_ID AND ACT=1";
                              
                var user = await _db.QuerySingleOrDefaultAsync<U_USER>(sql, new { USER_ID = userId });
                
                if (user != null)
                {
                    return new JWT_USER_DTO
                    {
                        USER_ID = user.USER_ID,
                        USER_MAIL = user.EMAIL,
                        USER_NAME = user.NAME
                    };
                }
                
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving JWT user by ID: {ex.Message}");
                throw;
            }
        }
        //public async Task<EQResponse<U_USER>> CreateUserDataAsync(U_USER UserData)
        //{
        //    var response = new EQResponse<U_USER>();

        //    try
        //    {
        //        var connectionString = GetConnectionString(1);
        //        using (var connection = new OracleConnection(connectionString))
        //        {
        //            await connection.OpenAsync();
        //            using (var command = new OracleCommand())
        //            {
        //                command.Connection = connection;
        //                command.CommandText = "SP_INSERT_USER_DATA";
        //                command.CommandType = CommandType.StoredProcedure;

        //                command.Parameters.Add(new OracleParameter("p_user_id", OracleDbType.Varchar2) { Value = UserData.MOBILE });
        //                command.Parameters.Add(new OracleParameter("p_password", OracleDbType.Varchar2) { Value = UserData.PASSWORD });

        //                command.Parameters.Add(new OracleParameter("p_name", OracleDbType.Varchar2) { Value = UserData.NAME });
        //                command.Parameters.Add(new OracleParameter("p_staff_id", OracleDbType.Varchar2) { Value = UserData.STAFF_ID });
        //                command.Parameters.Add(new OracleParameter("p_mobile", OracleDbType.Varchar2) { Value = UserData.MOBILE });
        //                command.Parameters.Add(new OracleParameter("p_email", OracleDbType.Varchar2) { Value = UserData.EMAIL });
        //                command.Parameters.Add(new OracleParameter("p_divition_id", OracleDbType.Varchar2) { Value = UserData.DIVITION_ID });
        //                command.Parameters.Add(new OracleParameter("p_district_id", OracleDbType.Varchar2) { Value = UserData.DISTRICT_ID });
        //                command.Parameters.Add(new OracleParameter("p_thaha_id", OracleDbType.Varchar2) { Value = UserData.THANA_ID });

                       
        //                command.Parameters.Add(new OracleParameter("p_address", OracleDbType.Varchar2) { Value = UserData.ADDRESS });
        //                command.Parameters.Add(new OracleParameter("p_nid", OracleDbType.Varchar2) { Value = UserData.NID });
        //                command.Parameters.Add(new OracleParameter("p_user_type_id", OracleDbType.Varchar2) { Value = UserData.USER_TYPE_ID });
        //                command.Parameters.Add(new OracleParameter("p_user_image_path", OracleDbType.Varchar2) { Value = UserData.USER_IMAGE_PATH });
        //                command.Parameters.Add(new OracleParameter("p_fb_token", OracleDbType.Varchar2) { Value = UserData.FB_TOKEN });
        //                command.Parameters.Add(new OracleParameter("p_approve_by", OracleDbType.Varchar2) { Value = UserData.APPROVE_BY });
        //                command.Parameters.Add(new OracleParameter("p_group_id", OracleDbType.Varchar2) { Value = UserData.GROUP_ID });
        //                command.Parameters.Add(new OracleParameter("p_bu_id", OracleDbType.Varchar2) { Value = UserData.BU_ID });

        //                command.Parameters.Add(new OracleParameter("p_id", OracleDbType.Int32) { Direction = ParameterDirection.Output });

        //                await command.ExecuteNonQueryAsync();

        //                // Get the generated ID
        //                //var newId = Convert.ToInt32(command.Parameters["p_id"].Value);
        //                var oracleDecimal = (OracleDecimal)command.Parameters["p_id"].Value;
        //                var newId = oracleDecimal.ToInt32();

        //                // Retrieve the created record
        //                var createdData = GetUserDataByIdAsync(newId);

        //                if (createdData.Success == true)
        //                {
        //                    response.Success = true;
        //                    response.Message = "User registration successfully.";
        //                    response.Data = createdData.Data;
        //                }
        //                else
        //                {
        //                    response.Success = false;
        //                    response.Message = "User registration but could not be retrieved.";
        //                }
        //            }
        //        }
        //    }
        //    //catch (Exception ex)
        //    //{
        //    //    Console.WriteLine($"Error creating sample data: {ex.Message}");
        //    //    response.Success = false;
        //    //    response.Message = $"Error creating sample data: {ex.Message}";
        //    //}
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error creating sample data: {ex.Message}");

        //        string userMessage = $"Error creating sample data: {ex.Message}";

        //        if (ex.Message.Contains("ORA-00001") && ex.Message.Contains("U_USERS_NID"))
        //        {
        //            userMessage = "A user with the same NID already exists.";
        //        }
        //        else if (ex.Message.Contains("ORA-00001") && ex.Message.Contains("U_USER_USER_ID"))
        //        {
        //            userMessage = "A user with the same User ID already exists.";
        //        }
        //        else if (ex.Message.Contains("ORA-00001"))
        //        {
        //            userMessage = "A record with the same unique field already exists.";
        //        }

        //        response.Success = false;
        //        response.Message = userMessage;
        //    }


        //    return response;
        //}
        public async Task<EQResponse<U_USER>> CreateUserDataAsync(U_USER UserData)
        {
            var response = new EQResponse<U_USER>();

            try
            {
                var connectionString = GetConnectionString(1);
                IDBHelper _db = new OracleDbHelper(connectionString);

                // Prepare parameters including OUTPUT
                var parameters = new DynamicParameters(); // Assuming you're using Dapper-based IDBHelper

                parameters.Add("p_user_id", UserData.MOBILE);
                parameters.Add("p_password", UserData.PASSWORD);
                parameters.Add("p_name", UserData.NAME);
                parameters.Add("p_staff_id", UserData.STAFF_ID);
                parameters.Add("p_mobile", UserData.MOBILE);
                parameters.Add("p_email", UserData.EMAIL);
                parameters.Add("p_division_id", UserData.DIVISION_ID);
                parameters.Add("p_district_id", UserData.DISTRICT_ID);
                parameters.Add("p_thaha_id", UserData.THANA_ID);
                parameters.Add("p_address", UserData.ADDRESS);
                parameters.Add("p_nid", UserData.NID);
                parameters.Add("p_user_type_id", UserData.USER_TYPE_ID);
                parameters.Add("p_user_image_path", UserData.USER_IMAGE_PATH);
                parameters.Add("p_fb_token", UserData.FB_TOKEN);
                parameters.Add("p_approve_by", UserData.APPROVE_BY);
                parameters.Add("p_group_id", UserData.GROUP_ID);
                parameters.Add("p_bu_id", UserData.BU_ID);
                if (string.IsNullOrWhiteSpace(UserData.DATEOFBIRTH))
                {
                    parameters.Add("p_date_of_birth", DBNull.Value, DbType.Date);
                }
                else
                {
                    parameters.Add("p_date_of_birth", DateTime.Parse(UserData.DATEOFBIRTH), DbType.Date);
                }
                // Output parameter
                parameters.Add("p_id", dbType: DbType.Int32, direction: ParameterDirection.Output);

                // Execute the stored procedure
                await _db.ExecuteAsync("SP_INSERT_USER_DATA", parameters, commandType: CommandType.StoredProcedure);

                int newId = parameters.Get<int>("p_id");


                // Retrieve the created user data (optional)
                //var createdUser = await GetUserDataByIdAsync(newId);


                //if (createdUser.Success)
                //{
                      response.Success = true;
                      response.Message = "User registration successful.";
                //    response.Data = createdUser.Data;
                //}
                //else
                //{
                    //response.Success = false;
                    //response.Message = "User registered, but could not retrieve the data.";
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating sample data: {ex.Message}");

                string userMessage = $"Error creating sample data: {ex.Message}";

                if (ex.Message.Contains("ORA-00001") && ex.Message.Contains("U_USERS_NID"))
                {
                    userMessage = "Same NID already exists.";
                }
                else if (ex.Message.Contains("ORA-00001") && ex.Message.Contains("U_USER_USER_ID"))
                {
                    userMessage = "Same Mobile Number already exists.";
                }
                else if (ex.Message.Contains("ORA-00001"))
                {
                    userMessage = "A record with the same unique field already exists.";
                }

                response.Success = false;
                response.Message = userMessage;
            }

            return response;
        }

        public EQResponse<U_USER> GetUserDataByIdAsync(int id)
        {
            var response = new EQResponse<U_USER>();

            try
            {
                var connectionString = GetConnectionString(1);
                IDBHelper _db = new OracleDbHelper(connectionString);

                var parameters = new { p_id = id };
                var result = _db.ExecuteStoredProcedureWithRefCursor("SP_GET_SAMPLE_DATA_BY_ID", "o_cursor", parameters);

                var UserDataList = new List<U_USER>();
                foreach (var row in result)
                {
                    UserDataList.Add(new U_USER
                    {
                        ID = Convert.ToInt32(row["ID"]),
                        NAME = row["NAME"]?.ToString() ?? string.Empty,
                        MOBILE = row["MOBILE"]?.ToString() ?? string.Empty
                     
                    });
                }

                if (UserDataList.Count > 0)
                {
                    response.Success = true;
                    response.Message = "Sample data retrieved successfully.";
                    response.Data = new EQResponseData<U_USER>
                    {
                        SingleValue = UserDataList[0]
                    };
                }
                else
                {
                    response.Success = false;
                    response.Message = "User data not found.";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving user data by ID: {ex.Message}");
                response.Success = false;
                response.Message = $"Error retrieving user data: {ex.Message}";
            }

            return response;
        }

        public EQResponse<U_USER> GetSampleDataByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}