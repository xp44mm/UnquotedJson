import { queryStringify } from './queryStringify'

test('queryStringify', () => {
    let obj = { foo: 'bar', baz: ['qux', 'quux'], corge: '' }
    let y = queryStringify(obj)
    expect(y).toEqual("?foo=bar&baz=[qux,quux]")
})

describe('queryStringify ignore', () => {
    test('undefined', () => {
        let obj = { foo: undefined }
        let y = queryStringify(obj)
        expect(y).toEqual('')
    })

    test('null', () => {
        let obj = { foo: null }
        let y = queryStringify(obj)
        expect(y).toEqual('')
    })

    test('NaN', () => {
        let obj = { foo: NaN }
        let y = queryStringify(obj)
        expect(y).toEqual('')
    })

    test('Infinity', () => {
        let obj = { foo: Infinity, bar: -Infinity }
        let y = queryStringify(obj)
        expect(y).toEqual('')
    })

    test('function', () => {
        let obj = { foo: x => x }
        let y = queryStringify(obj)
        expect(y).toEqual('')
    })

    test('Symbol', () => {
        let obj = { foo: Symbol() }
        let y = queryStringify(obj)
        expect(y).toEqual('')
    })

    test('empty string', () => {
        let obj = { foo: '' }
        let y = queryStringify(obj)
        expect(y).toEqual('')
    })
})

describe('queryStringify real', () => {

    test('boolean', () => {
        let obj = { foo: true, bar: false }
        let y = queryStringify(obj)
        expect(y).toEqual('?foo=true&bar=false')
    })

    test('bigint', () => {
        let obj = { foo: 0n, bar: 1n }
        let y = queryStringify(obj)
        expect(y).toEqual('?foo=0&bar=1')
    })

    test('number 0', () => {
        let obj = { foo: 1 / Infinity, bar: -1 / Infinity }
        let y = queryStringify(obj)
        expect(y).toEqual('?foo=0&bar=0')
    })

    test('string', () => {
        let obj = { foo: 'test' }
        let y = queryStringify(obj)
        expect(y).toEqual('?foo=test')
    })

    test('object', () => {
        let obj = { foo: {} }
        let y = queryStringify(obj)
        expect(y).toEqual('?foo={}')
    })

    test('array', () => {
        let obj = { foo: [] }
        let y = queryStringify(obj)
        expect(y).toEqual('?foo=[]')
    })

    test('encode data', () => {
        const data = {
            name: 'this is a test',
            inlet: {
                SO2: 4273.11,
                SO3: 45.35924,
                'CaSO4*(1/2)H2O': 49.79,
                HF: 38.48,
                ash: 'NO',
            },
            effect: [96, 30, 95, 95, 85],
            'CaSO4*(1/2)H2O': -1,
            cleanLeakage: -1,
        }

        let res = queryStringify(data)
        expect(res).toBe("?name=this%20is%20a%20test&inlet={SO2:4273.11,SO3:45.35924,CaSO4*(1/2)H2O:49.79,HF:38.48,ash:NO}&effect=[96,30,95,95,85]&CaSO4*(1/2)H2O=-1&cleanLeakage=-1")
    })

})

