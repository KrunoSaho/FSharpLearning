type Comms = System.Collections.Concurrent.ConcurrentQueue<string>

let printer (object: obj) =
    let myChan, otherChan, msgToSend = object :?> Comms*Comms*string
    let value = ref ""
    
    let rng = System.Random()
    let dataRanges = [100..200..1000]
    let selectValue = fun () -> dataRanges.[rng.Next(0, dataRanges.Length)]
    
    while true do
        myChan.Enqueue(msgToSend)
        if otherChan.TryDequeue(value) then
            printfn "%A" msgToSend
        System.Threading.Thread.Sleep(selectValue ())
    ()

let c0 = System.Collections.Concurrent.ConcurrentQueue<string>()
let c1 = System.Collections.Concurrent.ConcurrentQueue<string>()
let t0 = System.Threading.Thread(System.Threading.ParameterizedThreadStart(printer))
let t1 = System.Threading.Thread(System.Threading.ParameterizedThreadStart(printer))
t1.Start((c1, c0, "bat"))
t0.Start((c0, c1, "cat"))
