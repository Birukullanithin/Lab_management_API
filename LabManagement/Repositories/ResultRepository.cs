using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LabManagement.Data;
using LabManagement.Interfaces;
using LabManagement.Models;

namespace LabManagement.Repositories
{
    public class ResultRepository : IResultRepository
    {
        private readonly LabDbContext _dbContext;

        public ResultRepository(LabDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<TestResult?> GetResultByOrderIdAsync(int orderId)
        {
            const string sql = @"SELECT test_result_id, order_id, patient_id, test_id,
                                        component_name, entered_value, calculated_value, result_text,
                                        final_result, unit, reference_min, reference_max,
                                        abnormal_flag, critical_flag, status, method_used,
                                        remarks, advice, entry_status, entered_by,
                                        entered_at, created_at, updated_at
                                 FROM lab.test_result
                                 WHERE order_id = @OrderId
                                 ORDER BY test_result_id
                                 LIMIT 1";

            return _dbContext.QuerySingleAsync<TestResult>(
                sql,
                new Dictionary<string, object?> { { "OrderId", orderId } });
        }

        public Task<List<TestResult>> GetResultsByOrderIdAsync(int orderId)
        {
            const string sql = @"SELECT test_result_id, order_id, patient_id, test_id,
                                        component_name, entered_value, calculated_value, result_text,
                                        final_result, unit, reference_min, reference_max,
                                        abnormal_flag, critical_flag, status, method_used,
                                        remarks, advice, entry_status, entered_by,
                                        entered_at, created_at, updated_at
                                 FROM lab.test_result
                                 WHERE order_id = @OrderId
                                 ORDER BY test_result_id";

            return _dbContext.QueryAsync<TestResult>(
                sql,
                new Dictionary<string, object?> { { "OrderId", orderId } });
        }

        public Task<List<TestResultDetail>> GetResultDetailsByResultIdAsync(int resultId)
        {
            const string sql = @"SELECT result_detail_id, test_result_id AS result_id, component_name, entered_value,
                                        reference_min, reference_max, critical_min, critical_max,
                                        status, abnormal_flag, is_critical, unit, method,
                                        formula_name, created_at
                                 FROM lab.test_result_detail
                                 WHERE test_result_id = @ResultId
                                 ORDER BY result_detail_id";

            return _dbContext.QueryAsync<TestResultDetail>(
                sql,
                new Dictionary<string, object?> { { "ResultId", resultId } });
        }

        public Task<List<TestResultAttachment>> GetResultAttachmentsByResultIdAsync(int resultId)
        {
            const string sql = @"SELECT attachment_id, test_result_id AS result_id, file_name, content_type,
                                        file_size, file_content, uploaded_at
                                 FROM lab.test_result_attachment
                                 WHERE test_result_id = @ResultId
                                 ORDER BY attachment_id";

            return _dbContext.QueryAsync<TestResultAttachment>(
                sql,
                new Dictionary<string, object?> { { "ResultId", resultId } });
        }

        public Task<Order?> GetOrderByIdAsync(int orderId)
        {
            const string sql = @"SELECT order_id, patient_id, order_number, order_date, status,
                                        total_amount, remarks, created_at
                                 FROM lab.orders
                                 WHERE order_id = @OrderId";

            return _dbContext.QuerySingleAsync<Order>(
                sql,
                new Dictionary<string, object?> { { "OrderId", orderId } });
        }

        public Task<Patient?> GetPatientByIdAsync(int patientId)
        {
            const string sql = @"SELECT patient_id, organization_id, legal_entity_id, patient_name,
                                        age, gender, date_of_birth, phone, email, address,
                                        created_at, updated_at
                                 FROM lab.patient
                                 WHERE patient_id = @PatientId";

            return _dbContext.QuerySingleAsync<Patient>(
                sql,
                new Dictionary<string, object?> { { "PatientId", patientId } });
        }

        public Task<TestMaster?> GetTestMasterByIdAsync(int testId)
        {
            const string sql = @"SELECT test_id, test_name, test_code, normal_min, normal_max,
                                        unit, description, is_active, created_at
                                 FROM lab.test_master
                                 WHERE test_id = @TestId";

            return _dbContext.QuerySingleAsync<TestMaster>(
                sql,
                new Dictionary<string, object?> { { "TestId", testId } });
        }

        public Task<List<TestMaster>> GetOrderTestsAsync(int orderId)
        {
            const string sql = @"SELECT tm.test_id, tm.test_name, tm.test_code, tm.normal_min, tm.normal_max,
                                        tm.unit, tm.description, tm.is_active, tm.created_at
                                 FROM lab.order_tests ot
                                 INNER JOIN lab.test_master tm ON tm.test_id = ot.test_id
                                 WHERE ot.order_id = @OrderId
                                 ORDER BY ot.order_test_id";

            return _dbContext.QueryAsync<TestMaster>(
                sql,
                new Dictionary<string, object?> { { "OrderId", orderId } });
        }

        public async Task<TestResult> SaveResultAsync(TestResult result)
        {
            if (result.TestResultId > 0)
            {
                const string updateSql = @"UPDATE lab.test_result
                                           SET patient_id = @PatientId,
                                               test_id = @TestId,
                                               component_name = @ComponentName,
                                               entered_value = @EnteredValue,
                                               calculated_value = @CalculatedValue,
                                               result_text = @ResultText,
                                               final_result = @FinalResult,
                                               unit = @Unit,
                                               reference_min = @ReferenceMin,
                                               reference_max = @ReferenceMax,
                                               abnormal_flag = @AbnormalFlag,
                                               critical_flag = @CriticalFlag,
                                               status = @Status,
                                               method_used = @MethodUsed,
                                               remarks = @Remarks,
                                               advice = @Advice,
                                               entry_status = @ResultStatus,
                                               entered_by = @EnteredBy,
                                               entered_at = @EnteredAt,
                                               updated_at = @UpdatedAt
                                           WHERE test_result_id = @ResultId";

                await _dbContext.ExecuteNonQueryAsync(updateSql, new Dictionary<string, object?>
                {
                    { "PatientId", result.PatientId },
                    { "TestId", result.TestId },
                    { "ComponentName", result.ComponentName },
                    { "EnteredValue", result.EnteredValue },
                    { "CalculatedValue", result.CalculatedValue },
                    { "ResultText", result.ResultText },
                    { "FinalResult", result.FinalResult },
                    { "Unit", result.Unit },
                    { "ReferenceMin", result.ReferenceMin },
                    { "ReferenceMax", result.ReferenceMax },
                    { "AbnormalFlag", result.AbnormalFlag },
                    { "CriticalFlag", result.CriticalFlag },
                    { "Status", result.Status },
                    { "MethodUsed", result.MethodUsed },
                    { "Remarks", result.Remarks },
                    { "Advice", result.Advice },
                    { "ResultStatus", result.ResultStatus },
                    { "EnteredBy", result.EnteredBy },
                    { "EnteredAt", result.EnteredAt },
                    { "UpdatedAt", result.UpdatedAt ?? DateTime.UtcNow },
                    { "ResultId", result.TestResultId }
                });
            }
            else
            {
                const string insertSql = @"INSERT INTO lab.test_result
                                           (order_id, patient_id, test_id, component_name, entered_value,
                                            calculated_value, result_text, final_result, unit, reference_min,
                                            reference_max, abnormal_flag, critical_flag, status, method_used,
                                            remarks, advice, entry_status, entered_by, entered_at, created_at, updated_at)
                                           VALUES
                                           (@OrderId, @PatientId, @TestId, @ComponentName, @EnteredValue,
                                            @CalculatedValue, @ResultText, @FinalResult, @Unit, @ReferenceMin,
                                            @ReferenceMax, @AbnormalFlag, @CriticalFlag, @Status, @MethodUsed,
                                            @Remarks, @Advice, @ResultStatus, @EnteredBy, @EnteredAt, @CreatedAt, @UpdatedAt)
                                           RETURNING test_result_id";

                var resultId = await _dbContext.ExecuteScalarAsync<int>(insertSql, new Dictionary<string, object?>
                {
                    { "OrderId", result.OrderId },
                    { "PatientId", result.PatientId },
                    { "TestId", result.TestId },
                    { "ComponentName", result.ComponentName },
                    { "EnteredValue", result.EnteredValue },
                    { "CalculatedValue", result.CalculatedValue },
                    { "ResultText", result.ResultText },
                    { "FinalResult", result.FinalResult },
                    { "Unit", result.Unit },
                    { "ReferenceMin", result.ReferenceMin },
                    { "ReferenceMax", result.ReferenceMax },
                    { "AbnormalFlag", result.AbnormalFlag },
                    { "CriticalFlag", result.CriticalFlag },
                    { "Status", result.Status },
                    { "MethodUsed", result.MethodUsed },
                    { "Remarks", result.Remarks },
                    { "Advice", result.Advice },
                    { "ResultStatus", result.ResultStatus },
                    { "EnteredBy", result.EnteredBy },
                    { "EnteredAt", result.EnteredAt },
                    { "CreatedAt", result.CreatedAt },
                    { "UpdatedAt", result.UpdatedAt }
                });
                result.TestResultId = resultId;
            }

            return result;
        }

        public async Task<List<TestResult>> ReplaceResultsAsync(int orderId, int patientId, IEnumerable<TestResult> results)
        {
            var normalizedResults = results.ToList();
            var existingResults = await _dbContext.QueryAsync<TestResult>(
                @"SELECT test_result_id, order_id, patient_id, test_id,
                         component_name, entered_value, calculated_value, result_text,
                         final_result, unit, reference_min, reference_max,
                         abnormal_flag, critical_flag, status, method_used,
                         remarks, advice, entry_status, entered_by,
                         entered_at, created_at, updated_at
                  FROM lab.test_result
                  WHERE order_id = @OrderId AND patient_id = @PatientId
                  ORDER BY test_result_id",
                new Dictionary<string, object?>
                {
                    { "OrderId", orderId },
                    { "PatientId", patientId }
                });

            var retainedResult = existingResults.FirstOrDefault();
            if (retainedResult != null)
            {
                const string deleteOthersSql = @"DELETE FROM lab.test_result
                                                 WHERE order_id = @OrderId AND patient_id = @PatientId
                                                   AND test_result_id <> @ResultId";
                await _dbContext.ExecuteNonQueryAsync(deleteOthersSql, new Dictionary<string, object?>
                {
                    { "OrderId", orderId },
                    { "PatientId", patientId },
                    { "ResultId", retainedResult.TestResultId }
                });
            }

            var savedResults = new List<TestResult>();
            if (normalizedResults.Count == 0)
            {
                return savedResults;
            }

            for (var index = 0; index < normalizedResults.Count; index++)
            {
                var result = normalizedResults[index];
                if (index == 0 && retainedResult != null)
                {
                    result.TestResultId = retainedResult.TestResultId;
                    result.CreatedAt = retainedResult.CreatedAt;
                }

                savedResults.Add(await SaveResultAsync(result));
            }

            return savedResults;
        }

        public async Task ReplaceResultDetailsAsync(int resultId, IEnumerable<TestResultDetail> details)
        {
            const string deleteSql = @"DELETE FROM lab.test_result_detail WHERE test_result_id = @ResultId";
            await _dbContext.ExecuteNonQueryAsync(deleteSql, new Dictionary<string, object?> { { "ResultId", resultId } });

            const string insertSql = @"INSERT INTO lab.test_result_detail
                                       (test_result_id, component_name, entered_value, reference_min, reference_max,
                                        critical_min, critical_max, status, abnormal_flag, is_critical,
                                        unit, method, formula_name, created_at)
                                       VALUES
                                       (@ResultId, @ComponentName, @EnteredValue, @ReferenceMin, @ReferenceMax,
                                        @CriticalMin, @CriticalMax, @Status, @AbnormalFlag, @IsCritical,
                                        @Unit, @Method, @FormulaName, @CreatedAt)";

            foreach (var detail in details)
            {
                await _dbContext.ExecuteNonQueryAsync(insertSql, new Dictionary<string, object?>
                {
                    { "ResultId", resultId },
                    { "ComponentName", detail.ComponentName },
                    { "EnteredValue", detail.EnteredValue },
                    { "ReferenceMin", detail.ReferenceMin },
                    { "ReferenceMax", detail.ReferenceMax },
                    { "CriticalMin", detail.CriticalMin },
                    { "CriticalMax", detail.CriticalMax },
                    { "Status", detail.Status },
                    { "AbnormalFlag", detail.AbnormalFlag },
                    { "IsCritical", detail.IsCritical },
                    { "Unit", detail.Unit },
                    { "Method", detail.Method },
                    { "FormulaName", detail.FormulaName },
                    { "CreatedAt", detail.CreatedAt }
                });
            }
        }

        public async Task<TestResultAttachment> SaveAttachmentAsync(TestResultAttachment attachment)
        {
            const string sql = @"INSERT INTO lab.test_result_attachment
                                 (test_result_id, file_name, content_type, file_size, file_content, uploaded_at)
                                 VALUES
                                 (@ResultId, @FileName, @ContentType, @FileSize, @FileContent, @UploadedAt)
                                 RETURNING attachment_id";

            var attachmentId = await _dbContext.ExecuteScalarAsync<int>(
                sql,
                new Dictionary<string, object?>
                {
                    { "ResultId", attachment.ResultId },
                    { "FileName", attachment.FileName },
                    { "ContentType", attachment.ContentType },
                    { "FileSize", attachment.FileSize },
                    { "FileContent", attachment.FileContent },
                    { "UploadedAt", attachment.UploadedAt }
                });

            attachment.AttachmentId = attachmentId;
            return attachment;
        }
    }
}
