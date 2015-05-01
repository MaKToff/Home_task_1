(*
Homework 9 (27.04.2015)
Tasks 46 - 49

Author: Mikhail Kita, group 171
*)

module Program

open System.Threading
open Calc

//the 46th task

//finds max element in given range
let maxInRange (arr : int []) left right =
    let mutable result = arr.[left]
    for i = left + 1 to right do
        if arr.[i] > result then result <- arr.[i]
    result

//finds max element in array
let maxElem (arr : int []) (threadNumber : int) =
    let step = arr.Length / threadNumber
    let result = ref arr.[0]
    
    let lock (elem : int ref) value =
        Monitor.Enter(elem);
        try
            if value > elem.Value then elem := value
        finally
            Monitor.Exit(elem)

    let threadArray = Array.init threadNumber (fun i ->
        new Thread(ThreadStart(fun _ ->
            let threadRes = maxInRange arr (i * step) ((i + 1) * step - 1)
            lock result threadRes
          ))
      )

    for t in threadArray do t.Start()
    for t in threadArray do t.Join()
    
    if (threadNumber * step - 1) < arr.Length - 1 then
        lock result (maxInRange arr (threadNumber * step) (arr.Length - 1))
    result.Value


//the 47th task

//finds value of defined integral from left to right border
let squareInRange expression left right precision =
    let mutable result = 0.0
    let mutable i = left
    let context = new Dictionary()
    
    let computeWithContext value =
        context.add "x" value
        compute expression context

    while i < right do
        let a = computeWithContext i
        let b = computeWithContext (i + precision)
        result <- result + ((a + b) / 2.0 * precision)
        i <- i + precision
    result

//finds value of defined integral on the whole interval
let definiteIntegral expression (threadNumber : int) left right precision =
    let n = right - left
    let step = n / (float threadNumber)
    let result = ref 0.0

    let lock (elem : float ref) value =
        Monitor.Enter(elem);
        try
            elem := elem.Value + value
        finally
            Monitor.Exit(elem)

    let l = ref left
    let r = ref left
    let threadArray = Array.init threadNumber (fun i ->
        new Thread(ThreadStart(fun _ ->
            l := r.Value
            r := r.Value + step
            let threadRes = squareInRange expression l.Value r.Value precision
            lock result threadRes
          ))
      )
    
    for t in threadArray do t.Start()
    for t in threadArray do t.Join()
    result.Value


//the 49th task

//sorts given interval in array by merging
let sortInRange (arr : int []) start1 start2 n =
    let mutable p1  = start1
    let mutable p2  = start2
    let mutable res = []

    for i = 0 to n - 1 do
        if p2 < start1 + n then
            if arr.[p1] < arr.[p2] then
                if p1 < start2 then
                    res <- List.append res [arr.[p1]]
                    p1 <- p1 + 1
                else
                    res <- List.append res [arr.[p2]]
                    p2 <- p2 + 1
            else
                res <- List.append res [arr.[p2]]
                p2 <- p2 + 1
        else
            res <- List.append res [arr.[p1]]
            p1 <- p1 + 1
    
    for i = 0 to res.Length - 1 do
        arr.[i + start1] <- res.[i]

//sorts array in ascending order
let mergeSort (arr : int []) =
    let n    = arr.Length
    let step = ref 2
    
    while step.Value < n do
        let start = ref -step.Value
        let threadArray = Array.init (n / step.Value) (fun i ->
            new Thread(ThreadStart(fun _ ->
                start := start.Value + step.Value
                sortInRange arr start.Value (start.Value + step.Value / 2) step.Value
            ))
        )
    
        for t in threadArray do t.Start()
        for t in threadArray do t.Join()
        step := step.Value * 2
    
    step := step.Value / 2
    if (step.Value) < n then 
        sortInRange arr 0 step.Value n
    arr


[<EntryPoint>]
let main argv =
    0