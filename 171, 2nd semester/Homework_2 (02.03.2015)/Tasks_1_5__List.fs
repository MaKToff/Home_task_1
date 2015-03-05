(*
Homework 2 (02.03.2015)
Tasks 1 - 5

Author: Mikhail Kita, group 171
*)

let rec fold f value list =
    match list with
    | []        -> value
    | x :: list -> fold f (f value x) list

[<EntryPoint>]
let main argv =
    //the first task
    let t = List.iter //val t : (('a -> unit) ->'a list -> unit) 

    let example = [1; 2; 3; 4; 5; 6; 7; 8; 9; 10]
    printf "Example:\n %A\n\n" example

    //the second task
    let reverseExample = fold (fun x y -> y :: x) [] example
    printf "Now we reverse it:\n %A\n\n" reverseExample

    //the third task
    let listFilter = fold (fun x y -> if (y % 2 = 0) then x @ [y] else x) [] example
    printf "Filter for even elements:\n %A\n\n" listFilter

    //the 4th task
    let listMap = fold (fun x y -> x @ [y + 1]) [] example
    printf "Map (x -> x + 1):\n %A\n\n" listMap

    //the 5th task
    let coefficients = [2; 7; 3; 4; 5; 9; 1] //leading coefficient -> ... -> constant
    let x = 5
    printf "================\n"
    printf "Horner's Method:\n\nCoefficients: %A\nX = %A\n\n" coefficients x
    let horner = fold (fun b a -> a + b * x) 0 coefficients
    printf "Result: %d\n" horner
    0