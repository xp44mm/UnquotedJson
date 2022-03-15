import { stringifyKey } from './stringifyKey'

test("stringifyKey empty", () => {
    let x = ""
    let y = stringifyKey(x)
    let e = '""'
    expect(y).toEqual(e)
})

test("stringifyKey control char", () => {
    let x = "\u0000"
    let y = stringifyKey(x)
    let e = '"\\u0000"'
    expect(y).toEqual(e)
})

test("stringifyKey space", () => {
    let x = " x"
    let y = stringifyKey(x)
    let e = '" x"'
    expect(y).toEqual(e)
})

test("stringifyKey keyword", () => {
    let x = "]"
    let y = stringifyKey(x)
    let e = '"]"'
    expect(y).toEqual(e)
})

test("stringifyKey normal", () => {
    let x = "abc"
    let y = stringifyKey(x)
    let e = x
    expect(y).toEqual(e)
})

test("stringifyKey null", () => {
    let x = "null"
    let y = stringifyKey(x)
    let e = x
    expect(y).toEqual(e)
})

test("stringifyKey 123", () => {
    let x = "-123.45"
    let y = stringifyKey(x)
    let e = x
    expect(y).toEqual(e)
})

