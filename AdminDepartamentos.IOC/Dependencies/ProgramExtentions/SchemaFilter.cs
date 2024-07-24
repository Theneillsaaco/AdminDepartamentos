using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace AdminDepartamentos.API.Extentions.ProgramExtentions;

public class SchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (schema.Properties.ContainsKey("twoFactorCode"))
        {
            schema.Properties.Remove("twoFactorCode");
        }
        if (schema.Properties.ContainsKey("twoFactorRecoveryCode"))
        {
            schema.Properties.Remove("twoFactorRecoveryCode");
        }
    }
}