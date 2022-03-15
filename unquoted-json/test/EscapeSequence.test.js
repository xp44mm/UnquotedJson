
//https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/JSON
//https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/String
describe('escape notation in json', () => {

    test('single slash', () => {
        let x = "/"
        let y = JSON.stringify(x)
        expect(y).toEqual(['"', "/", '"'].join('')) //不转义
    })

    test('single apostrophe', () => {
        let x = "'"
        let y = JSON.stringify(x)
        expect(y).toEqual(['"', "'", '"'].join('')) //不转义
    })

    test('single quote', () => {
        let x = '"'
        let y = JSON.stringify(x)
        expect(y).toEqual(['"', '\\', '"', '"'].join(''))
    })

    test('escape backslash', () => {
        let x = '\\'
        let y = JSON.stringify(x)
        expect(y).toEqual(['"', '\\', '\\', '"'].join(''))
    })

    test('escape newline', () => {
        let x = '\n'
        let y = JSON.stringify(x)
        expect(y).toEqual(['"', '\\', 'n', '"'].join(''))
    })

    test('escape carriage return', () => {
        let x = '\r'
        let y = JSON.stringify(x)
        expect(y).toEqual(['"', '\\', 'r', '"'].join(''))
    })

    test('escape vertical tab', () => {
        let x = '\v'
        let y = JSON.stringify(x)
        expect(y).toEqual(['"', '\\', 'u000b', '"'].join('')) //转义为unicode格式。
    })

    test('escape tab', () => {
        let x = '\t'
        let y = JSON.stringify(x)
        expect(y).toEqual(['"', '\\', 't', '"'].join(''))
    })

    test('escape backspace', () => {
        let x = '\b'
        let y = JSON.stringify(x)
        expect(y).toEqual(['"', '\\', 'b', '"'].join(''))
    })

    test('escape form feed', () => {
        let x = '\f'
        let y = JSON.stringify(x)
        expect(y).toEqual(['"', '\\', 'f', '"'].join(''))
    })

})


//https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/String
describe('char code', () => {

    test('single slash', () => {
        let x = "/"
        let y = x.charCodeAt(0)
        expect(y).toEqual(47)
    })

    test('single apostrophe', () => {
        let x = "'"
        let y = x.charCodeAt(0)
        expect(y).toEqual(39)
    })

    test('single quote', () => {
        let x = '"'
        let y = x.charCodeAt(0)
        expect(y).toEqual(34)
    })

    test('escape backslash', () => {
        let x = '\\'
        let y = x.charCodeAt(0)
        expect(y).toEqual(92)
    })

    test('escape newline', () => {
        let x = '\n'
        let y = x.charCodeAt(0)
        expect(y).toEqual(10)
    })

    test('escape carriage return', () => {
        let x = '\r'
        let y = x.charCodeAt(0)
        expect(y).toEqual(13)
    })

    test('escape vertical tab', () => {
        let x = '\v'
        let y = x.charCodeAt(0)
        expect(y).toEqual(11)
    })

    test('escape tab', () => {
        let x = '\t'
        let y = x.charCodeAt(0)
        expect(y).toEqual(9)
    })

    test('escape backspace', () => {
        let x = '\b'
        let y = x.charCodeAt(0)
        expect(y).toEqual(8)
    })

    test('escape form feed', () => {
        let x = '\f'
        let y = x.charCodeAt(0)
        expect(y).toEqual(12)
    })

})



describe('ASCII control characters (character code 0-31)', () => {
    let xs =
        [...Array(32).keys()]
            .map((v, i) => {
                let x = String.fromCharCode(i)
                return x
            })

    test('control characters 0', () => {
        
    })


})

