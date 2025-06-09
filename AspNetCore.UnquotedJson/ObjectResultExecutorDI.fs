module AspNetCore.UnquotedJson.ObjectResultExecutorDI

open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.DependencyInjection.Extensions
open Microsoft.AspNetCore.Mvc
open Microsoft.AspNetCore.Mvc.Infrastructure

[<System.Obsolete("services.Replace(ServiceDescriptor.Singleton<IActionResultExecutor<ObjectResult>, ObjectResultExecutor>())")>]
let Add(services:IServiceCollection) =
    services.Replace(ServiceDescriptor.Singleton<IActionResultExecutor<ObjectResult>, ObjectResultExecutor>())
