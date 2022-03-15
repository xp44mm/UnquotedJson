import { stringifyStringValue } from './stringifyStringValue'

test("stringifyStringValue empty", () => {
    let x = ""
    let y = stringifyStringValue(x)
    let e = '""'
    expect(y).toEqual(e)
})

test("stringifyStringValue control char", () => {
    let x = "\u0000"
    let y = stringifyStringValue(x)
    let e = '"\\u0000"'
    expect(y).toEqual(e)
})

test("stringifyStringValue space", () => {
    let x = " x"
    let y = stringifyStringValue(x)
    let e = '" x"'
    expect(y).toEqual(e)
})

test("stringifyStringValue keyword", () => {
    let x = "]"
    let y = stringifyStringValue(x)
    let e = '"]"'
    expect(y).toEqual(e)
})

test("stringifyStringValue normal", () => {
    let x = "abc"
    let y = stringifyStringValue(x)
    let e = x
    expect(y).toEqual(e)
})

test("stringifyStringValue null", () => {
    let x = "null"
    let y = stringifyStringValue(x)
    let e = '"null"'
    expect(y).toEqual(e)
})

test("stringifyStringValue 123", () => {
    let x = "-123.45"
    let y = stringifyStringValue(x)
    let e = '"-123.45"'
    expect(y).toEqual(e)
})


