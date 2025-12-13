namespace Dictionary

[<AllowNullLiteral>]
type WordEntity() =
    member val Id = 0 with get, set
    member val Word = "" with get, set
    member val Definition = "" with get, set
    member val WordType = "" with get, set
    member val Example = "" with get, set
