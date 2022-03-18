import { of } from 'rxjs'
import { mergeMap } from 'rxjs/operators'
import { queryStringify } from 'unquoted-json'
import { httpGetJson } from './httpGetJson'

const mainUrl = action => "http://localhost:5153/" + action

export function subscriber(action, obj = {}) {
    return click$ =>
        click$
        |> mergeMap(() => {
            let url = mainUrl(action) + queryStringify(obj)
            return of(url)
        })
        |> mergeMap(url => httpGetJson(url))
        |> (o => o.subscribe(console.log))
}
