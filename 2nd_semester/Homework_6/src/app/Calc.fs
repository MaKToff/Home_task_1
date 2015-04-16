(*
Homework 6 (06.04.2015)
Calculator for expressions

Author: Mikhail Kita, group 171
*)

module Calc

open Common

exception InvalidOperationException

//realisation of stack
type Stack<'A when 'A: equality> () = 
    class
        let mutable (stack : List<'A>) = Nil
        
        member this.isEmpty = (stack = Nil)
        member this.push elem = stack <- Cons (elem, stack)
        
        member this.pop () =
            match stack with
            | Nil               -> raise ListIsEmpty
            | Cons (elem, next) ->
                stack <- next
                elem

        member this.top () = 
            match stack with
            | Nil               -> raise ListIsEmpty
            | Cons (elem, next) -> elem
    end

//checks if string is number
let isNumber (str : string) =
    if not (str.[0] = '-' || (System.Char.IsDigit(str.[0]))) then
        false
    else
        let mutable answer = true
        for i = 1 to str.Length - 1 do
            if not (System.Char.IsDigit(str.[i])) then answer <- false
        answer

//converts string to float
let stringToFloat (str : string) =
    let charToDouble (elem : char) = 
        System.Convert.ToDouble(System.Convert.ToInt32(elem) - 48)
    let mutable temp = 0.0
    let negative = 
        match str.[0] with
        | '-' -> true 
        |  _  -> 
            temp <- charToDouble(str.[0])
            false
    for i = 1 to str.Length - 1 do
        temp <- temp * 10.0 + charToDouble(str.[i])
    if negative then -temp else temp

//computes value of expression
let compute (expression : Expr) (context : MyMap<string, float>) =
    
    //calculates the result
    let rec apply tree =
        match tree with
        | Null        -> 0.0
        | Num (value) -> 
            if isNumber(value) then stringToFloat(value)
            else context.find(value)
        | BinOp (left, operator, right) ->
            let a = apply left
            let b = apply right
        
            //applies operator to two operands
            match operator with
            | '+' -> a + b
            | '-' -> a - b
            | '*' -> a * b
            | '/' -> a / b
            | '%' -> a % b 
            | '^' ->
                if b >= 0.0 then pown a (System.Convert.ToInt32(b))
                else 1.0 / (pown a (-System.Convert.ToInt32(b)))
            | _   -> raise InvalidOperationException

    apply expression