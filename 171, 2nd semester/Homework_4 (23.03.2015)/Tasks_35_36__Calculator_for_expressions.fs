(*
Homework 4 (23.03.2015)
Tasks 35 - 36

Author: Mikhail Kita, group 171
*)

open NUnit.Framework

type Stack<'A> () = 
    class
        let mutable (stack : 'A array) = [||]
        
        member this.length = stack.Length
        member this.push elem = stack <- Array.append [|elem;|] stack
        
        member this.pop () =
            if stack.Length = 0 then failwith "Stack is empty"
            else
                let mutable temp = [||]
                for i = stack.Length - 1 downto 1 do
                    temp <- Array.append [|stack.[i];|] temp
                let res = stack.[0]
                stack <- temp
                res

        member this.top () = 
            if stack.Length = 0 then failwith "Stack is empty"
            else stack.[0]
    end

//returns priority of operator
let priority operator =
    match operator with
    | '+' -> 1
    | '-' -> 1
    | '*' -> 2
    | '/' -> 2
    | '%' -> 2
    | '^' -> 3
    | _   -> 0

//checks if string is operator
let isOperator (a : string) = 
    if a.Length = 1 && priority(a.[0]) > 0 then true else false

//converts string to float
let stringToFloat (str : string) =
    let charToDouble (elem : char) = 
        System.Convert.ToDouble(System.Convert.ToInt32(elem) - 48)
    let mutable temp = 0.0
    let negative = 
        match str.[0] with
        | '-' -> true 
        | _   -> 
            temp <- charToDouble(str.[0])
            false
    for i = 1 to str.Length - 1 do
        temp <- temp * 10.0 + charToDouble(str.[i])
    if negative then -temp else temp

//converts input string to expression in reverse polish notation and splits it to tokens
let convert (expression : string) =
    let stack = new Stack<char>()
    let result = new Stack<string>()
    
    //inserts the sign '*' between the coefficient and the variable (if coefficients exist) 
    //and adds brackets around this expression
    let mutable str = expression
    let mutable j = 1
    let mutable pos = 0
    if str.[0] = 'p' then str <- str.Insert(0, "(")
    while j < str.Length do
        if System.Char.IsDigit(str.[j]) && (str.[j - 1] = ' ' || str.[j - 1] = '-') then 
            pos <- j - 1
        if str.[j] = ']' then str <- str.Insert(j + 1, ")")
        if str.[j] = 'p' then
            if str.[j - 1] = ' ' then str <- str.Insert(j, "(")
            if str.[j - 1] = '-' && (str.[j - 2] = ' ' || str.[j - 2] = '(') then 
                str <- str.Insert(j - 1, "(")
            if System.Char.IsDigit(str.[j - 1])  then 
                str <- str.Insert(j, " * ")
                str <- str.Insert(pos, "(")
                j <- j + 4
            j <- j + 1
        j <- j + 1

    let mutable temp = ""
    for i = 0 to str.Length - 1 do
        let t = str.[i]
        if System.Char.IsDigit(t) || t = 'p' then temp <- temp + t.ToString()
        else
            match t with
            | ' ' ->
                if System.Char.IsDigit(str.[i - 1]) || str.[i - 1] = ']' then
                    result.push(temp)
                    temp <- ""
            | '[' -> temp <- temp + t.ToString()
            | ']' -> temp <- temp + t.ToString()
            | '(' -> stack.push(t)
            | ')' ->
                if temp.Length > 0 then
                    result.push(temp)
                    temp <- "" 
                while stack.top() <> '(' && stack.length > 0 do
                    result.push(stack.pop().ToString())
                ignore(stack.pop())
            | _   ->
                if t = '-' && System.Char.IsDigit(str.[i + 1]) || str.[i + 1] = 'p' then 
                    temp <- "-"
                else
                    while stack.length > 0 
                        && (priority(stack.top()) >= priority(t) && priority(t) < 3
                            || (priority(stack.top()) >  priority(t) && priority(t) = 3))
                                do result.push(stack.pop().ToString())
                    stack.push(t)
            
    if temp.Length > 0 then
        result.push(temp)
        temp <- "" 
    while stack.length > 0 do result.push(stack.pop().ToString())
    result

//computes value of expression
let compute expression (context : float array) =
    let stack = convert(expression)

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

    let rec apply operator =
        let mutable a = 0.0
        let mutable b = 0.0
        let mutable temp = stack.pop()

        if isOperator(temp) then a <- apply temp
        else a <- analyse temp
        temp <- stack.pop()
        if isOperator(temp) then b <- apply temp
        else b <- analyse temp
        
        //applies operator to two operands
        match operator with
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
            
    apply (stack.pop())


[<TestCase ("5 + (-5)", Result = 0.0)>]
[<TestCase ("555 % 10", Result = 5.0)>]
[<TestCase ("2 + 2 * 2", Result = 6.0)>]
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
let ``Test 02: calculator with varaibles`` expression context =
    compute expression context

//Tests are cover 96.49% of code

[<EntryPoint>]
let main argv =
    0