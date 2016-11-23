(*
Homework 9 (27.04.2015)
Tasks 46 - 49.

Author: Mikhail Kita, group 171
*)

module Program

open System.Threading
open Calc


// The 46th task

// Finds max element in a given range
let maxInRange (arr : int []) left right =
    let mutable result = arr.[left]
    for i = left + 1 to right do
        if arr.[i] > result then result <- arr.[i]
    result

// Finds max element in the array
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


// The 47th task

// Finds value of a defined integral from left to right border
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

// Finds value of a defined integral on the whole interval
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


// The 48th task

// Multiplies two matrix in a given range
let multiplicationInRange (res : int [][]) (m1 : int [][]) (m2 : int [][]) left right =
    let n = m1.[0].Length
    let l = m2.[0].Length

    for i = left to right do
        for j = 0 to l - 1 do
            let mutable temp = 0
            for k = 0 to n - 1 do
                temp <- temp + m1.[i].[k] * m2.[k].[j]
            res.[i].[j] <- temp

// Multiplies two matrix
let matrixMultiplication (matrix1 : int [][]) (matrix2 : int [][]) threadNumber =
    let n    = matrix1.Length
    let step = n / threadNumber
    let res  = Array.init (matrix1.Length) (fun i ->
        Array.init (matrix2.[0].Length) (fun j -> 0))

    let threadArray = Array.init threadNumber (fun i ->
        new Thread(ThreadStart(fun _ ->
            multiplicationInRange res matrix1 matrix2 (i * step) ((i + 1) * step - 1)
        ))
    )
    
    for t in threadArray do t.Start()
    for t in threadArray do t.Join()

    if (threadNumber * step) < n then
        multiplicationInRange res matrix1 matrix2 (threadNumber * step) (n - 1)
    res


// The 49th task

// Merges two parts of the array
let merge (arr : int []) start1 start2 n =
    let mutable p1  = start1
    let mutable p2  = start2
    let result = new Stack<int>()

    for i = 0 to n - 1 do
        if p2 < start1 + n then
            if arr.[p1] < arr.[p2] then
                if p1 < start2 then
                    result.push(arr.[p1])
                    p1 <- p1 + 1
                else
                    result.push(arr.[p2])
                    p2 <- p2 + 1
            else
                result.push(arr.[p2])
                p2 <- p2 + 1
        else
            result.push(arr.[p1])
            p1 <- p1 + 1
    
    let mutable i = n - 1 + start1
    while not result.isEmpty do
        arr.[i] <- result.pop()
        i <- i - 1

// Sorts a given interval in the array by merging
let sortInRange (arr : int []) left right =
    let n = right - left + 1
    let mutable pow = 2
    
    while pow <= n - 1 do
        let m = n / pow
        for i = 0 to m - 1 do
            merge arr (i * pow + left) (i * pow + pow / 2 + left) pow
        if (m * pow + left) <= right then
            merge arr ((m - 1) * pow + left) (m * pow + left) (pow + n - m * pow)
        pow <- pow * 2

    pow <- pow / 2
    if pow <= right then
        merge arr left (pow + left) n

// Sorts the array in ascending order
let mergeSort (arr : int []) threadNumber =
    let n    = arr.Length
    let step = n / threadNumber

    let threadArray = Array.init threadNumber (fun i ->
        new Thread(ThreadStart(fun _ ->
            sortInRange arr (i * step) ((i + 1) * step - 1)
        ))
    )
    
    for t in threadArray do t.Start()
    for t in threadArray do t.Join()

    for i = 0 to threadNumber - 2 do
        merge arr 0 ((i + 1) * step) ((i + 2) * step)

    if (threadNumber * step) < n then
        sortInRange arr (threadNumber * step) (n - 1)
        merge arr 0 (threadNumber * step) n
    arr


[<EntryPoint>]
let main argv =
    0