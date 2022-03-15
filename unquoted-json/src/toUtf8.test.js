import { toUtf8 } from './toUtf8'

describe('toUtf8', () => {
    test('single min', () => {
        let x = 0
        let y = toUtf8(x)
        let e = [0]
        expect(e).toEqual(y)
    })

    test('single max', () => {
        let x = 127
        let y = toUtf8(x)
        let e = [127]
        expect(e).toEqual(y)
    })

    
    test('double min', () => {
        let y = toUtf8(128)
        let e = [194, 128]
        expect(e).toEqual(y)
        expect(toUtf8(2047)).toEqual([223,191])
    })
    test('double max', () => {
        let y = toUtf8(2047)
        let e = [223,191]
        expect(e).toEqual(y)
    })

    test('triple', () => {
        let n = '中'.charCodeAt(0)        
        expect(n).toEqual(20013)
        let y = toUtf8(n)
        let e = [228, 184, 173]
        expect(e).toEqual(y)
    })


})
