(*
Homework 3 (16.03.2015)
Tasks 27 - 29

Author: Mikhail Kita, group 171
*)

//the 27th task
type IList<'A> =
    interface
        abstract isEmpty : unit -> bool
        abstract top : unit -> 'A
        abstract push_front : 'A -> unit
        abstract push_back : 'A -> unit
        abstract push_to : 'A -> int -> unit
        abstract del_front : unit -> unit
        abstract del_back : unit -> unit
        abstract del_from : int -> unit
        abstract find : ('A -> bool) -> 'A
        abstract concat : IList<'A> -> unit
        abstract print : unit -> unit
        abstract return_list : unit -> IList<'A>
    end


//the 28th task
type List<'A when 'A: equality> = Null | Node of 'A * List<'A>

type ADTList<'A when 'A: equality> () =
    class
        let mutable list = Null

        interface IList<'A> with 
            member this.isEmpty () = 
                if list = Null then true else false

            member this.top () = 
                match list with
                | Null        -> failwith "List is empty"
                | Node (x, y) -> x

            member this.push_front elem = list <- Node (elem, list)
            
            member this.push_back elem =
                let rec pushBackTo l =
                    match l with
                    | Null        -> Node (elem, Null) 
                    | Node (x, y) -> Node (x, pushBackTo y)
                list <- pushBackTo list
            
            member this.push_to elem pos =
                if pos = 0 then (this :> IList<'A>).push_front(elem)
                else
                    let rec pushBackTo n l =
                        if n = pos then Node (elem, l)
                        else
                            match l with
                            | Null        -> failwith "Not enough elements in list"
                            | Node (x, y) -> Node (x, pushBackTo (n + 1) y)
                    list <- pushBackTo 0 list

            member this.del_front () =
                match list with
                | Null        -> failwith "List is empty"
                | Node (x, y) -> list <- y

            member this.del_back () =
                let rec delBackFrom l =
                    match l with
                    | Null                      -> failwith "List is empty"
                    | Node (x, Node (x', Null)) -> Node (x, Null)
                    | Node (x, y)               -> Node (x, delBackFrom y)
                list <- delBackFrom list
            
            member this.del_from pos =
                if pos = 0 then (this :> IList<'A>).del_front()
                else
                    let rec delBackFrom n l =
                        match l with
                        | Null        -> failwith "Not enough elements in list"
                        | Node (x, y) ->
                            if n = pos - 1 then 
                                match y with
                                | Null         -> failwith "Not enough elements in list"
                                | Node (_, y') -> Node (x, y')
                            else
                                Node (x, delBackFrom (n + 1) y)
                    list <- delBackFrom 0 list
            
            member this.find func = 
                let rec findElem l =
                    match l with
                    | Null        -> failwith "Element not found"
                    | Node (x, y) ->
                        if func x = true then x
                        else findElem y
                findElem list

            member this.concat newList =
                let rec addTo l elem =
                    match l with
                    | Null           -> elem
                    | Node (x, y)    -> Node (x, addTo y elem)
                let mutable temp = Null
                while newList.isEmpty() = false do
                    temp <- addTo temp (Node (newList.top(), Null))
                    newList.del_front()
                list <- addTo list temp
             
             member this.print () = printf "%A\n\n" list
             
             member this.return_list () =
                let answer = new ADTList<'A> () 
                let rec pushFrom l =
                    match l with
                    | Null           -> failwith "List is empty"
                    | Node (x, Null) -> (answer :> IList<'A>).push_back(x) 
                    | Node (x, y)    -> 
                        (answer :> IList<'A>).push_back(x)
                        pushFrom y
                pushFrom list
                answer :> IList<'A>
    end


//the 29th task
type ArrayList<'A when 'A: equality> () =
    class
        let mutable list = [||]

        interface IList<'A> with      
            member this.isEmpty () = 
                if list = [||] then true else false

            member this.top () = 
                match list with
                | [||] -> failwith "List is empty"
                | _    -> list.[0]

            member this.push_front elem = list <- Array.append [|elem;|] list
            
            member this.push_back elem = list <- Array.append list [|elem;|]
            
            member this.push_to elem pos =
                if list.Length < pos then 
                    failwith "Not enough elements in list"
                else
                    let mutable temp = [||]
                    for i = list.Length - 1 downto pos do
                        temp <- Array.append [|list.[i];|] temp
                    temp <- Array.append [|elem;|] temp
                    for i = pos - 1 downto 0 do
                        temp <- Array.append [|list.[i];|] temp
                    list <- temp

            member this.del_front () =
                if list.Length = 0 then failwith "List is empty"
                else
                    let mutable temp = [||]
                    for i = list.Length - 1 downto 1 do
                        temp <- Array.append [|list.[i];|] temp
                    list <- temp

            member this.del_back () =
                if list.Length = 0 then failwith "List is empty"
                else
                    let mutable temp = [||]
                    for i = list.Length - 2 downto 0 do
                        temp <- Array.append [|list.[i];|] temp
                    list <- temp
            
            member this.del_from pos =
                if list.Length = 0 then failwith "List is empty"
                else 
                    if list.Length < pos then 
                        failwith "Not enough elements in list"
                    else
                        let mutable temp = [||]
                        for i = list.Length - 1 downto 0 do
                            if i <> pos then 
                                temp <- Array.append [|list.[i];|] temp
                        list <- temp
            
            member this.find func = Array.find func list
            
            member this.concat newList = 
                while newList.isEmpty() = false do
                    list <- Array.append list [|newList.top();|]
                    newList.del_front()

            member this.print () = printf "%A\n\n" list

            member this.return_list () =
                let l = new ArrayList<'A> ()
                for i in list do (l :> IList<'A>).push_back(i)
                l :> IList<'A>
    end
 

//demonstration of all functions of IList
let test (list : IList<int>) (list' : IList<int>) = 
    printf "List at the beginning:\n"
    list.push_back(1); list.push_back(3); 
    list.print()
    printf "Push_back (4):\n"; list.push_back(4); 
    list.print()
    printf "Push_front (0):\n"; list.push_front(0); 
    list.print()
    printf "Push_to 2 2 (push '2' to position #2):\n"; list.push_to 2 2
    list.print()
    printf "Del_back:\n"; list.del_back()
    list.print()
    printf "Del_front:\n"; list.del_front()
    list.print()
    printf "Del_from 1 (delete element from position #1):\n"; list.del_from(1)
    list.print()
    list'.push_back(2); list'.push_back(4);
    printf "Concat list [2; 4;] with our list:\n"; list.concat list'
    list.print()
    printf "First even element: %d\n" (list.find (fun elem -> elem % 2 = 0))

[<EntryPoint>]
let main argv =
    let list = new ArrayList<int> ()
    let list' = new ADTList<int> ()
    printf "ARRAY LIST\n\n"
    test list list'

    let newList = new ADTList<int> ()
    let newList' = new ArrayList<int> ()
    printf "\n\nADT LIST\n\n"
    test newList newList'
    0