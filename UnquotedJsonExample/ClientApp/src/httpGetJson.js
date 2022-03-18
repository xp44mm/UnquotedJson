import { fromFetch } from 'rxjs/fetch'
import { mergeMap } from 'rxjs/operators'

export function httpGetJson(url) {
    return (
        fromFetch(url, { method: 'GET' })
        |> mergeMap(response => {
            if (response.ok) {
                // OK return data
                return response.json()
            } else {
                // Server is returning a status requiring the client to try something else.
                throw new Error(`response not ok: ${response.status}`)
            }
        })
        //|> catchError(err => {
        //    // Network or other error, handle appropriately
        //    return of({ error: true, message: err.message })
        //})
    )
}
