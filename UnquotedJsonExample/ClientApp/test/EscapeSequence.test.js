
//https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/JSON
//https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/String
describe('escape notation in json', () => {

    test('single slash', () => {
        let x = "/"
        let y = JSON.stringify(x)
        expect(y).toBe(['"', "/", '"'].join('')) //不转义
    })

    test('single apostrophe', () => {
        let x = "'"
        let y = JSON.stringify(x)
        expect(y).toBe(['"', "'", '"'].join('')) //不转义
    })

    test('single quote', () => {
        let x = '"'
        let y = JSON.stringify(x)
        expect(y).toBe(['"', '\\', '"', '"'].join(''))
    })

    test('escape backslash', () => {
        let x = '\\'
        let y = JSON.stringify(x)
        expect(y).toBe(['"', '\\', '\\', '"'].join(''))
    })

    test('escape newline', () => {
        let x = '\n'
        let y = JSON.stringify(x)
        expect(y).toBe(['"', '\\', 'n', '"'].join(''))
    })

    test('escape carriage return', () => {
        let x = '\r'
        let y = JSON.stringify(x)
        expect(y).toBe(['"', '\\', 'r', '"'].join(''))
    })

    test('escape vertical tab', () => {
        let x = '\v'
        let y = JSON.stringify(x)
        expect(y).toBe(['"', '\\', 'u000b', '"'].join('')) //转义为unicode格式。
    })

    test('escape tab', () => {
        let x = '\t'
        let y = JSON.stringify(x)
        expect(y).toBe(['"', '\\', 't', '"'].join(''))
    })

    test('escape backspace', () => {
        let x = '\b'
        let y = JSON.stringify(x)
        expect(y).toBe(['"', '\\', 'b', '"'].join(''))
    })

    test('escape form feed', () => {
        let x = '\f'
        let y = JSON.stringify(x)
        expect(y).toBe(['"', '\\', 'f', '"'].join(''))
    })

})


//https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/String
describe('char code', () => {

    test('single slash', () => {
        let x = "/"
        let y = x.charCodeAt(0)
        expect(y).toBe(47)
    })

    test('single apostrophe', () => {
        let x = "'"
        let y = x.charCodeAt(0)
        expect(y).toBe(39) //不转义
    })

    test('single quote', () => {
        let x = '"'
        let y = x.charCodeAt(0)
        expect(y).toBe(34)
    })

    test('escape backslash', () => {
        let x = '\\'
        let y = x.charCodeAt(0)
        expect(y).toBe(92)
    })

    test('escape newline', () => {
        let x = '\n'
        let y = x.charCodeAt(0)
        expect(y).toBe(10)
    })

    test('escape carriage return', () => {
        let x = '\r'
        let y = x.charCodeAt(0)
        expect(y).toBe(13)
    })

    test('escape vertical tab', () => {
        let x = '\v'
        let y = x.charCodeAt(0)
        expect(y).toBe(11) //转义为unicode格式。
    })

    test('escape tab', () => {
        let x = '\t'
        let y = x.charCodeAt(0)
        expect(y).toBe(9)
    })

    test('escape backspace', () => {
        let x = '\b'
        let y = x.charCodeAt(0)
        expect(y).toBe(8)
    })

    test('escape form feed', () => {
        let x = '\f'
        let y = x.charCodeAt(0)
        expect(y).toBe(12)
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

