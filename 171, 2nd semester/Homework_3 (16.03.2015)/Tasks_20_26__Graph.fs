(*
Homework 3 (16.03.2015)
Tasks 20 - 26

Author: Mikhail Kita, group 171
*)

//the 20th task
type IGraph<'A> = 
    interface
        abstract nodes : 'A array
        abstract nodesNumber : int
        abstract hasEdge : 'A -> 'A -> bool
    end 


//the 21st task
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


//the 22nd task
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


//the 23th task
let availableFrom (graph : IGraph<'A>) node =
    let visited = [|for i in 0 .. graph.nodesNumber - 1 -> false|]
    dfs graph visited node 0


//the 24th task
let haveAccessTo (graph : IGraph<'A>) node =
    let visited = [|for i in 0 .. graph.nodesNumber - 1 -> false|]
    dfs graph visited node 1


//the 25th task
type ILabeledGraph<'A, 'B> =
    interface
        inherit IGraph<'A>
        abstract labels : 'B array
    end


//the 26th task
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
        member this.isInfected = infected
        
        member this.infect (probability) = 
            if probability <= infectionProb (OS) then infected <- true

        member this.info () = 
            if infected then printfn "%d. Infected" number 
            else printfn "%d. Not infected" number
    end

type LocalNetwork (OSList : string list, label : bool array, list : (int list) array) =
    class
        let mutable move = 0
        let n = OSList.Length
        let comp = [|for i in [0 .. n - 1] -> new Computer(i, OSList.[i])|]

        do
            for i = 0 to n - 1 do
                if label.[i] then comp.[i].infect(0.0)

        member this.infectedNumber () =
            let mutable answer = 0
            for i in comp do
                if i.isInfected then answer <- answer + 1
            answer

        member this.status () =
            printf "Move: %d\nStatus:\n" move 
            for i in comp do i.info()
            printf "\n\n"
            match System.Console.ReadKey().Key with
            | _ -> ()

        member this.start () =
            move <- move + 1
            let inf = Array.filter (fun i -> comp.[i].isInfected) [|0 .. n - 1|]
            for i in inf do
                for j in list.[i] do
                    let rand = System.Random().NextDouble()
                    comp.[j].infect(rand)
    end


[<EntryPoint>]
let main argv =
    let example = [|5; 8; 3; 9; 2; 1; 4;|]
    let fls = false
    let matrix = 
        [| 
            [| fls;  fls;  fls;  fls;  fls;  fls;  fls;|]; 
            [| fls;  fls; true;  fls;  fls;  fls; true;|]; 
            [| fls;  fls;  fls; true;  fls;  fls;  fls;|]; 
            [| fls;  fls; true;  fls;  fls;  fls;  fls;|]; 
            [| fls;  fls;  fls;  fls;  fls; true;  fls;|]; 
            [|true;  fls;  fls;  fls;  fls;  fls;  fls;|]; 
            [|true;  fls;  fls; true; true;  fls;  fls;|];
        |]
    let arrayOfLists = [| []; [3; 4;]; [9;]; [3;]; [1;]; [5;]; [2; 5; 9;]; |]

    let mgr = new MatrixGraph<int> (example, matrix)
    let lgr = new ListGraph<int> (example, arrayOfLists)
    
    printf "Nodes which available from node '4' (using adjacency matrix):\n"
    printf "%A\n\n" (availableFrom mgr 4)
    printf "Nodes which available from node '4' (using adjacency list):\n"
    printf "%A\n\n" (availableFrom lgr 4)

    printf "Nodes which have access to node '5' (using adjacency matrix):\n"
    printf "%A\n\n" (haveAccessTo mgr 5)
    printf "Nodes which have access to node '5' (using adjacency list):\n"
    printf "%A\n\n\n" (haveAccessTo lgr 5)


    let OSList = [ "Linux"; "Windows"; "FreeBSD"; "OS X"; "Linux"; "Windows"; 
        "Linux"; "Windows"; "OS X" ]
    
    let labels = [| false; true; false; false; false; false; false; true; false |]
    let aList = [| [1;]; [0; 2;]; [1; 3; 4;]; [2;]; [2; 5;]; [4;]; [7; 8;]; [6;]; [6;] |]
    let network = new LocalNetwork (OSList, labels, aList)
    
    printf "Demonstration of work of local network\n\n"
    printf "== Press any key to watch next move ==\n\n"
    network.status()
    while network.infectedNumber() < OSList.Length do
        network.start()
        network.status()
    0