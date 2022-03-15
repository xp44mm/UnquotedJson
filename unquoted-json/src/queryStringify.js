import { queryPctEncode } from './queryPctEncode'
import { unquotedJsonStringify } from './unquotedJsonStringify'

const fieldtostr = v =>
    typeof v === 'string'
        ? v
        : typeof v === 'object'
            ? unquotedJsonStringify(v)
            : v.toString()

/// 第一层为名值对，其后序列化为压缩格式。
export function queryStringify(data) {
    //名值对的先决条件是普通对象
    if (!data || Array.isArray(data) || typeof data !== 'object') {
        console.error(data)
        throw new Error("input should be a plain object.")
    }

    let pairs = Object.entries(data).filter(
        ([k, v]) =>
            (typeof v === 'object' && v !== null)
            || typeof v === 'boolean'
            || Number.isFinite(v)
            || typeof v === 'bigint'
            || (typeof v === 'string' && v !== '')
    )

    if (pairs.length === 0) {
        return ''
    }

    // 如果字段是对象，包括数组，则序列化字段值，否则不变。
    return (
        '?' +
        pairs
            .map(([k, v]) => [k, fieldtostr(v)])
            .map(([k, v]) => queryPctEncode(k) + '=' + queryPctEncode(v))
            .join('&')
    )
}
