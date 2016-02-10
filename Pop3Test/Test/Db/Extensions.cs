using System.Data;
using Npgsql;
using NpgsqlTypes;

namespace Test.Db
{
    public static class Extensions
    {
        public static void AddParameter(this NpgsqlCommand command, string name, NpgsqlDbType type, object value)
        {
            var param = new NpgsqlParameter
            {
                ParameterName = name,
                NpgsqlDbType = type,
                Value = value
            };
            command.Parameters.Add(param);
        }
    }
}