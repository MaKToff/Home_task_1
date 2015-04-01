(*
Homework 5 (30.03.2015)
Tasks 37 - 38

Author: Mikhail Kita, group 171
*)

open NUnit.Framework
open System.IO

type ADTStack<'A> = Nil | Cons of 'A * ADTStack<'A>

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


//the 37th task

//splits input string to tokens and converts them to expression in reverse polish notation
let convert () =
    use inputStream = new StreamReader("test.in")
    use outputStream = new StreamWriter("test.out")
    let str = inputStream.ReadToEnd()

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

    //checks if string is operator
    let isOperator (a : string) = (a.Length = 1 && priority(a) > 0)    

    //lexical analysis of input expression
    let mutable tokens = []
    let mutable temp = ""
    for i = 0 to str.Length - 3 do
        let t = str.[i] 
        if System.Char.IsDigit(t) then temp <- temp + t.ToString()
        else
            match t with
            | ' ' ->
                if System.Char.IsDigit(str.[i - 1]) then
                    tokens <- List.append tokens [temp;]
                    temp <- ""
            | '-' ->
                if System.Char.IsDigit(str.[i + 1]) then temp <- "-"
                else tokens <- List.append tokens ["-";]
            | '(' -> tokens <- List.append tokens ["(";]
            | ')' ->
                if temp.Length > 0 then
                    tokens <- List.append tokens [temp;]
                    temp <- "" 
                tokens <- List.append tokens [")";]
            |  _  -> tokens <- List.append tokens [t.ToString();]
    
    if temp.Length > 0 then tokens <- List.append tokens [temp;]

    //syntax analysis of input expression
    let stack = new Stack<string>()
    for t in tokens do
        if t.Length > 1 || System.Char.IsDigit(t.[0]) then 
            outputStream.WriteLine(t)
        else
            match t with
            | "(" -> stack.push(t)
            | ")" ->
                while stack.top() <> "(" && (not stack.isEmpty) do
                    outputStream.WriteLine(stack.pop())
                ignore(stack.pop())
            | _   ->
                while not stack.isEmpty 
                    && (priority(stack.top()) >= priority(t) && priority(t) < 3
                        || (priority(stack.top()) >  priority(t) && priority(t) = 3))
                            do outputStream.WriteLine(stack.pop())
                stack.push(t)

    while not stack.isEmpty do outputStream.WriteLine(stack.pop())


//the 38th task

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

//computes value of expression
let compute () =
    use inputStream = new StreamReader("test.in")
    use outputStream = new StreamWriter("test.out")
    
    //calculates the result
    let stack = new Stack<float>()
    while not inputStream.EndOfStream do
        let str = inputStream.ReadLine()
        if str.Length > 0 then
            if str.Length > 1 || System.Char.IsDigit(str.[0]) then 
                stack.push(stringToFloat str)
            else
                let a = stack.pop()
                let b = stack.pop()
        
                //applies operator to two operands
                match str with
                | "+" -> stack.push(a + b)
                | "-" -> stack.push(b - a)
                | "*" -> stack.push(a * b)
                | "/" -> stack.push(b / a)
                | "%" -> stack.push(b % a)
                | "^" ->
                    let rec pow elem p =
                        match p with
                        | 0 -> 1.0
                        | 1 -> elem
                        | p -> elem * (pow elem (p - 1))
                    if a >= 0.0 then stack.push(pow b (System.Convert.ToInt32(a)))
                    else stack.push(1.0 / (pow b (-System.Convert.ToInt32(a))))
                | _   -> failwith "Incorrect operator"
    
    outputStream.WriteLine(stack.pop())


//writes string into input file
let write (str : string) =
    use stream = new StreamWriter("test.in")
    stream.WriteLine(str)

//reads string from output file
let read () =
    use stream = new StreamReader("test.out")
    stream.ReadToEnd()

[<TestCase ("5 + (-5)", Result = "0")>]
[<TestCase ("555 % 10", Result = "5")>]
[<TestCase ("2 + 2 * 2", Result = "6")>]
[<TestCase ("1 ^ 10000", Result = "1")>]
[<TestCase ("1 + (-101)", Result = "-100")>]
[<TestCase ("123456789 ^ 0", Result = "1")>]
[<TestCase ("((1 + 2) * 3) ^ 4", Result = "6561")>]
[<TestCase ("999999999 + 1", Result = "1000000000")>]
[<TestCase ("3 + 4 * 2 / (1 - 5) ^ 2", Result = "3,5")>]
[<TestCase ("1 + 1 + 1 + 1 + 1 + 1 * 0", Result = "5")>]
[<TestCase ("7 + 6 - 5 * (4 / (3 % 2 ^ 1))", Result = "-7")>]
[<TestCase ("((1 + 2) * 3) ^ (-1)", Result = "0,111111111111111")>]
[<TestCase ("7 + 6 - 5 * 4 / 3 % 2 ^ 1", Result = "12,3333333333333")>]
[<TestCase ("(34 + 81) * 59 / 134 - (35 - 31) ^ 3", Result = "-13,365671641791")>]
[<TestCase ("110 - (73 / 56 * 98 - 465 % 23) + 121 / (45 - 34) ^ 2", Result = "-11,75")>]
let ``Test`` (expression : string) =
    write(expression)
    convert()
    write(read())
    compute()
    (read()).TrimEnd('\r', '\n')

//Tests are cover 93.47% of code

[<EntryPoint>]
let main argv =
    0