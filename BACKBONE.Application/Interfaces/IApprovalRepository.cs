using BACKBONE.Core.Dtos;
using BACKBONE.Core.Models;
using BACKBONE.Core.ResponseClasses;
using BACKBONE.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACKBONE.Application.Interfaces
{
    public interface IApprovalRepository
    {
        Task<EQResponse<APPROVAL_DTO>> InsertApprovalDataAsync(APPROVAL_DTO ApprovalData);
    }
}
