(*
Homework 4 (23.03.2015)
Tasks 33 - 34

Author: Mikhail Kita, group 171
*)

open NUnit.Framework

//the interface for list
type IList<'A> =
    interface
        abstract is_empty : unit -> bool
        abstract top : unit -> Option<'A>
        abstract push_front : 'A -> unit
        abstract push_back : 'A -> unit
        abstract push_to : 'A -> int -> bool
        abstract del_front : unit -> bool
        abstract del_back : unit -> bool
        abstract del_from : int -> bool
        abstract find : ('A -> bool) -> Option<'A>
        abstract concat : IList<'A> -> bool
        abstract print : unit -> unit
    end

//realisation of IList on ADT
type List<'A when 'A: equality> = Null | Node of 'A * List<'A>

type ADTList<'A when 'A: equality> () =
    class
        let mutable list = Null
        let rec length l =
            match l with
            | Null        -> 0
            | Node (x, y) -> length y + 1

        interface IList<'A> with 
            member this.is_empty () = 
                if list = Null then true else false

            member this.top () = 
                match list with
                | Null        -> None
                | Node (x, y) -> Some x

            member this.push_front elem = list <- Node (elem, list)
            
            member this.push_back elem =
                let rec pushBackTo l =
                    match l with
                    | Null        -> Node (elem, Null) 
                    | Node (x, y) -> Node (x, pushBackTo y)
                list <- pushBackTo list
            
            member this.push_to elem pos =
                if pos = 0 then 
                    (this :> IList<'A>).push_front(elem)
                    true
                else
                    let rec pushBackTo n l =
                        if n = pos then Node (elem, l)
                        else
                            match l with
                            | Null        -> Null
                            | Node (x, y) -> Node (x, pushBackTo (n + 1) y)
                    if (length list) < pos then false
                    else
                        list <- pushBackTo 0 list
                        true

            member this.del_front () =
                match list with
                | Null        -> false
                | Node (x, y) -> 
                    list <- y
                    true

            member this.del_back () =
                let rec delBackFrom l =
                    match l with
                    | Null                      -> Null
                    | Node (x, Null)            -> Null
                    | Node (x, Node (x', Null)) -> Node (x, Null)
                    | Node (x, y)               -> Node (x, delBackFrom y)
                if list = Null then false
                else 
                    list <- delBackFrom list
                    true
            
            member this.del_from pos =
                if pos = 0 then (this :> IList<'A>).del_front()
                else
                    let rec delBackFrom n l =
                        match l with
                        | Null        -> Null
                        | Node (x, y) ->
                            if n = pos - 1 then 
                                match y with
                                | Null         -> Null
                                | Node (_, y') -> Node (x, y')
                            else
                                Node (x, delBackFrom (n + 1) y)
                    if list = Null || (length list) < pos then false
                    else 
                        list <- delBackFrom 0 list
                        true
            
            member this.find func = 
                let rec findElem l =
                    match l with
                    | Null        -> None
                    | Node (x, y) ->
                        if func x = true then Some x
                        else findElem y
                findElem list

            member this.concat newList =
                let rec addTo l elem =
                    match l with
                    | Null           -> elem
                    | Node (x, y)    -> Node (x, addTo y elem)
                let mutable temp = Null
                let mutable result = true
                while newList.is_empty() = false do
                    match newList.top() with
                    | None   -> result <- false
                    | Some x -> 
                        temp <- addTo temp (Node (x, Null))
                        result <- newList.del_front()
                list <- addTo list temp
                result
             
             member this.print () = printf "%A\n\n" list
        
        override this.ToString() =
            let rec stringFrom l = 
                match l with
                | Null        -> "]"
                | Node (x, y) -> x.ToString() + "; " + stringFrom y
            "[ " + stringFrom list
    end

//realisation of IList on array
type ArrayList<'A when 'A: equality> () =
    class
        let mutable list = [||]

        interface IList<'A> with      
            member this.is_empty () = 
                if list = [||] then true else false

            member this.top () = 
                match list with
                | [||] -> None
                | _    -> Some list.[0]

            member this.push_front elem = list <- Array.append [|elem;|] list
            
            member this.push_back elem = list <- Array.append list [|elem;|]
            
            member this.push_to elem pos =
                if list.Length < pos then false
                else
                    let mutable temp = [||]
                    for i = list.Length - 1 downto pos do
                        temp <- Array.append [|list.[i];|] temp
                    temp <- Array.append [|elem;|] temp
                    for i = pos - 1 downto 0 do
                        temp <- Array.append [|list.[i];|] temp
                    list <- temp
                    true

            member this.del_front () =
                if list.Length = 0 then false
                else
                    let mutable temp = [||]
                    for i = list.Length - 1 downto 1 do
                        temp <- Array.append [|list.[i];|] temp
                    list <- temp
                    true

            member this.del_back () =
                if list.Length = 0 then false
                else
                    let mutable temp = [||]
                    for i = list.Length - 2 downto 0 do
                        temp <- Array.append [|list.[i];|] temp
                    list <- temp
                    true
            
            member this.del_from pos =
                if list.Length = 0 then false
                else 
                    if list.Length < pos then false
                    else
                        let mutable temp = [||]
                        for i = list.Length - 1 downto 0 do
                            if i <> pos then 
                                temp <- Array.append [|list.[i];|] temp
                        list <- temp
                        true
            
            member this.find func =
                let t = Array.filter func list
                if t.Length = 0 then None else Some t.[0]
            
            member this.concat newList = 
                let mutable result = true
                while newList.is_empty() = false do
                    match newList.top(); with
                    | None   -> result <- false 
                    | Some x -> 
                        list <- Array.append list [|x;|]
                        result <- newList.del_front()
                result

            member this.print () = printf "%A\n\n" list
        
        override this.ToString() =
            let mutable result = "[ "
            for i in list do
                result <- result + i.ToString() + "; "
            result <- result + "]"
            result
    end


[<EntryPoint>]
let main argv =
    0