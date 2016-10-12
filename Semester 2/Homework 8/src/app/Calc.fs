(*
Homework 8 (20.04.2015)
Calculator for expressions.

Author: Mikhail Kita, group 171
*)

module Calc

type List<'A> = Nil | Cons of 'A * List<'A>
type Tree<'A> = Null | Node of Tree<'A> * 'A * Tree<'A> 

exception ListIsEmpty
exception InvalidOperation
exception KeyNotFound

// Realisation of stack
type Stack<'A when 'A: equality> () = 
    class
        let mutable (stack : List<'A>) = Nil
        
        member this.isEmpty   = (stack = Nil)
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

// Realisation of map
type Dictionary () =
    class
        let mutable dictionary = Nil

        let rec tryFind dict key =
            match dict with
            | Nil                             -> raise KeyNotFound
            | Cons ((keyElem, valElem), next) -> 
                if keyElem = key then valElem else tryFind next key

        let rec tryAdd dict key value =
            match dict with
            | Nil                             -> Cons ((key, value), Nil)
            | Cons ((keyElem, valElem), next) -> 
                if keyElem = key then Cons ((keyElem, value), next)
                else Cons ((keyElem, valElem), tryAdd next key value)

        member this.add key value = 
            dictionary <- tryAdd dictionary key value
        member this.find key      = tryFind dictionary key
    end

// Checks if string is a number
let isNumber (str : string) =
    if str.[0] = '-' && str.Length = 1 
        || (not (str.[0] = '-' || (System.Char.IsDigit(str.[0])))) then false
        else
            let mutable answer = true
            let mutable dot    = false
            for i = 1 to str.Length - 1 do
                if not (System.Char.IsDigit(str.[i])) then 
                    if str.[i] = '.' && not dot then dot <- true
                    else answer <- false
            answer

// Splits the input string to tokens and converts them to the expression in reverse polish notation
// Returns result as a tree
let convert (str : string) =
    
    // Returns priority of the operator
    let priority operator =
        match operator with
        | "sin" | "cos" | "tg" | "ctg" | "!" |"sqrt" | "ln" | "lg" -> 4
        | "^" -> 3
        | "*" | "/" | "%" -> 2
        | "+" | "-" -> 1
        |  _  -> 0
    
    // Lexical analysis of input expression
    let mutable tokens = []
    let mutable temp   = ""
    for i = 0 to str.Length - 1 do
        match str.[i] with
            | '-' -> 
                if str.[i + 1] = ' ' then tokens <- List.append tokens ["-";]
                else tokens <- List.append tokens ["-1"; "*";]
                temp <- ""
            
            | '(' -> tokens <- List.append tokens ["(";]
            
            | ')' | '!' ->
                if temp.Length > 0 then
                    tokens <- List.append tokens [temp;]
                    temp <- ""
                tokens <- List.append tokens [str.[i].ToString();]
            | ' ' ->
                if temp <> "" then tokens <- List.append tokens [temp;]
                temp <- ""
            
            |  _  -> temp <- temp + str.[i].ToString()
    
    if temp.Length > 0 then tokens <- List.append tokens [temp;]

    // Syntax analysis of input expression
    let stack  = new Stack<string>()
    let result = new Stack<Tree<string>>()
    for t in tokens do
        match t with
        | "(" -> stack.push(t)
        | ")" ->
            while stack.top() <> "(" && (not stack.isEmpty) do
                if priority(stack.top()) = 4 then 
                    result.push(Node(result.pop(), stack.pop(), Null))
                else result.push(Node(result.pop(), stack.pop(), result.pop()))
            ignore(stack.pop())

        | "sin" | "cos" | "tg" | "ctg" | "!" |"sqrt" | "ln" | "lg" -> stack.push(t)

        | "+" | "-" | "*" | "/" | "^" | "%"   ->
            while not stack.isEmpty
                && (priority(stack.top()) >= priority(t) && priority(t) < 3
                || priority(stack.top()) >  priority(t) && priority(t) = 3
                || priority(stack.top()) = 4) do
                    if not stack.isEmpty && priority(stack.top()) = 4 then
                        result.push(Node(result.pop(), stack.pop(), Null))
                    else result.push(Node(result.pop(), stack.pop(), result.pop()))
            stack.push(t)

        | _ -> result.push(Node(Null, t, Null))

    while not stack.isEmpty do 
        let temp = stack.pop()
        match temp with
        | "sin" | "cos" | "tg" | "ctg" | "sqrt" | "!" | "ln" | "lg" ->
            result.push(Node(result.pop(), temp, Null))
        | _ -> result.push(Node(result.pop(), temp, result.pop()))
    result.pop()

// Computes value of the expression
let compute expression (context : Dictionary) =
    let tree = convert(expression)

    let computeWithPrecision value =
        if abs(value) < 1e-15 then 0.0 else value
    
    // Calculates the result
    let rec apply t =
        match t with
        | Null                       -> 0.0
        | Node (Null, value, Null)   -> 
            if isNumber value then float (value) 
            else context.find(value)
        | Node (left, center, right) ->
            let a = apply left
            let b = apply right

            // Applies operator to two operands
            match center with
            | "+" -> a + b
            | "-" -> b - a
            | "*" -> a * b
            | "/" -> b / a
            | "%" -> 
                if (b % a) < 0.0 then b % a + a else b % a
            
            | "^" ->
                if a >= 0.0 then b ** a else (1.0 / b ** (-a))
            
            | "!" ->
                let mutable res = 1.0
                for i = 1 to (int a) do res <- res * (float i)
                res
            
            | "sin"  -> computeWithPrecision (sin a)
            | "cos"  -> computeWithPrecision (cos a)
            | "tg"   -> computeWithPrecision (tan a)
            | "ctg"  -> computeWithPrecision (1.0 / (tan a))
            | "sqrt" -> sqrt a
            | "ln"   -> log a
            | "lg"   -> (log a) / (log 10.0)
            |  _     -> raise InvalidOperation

    apply tree

// Tries to compute value of the expression
let tryToCompute expr =
    try
        let dict           = new Dictionary()
        let mutable result = (compute expr dict).ToString()
        let index          = result.IndexOf(",")
            
        if index > 0 then
            result <- (result.Remove(index, 1)).Insert(index, ".")
        result
    with
    | :? ListIsEmpty | :? InvalidOperation | :? KeyNotFound -> "Error"