import { unquotedJsonStringify } from './unquotedJsonStringify'

test("null", () => {
    let x = null
    let y = unquotedJsonStringify(x)
    let e = 'null'
    expect(y).toEqual(e)
})

test("string unquoted", () => {
    let x = "string"
    let y = unquotedJsonStringify(x)
    let e = x
    expect(y).toEqual(e)
})

test("string quoted", () => {
    let x = "true"
    let y = unquotedJsonStringify(x)
    let e = '"true"'
    expect(y).toEqual(e)
})


test("number", () => {
    let x = -123
    let y = unquotedJsonStringify(x)
    let e = '-123'
    expect(y).toEqual(e)
})

test("boolean", () => {
    let x = true
    let y = unquotedJsonStringify(x)
    let e = 'true'
    expect(y).toEqual(e)
})

test("array", () => {
    let x = []
    let y = unquotedJsonStringify(x)
    let e = '[]'
    expect(y).toEqual(e)
})

test("obj", () => {
    let x = {}
    let y = unquotedJsonStringify(x)
    let e = '{}'
    expect(y).toEqual(e)
})

