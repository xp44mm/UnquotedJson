// utf8编码是一个整数数组。uft8转换为pctencode
export function pctEncodeChar(utf8) {
    //c= code
    return utf8
        .map(i => {
            let f = i.toString(16).toUpperCase()
            let ff = i < 0x10 ? '0' + f : f
            return '%' + ff
        })
        .join('')
}
