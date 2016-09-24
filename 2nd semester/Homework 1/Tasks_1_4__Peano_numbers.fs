(*
Homework 1 (16.02.2015)
Tasks 1 - 4.

Author: Mikhail Kita, group 171
*)

type Peano = Zero | S of Peano

// The first task
let rec minus a b =
    match a, b with
    | a, Zero  -> a
    | Zero, b  -> Zero
    | S a, S b -> minus a b


// The second task
let rec peanoToInt p =
    match p with
    | Zero -> 0
    | S p  -> 1 + (peanoToInt p)


// The third task
let rec plus a b =
    match a with
    | Zero -> b
    | S a  -> S (plus a b)

let rec mult a b =
    match a with
    | Zero -> Zero
    | S a  -> plus b (mult a b)
    
     
// The 4th task
let rec exp x y =
    match y with
    | Zero -> S Zero
    | S y  -> mult x (exp x y)


[<EntryPoint>]
let main args =
    let arg1 = (S (S (S (S Zero))))
    let arg2 = (S (S Zero))
    printf "arg1: %A\n"   arg1
    printf "arg2: %A\n\n" arg2
    printf "arg1 - arg2: %A\n"     (minus arg1 arg2)
    printf "arg1,  arg2: %d, %d\n" (peanoToInt arg1) (peanoToInt arg2)
    printf "arg1 * arg2: %A\n"     (mult arg1 arg2)
    printf "arg1 ^ arg2: %A\n"     (exp arg1 arg2)
    0