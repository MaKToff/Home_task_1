(*
Homework 6 (06.04.2015)
Tests for calculator

Author: Mikhail Kita, group 171
*)

module CalcTests

open NUnit.Framework
open Common
open Calc

[<TestCase ("^\n2\n-1\n", Result = 0.5)>] // 2 ^ (-1)
[<TestCase ("/\n54\n6\n)", Result = 9.0)>] // 54 / 6
[<TestCase ("+\n5\n-5\n)", Result = 0.0)>] // 5 + (-5)
[<TestCase ("%\n555\n10\n", Result = 5.0)>] // 555 % 10
[<TestCase ("-\n1\n101\n)", Result = -100.0)>] // 1 - 101
[<TestCase ("+\n*\n2\n2\n2\n", Result = 6.0)>] // 2 + 2 * 2
[<TestCase ("^\n3\n^\n1\n2\n", Result = 3.0)>] // 3 ^ 1 ^ 2
[<TestCase ("-\n-\n1\n2\n3\n", Result = -4.0)>] // 1 - 2 - 3
[<TestCase ("^\n123456789\n0\n", Result = 1.0)>] // 123456789 ^ 0
[<TestCase ("^\n*\n+\n1\n2\n3\n4\n", Result = 6561.0)>] // ((1 + 2) * 3) ^ 4
[<TestCase ("+\n3\n/\n*\n4\n2\n^\n-\n1\n5\n2\n", Result = 3.5)>] // 3 + 4 * 2 / (1 - 5) ^ 2
let ``Test for calculator`` (expression : string) =
    let q = new Queue<string>()
    let mutable temp = ""
    for c in expression do
        match c with
        | '\n' ->
            q.push(temp)
            temp <- ""
        |   _  -> temp <- temp + c.ToString()
    let map = new MyMap<string, float>()

    compute (makeExpr(q)) map