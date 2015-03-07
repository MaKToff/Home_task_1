(*
Homework 2 (02.03.2015)
Tasks 1 - 5

Author: Mikhail Kita, group 171
*)

//the 9th task
let t = List.iter //val t : (('a -> unit) ->'a list -> unit) 

let rec fold f value list =
    match list with
    | []        -> value
    | x :: list -> fold f (f value x) list


//the 10th task
let list_reverse list = fold (fun x y -> y :: x) [] list

//the 11th task
let list_filter f list = fold (fun x y -> if (f y) then y :: x else x) [] list

//the 12th task
let list_map f list = fold (fun x y -> x @ [(f y)]) [] list

//the 13th task
let horner x coefficients = fold (fun b a -> a + b * x) 0 coefficients

[<EntryPoint>]
let main argv =
    let example = [1; 2; 3; 4; 5; 6; 7; 8; 9; 10]
    printf "Example:\n %A\n\n" example

    let reverseExample = list_reverse example
    printf "Now we reverse it:\n %A\n\n" reverseExample

    let filter = list_filter (fun value -> (value % 2) = 0) reverseExample
    printf "Filter for even elements:\n %A\n\n" filter
    
    let map = list_map (fun value -> value + 1) example
    printf "Map (x -> x + 1):\n %A\n\n" map

    let coefficients = [2; 7; 3; 4; 5; 9; 1] //leading coefficient -> ... -> constant
    let x = 5
    printf "================\n"
    printf "Horner's Method:\n\nCoefficients: %A\nX = %A\n\n" coefficients x
    let result = horner x coefficients
    printf "Result: %d\n" result
    0