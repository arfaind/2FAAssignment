# Huntr Second Factor Authentication Assignment
### For Assignment Purpose, below are the assumptions/points
* InMemory Database Considered, returning hard-coded code and verifying based on that.
* Interface has been created to extend the database to **DynamoDb** for actual implementation.
* Unit tests has been written only for Code Service and is having 100% code coverage for that specific class. Further UTs are needed for other classes.
* Functional Tests needs to be written to test end to end API call.

### Open the solution in Visual Studio and run the project as db is inmemory for assignment, it will work for below requests.

### Below are the API Endpoints
* `/api/auth/second-factor/send`
    #### Valid Request
    `{
        "countryCode": "+1",
        "phoneNumber": "223161110"
    }`

    #### Too Many Concurrent Code Error
    `{
        "countryCode": "+2",
        "phoneNumber": "234567890"
    }`

* `/api/auth/second-factor/verify`
    #### Valid Code
    `{
        "countryCode": "+1",
        "phoneNumber": "234567890",
        "confirmationCode": "123",
        "utcNow": "2023-06-25T07:37:01.040Z"
    }`

    #### Invalid Code
    `{
        "countryCode": "+1",
        "phoneNumber": "234567890",
        "confirmationCode": "213",
        "utcNow": "2023-06-25T07:37:01.040Z"
    }`
