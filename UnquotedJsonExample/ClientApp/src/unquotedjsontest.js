import { button, div, ul } from 'hyperscript-rxjs'
import { subscriber } from './subscriber'

export function unquotedjsontest() {
    let root = div(
        button('emptyParam'),
        button('parseField'),
        button('apostrophe'),
        button('SectionInput'),
        button('tuple'),

        ul()
    )

    root.childNodes[0].pipeEvent('click',
        subscriber('unquotedjson/emptyParam', {
            a: undefined,
            b: null,
            c: NaN,
        })
    )

    root.childNodes[1].pipeEvent('click',
        subscriber('unquotedjson/parseField', {
            section: { span: 5000, pipeNumber: 0, pipeSpec: "'Φ76×6'" },
            panel: { t: 6, ribSpec: '[16a' },
        })
    )

    root.childNodes[2].pipeEvent('click',
        subscriber('unquotedjson/apostrophe', {
            a: [''],
            b: ["'"],
            c: ["\\"],
            d: ["\f"],
            e: ["\u0002"],
            f: ["char"],
        })
    )

    root.childNodes[3].pipeEvent('click',
        subscriber('unquotedjson/SectionInput', {
            pipeNumber: 0, pipeSpec: 'Φ76×6', span: 0
        })
    )

    root.childNodes[4].pipeEvent('click',
        subscriber('unquotedjson/tuple', {})
    )

    return root
}
