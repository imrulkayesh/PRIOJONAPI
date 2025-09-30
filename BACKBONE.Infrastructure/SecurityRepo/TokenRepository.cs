﻿﻿using BACKBONE.Application.Interfaces.SecurityInterface;
using BACKBONE.Core.Models;
using BACKBONE.DB;
using Oracle.ManagedDataAccess.Client;
using static BACKBONE.Core.ApplicationConnectionString.ApplicationConnectionString;
using System.Data;
using System.Threading.Tasks;

namespace BACKBONE.Infrastructure.SecurityRepo
{
    public class TokenRepository : ITokenRepository
    {
        public async Task SaveRefreshTokenAsync(REFRESH_TOKEN refreshToken)
        {
            try
            {
                var _connectionString = GetConnectionString(1);
                IDBHelper _db = new OracleDbHelper(_connectionString);
                
                var sql = @"INSERT INTO JWT_REFRESH_TOKENS (USER_ID, TOKEN, EXPIRATION_DATE) 
                            VALUES (:USER_ID, :TOKEN, :EXPIRATION_DATE)";
                
                await _db.ExecuteAsync(sql, new
                {
                    USER_ID = refreshToken.USER_ID,
                    TOKEN = refreshToken.TOKEN,
                    EXPIRATION_DATE = refreshToken.EXPIRATION_DATE
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving refresh token: {ex.Message}");
                throw;
            }
        }

        public async Task<REFRESH_TOKEN?> GetRefreshTokenAsync(string token)
        {
            try
            {
                var _connectionString = GetConnectionString(1);
                IDBHelper _db = new OracleDbHelper(_connectionString);
                var sql = @"SELECT USER_ID as USER_ID, TOKEN as TOKEN, EXPIRATION_DATE as EXPIRATION_DATE 
                            FROM JWT_REFRESH_TOKENS 
                            WHERE TOKEN = :TOKEN AND IS_REVOKED = 0";
                return await _db.QuerySingleOrDefaultAsync<REFRESH_TOKEN>(sql, new { TOKEN = token });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving refresh token: {ex.Message}");
                throw;
            }
        }

        public REFRESH_TOKEN? GetRefreshToken(string token)
        {
            try
            {
                var _connectionString = GetConnectionString(1);
                IDBHelper _db = new OracleDbHelper(_connectionString);
                var sql = @"SELECT USER_ID as USER_ID, TOKEN as TOKEN, EXPIRATION_DATE as EXPIRATION_DATE 
                            FROM JWT_REFRESH_TOKENS 
                            WHERE TOKEN = :TOKEN AND IS_REVOKED = 0";
                return _db.QuerySingleOrDefault<REFRESH_TOKEN>(sql, new { TOKEN = token });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving refresh token: {ex.Message}");
                throw;
            }
        }

        public async Task RevokeRefreshTokenAsync(string token)
        {
            try
            {
                var _connectionString = GetConnectionString(1);
                IDBHelper _db = new OracleDbHelper(_connectionString);
                var sql = @"UPDATE JWT_REFRESH_TOKENS 
                            SET IS_REVOKED = 1 
                            WHERE TOKEN = :TOKEN";
                await _db.ExecuteAsync(sql, new { TOKEN = token });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error revoking refresh token: {ex.Message}");
                throw;
            }
        }
    }
}