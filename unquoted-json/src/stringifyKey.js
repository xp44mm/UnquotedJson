export const stringifyKey = x => {
    if (x === "" ||
        /(^\u0020)|[,:{}[\]"\u0000-\u001F\u007F]|(\u0020$)/.test(x)
    )
        return JSON.stringify(x)
    else
        return x
}
