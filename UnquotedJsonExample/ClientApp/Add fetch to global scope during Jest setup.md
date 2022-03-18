# polyfill fetch to Nodejs during Jest setup

rxjs的`fromFetch`用到了新版本的API接口，这些接口在最新浏览器中已经存在，但是在Nodejs中还没有实现，但是当我们用Jest测试rxjs时，必须考虑Nodejs与浏览器的差异，填充功能将Nodejs环境与浏览器拉平。

```js
import { fromFetch } from 'rxjs/fetch'
```

`AbortController` and `fetch` is not available in Nodejs, which is where Jest is running your tests. Is it an experimental browser technology.

You will need to polyfill the behavior if you want to make actual http calls.

## 安装

`package.json`中：

```json
{
    "devDependencies": {
        "abort-controller": "3.0.0",
        "node-fetch": "2.6.1",
    }
}
```

## Jest configuration

File `jest.setup.js`

```js
import { AbortController, AbortSignal } from "abort-controller"
import realFetch from 'node-fetch'

if (!global.AbortController) {
    global.AbortController = AbortController
}

if (!global.AbortSignal) {
    global.AbortSignal = AbortSignal
}

if (!global.fetch) {
    global.fetch = realFetch
}
```

File `jest.config.js`

```js
module.exports = {
    setupFiles: ['./jest.setup.js'],
};
```

享受Nodejs，像浏览器一样，全文完！
