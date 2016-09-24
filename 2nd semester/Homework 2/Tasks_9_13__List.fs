(*
Homework 2 (02.03.2015)
Tasks 9 - 13.

Author: Mikhail Kita, group 171
*)

// The 9th task
let t = List.iter //val t : (('a -> unit) ->'a list -> unit) 

// The 10th task
let list_reverse list = 
    List.fold (fun list arg -> arg :: list) [] list

// The 11th task
let list_filter func list = 
    List.foldBack (fun arg list -> if (func arg) then arg :: list else list) list []

// The 12th task
let list_map func list = 
    List.foldBack (fun arg list -> (func arg) :: list) list []

// The 13th task
let horner x coefficients = 
    List.fold (fun b a -> a + b * x) 0 coefficients

[<EntryPoint>]
let main argv =
    let example = [1; 2; 3; 4; 5; 6; 7; 8; 9; 10]
    printf "Example:\n %A\n\n" example

    let reverseExample = list_reverse example
    printf "Now we reverse it:\n %A\n\n" reverseExample

    let filter = list_filter (fun value -> (value % 2) = 0) example
    printf "Filter for even elements:\n %A\n\n" filter
    
    let map = list_map (fun value -> value + 1) example
    printf "Map (x -> x + 1):\n %A\n\n" map

    let coefficients = [2; 7; 3; 4; 5; 9; 1] // Leading coefficient -> ... -> constant
    let x = 5
    printf "================\n"
    printf "Horner's Method:\n\nCoefficients: %A\nX = %A\n\n" coefficients x
    let result = horner x coefficients
    printf "Result: %d\n" result
    0
