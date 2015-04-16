(*
Homework 6 (06.04.2015)
Tests for interpreter

Author: Mikhail Kita, group 171
*)

module InterpreterTests

open NUnit.Framework
open System.IO
open Common
open Interpreter
open Program

[<TestCase ("test1_read.in", [|5.0; 2.0;|], Result = "")>]
[<TestCase ("test2_write.in", [|5.0; 2.0;|], Result = "6 4 100 ")>]
[<TestCase ("test3_assign.in", [|0.0|], Result = "10 5 ")>]
[<TestCase ("test4_if.in", [|7.0; 12.0;|], Result = "10000 7 100 8 777 12 ")>]
[<TestCase ("test5_while.in", [|7.0; 3.0;|], Result = "343 ")>]
[<TestCase ("test6_all_easy.in", [|4.0; 6.0;|], Result = "27 8 ")>]
[<TestCase ("test7_all_hard.in", [|5.0; 8.0; 15.0; 7.0|], Result = "2 26 10 1.75 ")>]
let ``Test for interpreter`` (file : string) (args : float array) =
    let q = new Queue<float>()
    for l in args do q.push(l)
    let output = interprate (makeQueue(file)) (L q)
    
    let mutable result = ""
    while not output.isEmpty do
        result <- result + (output.pop()).ToString() + " "
    result