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
    let arr = Array.init 1000 (fun i -> 1)
    for i = 1 to 100 do
        Assert.AreEqual(1, maxElem arr i)

[<Test>]
let ``Test for task #46 03`` () =
    let n = 1000000
    let arr = Array.init n (fun i -> System.Random(0).Next(0, n))
    for i = 1 to 100 do
        Assert.AreEqual(726243, maxElem arr i)


// | Test                                   | Time (1 thread) | Minimum time     |
// +========================================+=================+==================+
// | [|7|]                                  | 13ms            | 13ms (1 thread)  |
// | [|1; -2; 3; -4; 9; -5; 6; -7; 8; 0|]   | 16ms            | 14ms (2 threads) |
// | [|-1; -2; -3; -4; -5; -6; -7; -8; -9|] | 14ms            | 14ms (2 threads) |
// | Test for task #46 02                   | 20ms            | 15ms (2 threads) |
// | Test for task #46 03                   | 59ms            | 44ms (4 threads) |


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


// | Test                              | Time (1 thread) | Minimum time      |
// +===================================+=================+===================+
// | "x"             -5.0  7.0  0.0001 | 997ms           | 591ms (8 threads) |
// | "x"             -5.0  5.0  0.0001 | 834ms           | 526ms (2 threads) |
// | "5 * ln x"       1.0  5.0  0.0001 | 848ms           | 475ms (2 threads) |
// | "x ^ 2 + 3"      0.0  3.0  0.0001 | 914ms           | 500ms (4 threads) |
// | "sin x + cos x"  0.0  pi   0.0001 | 830ms           | 488ms (2 threads) |


//tests for 48th task
[<Test>]
let ``Test for task #48 01`` () =
    Assert.AreEqual([|[|35|]|], matrixMultiplication [|[|5|]|] [|[|7|]|] 1)

[<Test>]
let ``Test for task #48 02`` () =
    let matrix1 = Array.init 3 (fun i -> Array.init 2 (fun j -> i * 2 + j + 1))
    let matrix2 = Array.init 2 (fun i -> Array.init 4 (fun j -> i * 4 + j + 1))
    let result  = [|
        [|11; 14; 17; 20|];
        [|23; 30; 37; 44|];
        [|35; 46; 57; 68|];
    |]
    for i = 1 to 3 do
        Assert.AreEqual(result, matrixMultiplication matrix1 matrix2 i)

[<Test>]
let ``Test for task #48 03`` () =
    let matrix1 = [|
        [|-3; 8;  5;  7|];
        [| 2; 6; -4;  0|];
        [| 3; 9;  1; -1|];
    |]
    let matrix2 = [|
        [|-6; 8;  11;  3;  4|];
        [| 9; 6; -10;  0;  2|];
        [|18; 9;   3; 17;  5|];
        [| 2; 0;  -8;  3; 13|];
    |]
    let result  = [|
        [|194; 69; -154;  97; 120|];
        [|-30; 16;  -50; -62;   0|];
        [| 79; 87;  -46;  23;  22|];
    |]
    for i = 1 to 3 do
        Assert.AreEqual(result, matrixMultiplication matrix1 matrix2 i)

[<Test>]
let ``Test for task #48 04`` () =
    let matrix1 = Array.init 100 (fun i -> Array.init 1000 (fun j -> 1))
    let matrix2 = Array.init 1000 (fun i -> Array.init 100 (fun j -> 1))
    let result  = Array.init 100 (fun i -> Array.init 100 (fun j -> 1000))
    for i = 1 to 10 do
        Assert.AreEqual(result, matrixMultiplication matrix1 matrix2 i)

[<Test>]
let ``Test for task #48 05`` () =
    let matrix1 = Array.init 500 (fun i -> Array.init 500 (fun j -> 1))
    let matrix2 = Array.init 500 (fun i -> Array.init 500 (fun j -> 1))
    let result  = Array.init 500 (fun i -> Array.init 500 (fun j -> 500))
    Assert.AreEqual(result, matrixMultiplication matrix1 matrix2 1)
    Assert.AreEqual(result, matrixMultiplication matrix1 matrix2 2)
    Assert.AreEqual(result, matrixMultiplication matrix1 matrix2 4)


// | Test                 | Time (1 thread) | Minimum time      |
// +======================+=================+===================+
// | Test for task #48 01 | 15ms            | 15ms  (1 thread)  |
// | Test for task #48 02 | 19ms            | 17ms  (2 threads) |
// | Test for task #48 03 | 27ms            | 19ms  (2 threads) |
// | Test for task #48 04 | 159ms           | 86ms  (2 threads) |
// | Test for task #48 05 | 1860ms          | 942ms (2 threads) |


//tests for 49th task
[<TestCase ([|3|], [|3|])>]
[<TestCase ([|4; 3; 2; 1|], [|1; 2; 3; 4|])>]
[<TestCase ([|-1; 4; -17; 3; 8|], [|-17; -1; 3; 4; 8;|])>]
[<TestCase ([|9; 2; 3; 8; 5; 7; 4; 6; 1|], [|1; 2; 3; 4; 5; 6; 7; 8; 9|])>]
let ``Test for task #49 01`` (arr : int []) (result : int []) =
    for i = 1 to arr.Length do
        let temp = arr
        Assert.AreEqual(result, mergeSort temp i)

[<Test>]
let ``Test for task #49 02`` () =
    let arr = Array.init 5000 (fun i -> i)
    Assert.AreEqual(arr, mergeSort arr 1)
    Assert.AreEqual(arr, mergeSort arr 2)
    Assert.AreEqual(arr, mergeSort arr 4)


// | Test                          | Time (1 thread) | Minimum time      |
// +===============================+=================+===================+
// | [|3|]                         | 13ms            | 13ms  (1 thread)  |
// | [|4; 3; 2; 1|]                | 17ms            | 16ms  (2 threads) |
// | [|-1; 4; -17; 3; 8|]          | 19ms            | 17ms  (2 threads) |
// | [|9; 2; 3; 8; 5; 7; 4; 6; 1|] | 21ms            | 18ms  (2 threads) |
// | Test for task #49 02          | 1193ms          | 930ms (2 threads) |