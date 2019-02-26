type Disk =
| HardDisk of RPM:int * Platters:int
| SolidState
| MMC of NumberOfPins:int

let hardDisk = HardDisk (250, 7)
let mmcDisk = MMC 5
let ssdDisk = SolidState

let describe disk =
    match disk with
    | SolidState         -> "I'm a newfangled SSD."
    | MMC 1              -> "I have only 1 pin."
    | MMC p when p < 5   -> "I'm an MMC with a few pins."
    | MMC p      -> sprintf "I'm an MMC with %d pins." p
    | HardDisk (5400, _) -> "I'm a slow hard disk."
    | HardDisk (_, 7)    -> "I have 7 spindles!"
    | HardDisk _         -> "I'm a hard disk."

describe SolidState

// Print a discriminated union
printfn "%A" hardDisk

// Final exercise
type DataSet =
    { Name : string }

type PlanType =
| Plan
| PlanSum

type Plan =
    { Name : string
      Type : PlanType }

type Goal =
    { Metric : string
      Plan : Plan }

type Directive =
    { Name : string
      DataSets : DataSet list
      Plans : Plan list
      Goals : Goal list }