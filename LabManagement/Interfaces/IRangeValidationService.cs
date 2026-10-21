using LabManagement.Dtos;

namespace LabManagement.Interfaces
{
    public interface IRangeValidationService
    {
        RangeValidationResultDto Validate(decimal? result, decimal? min, decimal? max, decimal? criticalMin, decimal? criticalMax);
    }
}
