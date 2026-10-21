using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LabManagement.Dtos;
using LabManagement.Interfaces;
using LabManagement.Models;

namespace LabManagement.Services
{
    public class ResultService : IResultService
    {
        private readonly IResultRepository _resultRepository;
        private readonly IRangeValidationService _rangeValidationService;
        private readonly Dictionary<string, IFormulaCalculator> _calculators;

        public ResultService(
            IResultRepository resultRepository,
            IRangeValidationService rangeValidationService,
            IEnumerable<IFormulaCalculator> calculators)
        {
            _resultRepository = resultRepository;
            _rangeValidationService = rangeValidationService;
            _calculators = calculators.ToDictionary(
                calculator => calculator.FormulaName,
                calculator => calculator,
                StringComparer.OrdinalIgnoreCase);
        }

        public async Task<ResultResponseDto> GetByOrderIdAsync(int orderId)
        {
            var existingResults = await _resultRepository.GetResultsByOrderIdAsync(orderId);
            var orderedTests = await _resultRepository.GetOrderTestsAsync(orderId);
            if (existingResults.Count > 0)
            {
                var anchorResult = existingResults[0];
                //anchorResult.Attachments = await _resultRepository.GetResultAttachmentsByResultIdAsync(anchorResult.TestResultId);
                return await BuildResponseAsync(anchorResult, existingResults, orderedTests);
            }

            var order = await _resultRepository.GetOrderByIdAsync(orderId)
                        ?? throw new KeyNotFoundException($"Order {orderId} was not found.");
            var test = ResolvePrimaryTestMaster(orderedTests);

            return new ResultResponseDto
            {
                OrderId = order.OrderId,
                PatientId = order.PatientId ?? 0,
                TestId = test?.TestId ?? 0,
                OrderNumber = order.OrderNumber,
                PatientName = order.PatientId.HasValue
                    ? (await _resultRepository.GetPatientByIdAsync(order.PatientId.Value))?.PatientName
                    : null,
                TestName = test?.TestName ?? "Pending Test",
                ResultStatus = "Pending",
                CreatedAt = DateTime.UtcNow,
                Components = BuildComponentsFromMaster(orderedTests)
            };
        }

        public Task<ResultResponseDto> SaveAsync(ResultSaveRequestDto request)
        {
            return SaveInternalAsync(request, "Pending");
        }

        public Task<ResultResponseDto> CompleteAsync(ResultSaveRequestDto request)
        {
            return SaveInternalAsync(request, "Completed");
        }

        public async Task<ResultAttachmentDto> UploadAsync(ResultUploadRequestDto request)
        {
            var result = await EnsureResultAsync(request.OrderId, request.PatientId, request.TestId);

            await using var stream = new MemoryStream();
            await request.File.CopyToAsync(stream);

            var attachment = await _resultRepository.SaveAttachmentAsync(new TestResultAttachment
            {
                ResultId = result.TestResultId,
                FileName = request.File.FileName,
                ContentType = request.File.ContentType,
                FileSize = request.File.Length,
                FileContent = stream.ToArray(),
                UploadedAt = DateTime.UtcNow
            });

            return new ResultAttachmentDto
            {
                AttachmentId = attachment.AttachmentId,
                FileName = attachment.FileName,
                ContentType = attachment.ContentType,
                FileSize = attachment.FileSize,
                UploadedAt = attachment.UploadedAt
            };
        }

        private async Task<ResultResponseDto> SaveInternalAsync(ResultSaveRequestDto request, string fallbackStatus)
        {
            var existingResult = await _resultRepository.GetResultByOrderIdAsync(request.OrderId);
            if (existingResult != null &&
                string.Equals(existingResult.ResultStatus, "Completed", StringComparison.OrdinalIgnoreCase))
            {
                var completedResults = await _resultRepository.GetResultsByOrderIdAsync(request.OrderId);
                existingResult.Attachments = await _resultRepository.GetResultAttachmentsByResultIdAsync(existingResult.TestResultId);
                var completedOrderTests = await _resultRepository.GetOrderTestsAsync(request.OrderId);
                return await BuildResponseAsync(existingResult, completedResults, completedOrderTests);
            }

            var savedResults = await ReplaceResultsAsync(request, fallbackStatus, existingResult);
            var anchorResult = savedResults[0];
            anchorResult.Attachments = await _resultRepository.GetResultAttachmentsByResultIdAsync(anchorResult.TestResultId);
            var savedOrderTests = await _resultRepository.GetOrderTestsAsync(request.OrderId);

            return await BuildResponseAsync(anchorResult, savedResults, savedOrderTests);
        }

        private async Task<TestResult> EnsureResultAsync(int orderId, int patientId, int testId)
        {
            var order = await _resultRepository.GetOrderByIdAsync(orderId);
            if (order == null)
            {
                throw new KeyNotFoundException($"The requested order is not present for order id {orderId}.");
            }

            var existingResult = await _resultRepository.GetResultByOrderIdAsync(orderId);
            if (existingResult != null)
            {
                existingResult.PatientId = patientId;
                existingResult.TestId = testId;
                return existingResult;
            }

            return new TestResult
            {
                OrderId = orderId,
                PatientId = patientId,
                TestId = testId,
                ResultStatus = "Pending",
                EnteredAt = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
        }

        private async Task<List<TestResult>> ReplaceResultsAsync(
            ResultSaveRequestDto request,
            string fallbackStatus,
            TestResult? existingResult)
        {
            var resultStatus = NormalizeResultStatus(request.ResultStatus, fallbackStatus);
            var now = DateTime.UtcNow;
            var normalizedValues = request.Components
                .Where(component => component.EnteredValue.HasValue)
                .ToDictionary(
                    component => NormalizeKey(component.ComponentName),
                    component => component.EnteredValue!.Value,
                    StringComparer.OrdinalIgnoreCase);

            var createdAt = existingResult?.CreatedAt ?? now;
            var enteredAt = existingResult?.EnteredAt ?? now;
            var results = request.Components.Select(component =>
            {
                var value = ResolveEnteredValue(component, normalizedValues);
                var validation = _rangeValidationService.Validate(
                    value,
                    component.ReferenceMin,
                    component.ReferenceMax,
                    component.CriticalMin,
                    component.CriticalMax);

                return new TestResult
                {
                    OrderId = request.OrderId,
                    PatientId = request.PatientId,
                    TestId = request.TestId,
                    ComponentName = component.ComponentName,
                    EnteredValue = component.EnteredValue,
                    CalculatedValue = value,
                    ResultText = value?.ToString(),
                    FinalResult = value?.ToString(),
                    Unit = component.Unit,
                    ReferenceMin = component.ReferenceMin,
                    ReferenceMax = component.ReferenceMax,
                    AbnormalFlag = validation.AbnormalFlag,
                    CriticalFlag = validation.IsCritical,
                    Status = validation.Status,
                    MethodUsed = component.Method,
                    Remarks = request.Remarks,
                    Advice = request.Advice,
                    ResultStatus = resultStatus,
                    EnteredAt = enteredAt,
                    CreatedAt = createdAt,
                    UpdatedAt = now
                };
            }).ToList();

            if (results.Count == 0)
            {
                throw new KeyNotFoundException("At least one component is required to save result values.");
            }

            return await _resultRepository.ReplaceResultsAsync(request.OrderId, request.PatientId, results);
        }

        private async Task<ResultResponseDto> BuildResponseAsync(
            TestResult anchorResult,
            IReadOnlyList<TestResult> results,
            IReadOnlyList<TestMaster> orderedTests)
        {
            var order = await _resultRepository.GetOrderByIdAsync(anchorResult.OrderId);
            var patient = await _resultRepository.GetPatientByIdAsync(anchorResult.PatientId);
            var test = ResolvePrimaryTestMaster(orderedTests)
                ?? await ResolveTestMasterAsync(anchorResult.OrderId, anchorResult.TestId);

            return new ResultResponseDto
            {
                ResultId = anchorResult.TestResultId,
                OrderId = anchorResult.OrderId,
                PatientId = anchorResult.PatientId,
                TestId = anchorResult.TestId,
                OrderNumber = order?.OrderNumber,
                PatientName = patient?.PatientName,
                TestName = test?.TestName ?? "Pending Test",
                ResultStatus = anchorResult.ResultStatus,
                HasCriticalAlert = results.Any(component => component.CriticalFlag),
                Remarks = anchorResult.Remarks,
                Advice = anchorResult.Advice,
                CreatedAt = anchorResult.CreatedAt,
                UpdatedAt = anchorResult.UpdatedAt,
                Components = MergeComponents(orderedTests, results),
                Attachments = anchorResult.Attachments.Select(attachment => new ResultAttachmentDto
                {
                    AttachmentId = attachment.AttachmentId,
                    FileName = attachment.FileName,
                    ContentType = attachment.ContentType,
                    FileSize = attachment.FileSize,
                    UploadedAt = attachment.UploadedAt
                }).ToList()
            };
        }

        private decimal? ResolveEnteredValue(ResultComponentDto component, IReadOnlyDictionary<string, decimal> normalizedValues)
        {
            if (component.EnteredValue.HasValue)
            {
                return component.EnteredValue.Value;
            }

            var formulaName = string.IsNullOrWhiteSpace(component.FormulaName)
                ? component.ComponentName
                : component.FormulaName;

            if (_calculators.TryGetValue(formulaName, out var calculator))
            {
                return calculator.Calculate(normalizedValues);
            }

            return null;
        }

        private async Task<TestMaster?> ResolveTestMasterAsync(int orderId, int testId)
        {
            if (testId > 0)
            {
                var byId = await _resultRepository.GetTestMasterByIdAsync(testId);
                if (byId != null)
                {
                    return byId;
                }
            }

            var orderTests = await _resultRepository.GetOrderTestsAsync(orderId);
            return orderTests.FirstOrDefault();
        }

        private static TestMaster? ResolvePrimaryTestMaster(IReadOnlyList<TestMaster> orderedTests)
        {
            return orderedTests.FirstOrDefault(test => test.IsActive) ?? orderedTests.FirstOrDefault();
        }

        private static List<ResultComponentDto> BuildComponentsFromMaster(IReadOnlyList<TestMaster> orderedTests)
        {
            return orderedTests
                .Where(test => test.IsActive && !string.IsNullOrWhiteSpace(test.TestName))
                .Select(test => new ResultComponentDto
                {
                    ComponentName = test.TestName,
                    ReferenceMin = test.NormalMin,
                    ReferenceMax = test.NormalMax,
                    Unit = test.Unit,
                    Status = "Pending"
                })
                .ToList();
        }

        private static List<ResultComponentDto> MergeComponents(
            IReadOnlyList<TestMaster> orderedTests,
            IReadOnlyList<TestResult> results)
        {
            var masterComponents = BuildComponentsFromMaster(orderedTests);
            var resultsByName = results
                .Where(result => !string.IsNullOrWhiteSpace(result.ComponentName))
                .GroupBy(result => NormalizeKey(result.ComponentName!))
                .ToDictionary(group => group.Key, group => group.First(), StringComparer.OrdinalIgnoreCase);

            var merged = masterComponents
                .Select(component =>
                {
                    if (!resultsByName.TryGetValue(NormalizeKey(component.ComponentName), out var result))
                    {
                        return component;
                    }

                    return new ResultComponentDto
                    {
                        ComponentName = component.ComponentName,
                        EnteredValue = result.EnteredValue,
                        ReferenceMin = result.ReferenceMin ?? component.ReferenceMin,
                        ReferenceMax = result.ReferenceMax ?? component.ReferenceMax,
                        Status = result.Status ?? "Pending",
                        AbnormalFlag = result.AbnormalFlag,
                        IsCritical = result.CriticalFlag,
                        Unit = result.Unit ?? component.Unit,
                        Method = result.MethodUsed
                    };
                })
                .ToList();

            foreach (var result in results)
            {
                if (string.IsNullOrWhiteSpace(result.ComponentName))
                {
                    continue;
                }

                var normalizedName = NormalizeKey(result.ComponentName);
                var alreadyIncluded = merged.Any(component =>
                    string.Equals(NormalizeKey(component.ComponentName), normalizedName, StringComparison.OrdinalIgnoreCase));
                if (alreadyIncluded)
                {
                    continue;
                }

                merged.Add(new ResultComponentDto
                {
                    ComponentName = result.ComponentName,
                    EnteredValue = result.EnteredValue,
                    ReferenceMin = result.ReferenceMin,
                    ReferenceMax = result.ReferenceMax,
                    Status = result.Status ?? "Pending",
                    AbnormalFlag = result.AbnormalFlag,
                    IsCritical = result.CriticalFlag,
                    Unit = result.Unit,
                    Method = result.MethodUsed
                });
            }

            return merged;
        }

        private static string NormalizeKey(string value)
        {
            return value.Trim().ToLowerInvariant();
        }

        private static string NormalizeResultStatus(string? requestedStatus, string fallbackStatus)
        {
          var value = requestedStatus?.Trim();
          if (string.Equals(value, "Completed", StringComparison.OrdinalIgnoreCase) ||
              string.Equals(fallbackStatus, "Completed", StringComparison.OrdinalIgnoreCase))
          {
              return "Completed";
          }

          return "Pending";
        }
    }
}
