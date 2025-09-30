using BACKBONE.Core.Models;
using BACKBONE.Core.ResponseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACKBONE.Application.Interfaces
{
    public interface ISampleDataRepository
    {
        EQResponse<List<SAMPLE_DATA>> GetAllSampleDataAsync();
        EQResponse<SAMPLE_DATA> GetSampleDataByIdAsync(int id);
        Task<EQResponse<SAMPLE_DATA>> CreateSampleDataAsync(SAMPLE_DATA sampleData);
        Task<EQResponse<SAMPLE_DATA>> UpdateSampleDataAsync(int id, SAMPLE_DATA sampleData);
        Task<EQResponse<bool>> DeleteSampleDataAsync(int id);
    }
}