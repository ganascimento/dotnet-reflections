using System.Reflection;
using System.Text.Json;
using Dotnet.Reflaction.Api.Models;
using Dotnet.Reflaction.Api.Services.Interfaces;

namespace Dotnet.Reflaction.Api.Services;

public class CallerService : ICallerService
{
    private readonly string _baseDllPath;

    public CallerService(IConfiguration configuration)
    {
        _baseDllPath = configuration["BasePath"] ?? throw new InvalidOperationException("BasePath not fount");
    }

    public dynamic? Call(CallerModel model)
    {
        var className = "";
        var dllName = "";

        if (model.Target.IndexOf(".dll") == -1)
        {
            className = $"{model.Namespace}.{model.Target}";
            dllName = model.Target + ".dll";
        }
        else
        {
            dllName = model.Target;
            className = $"{model.Namespace}.{model.Target.Split(".").First()}";
        }

        var path = Path.Combine(_baseDllPath, dllName);
        var assembly = Assembly.LoadFrom(path);

        var currentClass = assembly.GetType(className);
        if (currentClass == null) return null;

        var method = currentClass.GetMethod("Invoke");
        if (method == null) return null;

        var instance = assembly.CreateInstance(className);
        if (model.Params != null)
        {
            var parameters = new List<object>();

            foreach (var parameter in method.GetParameters())
            {
                var type = parameter.ParameterType;
                JsonElement jsonValue = model.Params[parameter.Name ?? ""];

                if (type.IsArray)
                {
                    var elementType = type.GetElementType()!;
                    var arrayLength = jsonValue.GetArrayLength();
                    var arrayValue = jsonValue.EnumerateArray().Select(x => Convert.ChangeType(x.ToString(), elementType)).ToArray();

                    var property = Array.CreateInstance(elementType, arrayLength);
                    for (var i = 0; i < arrayValue.Length; i++)
                    {
                        property.SetValue(arrayValue[i], i);
                    }

                    parameters.Add(property);
                }
                else
                {
                    var property = Convert.ChangeType(jsonValue.ToString(), type);
                    parameters.Add(property);
                }
            }

            var result = method.Invoke(instance, parameters.ToArray());
            return result;
        }
        else
            method.Invoke(instance, null);

        return null;
    }
}