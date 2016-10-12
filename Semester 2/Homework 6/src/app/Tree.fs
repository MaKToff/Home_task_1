(*
Homework 6 (06.04.2015)
Contains definition of syntax tree and function which create it.

Author: Mikhail Kita, group 171
*)

module Tree

open Common

type Tree =
          | Empty
          | Read   of string
          | Write  of Expr
          | Assign of string * Expr
          | Seq    of Tree * Tree
          | If     of Expr * Tree * Tree
          | While  of Expr * Tree 

// Creates a tree from input list    
let createTree (input : Queue<string>) =
    let mutable result = Empty

    // Adds new nodes to the syntax tree
    let rec addTo t value =
        match t with
        | Empty -> 
            if value = Empty then Empty
            else Seq (value, Empty)
        | Seq (elem, next) -> Seq (elem, addTo next value)
        | _ -> Empty

    // Creates a node of the syntax tree
    let rec makeSeq() =
        
        // Creates a new branch
        let makeBranch() =
            let mutable branch = Empty
            let mutable temp   = makeSeq()
            branch <- addTo branch temp
            if temp = Empty then
                while temp = Empty do
                    branch <- addTo branch (makeSeq())
                    temp <- makeSeq()
                branch <- addTo branch temp
            branch

        match input.pop() with
        | "read"  -> Read(input.pop())
        | "write" -> Write(makeExpr input)
        | ":="    -> Assign(input.pop(), makeExpr input)
        | "if"    -> If(makeExpr input, makeBranch(), makeBranch())
        | "while" -> While(makeExpr input, makeBranch())
        |    _    -> Empty

    while not input.isEmpty do
        result <- addTo result (makeSeq())
        
    result