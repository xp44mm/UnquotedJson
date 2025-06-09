module AspNetCore.UnquotedJson.Query

let firstOrDefault sv = sv |> Seq.tryHead |> Option.defaultValue ""

let render sv = sv |> String.concat ""

let toPair (KeyValue(k,v)) = k, firstOrDefault v

let toPairs sq = Seq.map toPair sq
