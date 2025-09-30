using BACKBONE.Core.Dtos;
using BACKBONE.Core.Models;
using BACKBONE.Core.ResponseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACKBONE.Application.Interfaces
{
    public interface ICommon
    {
        EQResponse<List<U_USER_TYPE_DTO>> GetAllUserTypeDataAsync();
        EQResponse<List<S_DIVISION_DTO>> GetAllDivitionDataAsync();
        EQResponse<List<S_DISTRICT_DTO>> GetDistrictDataByDivisionIdAsync(string id);
        EQResponse<List<S_THANA_DTO>> GetThanaDataByDistrictIdAsync(string id);
        // test
        Task<EQResponse2<UserDivisionData>> GetUserDivisionDataAsync();

    }
}
