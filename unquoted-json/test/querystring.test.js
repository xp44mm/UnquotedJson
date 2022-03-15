import querystring from "querystring"

describe('querystring stringify', () => {
    test('basis', () => {
        let obj = { foo: 'bar', baz: ['qux', 'quux'], corge: '' }
        let y = querystring.stringify(obj)
        expect(y).toEqual('foo=bar&baz=qux&baz=quux&corge=')
    })

    //它会序列化传入的 obj 中以下类型的值：<string> | <number> | <boolean>。 
    //任何其他的输入值都将会被强制转换为空字符串。

    test('undefined', () => {
        let obj = { foo: undefined }
        let y = querystring.stringify(obj)
        expect(y).toEqual('foo=')
    })

    test('null', () => {
        let obj = { foo: null }
        let y = querystring.stringify(obj)
        expect(y).toEqual('foo=')
    })

    test('NaN', () => {
        let obj = { foo: NaN }
        let y = querystring.stringify(obj)
        expect(y).toEqual('foo=')
    })

    test('Infinity', () => {
        let obj = { foo: Infinity, bar: -Infinity }
        let y = querystring.stringify(obj)
        expect(y).toEqual('foo=&bar=')
    })

    test('big int', () => {
        let obj = { foo: 0n, bar: 1n }
        let y = querystring.stringify(obj)
        expect(y).toEqual('foo=0&bar=1')
    })

    test('empty string', () => {
        let obj = { foo: '' }
        let y = querystring.stringify(obj)
        expect(y).toEqual('foo=')
    })

    test('boolean', () => {
        let obj = { foo: true, bar: false }
        let y = querystring.stringify(obj)
        expect(y).toEqual('foo=true&bar=false')
    })

    test('function', () => {
        let obj = { foo: x => x }
        let y = querystring.stringify(obj)
        expect(y).toEqual('foo=')
    })

    test('Symbol', () => {
        let obj = { foo: Symbol() }
        let y = querystring.stringify(obj)
        expect(y).toEqual('foo=')
    })


})

describe('querystring pct encode', () => {
    let inpobj = sym => ({ [sym]: sym })

    let resultstr = hexdigs => {
        let pct = '%' + hexdigs
        return pct + '=' + pct
    }

    test('control characters pct', () => {
        Array.from({ length: 0x20 })
            .map((c, i) => {
                let sym = String.fromCodePoint(i)
                let hexdigs = i < 0x10 ? '0' + i.toString(16).toUpperCase() : i.toString(16).toUpperCase()
                return [sym, hexdigs]
            })
            .forEach(([sym, hexdigs]) => {
                let obj = inpobj(sym)
                let y = querystring.stringify(obj)
                expect(y).toEqual(resultstr(hexdigs))
            })
    })


    test('space', () => {
        let obj = inpobj(' ')
        let y = querystring.stringify(obj)
        expect(y).toEqual(resultstr('20'))
    })

    test('percent', () => {
        let obj = { '%': '%' }
        let y = querystring.stringify(obj)
        expect(y).toEqual('%25=%25')
    })

    test('hash', () => {
        let obj = { '#': '#' }
        let y = querystring.stringify(obj)
        expect(y).toEqual('%23=%23')
    })

    test('Ampersand', () => {
        let obj = { '&': '&' }
        let y = querystring.stringify(obj)
        expect(y).toEqual('%26=%26')
    })

    test('plus', () => {
        let obj = { '+': '+' }
        let y = querystring.stringify(obj)
        expect(y).toEqual('%2B=%2B')
    })

    test('equal', () => {
        let obj = { '=': '=' }
        let y = querystring.stringify(obj)
        expect(y).toEqual('%3D=%3D')
    })

    test('backslash', () => {
        let obj = { '\\': '\\' }
        let y = querystring.stringify(obj)
        expect(y).toEqual('%5C=%5C')
    })

    test('colon', () => {
        let obj = inpobj(':')
        let y = querystring.stringify(obj)
        expect(y).toEqual(resultstr('3A'))
    })

    test('tidle', () => {
        let obj = { '~': '~' }
        let y = querystring.stringify(obj)
        expect(y).toEqual('~=~')
    })

})
