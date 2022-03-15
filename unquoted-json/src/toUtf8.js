//utf16编码转换为utf8编码数组，utf16<0x10000
export function toUtf8(utf16) {
    if (utf16 < 0x80) {
        return [utf16]
    } else if (0x80 <= utf16 && utf16 < 0x800) {
        return [
            (utf16 >> 6) | 0xC0,
            (utf16 & 0x3F) | 0x80]
    } else {
        return [
            (utf16 >> 0xC) | 0xE0,
            ((utf16 >> 6) & 0x3F) | 0x80,
            (utf16 & 0x3F) | 0x80]
    }
}
