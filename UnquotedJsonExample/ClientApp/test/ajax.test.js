import { of } from 'rxjs'
import { map, mergeMap, tap } from 'rxjs/operators'
import { urlquery } from 'urljson-serializer'
import { httpGetJson } from '../src/httpGetJson'

const mainUrl = action => 'http://localhost:5000/Urljson/' + action

//运行测试前，需要启动服务器端。
describe('Ajax test', () => {
    test('emptyParam', done => {
        let obj = {
            a: undefined,
            b: null,
            c: NaN,
        }

        expect.assertions(2)

        of(urlquery(obj))
            |> tap(qs => expect(qs).toEqual(''))
            |> map(qs => mainUrl('emptyParam') + qs)
            |> tap(console.log)
            |> mergeMap(url => httpGetJson(url))
            |> (o =>
                o.subscribe({
                    next: response => {
                        console.log(response)
                        expect(response).toEqual([])
                    },
                    complete: done,
                }))
    })

    test('apostrophe', done => {
        let obj = {
            a: [''],
            b: ["'"],
            c: ['\\'],
            d: ['\f'],
            e: ['\u0002'],
            f: ['char'],
        }

        expect.assertions(2)

        of(urlquery(obj))
            |> tap(qs => expect(qs).toEqual("?a=[~~]&b=[~'~]&c=[~\\\\~]&d=[~\\f~]&e=[~\\02~]&f=[~char~]"))
            |> map(qs => mainUrl('apostrophe') + qs)
            |> //|> tap(console.log)
            mergeMap(url => httpGetJson(url))
            |> (o =>
                o.subscribe({
                    next: response => {
                        //console.log(response)
                        expect(response).toEqual(Object.entries(obj))
                    },
                    complete: done,
                }))
    })

    test('parseField', done => {
        let obj = {
            section: { span: 5000, pipeNumber: 0, pipeSpec: 'Φ76×6' },
            panel: { t: 6, ribSpec: '[16a' },
        }

        expect.assertions(2)
        of(urlquery(obj))
            |> tap(qs => expect(qs).toEqual('?section={span:5000,pipeNumber:0,pipeSpec:~Φ76×6~}&panel={t:6,ribSpec:~[16a~}'))
            |> map(qs => mainUrl('parseField') + qs)
            |> //|> tap(console.log)
            mergeMap(url => httpGetJson(url))
            |> (o =>
                o.subscribe({
                    next: response => {
                        //console.log(response)
                        expect(response).toEqual(obj)
                    },
                    complete: done,
                }))
    })

    test('SectionInput', done => {
        let obj = { pipeNumber: 0, pipeSpec: 'Φ76×6', span: 0 }
        expect.assertions(2)
        of(urlquery(obj))
            |> tap(qs => expect(qs).toEqual('?pipeNumber=0&pipeSpec=Φ76×6&span=0'))
            |> map(qs => mainUrl('SectionInput') + qs)
            |> //|> tap(console.log)
            mergeMap(url => httpGetJson(url))
            |> (o =>
                o.subscribe({
                    next: response => {
                        //console.log(response)
                        expect(response).toEqual(obj)
                    },
                    complete: done,
                }))
    })

    test('tuple', done => {
        of(mainUrl('tuple'))
            |> mergeMap(url => httpGetJson(url))
            |> (o =>
                o.subscribe({
                    next: response => {
                        //console.log(response)
                        expect(response).toEqual(['1', 1])
                    },
                    complete: done,
                }))
    })
})
