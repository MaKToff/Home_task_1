(*
Homework 9 (27.04.2015)
Tests for tasks 46 - 49.

Author: Mikhail Kita, group 171
*)

module Tests

open NUnit.Framework
open Program

// Tests for the 46th task
[<TestCase ([|1; -2; 3; -4; 9; -5; 6; -7; 8; 0|], 9)>]
[<TestCase ([|-100; -53; -1; -4; -21; -7; -9|], -1)>]
let ``Test for task #46 01`` (arr : int []) value =
    for i = 1 to arr.Length do
        Assert.AreEqual(value, maxElem arr i)

[<Test>]
let ``Test for task #46 02`` () =
    let n = 10000000
    let arr = Array.init n (fun i -> System.Random(0).Next(0, n))
    for i = 0 to 3 do
        Assert.AreEqual(7262432, maxElem arr (pown 2 i))

[<Test>]
let ``Test for task #46 03`` () =
    let arr = Array.init 100000000 (fun i -> -i)
    for i = 0 to 3 do
        Assert.AreEqual(0, maxElem arr (pown 2 i))

[<Test>]
let ``Test for task #46 04`` () =
    let arr = Array.init 200000000 (fun i -> 1)
    for i = 0 to 3 do
        Assert.AreEqual(1, maxElem arr (pown 2 i))


// | Test                                 | 1 thread | 2 threads | 4 threads | 8 threads |
// +======================================+==========+===========+===========+===========+
// | [|1; -2; 3; -4; 9; -5; 6; -7; 8; 0|] |   13 ms  |   6 ms    |  13 ms    |  25 ms    |
// | [|-100; -53; -1; -4; -21; -7; -9|]   |   14 ms  |   7 ms    |  12 ms    |  22 ms    |
// | Test for task #46 02                 |   78 ms  |  39 ms    |  43 ms    |  63 ms    |
// | Test for task #46 03                 |  653 ms  | 359 ms    | 381 ms    | 341 ms    |
// | Test for task #46 04                 | 1304 ms  | 690 ms    | 823 ms    | 732 ms    |


// Tests for the 47th task
[<TestCase ("x", -5.0, 7.0, 0.0001, 2, 12.0)>]
[<TestCase ("x", -7.0, 7.0, 0.0001, 2, 0.0)>]
[<TestCase ("5 * ln x", 1.0, 5.0, 0.0001, 2, 20.24)>]
[<TestCase ("x ^ 2 + 3", -1.0, 3.0, 0.0001, 1, 21.3)>]
[<TestCase ("sin x + cos x", 0.0, System.Math.PI, 0.00005, 3, 2.0)>]
let ``Test for task #47 01`` expression left right precision (digits : int) answer =
    for i = 1 to 10 do
        let result = definiteIntegral expression i left right precision
        Assert.AreEqual(answer, (System.Math.Round(result, digits)))


// | Test                               | 1 thread | 2 threads | 4 threads | 8 threads |
// +====================================+==========+===========+===========+===========+
// | "x"             -5.0  7.0  0.0001  |  956 ms  |  544 ms   |  558 ms   |  581 ms   |
// | "x"             -7.0  7.0  0.0001  | 1078 ms  |  669 ms   |  787 ms   |  697 ms   |
// | "5 * ln x"       1.0  5.0  0.0001  |  825 ms  |  570 ms   |  496 ms   |  577 ms   |
// | "x ^ 2 + 3"     -1.0  3.0  0.0001  | 1032 ms  |  682 ms   |  686 ms   |  708 ms   |
// | "sin x + cos x"  0.0  pi   0.00005 | 1707 ms  | 1082 ms   | 1061 ms   | 1101 ms   |


// Tests for the 48th task
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
    for i = 0 to 3 do
        Assert.AreEqual(result, matrixMultiplication matrix1 matrix2 (pown 2 i))

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
    for i = 0 to 3 do
        Assert.AreEqual(result, matrixMultiplication matrix1 matrix2 (pown 2 i))

[<Test>]
let ``Test for task #48 04`` () =
    let matrix1 = Array.init 200 (fun i -> Array.init 2000 (fun j -> 1))
    let matrix2 = Array.init 2000 (fun i -> Array.init 200 (fun j -> 1))
    let result  = Array.init 200 (fun i -> Array.init 200 (fun j -> 2000))
    for i = 1 to 10 do
        Assert.AreEqual(result, matrixMultiplication matrix1 matrix2 i)

[<Test>]
let ``Test for task #48 05`` () =
    let matrix1 = Array.init 500 (fun i -> Array.init 500 (fun j -> 1))
    let matrix2 = Array.init 500 (fun i -> Array.init 500 (fun j -> 1))
    let result  = Array.init 500 (fun i -> Array.init 500 (fun j -> 500))
    for i = 0 to 3 do
        Assert.AreEqual(result, matrixMultiplication matrix1 matrix2 (pown 2 i))


// | Test                 | 1 thread | 2 threads | 4 threads | 8 threads |
// +======================+==========+===========+===========+===========+
// | Test for task #48 01 |   14 ms  |   7 ms    |   20 ms   |   31 ms   |
// | Test for task #48 02 |   14 ms  |   8 ms    |   15 ms   |   30 ms   |
// | Test for task #48 03 |   28 ms  |  11 ms    |   25 ms   |   43 ms   |
// | Test for task #48 04 | 1414 ms  | 711 ms    |  814 ms   |  804 ms   |
// | Test for task #48 05 | 1885 ms  | 965 ms    | 1153 ms   | 1171 ms   |


// Tests for the 49th task
[<TestCase ([|-1; 4; -17; 3; 8|], [|-17; -1; 3; 4; 8;|])>]
[<TestCase ([|9; 2; 3; 8; 5; 7; 4; 6; 1|], [|1; 2; 3; 4; 5; 6; 7; 8; 9|])>]
let ``Test for task #49 01`` (arr : int []) (result : int []) =
    for i = 1 to arr.Length do
        let temp = Array.copy arr
        Assert.AreEqual(result, mergeSort temp i)

[<Test>]
let ``Test for task #49 02`` () =
    let arr = Array.init 100000 (fun i -> i)
    for i = 0 to 3 do
        let temp = Array.copy arr
        Assert.AreEqual(arr, mergeSort temp (pown 2 i))

[<Test>]
let ``Test for task #49 03`` () =
    let arr = Array.init 500000 (fun i -> 500000 - i)
    let res = Array.init 500000 (fun i -> i + 1)
    for i = 0 to 3 do
        let temp = Array.copy arr
        Assert.AreEqual(res, mergeSort temp (pown 2 i))

[<Test>]
let ``Test for task #49 04`` () =
    let arr = Array.init 1000000 (fun i -> 1)
    for i = 0 to 3 do
        let temp = Array.copy arr
        Assert.AreEqual(arr, mergeSort temp (pown 2 i))


// | Test                          | 1 thread | 2 threads | 4 threads | 8 threads |
// +===============================+==========+===========+===========+===========+
// | [|-1; 4; -17; 3; 8|]          |   23 ms  |    9 ms   |   17 ms   |   33 ms   |
// | [|9; 2; 3; 8; 5; 7; 4; 6; 1|] |   24 ms  |    9 ms   |   16 ms   |   26 ms   |
// | Test for task #49 02          |  541 ms  |  312 ms   |  403 ms   |  424 ms   |
// | Test for task #49 03          | 2821 ms  | 1643 ms   | 1750 ms   | 2054 ms   |
// | Test for task #49 04          | 5177 ms  | 3123 ms   | 3611 ms   | 4349 ms   |