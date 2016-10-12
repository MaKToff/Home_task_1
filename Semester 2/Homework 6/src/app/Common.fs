(*
Homework 6 (06.04.2015)
Contains common types, data structures and funtions.

Author: Mikhail Kita, group 171
*)

module Common

type Expr =
          | Null
          | Num   of string 
          | BinOp of Expr * char * Expr

type List<'A> = Nil | Cons of 'A * List<'A>

exception ListIsEmpty

// Realisation of queue
type Queue<'A when 'A: equality> () = 
    class
        let mutable (queue : List<'A>) = Nil
        
        member this.isEmpty = (queue = Nil)
        member this.push elem =
            let rec addTo t =
                match t with
                | Nil              -> Cons(elem, Nil)
                | Cons(elem, next) -> Cons(elem, addTo next)
            queue <- addTo queue

        member this.pop () =
            match queue with
            | Nil               -> raise ListIsEmpty
            | Cons (elem, next) ->
                queue <- next
                elem
    end

// Checks if string is an operator
let isOperator (str : string) =
    match str with
    | "+" | "-" | "*" | "/" | "%" | "^" -> true
    |  _  -> false

// Converts expression to tree
let rec makeExpr (input : Queue<string>) =
    let temp = input.pop()
    if isOperator(temp) then
        BinOp(makeExpr(input), temp.[0], makeExpr(input)) 
    else Num(temp)

type Strm = S of string | L of Queue<float>

// Realisation of map
type MyMap<'A, 'B> () =
    class
        let mutable (keys : string array) = [||]
        let mutable (vals : float  array) = [||]

        member this.add key value =
            try
                let index = Array.findIndex (fun x -> x = key) keys
                vals.[index] <- value
            with
                | :? System.Collections.Generic.KeyNotFoundException ->
                    keys <- Array.append keys [|key;|]
                    vals <- Array.append vals [|value;|]

        member this.find key =
            let index = Array.findIndex (fun x -> x = key) keys
            vals.[index]
    end