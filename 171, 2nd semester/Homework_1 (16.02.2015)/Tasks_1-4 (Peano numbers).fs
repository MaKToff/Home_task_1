(*
Задания 1 - 4
==============
Tasks 1 - 4

Author: Mikhail Kita, group 171
*)

type Peano = Zero | S of Peano

//ancillary functions
let rec plus a b =
    match a with
    | Zero -> b
    | S a  -> S (plus a b)

let minus1 p =
    match p with
    | Zero -> Zero
    | S p  -> p


//the first task
let rec minus a b =
    match b with
    | Zero -> a
    | S b  -> 
        if a = Zero 
        then 
            Zero
        else 
            minus (minus1 a) b


//the second task
let rec peanoToInt p =
    match p with
    | Zero -> 0
    | S p  -> 1 + (peanoToInt p)


//the third task
let rec mult a b =
    match a with
    | Zero -> Zero
    | S a  -> 
        if b = Zero 
        then 
            Zero
        else 
            plus b (mult a b)
    
     
//the 4th task
let rec exp x y =
    match y with
    | Zero   -> S Zero
    | S Zero -> x
    | S y    ->
        if x = Zero 
        then 
            Zero
        else 
            exp (mult x x) y


[<EntryPoint>]
let main args =
    let arg1 = (S (S (S (S Zero))))
    let arg2 = (S (S Zero))
    printf "%A\n"    (minus arg1 arg2)
    printf "%d %d\n" (peanoToInt arg1) (peanoToInt arg2)
    printf "%A\n"    (mult arg1 arg2)
    printf "%A\n"    (exp arg1 arg2)
    0
