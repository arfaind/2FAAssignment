using Amazon.DynamoDBv2.DataModel;
using System;

namespace Huntr.SecondFactorAuth.Repository.Models
{
    [DynamoDBTable(KeyStore.DynamoDbTableName)]
    public class ConfirmationCode
    {
        [DynamoDBHashKey]
        public string PhoneNumber { get; set; }

        [DynamoDBRangeKey]
        public DateTime ExpiryTime { get; set; }

        public string Code { get; set; }
        public string TTL { get; set; }
    }
}
