import { of } from 'rxjs'
import { map, mergeMap, tap } from 'rxjs/operators'
import { urlquery } from 'urljson-serializer'
import { httpGetJson } from '../src/httpGetJson'

const mainUrl = (controller, action) => 'http://localhost:5000/' + controller + '/' + action

describe('Sample test', () => {
    test('pairs', done => {

        let obj = {
            unreserved: "!$()*,-./:;?@[\\]_{}~\"'"
        }

        of(urlquery(obj))
            |> tap(qs => {
                expect(qs).toEqual('?unreserved=' + obj.unreserved)
            })
            |> map(qs => mainUrl('sample', 'pairs') + qs)
            |> mergeMap(url => httpGetJson(url))
            |> (o => o.subscribe({
                next: response => {
                    //console.log(response)
                    expect(response).toEqual([['unreserved', obj.unreserved]])
                },
                complete: done
            }))
    })

    test('test1', done => {
        expect.assertions(2)

        let inp = {
            foo: 'bar',
            abc: ['xyz', '123']
        }

        of(urlquery(inp))
            |> tap(qs => {
                expect(qs).toEqual("?foo=bar&abc=[~xyz~,~123~]")
            })
            |> map(qs => mainUrl('sample', 'test1') + qs)
            |> mergeMap(url => httpGetJson(url))
            |> (o => o.subscribe({
                next: response => {
                    //console.log(response)
                    expect(response).toEqual(inp)
                },
                complete: done
            }))
    })

})
