(*
Homework 7 (13.04.2015)
Tasks 40 - 41

Author: Mikhail Kita, group 171
*)

module Workflow

//the 40th task
type Builder(n : int) =
    member this.Bind (x, rest) = rest (x % n)
    member this.Return x =
        if (x % n < 0) then (x % n + n) else x % n

//NB: Division is not defined in ring
let ring x = new Builder(x)


//the 41st task
type Tree<'A> = Null | Node of Tree<'A> * 'A * Tree<'A>

type TreeBuilder<'A, 'B> (f : 'A -> 'B -> 'A, value) =    
    member this.Bind (x, rest) = 
        match x with
        | Null                       -> value
        | Node (left, center, right) ->
            let temp = f value center 
            rest (rest temp left) right
    
    member this.Return x = x
    member this.ReturnFrom x = Node (Null, x, Null)

    member this.Combine (a, b) = 
        let rec merge tree newTree  =
            match tree with
            | Null                       -> newTree
            | Node (left, center, right) ->
                match newTree with
                | Null           -> tree
                | Node (l, c, r) ->
                    if c < center then 
                        Node (merge left newTree, center, right)
                    else 
                        Node (left, center, merge right newTree)
        merge a b
    
    member this.Delay f = f()

let rec fold f value tree =
    TreeBuilder(f, value) {
        let! temp = tree
        return fold f temp
    }

//adds new node to tree
let rec insert value tree =
    match tree with
    | Null                       -> Node (Null, value, Null)
    | Node (left, center, right) ->
        if value < center then 
            Node (insert value left, center, right)
        else 
            Node (left, center, insert value right)

let filter f tree =
    fold (fun t arg -> if (f arg) then insert arg t else t) Null tree

let map f tree = 
    fold (fun t arg -> insert (f arg) t) Null tree
    
[<EntryPoint>]
let main argv =
    0