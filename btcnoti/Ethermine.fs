namespace btcnoti

open Result
open Http
open FSharp.Data
open System.Text

module Ethermine =

    type private JsonType = JsonProvider<"""{"address":"6c65104506aa1d6a00eb45f2f0a1f0438e89cf90","hashRate":"145.1 MH/s","reportedHashRate":"154.3 MH/s","payouts":[],"workers":{"CanhQuangNghia":{"worker":"CanhQuangNghia","hashrate":"145.1 MH/s","validShares":126,"staleShares":7,"invalidShares":0,"workerLastSubmitTime":1497809342,"invalidShareRatio":0,"reportedHashRate":"154.3 MH/s"}},"settings":{"email":"","monitor":0,"name":"","minPayout":1,"vote":0,"ip":"*.*.*.122","voteip":""},"ethPerMin":0.0000067817074045871065,"usdPerMin":0.0024176108726612577,"btcPerMin":9.568989147872407e-7,"avgHashrate":18327932.09876543,"rounds":[{"id":2643747576,"miner":"6c65104506aa1d6a00eb45f2f0a1f0438e89cf90","block":3893951,"work":145,"amount":493624448931825,"processed":0},{"id":2643616012,"miner":"6c65104506aa1d6a00eb45f2f0a1f0438e89cf90","block":3893918,"work":143,"amount":551106517430180,"processed":0},{"id":2643485652,"miner":"6c65104506aa1d6a00eb45f2f0a1f0438e89cf90","block":3893884,"work":147,"amount":775033691978548,"processed":0},{"id":2643353936,"miner":"6c65104506aa1d6a00eb45f2f0a1f0438e89cf90","block":3893848,"work":144,"amount":634324982293233,"processed":0},{"id":2643222762,"miner":"6c65104506aa1d6a00eb45f2f0a1f0438e89cf90","block":3893817,"work":144,"amount":418117746265558,"processed":0},{"id":2643094032,"miner":"6c65104506aa1d6a00eb45f2f0a1f0438e89cf90","block":3893783,"work":155,"amount":323891856502716,"processed":0},{"id":2642963190,"miner":"6c65104506aa1d6a00eb45f2f0a1f0438e89cf90","block":3893752,"work":155,"amount":443764009704544,"processed":0},{"id":2642834274,"miner":"6c65104506aa1d6a00eb45f2f0a1f0438e89cf90","block":3893728,"work":166,"amount":493122687207586,"processed":0},{"id":2642703274,"miner":"6c65104506aa1d6a00eb45f2f0a1f0438e89cf90","block":3893703,"work":166,"amount":662202717030125,"processed":0},{"id":2642572156,"miner":"6c65104506aa1d6a00eb45f2f0a1f0438e89cf90","block":3893661,"work":166,"amount":292124328625624,"processed":0},{"id":2642442314,"miner":"6c65104506aa1d6a00eb45f2f0a1f0438e89cf90","block":3893630,"work":174,"amount":334760439879035,"processed":0},{"id":2642311220,"miner":"6c65104506aa1d6a00eb45f2f0a1f0438e89cf90","block":3893605,"work":174,"amount":459888273402386,"processed":0},{"id":2642180374,"miner":"6c65104506aa1d6a00eb45f2f0a1f0438e89cf90","block":3893578,"work":174,"amount":445215608401250,"processed":0},{"id":2642048916,"miner":"6c65104506aa1d6a00eb45f2f0a1f0438e89cf90","block":3893559,"work":172,"amount":307450316897460,"processed":0},{"id":2641913392,"miner":"6c65104506aa1d6a00eb45f2f0a1f0438e89cf90","block":3893534,"work":150,"amount":263838616221440,"processed":0},{"id":2641778098,"miner":"6c65104506aa1d6a00eb45f2f0a1f0438e89cf90","block":3893499,"work":130,"amount":619902249395100,"processed":0},{"id":2641647200,"miner":"6c65104506aa1d6a00eb45f2f0a1f0438e89cf90","block":3893460,"work":130,"amount":57863881334367,"processed":0},{"id":2641515920,"miner":"6c65104506aa1d6a00eb45f2f0a1f0438e89cf90","block":3893450,"work":130,"amount":231731113458510,"processed":0},{"id":2641380934,"miner":"6c65104506aa1d6a00eb45f2f0a1f0438e89cf90","block":3893420,"work":111,"amount":432222049849016,"processed":0},{"id":2641250058,"miner":"6c65104506aa1d6a00eb45f2f0a1f0438e89cf90","block":3893385,"work":111,"amount":199777429275267,"processed":0},{"id":2641113182,"miner":"6c65104506aa1d6a00eb45f2f0a1f0438e89cf90","block":3893357,"work":86,"amount":143211008758062,"processed":0},{"id":2640974726,"miner":"6c65104506aa1d6a00eb45f2f0a1f0438e89cf90","block":3893329,"work":56,"amount":73905845128940,"processed":0},{"id":2640843934,"miner":"6c65104506aa1d6a00eb45f2f0a1f0438e89cf90","block":3893317,"work":56,"amount":197439118575690,"processed":0},{"id":2640708228,"miner":"6c65104506aa1d6a00eb45f2f0a1f0438e89cf90","block":3893293,"work":42,"amount":207646038005380,"processed":0},{"id":2640566468,"miner":"6c65104506aa1d6a00eb45f2f0a1f0438e89cf90","block":3893254,"work":23,"amount":124289677250074,"processed":0},{"id":2640425654,"miner":"6c65104506aa1d6a00eb45f2f0a1f0438e89cf90","block":3893210,"work":9,"amount":45477059525131,"processed":0}],"unpaid":9231931711327048}""">

    let private formatUnpaid (x : int64) =
        float x / 1000000000000000000.0

    let getUnpaidRes proxyOp id =
        ("https://ethermine.org/api/miner_new/" + id)
        |> download proxyOp
        |> map JsonType.Parse
        |> map (fun data -> data.Unpaid |> formatUnpaid)

    let getUnpaid proxyOp id =
        match getUnpaidRes proxyOp id with
        | Ok unpaid -> Some unpaid
        | Error _ -> None
