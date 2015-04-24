(*
Homework 8 (20.04.2015)
Calculator for expressions

Author: Mikhail Kita, group 171
*)

module Calc

type List<'A> = Nil | Cons of 'A * List<'A>
type Tree<'A> = Null | Node of Tree<'A> * 'A * Tree<'A> 

exception ListIsEmpty
exception InvalidOperation

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
    if str.[0] = '-' && str.Length = 1 || 
        (not (str.[0] = '-' || (System.Char.IsDigit(str.[0])))) then false
        else
            let mutable answer = true
            let mutable dot = false
            for i = 1 to str.Length - 1 do
                if not (System.Char.IsDigit(str.[i])) then 
                    if str.[i] = '.' && not dot then dot <- true
                    else answer <- false
            answer

//splits input string to tokens and converts them to expression in reverse polish notation
//returns result in tree form
let convert (str : string) =
    
    //returns priority of operator
    let priority operator =
        match operator with
        | "+" | "-" -> 1
        | "*" | "/" | "%" -> 2
        | "^" -> 3
        |  _  -> 0   
    
    //lexical analysis of input expression
    let mutable tokens = []
    let mutable temp = ""
    for i = 0 to str.Length - 1 do
        match str.[i] with
            | ' ' ->
                if temp <> "" then tokens <- List.append tokens [temp;]
                temp <- ""
            
            | '-' -> temp <- "-"
            | '(' -> tokens <- List.append tokens ["(";]
            | ')' ->
                if temp.Length > 0 then
                    tokens <- List.append tokens [temp;]
                    temp <- "" 
                tokens <- List.append tokens [")";]
            
            |  _  -> temp <- temp + str.[i].ToString()
    
    if temp.Length > 0 then tokens <- List.append tokens [temp;]

    //syntax analysis of input expression
    let stack = new Stack<string>()
    let result = new Stack<Tree<string>>()
    for t in tokens do
        if isNumber t then result.push(Node(Null, t, Null))
        else
            match t with
            | "(" -> stack.push(t)
            | ")" ->
                while stack.top() <> "(" && (not stack.isEmpty) do
                    result.push(Node(result.pop(), stack.pop(), result.pop()))
                ignore(stack.pop())

            | "sin" | "cos" | "tg" | "ctg" | "sqrt" | "!" | "ln" | "lg" -> 
                stack.push(t)
               
            | _   ->
                while not stack.isEmpty 
                    && (priority(stack.top()) >= priority(t) && priority(t) < 3
                        || (priority(stack.top()) >  priority(t) && priority(t) = 3))
                            do result.push(Node(result.pop(), stack.pop(), result.pop()))
                stack.push(t)

    while not stack.isEmpty do 
        let temp = stack.pop()
        match temp with
        | "sin" | "cos" | "tg" | "ctg" | "sqrt" | "!" | "ln" | "lg" ->
            result.push(Node(result.pop(), temp, Null))
        | _ -> result.push(Node(result.pop(), temp, result.pop()))
    result.pop()

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
    let mutable dot = false
    let mutable t = 1.0

    for i = 1 to str.Length - 1 do
        if str.[i] = '.' || str.[i] = ',' then dot <- true
        else 
            if dot then 
                t <- t * 10.0
                temp <- temp + charToDouble(str.[i]) / t
            else temp <- temp * 10.0 + charToDouble(str.[i]) / t
    if negative then -temp else temp

//computes value of expression
let compute expression =
    let tree = convert(expression)
    printfn "%A" tree
    let computeWithPrecision value =
        if abs(value) < 1e-15 then "0" else (value).ToString()
    
    //calculates the result
    let rec apply t =
        match t with
        | Null                       -> "0"
        | Node (Null, value, Null)   -> value
        | Node (left, center, right) ->
            let a = stringToFloat(apply left)
            let b = stringToFloat(apply right)

            //applies operator to two operands
            match center with
            | "+" -> (a + b).ToString()
            | "-" -> (b - a).ToString()
            | "*" -> (a * b).ToString()
            | "/" -> (b / a).ToString()
            | "%" -> 
                if (b % a) < 0.0 then (b % a + a).ToString()
                else (b % a).ToString()
            
            | "^" ->
                if a >= 0.0 then (pown b (System.Convert.ToInt32(a))).ToString()
                else (1.0 / (pown b (-System.Convert.ToInt32(a)))).ToString()
            
            | "!" -> 
                let n = System.Convert.ToInt32(a)
                let mutable res = 1.0
                for i = 1 to n do res <- res * (System.Convert.ToDouble(i)) 
                (res).ToString()
            
            | "sin"  -> computeWithPrecision (sin a)
            | "cos"  -> computeWithPrecision (cos a)
            | "tg"   -> computeWithPrecision (tan a)
            | "ctg"  -> computeWithPrecision (1.0 / (tan a))
            | "sqrt" -> (sqrt a).ToString()
            | "ln"   -> (log a).ToString()
            | "lg"   -> ((log a) / (log 10.0)).ToString()
            |  _     -> raise InvalidOperation

    let mutable result = apply tree
    let index = result.IndexOf(",")
    if index > 0 then
        result <- (result.Remove(index, 1)).Insert(index, ".")
    result