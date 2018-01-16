using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Microsoft.WindowsAzure.Storage.Table;

namespace Lykke.Service.PayAuth.Assets
{
    public static class AutoMapperExtensions
    {
        public static IMappingExpression<TSource, TDestination> IgnoreTableEntityFields<TSource, TDestination>(
            this IMappingExpression<TSource, TDestination> map) where TDestination : TableEntity
        {
            map.ForMember(x => x.ETag, config => config.Ignore());
            map.ForMember(x => x.PartitionKey, config => config.Ignore());
            map.ForMember(x => x.RowKey, config => config.Ignore());
            map.ForMember(x => x.Timestamp, config => config.Ignore());
            return map;
        }
    }
}
