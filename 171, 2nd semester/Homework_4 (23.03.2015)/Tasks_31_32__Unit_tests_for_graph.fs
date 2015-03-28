(*
Homework 4 (23.03.2015)
Tasks 31 - 32

Author: Mikhail Kita, group 171
*)

open NUnit.Framework

//the interface of graph
type IGraph<'A> = 
    interface
        abstract nodes : 'A array
        abstract nodesNumber : int
        abstract hasEdge : 'A -> 'A -> bool
    end 

//realisation of IGraph on adjacency matrix
type MatrixGraph<'A when 'A: equality> (arr : 'A array, matrix : (bool array) array) =
    class
        interface IGraph<'A> with
            member this.nodes = arr
            member this.nodesNumber = arr.Length
            member this.hasEdge node1 node2 =
                let index1 = Array.findIndex (fun x -> x = node1) arr
                let index2 = Array.findIndex (fun x -> x = node2) arr
                matrix.[index1].[index2]
    end

//realisation of IGraph on adjacency list
type ListGraph<'A when 'A: equality> (arr : 'A array, arrayOfLists : ('A list) array) =
    class
        interface IGraph<'A> with
            member this.nodes = arr
            member this.nodesNumber = arr.Length
            member this.hasEdge node1 node2 =
                let index = Array.findIndex (fun x -> x = node1) arr
                List.exists (fun x -> x = node2) arrayOfLists.[index]
    end

//realisation of depth-first search algorithm
let rec dfs (graph : IGraph<'A>) (visited : bool array) node flag =
    let mutable result = []
    let index = Array.findIndex (fun x -> x = node) graph.nodes
    visited.[index] <- true 
    for i in 0 .. graph.nodesNumber - 1 do
        if not visited.[i] 
            && (flag = 0 && graph.hasEdge node graph.nodes.[i]
                || flag = 1 && graph.hasEdge graph.nodes.[i] node)
        then
            visited.[i] <- true
            result <- List.append result [graph.nodes.[i];]
            result <- List.append result (dfs graph visited graph.nodes.[i] flag)        
    result

//function for searching nodes which available from given node
let availableFrom (graph : IGraph<'A>) node =
    let visited = [|for i in 0 .. graph.nodesNumber - 1 -> false|]
    dfs graph visited node 0

//function for searching nodes which have access to given node
let haveAccessTo (graph : IGraph<'A>) node =
    let visited = [|for i in 0 .. graph.nodesNumber - 1 -> false|]
    dfs graph visited node 1


//the 31st task

//(1) ← (2) → (6)
// ↓     ↑
//(5) ← (4) ← (8)
// ↑     ↓     ↓
//(7) → (9) ↔ (3)

let fls = false
let example = [|5; 8; 3; 9; 2; 1; 4; 7; 6;|]
let matrix = 
    [|
        [| fls;  fls;  fls;  fls;  fls;  fls;  fls;  fls;  fls;|]; 
        [| fls;  fls; true;  fls;  fls;  fls; true;  fls;  fls;|]; 
        [| fls;  fls;  fls; true;  fls;  fls;  fls;  fls;  fls;|]; 
        [| fls;  fls; true;  fls;  fls;  fls;  fls;  fls;  fls;|]; 
        [| fls;  fls;  fls;  fls;  fls; true;  fls;  fls; true;|]; 
        [|true;  fls;  fls;  fls;  fls;  fls;  fls;  fls;  fls;|]; 
        [|true;  fls;  fls; true; true;  fls;  fls;  fls;  fls;|];
        [|true;  fls;  fls; true;  fls;  fls;  fls;  fls;  fls;|];
        [| fls;  fls;  fls;  fls;  fls;  fls;  fls;  fls;  fls;|]; 
    |]
let arrayOfLists = [| []; [3; 4;]; [9;]; [3;]; [1; 6;]; [5;]; [2; 5; 9;]; [5; 9;]; [];|]

let listToString list =
    let mutable result = "[ "
    for i in list do
        result <- result + i.ToString() + "; "
    result + "]"

[<TestCase (1, Result = "[ 5; ]")>]
[<TestCase (2, Result = "[ 1; 5; 6; ]")>]
[<TestCase (3, Result = "[ 9; ]")>]
[<TestCase (4, Result = "[ 5; 9; 3; 2; 1; 6; ]")>]
[<TestCase (5, Result = "[ ]")>]
[<TestCase (6, Result = "[ ]")>]
[<TestCase (7, Result = "[ 5; 9; 3; ]")>]
[<TestCase (8, Result = "[ 3; 9; 4; 5; 2; 1; 6; ]")>]
[<TestCase (9, Result = "[ 3; ]")>]
let ``Test 01-MatrixGraph: nodes, which available form`` elem =
    let mgr = new MatrixGraph<int> (example, matrix)
    listToString(availableFrom mgr elem)

[<TestCase (1, Result = "[ 5; ]")>]
[<TestCase (2, Result = "[ 1; 5; 6; ]")>]
[<TestCase (3, Result = "[ 9; ]")>]
[<TestCase (4, Result = "[ 5; 9; 3; 2; 1; 6; ]")>]
[<TestCase (5, Result = "[ ]")>]
[<TestCase (6, Result = "[ ]")>]
[<TestCase (7, Result = "[ 5; 9; 3; ]")>]
[<TestCase (8, Result = "[ 3; 9; 4; 5; 2; 1; 6; ]")>]
[<TestCase (9, Result = "[ 3; ]")>]
let ``Test 01-ListGraph: nodes, which available form`` elem =
    let lgr = new ListGraph<int> (example, arrayOfLists)
    listToString(availableFrom lgr elem) 

[<TestCase (1, Result = "[ 2; 4; 8; ]")>]
[<TestCase (2, Result = "[ 4; 8; ]")>]
[<TestCase (3, Result = "[ 8; 9; 4; 7; ]")>]
[<TestCase (4, Result = "[ 8; ]")>]
[<TestCase (5, Result = "[ 1; 2; 4; 8; 7; ]")>]
[<TestCase (6, Result = "[ 2; 4; 8; ]")>]
[<TestCase (7, Result = "[ ]")>]
[<TestCase (8, Result = "[ ]")>]
[<TestCase (9, Result = "[ 3; 8; 4; 7; ]")>]
let ``Test 02-MatrixGraph: nodes, which have access to`` elem =
    let mgr = new MatrixGraph<int> (example, matrix)
    listToString(haveAccessTo mgr elem)

[<TestCase (1, Result = "[ 2; 4; 8; ]")>]
[<TestCase (2, Result = "[ 4; 8; ]")>]
[<TestCase (3, Result = "[ 8; 9; 4; 7; ]")>]
[<TestCase (4, Result = "[ 8; ]")>]
[<TestCase (5, Result = "[ 1; 2; 4; 8; 7; ]")>]
[<TestCase (6, Result = "[ 2; 4; 8; ]")>]
[<TestCase (7, Result = "[ ]")>]
[<TestCase (8, Result = "[ ]")>]
[<TestCase (9, Result = "[ 3; 8; 4; 7; ]")>]
let ``Test 02-ListGraph: nodes, which have access to`` elem =
    let lgr = new ListGraph<int> (example, arrayOfLists)
    listToString(haveAccessTo lgr elem)


//the interface of labeled graph
type ILabeledGraph<'A, 'B> =
    interface
        inherit IGraph<'A>
        abstract labels : 'B array
    end

//imitation of local network
let infectionProb OS = 
    match OS with
    | "Windows" -> 0.647
    | "Linux"   -> 0.310
    | "FreeBSD" -> 0.154
    | "OS X"    -> 0.281
    | _         -> failwith "Incorrect OS"

type Computer (number, OS) =
    class
        let mutable infected = false

        member this.number = number
        member this.OS = OS
        member this.isInfected () = infected
        
        member this.infect (probability) = 
            if probability <= infectionProb (OS) then infected <- true

        member this.info () = 
            if infected then printfn "%d: Infected" number 
            else printfn "%d: Not infected" number
    end

type ComputerGraph (OSList : string list, labels : bool array, aList : (int list) array) =
    class
        let n = OSList.Length
        let comp = [|for i in [0 .. n - 1] -> new Computer(i, OSList.[i])|]

        interface ILabeledGraph<Computer, bool> with
            member this.nodes = comp
            member this.nodesNumber = n
            member this.hasEdge node1 node2 =
                List.exists (fun x -> x = node2.number) aList.[node1.number]
            
            member this.labels = labels
    end

type LocalNetwork (graph : ILabeledGraph<Computer, bool>) =
    class
        let mutable move = 0
        let n = graph.nodesNumber - 1

        do
            for i = 0 to n do
                if graph.labels.[i] then graph.nodes.[i].infect(0.0)

        member this.infectedNumber =
            let mutable answer = 0
            for i in graph.nodes do
                if i.isInfected() then answer <- answer + 1
            answer

        member this.status () =
            printf "\n\nMove: %d\nStatus:\n" move
            let c = [|for i in graph.nodes -> if i.isInfected() then '#' else ' '|]
            printf "
            (1. Windows)%c -- (2. FreeBSD)%c -- (3. OS X   )%c
             |                |
             |                |
            (0. Linux  )%c -- (4. Linux  )%c -- (5. Windows)%c
            
            
            (6. Linux  )%c -- (7. Windows)%c
             |                |
             |                |
            (8. OS X   )%c -- (9. FreeBSD)%c
            \n\n" c.[0] c.[1] c.[2] c.[3] c.[4] c.[5] c.[6] c.[7] c.[8] c.[9]
            for i in graph.nodes do i.info()
            printf "\nPress any key to continue . . . "
            match System.Console.ReadKey().Key with | _ -> ()

        member this.start correction =
            move <- move + 1
            let inf = Array.filter (fun i -> graph.nodes.[i].isInfected()) [|0 .. n|]
            for i in inf do
                for j = 0 to n do
                    if graph.hasEdge graph.nodes.[i] graph.nodes.[j] then
                        let rand = System.Random().NextDouble() + correction
                        graph.nodes.[j].infect(rand)
            for i = 0 to n do
                if graph.nodes.[i].isInfected() then graph.labels.[i] <- true
    end


//the 32nd task
let OSList = 
    [ "Linux"; "Windows"; "FreeBSD"; "OS X"; "Linux"; "Windows"; 
        "Linux"; "Windows"; "OS X"; "FreeBSD"]

[<TestCase ( 1.0, Result =  2)>] //probability of infection = 0
[<TestCase (-1.0, Result = 10)>] //probability of infection = 1
let ``Test 01: branched local network`` correction =
    let labels = [| fls; true; fls; fls; fls; fls; fls; true; fls; fls |]
    let aList = 
        [| [1; 4;]; [0; 2;]; [1; 3; 4;]; [2;]; [0; 2; 5;]; [4;]; 
            [7; 8;]; [6; 9;]; [6; 9;]; [7; 8;]; |]
    
    let graph = new ComputerGraph (OSList, labels, aList) :> ILabeledGraph<Computer, bool>
    let network = new LocalNetwork (graph)
    let mutable n = 0

    if correction = -1.0 then n <- 2 else n <- 10000
    for i = 0 to n do 
        network.start(correction)
    network.infectedNumber

[<TestCase ( 1.0, Result =  1)>] //probability of infection = 0
[<TestCase (-1.0, Result = 10)>] //probability of infection = 1
let ``Test 02: linear local network`` correction =
    let labels = [| true; fls; fls; fls; fls; fls; fls; fls; fls; fls |]
    let aList = [| [1;]; [2;]; [3;]; [4;]; [5;]; [6;]; [7;]; [8;]; [9;]; []; |]
    
    let graph = new ComputerGraph (OSList, labels, aList) :> ILabeledGraph<Computer, bool>
    let network = new LocalNetwork (graph)
    let mutable n = 0

    if correction = -1.0 then n <- 9 else n <- 10000
    for i = 0 to n do 
        network.start(correction) 
    network.infectedNumber

//Tests are cover 83.46 % of code

[<EntryPoint>]
let main argv =
    0