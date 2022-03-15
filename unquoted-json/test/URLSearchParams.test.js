describe('Easy URL manipulation with URLSearchParams', () => {
    test('extract parameter values', () => {
        // Can also constructor from another URLSearchParams
        let params = new URLSearchParams('q=search+string&version=1&person=Eric');

        expect(params.get('q')).toEqual("search string")
        expect(params.get('version')).toEqual("1")
        expect(Array.from(params).length).toEqual(3)

    })

    test('Working with URLs', () => {
        let url = new URL('https://example.com?foo=1&bar=2');
        let params = new URLSearchParams(url.search.slice(1));
        params.set('baz', 3);

        expect(params.has('baz')).toEqual(true)
        expect(params.toString()).toEqual( 'foo=1&bar=2&baz=3')

    })

    test('MDN Examples', () => {
        let paramsString = "q=URLUtils.searchParams&topic=api";
        let searchParams = new URLSearchParams(paramsString);


        expect(searchParams.has("topic")).toEqual( true)
        expect(searchParams.get("topic")).toEqual( "api")
        expect(searchParams.getAll("topic")).toEqual(["api"])

        expect(searchParams.get("foo")).toEqual(null)

        searchParams.append("topic", "webdev")
        expect(searchParams.toString()).toEqual("q=URLUtils.searchParams&topic=api&topic=webdev")

        searchParams.set("topic", "More webdev")
        expect(searchParams.toString()).toEqual("q=URLUtils.searchParams&topic=More+webdev")

        searchParams.delete("topic")
        expect(searchParams.toString()).toEqual("q=URLUtils.searchParams")
    })

    test('MDN Gotchas 1', () => {
        let paramsString1 = "http://example.com/search?query=%40";
        let searchParams1 = new URLSearchParams(paramsString1);

        expect(searchParams1.has("query")).toEqual(false)
        expect(searchParams1.has("http://example.com/search?query")).toEqual(true)

        expect(searchParams1.get("query")).toBeNull() // null
        expect(searchParams1.get("http://example.com/search?query")).toEqual("@") // "@" (equivalent to decodeURIComponent('%40'))
    })

    test('MDN Gotchas 2', () => {
        let paramsString2 = "?query=value";
        let searchParams2 = new URLSearchParams(paramsString2);
        expect(searchParams2.has("query")).toEqual(true); // true
    })

    test('MDN Gotchas 3', () => {
        let url = new URL("http://example.com/search?query=%40");
        let searchParams3 = new URLSearchParams(url.search);
        expect(searchParams3.has("query")).toEqual(true); // true
    })

    test('sequence of names/values pairs', () => {
        let pairs = [['q', 'URLUtils.searchParams'], ['topic', 'api']]
        let paramsString = "q=URLUtils.searchParams&topic=api"
        expect(new URLSearchParams(pairs).toString()).toEqual(paramsString)

    })
    

})

