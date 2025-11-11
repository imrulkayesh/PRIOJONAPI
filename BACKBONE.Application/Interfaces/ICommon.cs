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
        Task<EQResponse2<UserDivisionData>> GetUserDivisionDataAsync();
        EQResponse<List<PJ_ITEM_MASTER_DTO>> GetItemDataByBarcodeNumberAsync(string barcode);
        EQResponse<PJ_USER_POINT_DTO> GetPointByUserIDAsync(string userId);
        Task<EQResponse2<DashboardData>> GetDashboardDataAsync(string userId);
        EQResponse<List<U_USER_DTO>> GetAllApprovalPendingUserDataAsync(string userId);
        EQResponse<List<PJ_All_PENDING_INVOICE_DTO>> GetAllApprovalPendingInvoiceDataAsync(string userId);
        EQResponse<List<PJ_ITEM_MASTER_DTO>> GetAllItemsDataAsync();
        





    }
}
