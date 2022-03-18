namespace UnquotedJsonExample

type SectionInput() =
    member val span       = 5000.     with get, set
    member val pipeNumber = 0         with get, set
    member val pipeSpec   = "Φ76×6" with get, set

type PanelInput() =
    member val t = 6.0 with get, set
    member val ribSpec = "[16a" with get, set

type HorizDuctInput =
    {
        temperature : float
        material    : string[]
        horizontal  : SectionInput
        penels      : PanelInput[]
    }
