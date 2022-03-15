import { stringifyKey } from './stringifyKey'
import { stringifyStringValue } from './stringifyStringValue'

export function unquotedJsonStringify(value) {
    if (value === null) {
        return 'null'
    } else if (typeof value === 'string') {
        return stringifyStringValue(value)
    } else if (typeof value === 'number') {
        return isFinite(value) ? value.toString() : 'null'
    } else if (typeof value === 'boolean') {
        return value ? 'true' : 'false'
    } else if (typeof value === 'bigint') {
        return value.toString()
    } else if (Array.isArray(value)) {
        let elems = value.map(e => unquotedJsonStringify(e)).join(',')
        return '[' + elems + ']'
    } else if (typeof value === 'object') {
        let fields = Object.entries(value)
            .map(([k, v]) => stringifyKey(k) + ':' + unquotedJsonStringify(v))
            .join(',')
        return '{' + fields + '}'
    } else if (typeof value === 'undefined') {
        return 'null'
    } else if (typeof value === 'function') {
        return 'null'
    } else if (typeof value === 'symbol') {
        return 'null'
    } else {
        return 'null'
    }
}




