(*
Homework 9 (27.04.2015)
Tests for tasks 46 - 49

Author: Mikhail Kita, group 171
*)

module Tests

open NUnit.Framework
open Program

//tests for 46th task
[<TestCase ([|7|], 7, 1)>]
[<TestCase ([|1; -2; 3; -4; 9; -5; 6; -7; 8; 0|], 9, 10)>]
[<TestCase ([|-1; -2; -3; -4; -5; -6; -7; -8; -9|], -1, 9)>]
let ``Test for task #46 01`` (arr : int []) value n =
    for i = 1 to n do
        Assert.AreEqual(value, maxElem arr i)

[<Test>]
let ``Test for task #46 02`` () =
    let arr = Array.init 100 (fun i -> 1)
    for i = 1 to 100 do
        Assert.AreEqual(1, maxElem arr i)

[<Test>]
let ``Test for task #46 03`` () =
    let n = 100000
    let arr = Array.init n (fun i -> System.Random(0).Next(0, n))
    for i = 1 to 100 do
        Assert.AreEqual(72624, maxElem arr i)


//tests for 47th task
[<TestCase ("x", -5.0, 7.0, 0.0001, 2, 12.0)>]
[<TestCase ("x", -5.0, 5.0, 0.0001, 3, 0.0)>]
[<TestCase ("5 * ln x", 1.0, 5.0, 0.0001, 2, 20.24)>]
[<TestCase ("x ^ 2 + 3", 0.0, 3.0, 0.0001, 2, 18.0)>]
[<TestCase ("sin x + cos x", 0.0, System.Math.PI, 0.0001, 3, 2.0)>]
let ``Test for task #47 01`` expression left right precision (digits : int) answer =
    for i = 1 to 10 do
        let result = definiteIntegral expression i left right precision
        Assert.AreEqual(answer, (System.Math.Round(result, digits)))


//tests for 49th task
[<TestCase ([|3|], Result = [|3|])>]
[<TestCase ([|1; 2; 3|], Result = [|1; 2; 3|])>]
[<TestCase ([|-1; 4; -17; 3; 8|], Result = [|-17; -1; 3; 4; 8;|])>]
[<TestCase ([|9; 2; 3; 8; 5; 7; 4; 6; 1|], Result = [|1; 2; 3; 4; 5; 6; 7; 8; 9|])>]
let ``Test for task #49 01`` (arr : int []) =
    mergeSort arr

[<Test>]
let ``Test for task #49 02`` () =
    let arr = Array.init 1000 (fun i -> i)
    Assert.AreEqual(arr, mergeSort arr)