using Dapper;
using Pgvector;
using System.Data;

namespace Askly.Api.Infrastructure.Database.TypeHandlers;

public class VectorTypeHandler : SqlMapper.TypeHandler<Vector>
{
    public override Vector Parse(object value)
    {
        if (value is string stringValue)
        {
            // Parse vector string format like "[1.0, 2.0, 3.0]"
            string cleanValue = stringValue.Trim('[', ']');
            float[] floats = cleanValue.Split(',').Select(x => float.Parse(x.Trim())).ToArray();
            return new Vector(floats);
        }

        if (value is float[] floatArray)
        {
            return new Vector(floatArray);
        }

        throw new InvalidOperationException($"Unable to parse {value.GetType()} as Vector");
    }

    public override void SetValue(IDbDataParameter parameter, Vector? value)
    {
        if (value == null)
        {
            parameter.Value = DBNull.Value;
        }
        else
        {
            parameter.Value = value.ToArray();
        }
        parameter.DbType = DbType.Object;
    }
}
