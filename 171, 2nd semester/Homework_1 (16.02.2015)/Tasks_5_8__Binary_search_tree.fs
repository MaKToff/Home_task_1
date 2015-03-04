(*
Homework 1 (16.02.2015)
Tasks 5 - 8

Author: Mikhail Kita, group 171
*)

//the 5th task
type Tree = Null | Node of Tree * int * Tree


//the 6th task
let rec add value tree =
    match tree with
    | Null                       -> Node (Null, value, Null)
    | Node (left, center, right) ->
        if value < center 
        then 
            Node (add value left, center, right)
        else 
            Node (left, center, add value right)


//the 7th task

//the smallest node on the right side with not more than 1 child
let rec goodNode tree = 
    match tree with
    | Null                       -> 0 //because of that can appear problems with '0' :(
                                      //but this situation is impossible :)
    | Node (Null, center, right) -> center
    | Node (left, center, right) -> goodNode left

let rec remove value tree =
    match tree with
    | Null                       -> Null
    | Node (Null, center, Null)  -> Null
    | Node (left, center, right) ->
        if value = center
        then
            match right with
            | Null  -> left
            | right -> Node (left, goodNode right, remove (goodNode right) right)
        else
            if value < center
            then
                Node (remove value left, center, right)
            else
                Node (left, center, remove value right)


//the 8th task
let rec printLCR tree =
    match tree with
    | Null                       -> printf ""
    | Node (left, center, right) -> 
        printLCR left
        printf "%d " center
        printLCR right

let rec printLRC tree =
    match tree with
    | Null                       -> printf ""
    | Node (left, center, right) -> 
        printLRC left
        printLRC right
        printf "%d " center

let rec printCLR tree =
    match tree with
    | Null                       -> printf ""
    | Node (left, center, right) -> 
        printf "%d " center
        printCLR left
        printCLR right


let rec visualisation tree =
    match tree with
    | Null                       -> printf "Null"
    | Node (left, center, right) ->
        printf "Node (" 
        (visualisation left) 
        printf ", %d, " center 
        (visualisation right)
        printf ")"

let print tree =
    visualisation tree
    printf "\n\nCLR: "
    printCLR tree; 
    printf "\nLRC: "
    printLRC tree; 
    printf "\nLCR: "
    printLCR tree;
    printf "\n\n"

[<EntryPoint>]
let main args =
    let mutable example = (Node (Node (Null, 2, Node (Null, 4, Null)), 5, Node (Node (Null, 6, Null), 8, Node (Null, 9, Null))))
    print example

    printf "\n==========================\n"
    printf "Then we added '3' and '7':\n\n"
    example <- add 3 example
    example <- add 7 example
    print example

    printf "\n==========================\n"
    printf "Now we delete '5' and '2':\n\n"
    let mutable newExample = remove 5 example
    print newExample; 

    newExample <- remove 2 newExample
    print newExample; 
    0