import { pctEncodeChar } from './pctEncodeChar'

describe('pctEncodeChar', () => {
    test('single1', () => {
        let x = [0]
        let y = pctEncodeChar(x)
        let e = '%00'
        expect(e).toEqual(y)
    })

    test('single2', () => {
        let x = [127]
        let y = pctEncodeChar(x)
        let e = '%7F'
        expect(e).toEqual(y)
    })


    test('double', () => {
        expect(pctEncodeChar([194, 128])).toEqual('%C2%80')
        expect(pctEncodeChar([223, 191])).toEqual('%DF%BF')
    })

    test('triple', () => {
        expect(pctEncodeChar([228, 184, 173])).toEqual('%E4%B8%AD')
    })


})
