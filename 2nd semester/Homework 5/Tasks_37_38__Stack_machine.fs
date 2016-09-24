(*
Homework 5 (30.03.2015)
Tasks 37 - 38.

Author: Mikhail Kita, group 171
*)

open NUnit.Framework
open System.IO

type ADTStack<'A> = Nil | Cons of 'A * ADTStack<'A>

// Realisation of stack
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


// The 37th task

// Splits the input string to tokens and converts them to the expression in reverse polish notation
let convert (inputFile : string) (outputFile : string) =
    use inputStream = new StreamReader(inputFile)
    use outputStream = new StreamWriter(outputFile)
    let str = inputStream.ReadToEnd()

    // Returns priority of the operator
    let priority operator =
        match operator with
        | "+" -> 1
        | "-" -> 1
        | "*" -> 2
        | "/" -> 2
        | "%" -> 2
        | "^" -> 3
        |  _  -> 0

    // Checks if string is operator
    let isOperator (a : string) = (a.Length = 1 && priority(a) > 0)    

    // Lexical analysis of input expression
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

    // Syntax analysis of input expression
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


// The 38th task

// Converts string to float
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

// Computes value of the expression
let compute (inputFile : string) (outputFile : string) =
    use inputStream = new StreamReader(inputFile)
    use outputStream = new StreamWriter(outputFile)
    
    // Calculates the result
    let stack = new Stack<float>()
    while not inputStream.EndOfStream do
        let str = inputStream.ReadLine()
        if str.Length > 0 then
            if str.Length > 1 || System.Char.IsDigit(str.[0]) then 
                stack.push(stringToFloat str)
            else
                let a = stack.pop()
                let b = stack.pop()
        
                // Applies operator to two operands
                match str with
                | "+" -> stack.push(a + b)
                | "-" -> stack.push(b - a)
                | "*" -> stack.push(a * b)
                | "/" -> stack.push(b / a)
                | "%" -> stack.push(b % a)
                | "^" ->
                    if a >= 0.0 then 
                        stack.push(pown b (System.Convert.ToInt32(a)))
                    else stack.push(1.0 / (pown b (-System.Convert.ToInt32(a))))
                | _   -> failwith "Incorrect operator"
    
    outputStream.WriteLine(stack.pop())


// Writes string into input file
let write (str : string) (file : string) =
    use stream = new StreamWriter(file)
    stream.WriteLine(str)

// Reads string from output file
let read (file : string) =
    use stream = new StreamReader(file)
    stream.ReadToEnd()

[<TestCase ("5 + (-5)", Result = "5 -5 +")>]
[<TestCase ("555 % 10", Result = "555 10 %")>]
[<TestCase ("1 ^ 10000", Result = "1 10000 ^")>]
[<TestCase ("2 + 2 * 2", Result = "2 2 2 * +")>]
[<TestCase ("3 ^ 1 ^ 2", Result = "3 1 2 ^ ^")>]
[<TestCase ("1 - 2 - 3", Result = "1 2 - 3 -")>]
[<TestCase ("7 * (6 + 5)", Result = "7 6 5 + *")>]
[<TestCase ("((1 + 2) * 3) ^ 4", Result = "1 2 + 3 * 4 ^")>]
[<TestCase ("((1 + 2) * 3) ^ (-1)", Result = "1 2 + 3 * -1 ^")>]
[<TestCase ("3 + 4 * 2 / (1 - 5) ^ 2", Result = "3 4 2 * 1 5 - 2 ^ / +")>]
[<TestCase ("7 + 6 - 5 * 4 / 3 % 2 ^ 1", Result = "7 6 + 5 4 * 3 / 2 1 ^ % -")>]
[<TestCase ("7 + 6 - 5 * (4 / (3 % 2 ^ 1))", Result = "7 6 + 5 4 3 2 1 ^ % / * -")>]
let ``Test for 37th task`` (expression : string) =
    let inputFile = "test.in"
    let outputFile = "test.out"
    write expression inputFile
    convert inputFile outputFile
    let mutable answer = ""
    for c in (read(outputFile)) do
        match c with
        | '\r' -> answer <- answer + " "
        | '\n' -> ()
        | char -> answer <- answer + char.ToString()
    answer.TrimEnd(' ')

[<TestCase ("5 + (-5)", Result = "0")>]
[<TestCase ("555 % 10", Result = "5")>]
[<TestCase ("2 + 2 * 2", Result = "6")>]
[<TestCase ("1 ^ 10000", Result = "1")>]
[<TestCase ("3 ^ 1 ^ 2", Result = "3")>]
[<TestCase ("1 - 2 - 3", Result = "-4")>]
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
let ``Test for 38th task`` (expression : string) =
    let inputFile = "test.in"
    let outputFile = "test.out"
    write expression inputFile
    convert inputFile outputFile
    write (read(outputFile)) inputFile
    compute inputFile outputFile
    (read(outputFile)).TrimEnd('\r', '\n')

// Tests are cover 93.07% of code

[<EntryPoint>]
let main argv =
    0