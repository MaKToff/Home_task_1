(*
Homework 6 (06.04.2015)
Interpreter.

Author: Mikhail Kita, group 171
*)

module Interpreter

open System.IO
open Common
open Tree
open Calc

// Interprets input list and returns list with results
let interprate (input : Queue<string>) (inputStream : Strm) =
    let tree   = createTree(input)
    let map    = MyMap<string, float>()
    let result = new Queue<float>()

    // Executes a command from top of the syntax tree
    let rec execute t =
        match t with
        | Seq (command, next) ->
            
            match command with            
            | Read(value) ->
                match inputStream with
                | L (list) -> map.add value (list.pop())
                | S (file) ->
                    if file = "" then
                        printf "Enter value of variable %s : " value 
                        map.add value (stringToFloat(System.Console.ReadLine()))
                    else
                        use stream = new StreamReader(file)
                        map.add value (stringToFloat(stream.ReadLine()))
            
            | Write(expression) -> 
                result.push(compute expression map)
            
            | Assign(value, expression) ->
                map.add value (compute expression map)
            
            | If(expression, thenBranch, elseBranch) ->
                if (compute expression map) > 0.0 then execute thenBranch
                else execute elseBranch
            
            | While(expression, branch) ->
                while (compute expression map) > 0.0 do execute branch
            
            | _ -> ()
            
            execute next
        
        | _ -> ()

    execute tree
    result