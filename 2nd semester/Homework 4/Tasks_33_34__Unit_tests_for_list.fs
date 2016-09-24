(*
Homework 4 (23.03.2015)
Tasks 33 - 34.

Author: Mikhail Kita, group 171
*)

open NUnit.Framework

// The interface for list
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

// Realisation of IList using ADT
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

// Realisation of IList using array
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


// The 33rd task
let list = new ADTList<int> () :> IList<int>

[<Test>]
let ``Test 01-ADTList: is_empty`` () = 
    list.push_back(1)
    Assert.AreEqual(false, list.is_empty())
    ignore(list.del_back())
    Assert.AreEqual(true, list.is_empty())

[<Test>]
let ``Test 02-ADTList: top`` () = 
    list.push_back(1)
    Assert.AreEqual(Some 1, list.top())
    ignore(list.del_back())
    Assert.AreEqual(None, list.top())

[<TestCase (3, Result = "[ 3; ]")>]
[<TestCase (2, Result = "[ 2; 3; ]")>]
[<TestCase (1, Result = "[ 1; 2; 3; ]")>]
let ``Test 03-ADTList: push_front`` elem =
    list.push_front(elem)
    list.ToString()

[<TestCase (4, Result = "[ 1; 2; 3; 4; ]")>]
[<TestCase (5, Result = "[ 1; 2; 3; 4; 5; ]")>]
[<TestCase (6, Result = "[ 1; 2; 3; 4; 5; 6; ]")>]
let ``Test 04-ADTList: push_back`` elem =
    list.push_back(elem)
    list.ToString()

[<TestCase (7,   3, Result = "[ 1; 2; 3; 7; 4; 5; 6; ]")>]
[<TestCase (8,   0, Result = "[ 8; 1; 2; 3; 7; 4; 5; 6; ]")>]
[<TestCase (9,   8, Result = "[ 8; 1; 2; 3; 7; 4; 5; 6; 9; ]")>]
[<TestCase (10, 42, Result = "[ 8; 1; 2; 3; 7; 4; 5; 6; 9; ]")>]
let ``Test 05-ADTList: push_to`` elem pos =
    ignore(list.push_to elem pos)
    list.ToString()

[<TestCase (Result = "[ 1; 2; 3; 7; 4; 5; 6; 9; ]")>]
[<TestCase (Result = "[ 2; 3; 7; 4; 5; 6; 9; ]")>]
let ``Test 06.1-ADTList: del_front`` () =
    ignore(list.del_front())
    list.ToString()

[<Test>]
let ``Test 06.2-ADTList: del_front from empty list`` () =
    let list' = new ADTList<int> () :> IList<int>
    list'.push_back(1)
    ignore(list'.del_front())
    Assert.AreEqual("[ ]", list'.ToString())
    ignore(list'.del_front())
    Assert.AreEqual("[ ]", list'.ToString())

[<TestCase (Result = "[ 2; 3; 7; 4; 5; 6; ]")>]
[<TestCase (Result = "[ 2; 3; 7; 4; 5; ]")>]
let ``Test 07.1-ADTList: del_back`` () =
    ignore(list.del_back())
    list.ToString()

[<Test>]
let ``Test 07.2-ADTList: del_back from empty list`` () =
    let list' = new ADTList<int> () :> IList<int>
    list'.push_back(1)
    ignore(list'.del_back())
    Assert.AreEqual("[ ]", list'.ToString())
    ignore(list'.del_back())
    Assert.AreEqual("[ ]", list'.ToString())

[<TestCase ( 3, Result = "[ 2; 3; 7; 5; ]")>]
[<TestCase ( 0, Result = "[ 3; 7; 5; ]")>]
[<TestCase ( 2, Result = "[ 3; 7; ]")>]
[<TestCase (42, Result = "[ 3; 7; ]")>]
let ``Test 08.1-ADTList: del_from`` pos =
    ignore(list.del_from(pos))
    list.ToString()

[<Test>]
let ``Test 08.2-ADTList: del_from empty list`` () =
    let list' = new ADTList<int> () :> IList<int>
    list'.push_back(1)
    ignore(list'.del_from(0))
    Assert.AreEqual("[ ]", list'.ToString())
    ignore(list'.del_from(0))
    Assert.AreEqual("[ ]", list'.ToString())

[<Test>]
let ``Test 09-ADTList: find`` () =
    Assert.AreEqual(Some 7, list.find(fun x -> x = 7))
    Assert.AreEqual(Some 3, list.find(fun x -> x = 3))
    Assert.AreEqual(None, list.find(fun x -> x = 42))
    Assert.AreEqual(None, list.find(fun x -> x % 2 = 0))

[<Test>]
let ``Test 10-ADTList: concat``() =
    let newADTList = new ADTList<int> () :> IList<int>
    let newArrayList = new ArrayList<int> () :> IList<int>
    
    ignore(list.concat newADTList)
    Assert.AreEqual("[ 3; 7; ]", list.ToString())
    ignore(list.concat newArrayList)
    Assert.AreEqual("[ 3; 7; ]", list.ToString())
    
    newADTList.push_back(2)
    newADTList.push_back(4)
    ignore(list.concat newADTList)
    Assert.AreEqual("[ 3; 7; 2; 4; ]", list.ToString())

    newArrayList.push_back(5)
    newArrayList.push_back(9)
    ignore(list.concat newArrayList)
    Assert.AreEqual("[ 3; 7; 2; 4; 5; 9; ]", list.ToString())


// The 34th task
let newList = new ArrayList<int> () :> IList<int>

[<Test>]
let ``Test 01-ArrayList: is_empty`` () = 
    newList.push_back(1)
    Assert.AreEqual(false, newList.is_empty())
    ignore(newList.del_back())
    Assert.AreEqual(true, newList.is_empty())

[<Test>]
let ``Test 02-ArrayList: top`` () = 
    newList.push_back(1)
    Assert.AreEqual(Some 1, newList.top())
    ignore(newList.del_back())
    Assert.AreEqual(None, newList.top())

[<TestCase (3, Result = "[ 3; ]")>]
[<TestCase (2, Result = "[ 2; 3; ]")>]
[<TestCase (1, Result = "[ 1; 2; 3; ]")>]
let ``Test 03-ArrayList: push_front`` elem =
    newList.push_front(elem)
    newList.ToString()

[<TestCase (4, Result = "[ 1; 2; 3; 4; ]")>]
[<TestCase (5, Result = "[ 1; 2; 3; 4; 5; ]")>]
[<TestCase (6, Result = "[ 1; 2; 3; 4; 5; 6; ]")>]
let ``Test 04-ArrayList: push_back`` elem =
    newList.push_back(elem)
    newList.ToString()

[<TestCase (7,   3, Result = "[ 1; 2; 3; 7; 4; 5; 6; ]")>]
[<TestCase (8,   0, Result = "[ 8; 1; 2; 3; 7; 4; 5; 6; ]")>]
[<TestCase (9,   8, Result = "[ 8; 1; 2; 3; 7; 4; 5; 6; 9; ]")>]
[<TestCase (10, 42, Result = "[ 8; 1; 2; 3; 7; 4; 5; 6; 9; ]")>]
let ``Test 05-ArrayList: push_to`` elem pos =
    ignore(newList.push_to elem pos)
    newList.ToString()

[<TestCase (Result = "[ 1; 2; 3; 7; 4; 5; 6; 9; ]")>]
[<TestCase (Result = "[ 2; 3; 7; 4; 5; 6; 9; ]")>]
let ``Test 06.1-ArrayList: del_front`` () =
    ignore(newList.del_front())
    newList.ToString()

[<Test>]
let ``Test 06.2-ArrayList: del_front from empty list`` () =
    let list' = new ArrayList<int> () :> IList<int>
    list'.push_back(1)
    ignore(list'.del_front())
    Assert.AreEqual("[ ]", list'.ToString())
    ignore(list'.del_front())
    Assert.AreEqual("[ ]", list'.ToString())

[<TestCase (Result = "[ 2; 3; 7; 4; 5; 6; ]")>]
[<TestCase (Result = "[ 2; 3; 7; 4; 5; ]")>]
let ``Test 07.1-ArrayList: del_back`` () =
    ignore(newList.del_back())
    newList.ToString()

[<Test>]
let ``Test 07.2-ArrayList: del_back from empty list`` () =
    let list' = new ArrayList<int> () :> IList<int>
    list'.push_back(1)
    ignore(list'.del_back())
    Assert.AreEqual("[ ]", list'.ToString())
    ignore(list'.del_back())
    Assert.AreEqual("[ ]", list'.ToString())

[<TestCase ( 3, Result = "[ 2; 3; 7; 5; ]")>]
[<TestCase ( 0, Result = "[ 3; 7; 5; ]")>]
[<TestCase ( 2, Result = "[ 3; 7; ]")>]
[<TestCase (42, Result = "[ 3; 7; ]")>]
let ``Test 08.1-ArrayList: del_from`` pos =
    ignore(newList.del_from(pos))
    newList.ToString()

[<Test>]
let ``Test 08.2-ArrayList: del_from empty list`` () =
    let list' = new ArrayList<int> () :> IList<int>
    list'.push_back(1)
    ignore(list'.del_from(0))
    Assert.AreEqual("[ ]", list'.ToString())
    ignore(list'.del_from(0))
    Assert.AreEqual("[ ]", list'.ToString())

[<Test>]
let ``Test 09-ArrayList: find`` () =
    Assert.AreEqual (Some 7, newList.find(fun x -> x = 7))
    Assert.AreEqual (Some 3, newList.find(fun x -> x = 3))
    Assert.AreEqual (None, newList.find(fun x -> x = 42))
    Assert.AreEqual (None, newList.find(fun x -> x % 2 = 0))

[<Test>]
let ``Test 10-ArrayList: concat``() =
    let newADTList = new ADTList<int> () :> IList<int>
    let newArrayList = new ArrayList<int> () :> IList<int>
    
    ignore(newList.concat newADTList)
    Assert.AreEqual ("[ 3; 7; ]", newList.ToString())
    ignore(newList.concat newArrayList)
    Assert.AreEqual ("[ 3; 7; ]", newList.ToString())
    
    newADTList.push_back(2)
    newADTList.push_back(4)
    ignore(newList.concat newADTList)
    Assert.AreEqual ("[ 3; 7; 2; 4; ]", newList.ToString())

    newArrayList.push_back(5)
    newArrayList.push_back(9)
    ignore(newList.concat newArrayList)
    Assert.AreEqual ("[ 3; 7; 2; 4; 5; 9; ]", newList.ToString())

// Tests are cover 95.29% of code

[<EntryPoint>]
let main argv =
    0