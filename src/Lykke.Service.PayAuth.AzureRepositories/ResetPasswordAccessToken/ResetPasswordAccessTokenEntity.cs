using System;
using AutoMapper;
using AzureStorage.Tables.Templates.Index;
using Lykke.AzureStorage.Tables;
using Lykke.AzureStorage.Tables.Entity.Annotation;
using Lykke.AzureStorage.Tables.Entity.ValueTypesMerging;

namespace Lykke.Service.PayAuth.AzureRepositories.ResetPasswordAccessToken
{
    [ValueTypeMergingStrategy(ValueTypeMergingStrategy.UpdateIfDirty)]
    public class ResetPasswordAccessTokenEntity : AzureTableEntity
    {
        private DateTime _expiresOn;
        private bool _redeemed;

        public string Id => RowKey;

        public string PublicId { get; set; }

        public string EmployeeId { get; set; }

        public string MerchantId { get; set; }

        public DateTime ExpiresOn
        {
            get => _expiresOn;
            set
            {
                _expiresOn = value;
                MarkValueTypePropertyAsDirty(nameof(ExpiresOn));
            }
        }

        public bool Redeemed
        {
            get => _redeemed;
            set
            {
                _redeemed = value;
                MarkValueTypePropertyAsDirty(nameof(Redeemed));
            }
        }

        public static class ByEmployeeId
        {
            public static string GeneratePartitionKey(string employeeId)
            {
                return employeeId;
            }

            public static string GenerateRowKey(string id)
            {
                return id;
            }

            public static ResetPasswordAccessTokenEntity Create(Core.Domain.ResetPasswordAccessToken src)
            {
                var entity = new ResetPasswordAccessTokenEntity
                {
                    PartitionKey = GeneratePartitionKey(src.EmployeeId),
                    RowKey = GenerateRowKey(src.Id),
                };

                return Mapper.Map(src, entity);
            }
        }

        public static class IndexByPublicId
        {
            public static string GeneratePartitionKey(string publicId)
            {
                return publicId;
            }

            public static string GenerateRowKey()
            {
                return nameof(IndexByPublicId);
            }

            public static AzureIndex Create(ResetPasswordAccessTokenEntity entity)
            {
                return AzureIndex.Create(GeneratePartitionKey(entity.PublicId), GenerateRowKey(), entity);
            }
        }
    }
}
