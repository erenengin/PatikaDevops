using StackExchange.Redis;
using Microsoft.Extensions.Configuration;

public class RedisService
{
    private readonly IConfiguration _config;
    private readonly string _connectionString;
    private readonly string _instanceName;

    public const string REDIS_CONNECTION_STRING = "REDIS_CONNECTION_STRING";
    public RedisService(IConfiguration config)
    {
        var _connectionStringFromEnvironment = System.Environment.GetEnvironmentVariable(REDIS_CONNECTION_STRING);
        
        _config = config;

        if (_connectionStringFromEnvironment != null)
        {
            _connectionString = _connectionStringFromEnvironment;
            Console.WriteLine("Redis Connection String is set from Environment Variable");
        }
        else if (_config.GetValue<string>("RedisSettings:ConnectionString") != null)
        {
            _connectionString = _config.GetValue<string>("RedisSettings:ConnectionString");
            Console.WriteLine("Redis Connection String is set from appsettings.json");
        }
        else
        {
            throw new System.Exception("Redis Connection String is not set");
        }

        _instanceName = _config.GetValue<string>("RedisSettings:InstanceName");

        
        
        Console.WriteLine("Trying to connect Redis using connection string : {0}",_connectionString);
    }

    public void SetValue(string key, string value)
    {
        
        using (var connection = ConnectionMultiplexer.Connect(_connectionString))
        {
            var db = connection.GetDatabase();
            db.StringSet($"{_instanceName}:{key}", value);
            Console.WriteLine("Set Redis Key : {0} and Value : {1}",key,value);
        }
    }

    public string GetValue(string key)
    {
        using (var connection = ConnectionMultiplexer.Connect(_connectionString))
        {
            var db = connection.GetDatabase();
            string value = db.StringGet($"{_instanceName}:{key}");
            Console.WriteLine("Get Redis Key : {0} and Value : {1}",key,value);
            return value;
            
        }
    }
}
