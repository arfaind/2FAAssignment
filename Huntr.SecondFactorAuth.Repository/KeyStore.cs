namespace Huntr.SecondFactorAuth.Repository
{
    public static class KeyStore
    {
        public const string DynamoDbTableName = "SecondFactorCode";
        public const string DynamoDb = "DDB";
        public const string InMemory = "InMem";
    }
}
