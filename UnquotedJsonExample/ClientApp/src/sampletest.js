import { button, div, ul } from 'hyperscript-rxjs'
import { subscriber } from './subscriber'

export function sampletest() {
    let root = div(
        button('pairs'),
        ul()
    )

    root.childNodes[0].pipeEvent('click',
        subscriber('sample/pairs', {
            unreserved: "!$()*,-./:;?@[\\]_{}~\"'"
        })
    )


    return root
}
