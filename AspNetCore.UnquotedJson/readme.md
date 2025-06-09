# AspNetCore.UnquotedJson

This NuGet package provides the ability to integrate the `UnquotedJson` into the ASP.NET to replace the built-in Json serializer.

## The Usage

Build an ASP.NET Core Web App, and install following NuGet packages:

```
install-package Microsoft.AspNetCore.SpaServices.Extensions
install-package AspNetCore.UnquotedJson
```
## namespace

```fsharp
open AspNetCore.UnquotedJson
```

## How to serialize The return value of the controller's actions as JSON

in `Startup.cs` file, Modify the method `ConfigureServices` to add dependency injection to it:

```C#
public void ConfigureServices(IServiceCollection services) {
    ...
    services.Replace(ServiceDescriptor.Singleton<IActionResultExecutor<ObjectResult>, ObjectResultExecutor>());
}
```

This configuration means that `UnquotedJson` has been integrated into ASP.NET to replace the built-in json serializer.

```F#
[<HttpGet>]
member this.action() = 
    ...
    data
```

data will be serialized as json using the `UnquotedJson` serializer, which is ASP.NET's serializer.

## To read `Request.Query`

In a controller's actions, The Method to read `Request.Query` is as follows:

```F#
[<HttpGet>]
member this.kvps() = 
    let kvps = this.Request.Query |> Query.toPairs
    ...
```

where the type of `kvps` is `seq<string*string>`.



For example, a request's url query string is:

```
?foo=bar&baz=[qux,quux]
```

The kvps corresponding to the query string are parsed as follows:

```F#
[
  ["foo","bar"],
  ["baz","[qux,quux]"]
]
```
