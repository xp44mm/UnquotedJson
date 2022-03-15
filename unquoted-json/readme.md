npm包库`unquoted-json`是格式化 URL 查询字符串的实用工具。

## urlquery

```js
urlquery(data)
```

`urlquery`将data序列化为 URL 查询字符串。data是一个普通对象，表示一个属性集合。 data对象的每个属性序列化为一个键值对，键值对之间用`&`分隔，键和值之间用`=`分隔。

根据对象属性值的类型，下面的属性会被忽略，根本不会出现在查询字符串中：

```js
undefined
null
NaN
Infinity
function
Symbol
''
```

属性值类型为字符串时，序列化为字符串本身。

属性值为布尔，数字，大整数类型时，序列化调用`primitive.toString()`。

当属性值是对象或数组时，用urljson格式序列化。urljson格式详见下面。

当属性集合为空时，`urlquery`返回空字符串。

用法：

```js
test('urlquery', () => {
    let obj = { foo: 'bar', baz: ['qux', 'quux'], corge: '' }
    let y = urlquery(obj)
    expect(y).toEqual("?foo=bar&baz=[`qux`,`quux`]")
})
```

返回的字符串以`?`问号开头。字符串bar按原意出现在查询字符串中，两边直接接等号`=`和和号`&`。corge是空字符串，整个字段被忽略，不出现在查询字符串中。baz数组，是组合类型，序列化用urljson格式。

示例二：

对象序列化为查询字符串：

```js
let x = {
    a: false,
    b: 2,
    c: 'xyz',
    d: [1, 2],
    e: { x: 1, y: 2 },
}
let y = urlquery(x)
expect(y).toEqual("?a=false&b=2&c=xyz&d=[1,2]&e={x:1,y:2}")
```

生成的结果，对象直接属性如果是标量，简单调用`toString()`方法序列化。如果对象的属性是对象，则按Urljson格式化为字符串。注意数组是特殊的对象，也按Urljson格式化为字符串。`urlquery`兼容Form Data字符串格式，可以被浏览器开发者工具识别分解为键值对集合。

示例三：

```js
let y = urlquery({})
expect(y).toEqual("")
```

当属性集合为空时，`urlquery`返回空字符串。

## urlquery与Form Data格式比较

urlquery函数序列化格式采用的是一种单层Form Data格式。

1. 忽略空参数，当参数值为空字符串时，整个键值对被移除。
2. 一个键只会出现一次，并不会出现多次。用urljson序列化为一个参数。即使是基元数组也用urljson格式序列化，而不是像From Data多个同名参数顺序排列。
3. 没有用点号分隔的复合键。因为复杂对象都用urljson格式序列化。
4. 在输入数据属性都是基元类型时，兼容Form Data格式。所以，可以利用现有的各种form参数查看工具。
5. 设计原则是，使查询字符串尽可能短。

## Urljson格式

urlquery序列化对象类型的属性时采用urljson格式序列化属性值。

顾名思义，urljson格式类似于JSON格式，不同之处在于：

* 字符串限定符为反引号`` ` ``。代替json格式双引号`"`。

* 字符串中的转义符号仍然是反斜杠。对反引号、控制字符(`\u0000-\u0020`)，空白字符(`\s`)转义，转义方法为：

```
\` \\ \b \f \n \r \t \v
\ hexdigit hexdigit
```

* 属性键字符串的限定符是可选的。除非当属性键包含限定字符，控制字符，空白字符，标点符号时，限定符是必须的。

> 用反引号作为字符串限定符，是因为双引号会被百分号转义为三个字符，增加了url的长度，也增加了阅读难度。

## urljsonStringify

为了数据序列化为Urljson格式，使用函数`urljsonStringify`：

```js
let obj = {
    a: 1,
    b: true,
}
let s = urljsonStringify(obj)
expect(s).toEqual("{a:1,b:true}")
```



## urljson格式规范

The grammar can be transcribed as follows:

```
value : object
	  | array
	  | null
	  | boolean
	  | number
	  | string
	  ;
object : "{" "}"
       | "{" fields "}"
       ;
fields : field
	   | fields "," field
	   ;
field : key ":" value
	  ;
key : string 
    | identifier
    ;
array : "[" "]"
      | "[" values "]"
      ;
values : value
	   | values "," value
       ;
```

## 百分号编码

只对必要的字符进行pct编码，其余字符如有遗漏，浏览器会自动pct编码。`urlquery`百分号编码的字符有：

* 控制字符(\u0000-\u001F)，理由非打印不好传抄。
* 空格(\u0020)，理由经常被处理程序忽略
* 加号(+)，某些处理程序会翻译成空格
* 百分号(%)，百分号编码的开始符号
* 哈希(#)，url fragment部分的开始符号
* Ampersand(`&`)，键值对的分隔符号
* 等号(=)，键值的分隔符号
* 删除号，理由非打印不好传抄。
* 双引号
* 单引号
* 小于号
* 大于号

其他需要百分号编码的字符，浏览器会自动进行pct编码。并且浏览器不会对已经编码的字符多次编码。

* 大于`\uFFFF`以上的字符，由浏览器自动进行pct编码。

> It appears most browsers (tested in Firefox, Safari and Chromium) will url-encode paths automatically if they are not already encoded.

浏览器自动进行pct编码的字符有：



下面是其他工具函数

## toUtf8(utf16)

```js
let n = '中'.charCodeAt(0)
expect(toUtf8(n)).toEqual([228, 184, 173])
```

将utf16转换为utf8整数数组。utf16<0x10000

## pctEncodeChar(utf8)

```js
expect(pctEncodeChar([228, 184, 173])).toEqual('%E4%B8%AD')
```

根据字符的utf8编码，返回百分号编码字符串。

## queryPctEncode(s)

```js
let m = 'a+=1'
let y = queryPctEncode(m)
expect(y).toEqual("a%2B%3D1")
```

返回输入字符串的百分号编码字符串。

## 参见

- npm包库`unquoted-json`开源于github，位于xp44mm/unquoted-json仓库。
- FSharpCompiler.Json是一个NuGet开源json解析库，开源于github，它亦可以解析urljson，位于xp44mm/FSharpCompiler.Json仓库。
- AspNetCore.FSharpCompilerJson是一个NuGet开源库，开源于github，它将FSharpCompiler.Json整合进入Asp.net，位于xp44mm/AspNetCore.FSharpCompilerJson仓库。
- 可以应用于Asp.net项目序列化JSON格式，演练教程如下：xp44mm/UrljsonExample

