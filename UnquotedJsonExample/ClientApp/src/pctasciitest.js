import { button, div, h3, ul } from 'hyperscript-rxjs'
import { from, of, timer, zip } from 'rxjs'
import { map, mergeMap } from 'rxjs/operators'
import { httpGetJson } from './httpGetJson'

export function pctasciitest() {
    let source = from(
        Array.from({ length: 128 - 32 }).map((v, i) => {
            let code = i + 32
            let char = String.fromCharCode(code)
            let printcode = code.toString(16).toUpperCase()
            return { code, char, printcode }
        })
    )

    let root = div(
        h3('pct ascii test'),
        button('开始测试').pipeEvent(
            'click',
            click$ =>
                click$
                |> mergeMap(
                    () =>
                        zip(source, timer(0, 50))
                        |> map(([{ char, printcode }]) => 'http://localhost:5153/sample/pairs?' + `${char}=${printcode}`)
                        |> mergeMap(url => of(url))
                        |> mergeMap(url => httpGetJson(url))
                )
                |> (o => o.subscribe(console.log))
        ),

        ul()
    )

    return root
}

