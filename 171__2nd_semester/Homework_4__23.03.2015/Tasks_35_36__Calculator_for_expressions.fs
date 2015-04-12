(*
Homework 4 (23.03.2015)
Tasks 35 - 36

Author: Mikhail Kita, group 171
*)

open NUnit.Framework

type ADTStack<'A> = Nil | Cons of 'A * ADTStack<'A>

type Tree<'A> = Null | Node of Tree<'A> * 'A * Tree<'A> 

//realisation of stack
type Stack<'A when 'A: equality> () = 
    class
        let mutable (stack : ADTStack<'A>) = Nil
        
        member this.isEmpty = (stack = Nil)
        member this.push elem = stack <- Cons (elem, stack)
        
        member this.pop () =
            match stack with
            | Nil               -> failwith "Stack is empty"
            | Cons (elem, next) ->
                stack <- next
                elem

        member this.top () = 
            match stack with
            | Nil               -> failwith "Stack is empty"
            | Cons (elem, next) -> elem
    end

//splits input string to tokens and converts them to expression in reverse polish notation
//returns result in tree form
let convert (str : string) =
    
    //returns priority of operator
    let priority operator =
        match operator with
        | "+" -> 1
        | "-" -> 1
        | "*" -> 2
        | "/" -> 2
        | "%" -> 2
        | "^" -> 3
        |  _  -> 0   
    
    //lexical analysis of input expression
    let mutable tokens = []
    let mutable temp = ""
    for i = 0 to str.Length - 1 do
        let t = str.[i] 
        if System.Char.IsDigit(t) then temp <- temp + t.ToString()
        else
            match t with
            | ' ' ->
                if System.Char.IsDigit(str.[i - 1]) then
                    tokens <- List.append tokens [temp;]
                    temp <- ""
            | '-' ->
                if System.Char.IsDigit(str.[i + 1]) || str.[i + 1] = 'p' then temp <- "-"
                else tokens <- List.append tokens ["-";]
            | '(' -> tokens <- List.append tokens ["(";]
            | ')' ->
                if temp.Length > 0 then
                    tokens <- List.append tokens [temp;]
                    temp <- "" 
                tokens <- List.append tokens [")";]
            | '[' -> temp <- temp + t.ToString()
            | ']' -> 
                temp <- temp + t.ToString()
                tokens <- List.append tokens [temp; ")";]
                temp <- ""
            | 'p' ->
                if temp <> "" && temp <> "-" then
                    tokens <- List.append tokens ["("; temp;]
                    temp <- ""
                temp <- temp + t.ToString()
                if i = 0 then tokens <- List.append tokens ["(";]
                else
                    match str.[i - 1] with
                    | ' ' -> tokens <- List.append tokens ["(";]
                    | '-' -> 
                        if str.[i - 2] = ' ' || str.[i - 2] = '(' then 
                            tokens <- List.append tokens ["(";]
                    |  _  ->
                        if System.Char.IsDigit(str.[i - 1])  then 
                            tokens <- List.append tokens ["*";]
            |  _  -> tokens <- List.append tokens [t.ToString();]
    
    if temp.Length > 0 then tokens <- List.append tokens [temp;]

    //syntax analysis of input expression
    let stack = new Stack<string>()
    let result = new Stack<Tree<string>>()
    for t in tokens do
        if t.Length > 1 || System.Char.IsDigit(t.[0]) || t.[0] = 'p' then 
            result.push(Node(Null, t, Null))
        else
            match t with
            | "(" -> stack.push(t)
            | ")" ->
                while stack.top() <> "(" && (not stack.isEmpty) do
                    result.push(Node(result.pop(), stack.pop(), result.pop()))
                ignore(stack.pop())
            | _   ->
                while not stack.isEmpty 
                    && (priority(stack.top()) >= priority(t) && priority(t) < 3
                        || (priority(stack.top()) >  priority(t) && priority(t) = 3))
                            do result.push(Node(result.pop(), stack.pop(), result.pop()))
                stack.push(t)

    while not stack.isEmpty do result.push(Node(result.pop(), stack.pop(), result.pop()))
    result.pop()

//computes value of expression
let compute expression (context : float array) =
    let tree = convert(expression)

    //converts string to float
    let stringToFloat (str : string) =
        let charToDouble (elem : char) = 
            System.Convert.ToDouble(System.Convert.ToInt32(elem) - 48)
        let mutable temp = 0.0
        let negative = 
            match str.[0] with
            | '-' -> true 
            |  _  -> 
                temp <- charToDouble(str.[0])
                false
        for i = 1 to str.Length - 1 do
            temp <- temp * 10.0 + charToDouble(str.[i])
        if negative then -temp else temp
    
    //analyses string and returns float value or value of variable
    let analyse (str : string) =
        let getIndex (str : string) =
            let mutable temp = 0
            for i = 2 to str.Length - 2 do
                temp <- temp * 10 + System.Convert.ToInt32(str.[i]) - 48
            temp

        if str.[0] = 'p' then context.[getIndex str]
        else 
            if str.[0] = '-' && str.[1] = 'p' then 
                -1.0 * context.[getIndex (str.TrimStart('-'))]
            else stringToFloat str
    
    //calculates the result
    let rec apply t =
        match t with
        | Null                       -> failwith "Empty expression"
        | Node (Null, value, Null)   -> analyse(value)
        | Node (left, center, right) ->
            let a = apply left
            let b = apply right
        
            //applies operator to two operands
            match center with
            | "+" -> a + b
            | "-" -> b - a
            | "*" -> a * b
            | "/" -> b / a
            | "%" -> b % a 
            | "^" ->
                let rec pow elem p =
                    match p with
                    | 0 -> 1.0
                    | 1 -> elem
                    | p -> elem * (pow elem (p - 1))
                if a >= 0.0 then pow b (System.Convert.ToInt32(a))
                else 1.0 / (pow b (-System.Convert.ToInt32(a)))
            | _   -> failwith "Incorrect operator"

    apply tree


[<TestCase ("5 + (-5)", Result = 0.0)>]
[<TestCase ("555 % 10", Result = 5.0)>]
[<TestCase ("2 + 2 * 2", Result = 6.0)>]
[<TestCase ("1 ^ 10000", Result = 1.0)>]
[<TestCase ("1 + (-101)", Result = -100.0)>]
[<TestCase ("123456789 ^ 0", Result = 1.0)>]
[<TestCase ("((1 + 2) * 3) ^ 4", Result = 6561.0)>]
[<TestCase ("999999999 + 1", Result = 1000000000.0)>]
[<TestCase ("3 + 4 * 2 / (1 - 5) ^ 2", Result = 3.5)>]
[<TestCase ("1 + 1 + 1 + 1 + 1 + 1 * 0", Result = 5.0)>]
[<TestCase ("7 + 6 - 5 * (4 / (3 % 2 ^ 1))", Result = -7.0)>]
[<TestCase ("((1 + 2) * 3) ^ (-1)", Result = 0.11111111111111111)>]
[<TestCase ("7 + 6 - 5 * 4 / 3 % 2 ^ 1", Result = 12.333333333333333)>]
[<TestCase ("(34 + 81) * 59 / 134 - (35 - 31) ^ 3", Result = -13.365671641791046)>]
[<TestCase ("110 - (73 / 56 * 98 - 465 % 23) + 121 / (45 - 34) ^ 2", Result = -11.75)>]
let ``Test 01: calculator without variables`` expression =
    compute expression [||]

[<TestCase ("125 % p[0]", [|5.0;|], Result = 0.0)>]
[<TestCase ("1 + 101p[0]", [|-1.0;|], Result = -100.0)>]
[<TestCase ("p[0] ^ 0", [|123456789.0;|], Result = 1.0)>]
[<TestCase ("p[0] ^ p[1]", [|7.0; 3.0;|], Result = 343.0)>]
[<TestCase ("10p[0] + (-5p[1])", [|1.0; 2.0;|], Result = 0.0)>]
[<TestCase ("((1 + 2) * 3) ^ (-p[0])", [|-1.0;|], Result = 9.0)>]
[<TestCase ("p[0] + p[1] * p[2]", [|2.0; 2.0; 2.0;|], Result = 6.0)>]
[<TestCase ("32 + 2 * p[0] ^ p[1] ^ 2", [|2.0; 2.0;|], Result = 64.0)>]
[<TestCase ("1000000000 + p[0]", [|999999999.0;|], Result = 1999999999.0)>]
[<TestCase ("p[0] ^ 2 + p[1] ^ 2 + p[2] ^ 2", [|3.0; 4.0; 0.0;|], Result = 25.0)>]
[<TestCase ("3p[0] + 4 * 2 / (1 - 5) ^ 2", [|4.998997;|], Result = 15.496991000000001)>]
[<TestCase ("(34p[0] + 81) * 59 / 134p[1] - (35 - 31) ^ 3", [|1.0; 2.0;|], Result = -38.682835820895522)>]
let ``Test 02: calculator with variables`` expression context =
    compute expression context

//Tests are cover 91.67% of code

[<EntryPoint>]
let main argv =
    0