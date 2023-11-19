using Dotnet.Reflaction.Api.Models;

namespace Dotnet.Reflaction.Api.Services.Interfaces;

public interface ICallerService
{
    dynamic? Call(CallerModel model);
}