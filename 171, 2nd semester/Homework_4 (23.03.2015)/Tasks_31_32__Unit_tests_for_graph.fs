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


[<EntryPoint>]
let main argv =
    0