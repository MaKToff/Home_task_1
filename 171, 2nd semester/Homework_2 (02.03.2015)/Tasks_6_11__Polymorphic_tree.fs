(*
Homework 2 (02.03.2015)
Tasks 6 - 11

Author: Mikhail Kita, group 171
*)

//the 6th task
type Tree<'A> = Null | Node of Tree<'A> * ('A) * Tree<'A>


//the 7th task
let rec map f tree =
    match tree with
    | Null                       -> Null 
    | Node (left, center, right) -> 
        Node (map f left, f center, map f right)


//the 8th task
let rec fold f value tree = 
    match tree with
    | Null                       -> value
    | Node (left, center, right) ->
        let temp = f value center       //firstly we apply function to center
        fold f (fold f temp left) right //then we apply it to left part and after that - to right part


type Option<'A> = None | Some of 'A

let minElem arg1 arg2 =
    match arg1 with
    | None       -> Some (arg2)
    | Some value -> Some (min value arg2)

let rec insert value tree =
    match tree with
    | Null                       -> Node (Null, value, Null)
    | Node (left, center, right) ->
        if value < center 
        then 
            Node (insert value left, center, right)
        else 
            Node (left, center, insert value right)

[<EntryPoint>]
let main argv =
    let example = (Node (Node (Null, 2, Node (Null, 4, Null)), 5, Node (Node (Null, 6, Null), 8, Node (Null, 9, Null))))
    printf "Example:\n%A\n\n" example
    
    let newTree = map (fun x -> x + 1) example
    printf "Map (x -> x + 1):\n%A\n\n" newTree
    
    //NB: tasks 9-11 are examples of polymorphic fold for tree

    //the 9th task
    let sum = fold (+) 0 example
    printf "Sum of all nodes in example: %d\n\n" sum

    //the 10th task
    let m = fold minElem None example
    printf "Minimun node in example: %A\n\n" m
    
    //the 11th task
    let copyExample = fold (fun x y -> insert y x) Null example
    printf "Copy of example:\n%A\n" copyExample
    0