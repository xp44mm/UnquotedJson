import { stringifyKey } from './stringifyKey'

export const stringifyStringValue = x => {
    if (x === "true" || x === "false" || x === "null") 
        return JSON.stringify(x)
    else if (/^[-+]?\d+(\.\d+)?([eE][-+]?\d+)?$/.test(x)) 
        return JSON.stringify(x)
    else
        return stringifyKey(x)
}
