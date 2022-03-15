
describe('encodeURI', () => {
    test('Reserved Characters', () => {
        let y = encodeURI(";,/?:@&=+$")
        expect(y).toEqual(";,/?:@&=+$")
    })

    test('Unescaped Characters', () => {
        let y = encodeURI("-_.!~*'()")
        expect(y).toEqual("-_.!~*'()")
    })

    test('Number Sign', () => {
        let y = encodeURI("#")
        expect(y).toEqual("#")
    })

    test('Alphanumeric Characters + Space', () => {
        let y = encodeURI("ABC abc 123")
        expect(y).toEqual("ABC%20abc%20123")
    })

})


describe('encodeURIComponent', () => {
    test('Reserved Characters', () => {
        let y = encodeURIComponent(";,/?:@&=+$")
        expect(y).toEqual("%3B%2C%2F%3F%3A%40%26%3D%2B%24")
    })

    test('Unescaped Characters', () => {
        let y = encodeURIComponent("-_.!~*'()")
        expect(y).toEqual("-_.!~*'()")
    })

    test('Unescaped backslash', () => {
        let y = encodeURIComponent("\\")
        expect(y).toEqual("%5C")
    })

    test('Number Sign', () => {
        let y = encodeURIComponent("#")
        expect(y).toEqual("%23")
    })

    test('Alphanumeric Characters + Space', () => {
        let y = encodeURIComponent("ABC abc 123")
        expect(y).toEqual("ABC%20abc%20123")
    })

})


