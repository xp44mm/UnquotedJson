import { toUtf8 } from './toUtf8'
import { pctEncodeChar } from './pctEncodeChar'

///只对部分的符号进行pct编码，其余字符如有遗漏，浏览器会自动pct编码。
export function queryPctEncode(str) {
    return Array.from(str)
        .map(ch => [ch.charCodeAt(0), ch])
        .map(([utf16, ch]) => {
            if (
                utf16 <= 0x20 ||
                ch === '"' || //22
                ch === '#' || //23
                ch === '%' || //25
                ch === '&' || //26
                ch === "'" || //27
                ch === '+' || //2B
                ch === '<' || //3C
                ch === '=' || //3D
                ch === '>' || //3E
                (0x7F <= utf16 && utf16 <= 0xFFFF)
            ) {
                return pctEncodeChar(toUtf8(utf16))
            } else return ch
        })
        .join('')
}
