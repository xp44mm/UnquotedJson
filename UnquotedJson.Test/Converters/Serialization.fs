module UnquotedJson.Converters.Serialization
open UnquotedJson

let serialize<'t> =
    JSON.read<'t> >> 
    JSON.stringifyNormalJson

let deserialize<'t> =
    JSON.parse >> 
    JSON.write<'t>