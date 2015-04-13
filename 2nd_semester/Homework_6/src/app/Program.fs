(*
Homework 6 (06.04.2015)
Main file

Author: Mikhail Kita, group 171
*)

module Program

open System.IO
open Common
open Interpreter

//creates queue from list of string
let makeQueue (file : string) = 
    let q = new Queue<string>()
    
    use stream = new StreamReader(file)
    let str = stream.ReadToEnd ()

    let mutable temp = ""
    for c in str do
        match c with
        | '\r' -> ()
        | '\n' -> 
            q.push(temp); 
            temp <- ""
        |   c  -> temp <- temp + c.ToString()
    if temp <> "" then q.push(temp);
    q 

[<EntryPoint>]
let main argv =

    //the first argument is file with source code (should always exist)
    let input = makeQueue(argv.[0])
    
    //the second argument is file with input data
    if argv.Length < 2 then
        let output = interprate input (S "")
        while not output.isEmpty do
            System.Console.Write(output.pop())
            printf "\n"
    else
        let output = interprate input (S argv.[1])

        //the third argument is output file 
        if argv.Length < 3 then 
            while not output.isEmpty do
                System.Console.Write(output.pop())
        else
            use outputStream = new StreamWriter(argv.[2])
            while not output.isEmpty do
                outputStream.Write(output.pop())
    0