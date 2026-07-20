using MCP_Server.Internal_Models;
using Microsoft.OpenApi.Models;

namespace MCP_Server.Tools
{
    public class ToolGenerator
    {
        private readonly ToolFilter _toolFilter;

        public ToolGenerator(ToolFilter toolFilter)
        {
            _toolFilter = toolFilter;
        }

        public List<ToolDefinition> Generate(OpenApiDocument document)
        {
            var tools = new List<ToolDefinition>();

            foreach (var path in document.Paths)
            {
                foreach (var operation in path.Value.Operations)
                {
                    var tool = new ToolDefinition
                    {
                        Name =
                            operation.Value.OperationId
                            ?? GenerateName(path.Key, operation.Key),

                        Description =
                            operation.Value.Summary
                            ?? operation.Value.Description
                            ?? "No description",

                        HttpMethod =
                            operation.Key.ToString(),

                        Route = path.Key,

                        RequiresAuthentication =
                            operation.Value.Security.Any()
                            || document.SecurityRequirements.Any(),

                        Schema = BuildSchema(
                            operation.Value,
                            document)
                    };

                    if (!_toolFilter.ShouldExpose(tool.Name))
                    {
                        continue;
                    }

                    tools.Add(tool);
                }
            }

            return tools;
        }

        private ToolDefinitionSchema BuildSchema(
            OpenApiOperation operation,
            OpenApiDocument document)
        {
            var schema = new ToolDefinitionSchema
            {
                Type = "object",
                AdditionalProperties = false
            };

            foreach (var parameter in operation.Parameters)
            {
                schema.Properties[parameter.Name] = new
                {
                    type = parameter.Schema?.Type ?? "string",
                    description = parameter.Description
                };

                if (parameter.Required)
                {
                    schema.Required.Add(parameter.Name);
                }
            }

            // Body (POST, PUT, PATCH)
            var bodySchema = operation.RequestBody?
                .Content
                .Values
                .FirstOrDefault()?
                .Schema;

            if (bodySchema != null)
            {
                MergeSchema(
                    schema,
                    bodySchema,
                    document);
            }

            return schema;
        }

        private void MergeSchema(
            ToolDefinitionSchema target,
            OpenApiSchema source,
            OpenApiDocument document)
        {
            // Resolver $ref
            if (source.Reference != null)
            {
                source = document.Components.Schemas[source.Reference.Id];
            }

            foreach (var property in source.Properties)
            {
                var propertySchema = property.Value;

                if (propertySchema.Reference != null)
                {
                    propertySchema = document.Components
                        .Schemas[propertySchema.Reference.Id];
                }

                target.Properties[property.Key] = new
                {
                    type = propertySchema.Type ?? "string",
                    description = propertySchema.Description
                };
            }

            foreach (var required in source.Required)
            {
                if (!target.Required.Contains(required))
                {
                    target.Required.Add(required);
                }
            }
        }

        private string GenerateName(
            string route,
            OperationType method)
        {
            return $"{method}_{route}"
                .Replace("/", "_")
                .Replace("{", "")
                .Replace("}", "");
        }
    }
}