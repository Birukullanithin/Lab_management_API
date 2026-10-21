using LabManagement.Dtos;
using LabManagement.Interfaces;

namespace LabManagement.Services
{
    public class RangeValidationService : IRangeValidationService
    {
        public RangeValidationResultDto Validate(decimal? result, decimal? min, decimal? max, decimal? criticalMin, decimal? criticalMax)
        {
            var response = new RangeValidationResultDto();

            if (!result.HasValue)
            {
                response.Status = "Pending";
                return response;
            }

            if (criticalMin.HasValue && result.Value < criticalMin.Value)
            {
                response.Status = "Critical Low";
                response.AbnormalFlag = true;
                response.IsCritical = true;
                return response;
            }

            if (criticalMax.HasValue && result.Value > criticalMax.Value)
            {
                response.Status = "Critical High";
                response.AbnormalFlag = true;
                response.IsCritical = true;
                return response;
            }

            if (min.HasValue && result.Value < min.Value)
            {
                response.Status = "Low";
                response.AbnormalFlag = true;
                return response;
            }

            if (max.HasValue && result.Value > max.Value)
            {
                response.Status = "High";
                response.AbnormalFlag = true;
                return response;
            }

            response.Status = "Normal";
            return response;
        }
    }
}
