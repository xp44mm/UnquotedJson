import { queryPctEncode } from './queryPctEncode'


test('example', () => {
    let x = 'a+=1'
    let y = queryPctEncode(x)
    let e = "a%2B%3D1"
    expect(y).toEqual(e)
})

describe('encode tests', () => {

    test('pctEncode', () => {
        let m = '\u0007'
        let y = queryPctEncode(m)
        expect(y).toEqual('%07')
    })

    test('material pipe', () => {
        let m = 'Φ76×6'
        let y = queryPctEncode(m)
        expect(y).toEqual('%CE%A676%C3%976')
    })

})

