import { urlquery } from 'urljson-serializer'

test('urlquery', () => {
    let obj = { foo: 'bar', baz: ['qux', 'quux'], corge: '' }
    let y = urlquery(obj)
    expect(y).toEqual("?foo=bar&baz=[`qux`,`quux`]")
})

