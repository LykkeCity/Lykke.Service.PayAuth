using System;
using System.Threading.Tasks;
using AutoMapper;
using AzureStorage;
using AzureStorage.Tables.Templates.Index;
using JetBrains.Annotations;
using Lykke.Service.PayAuth.AzureRepositories.Extensions;
using Lykke.Service.PayAuth.Core.Exceptions;
using Lykke.Service.PayAuth.Core.Repositories;

namespace Lykke.Service.PayAuth.AzureRepositories.ResetPasswordAccessToken
{
    public class ResetPasswordAccessTokenRepository : IResetPasswordAccessTokenRepository
    {
        private readonly INoSQLTableStorage<ResetPasswordAccessTokenEntity> _storage;
        private readonly INoSQLTableStorage<AzureIndex> _indexByPublicIdStorage;

        public ResetPasswordAccessTokenRepository(
            [NotNull] INoSQLTableStorage<ResetPasswordAccessTokenEntity> storage, 
            [NotNull] INoSQLTableStorage<AzureIndex> indexByPublicIdStorage)
        {
            _storage = storage ?? throw new ArgumentNullException(nameof(storage));
            _indexByPublicIdStorage = indexByPublicIdStorage ?? throw new ArgumentNullException(nameof(indexByPublicIdStorage));
        }

        public async Task<Core.Domain.ResetPasswordAccessToken> CreateAsync(Core.Domain.ResetPasswordAccessToken token)
        {
            ResetPasswordAccessTokenEntity entity = ResetPasswordAccessTokenEntity.ByEmployeeId.Create(token);

            await _storage.InsertThrowConflict(entity);

            AzureIndex index = ResetPasswordAccessTokenEntity.IndexByPublicId.Create(entity);

            await _indexByPublicIdStorage.InsertThrowConflict(index);

            return Mapper.Map<Core.Domain.ResetPasswordAccessToken>(entity);
        }

        public async Task<Core.Domain.ResetPasswordAccessToken> GetByPublicIdAsync(string publicId)
        {
            AzureIndex index = await _indexByPublicIdStorage.GetDataAsync(
                ResetPasswordAccessTokenEntity.IndexByPublicId.GeneratePartitionKey(publicId),
                ResetPasswordAccessTokenEntity.IndexByPublicId.GenerateRowKey());

            if (index == null)
                return null;

            ResetPasswordAccessTokenEntity entity = await _storage.GetDataAsync(index);

            return Mapper.Map<Core.Domain.ResetPasswordAccessToken>(entity);
        }

        public async Task<Core.Domain.ResetPasswordAccessToken> UpdateAsync(Core.Domain.ResetPasswordAccessToken src)
        {
            AzureIndex index = await _indexByPublicIdStorage.GetDataAsync(
                ResetPasswordAccessTokenEntity.IndexByPublicId.GeneratePartitionKey(src.PublicId),
                ResetPasswordAccessTokenEntity.IndexByPublicId.GenerateRowKey());

            if (index == null)
                throw new KeyNotFoundException();

            ResetPasswordAccessTokenEntity updatedEntity = await _storage.MergeAsync(
                ResetPasswordAccessTokenEntity.ByEmployeeId.GeneratePartitionKey(index.PrimaryPartitionKey),
                ResetPasswordAccessTokenEntity.ByEmployeeId.GenerateRowKey(index.PrimaryRowKey),
                entity =>
                {
                    entity.Redeemed = src.Redeemed;

                    return entity;
                });

            if (updatedEntity == null)
                throw new KeyNotFoundException();

            return Mapper.Map<Core.Domain.ResetPasswordAccessToken>(updatedEntity);
        }
    }
}
