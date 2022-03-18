# 项目创建说明

url字符串长度是一种稀缺资源，根据服务器不同，对长度有不同的限制。我们应该只传递必要的数据。

要尽量使url可阅读，以便传抄。要尽量缩短URL，尽可能地使用GET请求，以便优化WEB的性能。

get参数过长的一种解决方案，尽可能避免百分号编码。

JSON格式经常用于Restful传递数据，`FSharpCompiler.Json`可以集成用于asp.net core，下面我们就说明创建过程。

## 项目简介

我们在客户端使用`urljson-serializer`来格式化查询数据为查询字符串，

在asp.net服务器端使用的编程语言是F#，F#是一门兼容面向对象的函数式编程语言。函数式编程语言非常擅长处理数据。

使用`FSharpCompiler.Json`解析url查询字符串为.net数据。

使用`AspNetCore.FSharpCompilerJson`将解析器包整合进Asp,net框架。



## 初始化项目

创建一个解决方案，ASP.NET Core Web 应用程序，F#语言的，命名为`UrljsonExample`，确定。

选择ASP.NET Core Web API 模板，创建。

将项目类型更改为类库，除了保留`Controllers`文件夹，删除模板生成的其他文件。

安装nuget包：

```
install-package AspNetCore.FSharpCompilerJson
install-package TaskBuilder.fs
```

向解决方案添加新项目：C#、 ASP.NET Core Web 应用程序，名称为`UrljsonExample.Client`，确定。

选择 .NET Core、 ASP.NET Core Web 应用（Mvc）模板

> 选择一个内容较多的模板，有些不需要的功能可以删除。

添加项目引用：选择UrljsonExample

安装nuget包：

```
install-package AspNetCore.FSharpCompilerJson
```

修改`Startup.cs`文件

修改方法`ConfigureServices`，这个方法是依赖注入，替换如下：

```C#
public void ConfigureServices(IServiceCollection services) {
    services.AddControllersWithViews()
        .AddApplicationPart(typeof(WeatherForecastController).Assembly)
        ;
    FSharpCompilerJsonDependencyInjection.AddFSharpJson(services);
}
```

上面代码整合了`FSharpCompiler.Json`将action返回的数据格式化为json格式。

控制器的架构

Web API控制器有如下架构：

```F#
[<ApiController>]
[<Route("[controller]/[action]")>]
type SampleController(_logger:ILogger<SampleController>) =
    inherit ControllerBase()

    [<HttpGet>]
    member this.Test1() = "hello action!"
```

这个方法是展示用的，下面将逐渐添加有用的功能到方法中。

客户端文件夹

客户端文件夹命名为`ClientApp`可以从其他项目直接整体复制过来，然后进行修改。

项目配置文件是`package.json`：

```json
{
  "dependencies": {
    "rxjs": "6.6.7",
    "urljson-serializer": "1.1.0"
  },
  "devDependencies": {
    "@babel/core": "7.13.14",
    "@babel/plugin-proposal-class-properties": "7.13.0",
    "@babel/plugin-proposal-export-default-from": "7.12.13",
    "@babel/plugin-proposal-export-namespace-from": "7.12.13",
    "@babel/plugin-proposal-pipeline-operator": "7.12.13",
    "@babel/preset-env": "7.13.12",
    "@webpack-cli/serve": "1.3.1",
    "babel-jest": "26.6.3",
    "babel-loader": "8.2.2",
    "clean-webpack-plugin": "3.0.0",
    "core-js": "3.10.0",
    "css-loader": "5.2.0",
    "html-loader": "2.1.2",
    "html-webpack-plugin": "5.3.1",
    "jest": "26.6.3",
    "style-loader": "2.0.0",
    "webpack": "5.28.0",
    "webpack-bundle-analyzer": "^4.4.0",
    "webpack-cli": "4.6.0",
    "webpack-dev-server": "3.11.2",
    "webpack-merge": "5.7.3",
    "xmlhttprequest": "1.8.0"
  },
  "name": "urljsonexample-client",
  "private": true,
  "license": "ISC",
  "scripts": {
    "build": "webpack --config webpack.prod.js",
    "start": "webpack serve --config webpack.dev.js",
    "test": "jest"
  }
}
```

配置文件中的大部分包是基础结构的设施，主要说明的是npm包`urljson-serializer`是将json结构的数据序列化到url查询字符串字段。采用专门设计的一种较短格式。格式说明详见说明文件。

## 测试JSON序列化与反序列化

### 客户端发送数据代码

在JavaScript客户端，首先准备要发送的数据：

```js
let obj = {
    foo: 'bar',
    abc: ['xyz', '123']
}
```

我们把数据序列化成查询字符串，并测试结果：

```js
of(queryStringify(obj))
    |> tap(qs =>
        expect(qs).toEqual("foo=bar&abc=[~xyz~,~123~]"))
```

序列化规则：

- 当字段是字符串时，字段值就是字符串的字面义。
- 当字段是`null`时，字段值是空字符串。
- 当字段是基元类型时，字段值是用toString转化为字符串。
- 当字段是复合类型时，字段值是用Urljson序列化器转化为字符串。
- 其他情况，字段将被忽略。

发送数据到服务器并测试是否返回的数据和输入数据是一样的：

```js
|> map(qs => mainUrl('sample', 'test1') + qs)
//|> tap(console.log)
|> mergeMap(url => ajax.getJSON(url))
//|> tap(console.log)
|> o => o.subscribe(response => {
    expect(response).toEqual(obj)
}, 0, done)
```

## 服务器端接收数据并返回同样的数据给客户端

如上所述，客户端向服务器发送了如下url:

```
http://localhost:53603/sample/test1?foo=bar&abc=[~xyz~,~123~]
```

在控制器`sample.test1`中我们获得查询字符串数据如下：

```F#
let args = this.Request.Query |> Query.toPairs
_logger.LogInformation(Render.stringify (List.ofSeq args))
```

获得数据为：

```F#
["foo","bar";"abc","[~xyz~,~123~]"]
```

以上数据是类型`seq<string*string>`，我们将其解析成相应的对象：

```F#
let resObj = UrlQuery.parse<{|foo:string;abc:string[]|}> args
_logger.LogInformation(Render.stringify resObj)
```

Action返回类型为`JsonResult`的数据：

```F#
JsonResult resObj
```

这将会把`resObj`的JSON格式写入响应体中。客户端将获得同样数据的响应。

删除log语句，整体的Action代码如下：

```F#
[<HttpGet>]
member this.Test1() = 
    let args = this.Request.Query |> Query.toPairs
    let resObj = UrlQuery.parse<{|foo:string;abc:string[]|}> args
    JsonResult resObj

```

也可以通过浏览器导航栏来发送数据，F12查看请求与响应。