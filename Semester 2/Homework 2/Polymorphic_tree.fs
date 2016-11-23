(*
Homework 2 (02.03.2015)
Tasks 14 - 19.

Author: Mikhail Kita, group 171
*)

// The 14th task
type Tree<'A> = Null | Node of Tree<'A> * ('A) * Tree<'A>


// The 15th task
let rec map f tree =
    match tree with
    | Null                       -> Null 
    | Node (left, center, right) -> 
        Node (map f left, f center, map f right)


// The 16th task
let rec fold f value tree = 
    match tree with
    | Null                       -> value
    | Node (left, center, right) ->
        let temp = f value center       // Firstly we apply the function to the center
        fold f (fold f temp left) right // Then we apply it to the left part and after that - to the right part


// The 17th task
let rec sumNodes tree = fold (+) 0 tree 


// The 18th task
let minElem arg1 arg2 =
    match arg1 with
    | None       -> Some (arg2)
    | Some value -> Some (min value arg2)

let minNode tree = fold minElem None tree


// The 19th task
let rec insert value tree =
    match tree with
    | Null                       -> Node (Null, value, Null)
    | Node (left, center, right) ->
        if value < center 
        then 
            Node (insert value left, center, right)
        else 
            Node (left, center, insert value right)

let copy tree = fold (fun x y -> insert y x) Null tree


let optionToInt value = 
    match value with
    | None     -> 0
    | Some arg -> arg

let rec remove value tree =
    match tree with
    | Null                       -> Null
    | Node (Null, center, Null)  -> Null
    | Node (left, center, right) ->
        if value = center
        then
            match right with
            | Null  -> left
            | right -> 
                let goodNode = optionToInt (minNode right)
                Node (left, goodNode, remove goodNode right)
        else
            if value < center
            then
                Node (remove value left, center, right)
            else
                Node (left, center, remove value right)

let rec printLCR tree =
    match tree with
    | Null                       -> printf ""
    | Node (left, center, right) -> 
        printLCR left
        printf "%A " center
        printLCR right

let rec printLRC tree =
    match tree with
    | Null                       -> printf ""
    | Node (left, center, right) -> 
        printLRC left
        printLRC right
        printf "%A " center

let rec printCLR tree =
    match tree with
    | Null                       -> printf ""
    | Node (left, center, right) -> 
        printf "%A " center
        printCLR left
        printCLR right

let printTree tree =
    printf "%A\n" tree
    printf "\nCLR: "; printCLR tree; 
    printf "\nLRC: "; printLRC tree; 
    printf "\nLCR: "; printLCR tree;
    printf "\n===\n\n"

[<EntryPoint>]
let main argv =
    let mutable example = (Node (Node (Null, 2, Null), 5, Node (Null, 8, Null)))
    example <- insert 9 example
    example <- insert 6 example
    example <- insert 4 example
    printf "Example:\n"
    printTree example
    
    let newTree = map (fun x -> x + 1) example
    printf "Map (x -> x + 1):\n%A\n\n" newTree
    
    let sum = sumNodes example
    printf "Sum of all nodes in example: %d\n\n" sum

    let m = minNode example
    printf "Minimum node in example: %A\n\n" (optionToInt m)
    
    let copyExample = copy example
    printTree copyExample

    example <- remove 5 example
    printf "Now we remove '5':\n"
    printTree example
    0