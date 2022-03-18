import { button, div, h3, ul } from 'hyperscript-rxjs'
import { from, of, timer, zip } from 'rxjs'
import { map, mergeMap } from 'rxjs/operators'
import { queryStringify } from 'unquoted-json'
import { httpGetJson } from './httpGetJson'

export function queryStringifyTest() {
    let source = from(
        Array.from({ length: 128 - 32 }).map((v, i) => {
            let code = i + 32
            let char = String.fromCharCode(code)
            return char
        })
    )

    let root = div(
        h3('queryStringify Test'),
        button('开始测试').pipeEvent(
            'click',
            click$ =>
                click$
                |> mergeMap(
                    () =>
                        zip(source, timer(0, 50))
                        //|> tap(console.log)
                        |> map(([char]) => queryStringify({ [char]: { [char]: char } }))
                        |> map(qs => 'http://localhost:5153/sample/pairs' + qs)
                        |> mergeMap(url => of(url))
                        |> mergeMap(url => httpGetJson(url))
                )
                |> (o => o.subscribe(console.log))
        ),

        ul()
    )

    return root
}

