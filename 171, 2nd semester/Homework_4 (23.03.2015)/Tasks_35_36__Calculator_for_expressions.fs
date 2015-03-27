(*
Homework 4 (23.03.2015)
Tasks 35 - 36

Author: Mikhail Kita, group 171
*)

open NUnit.Framework

//the 35th task
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

let priority operator =
    match operator with
    | '+' -> 1
    | '-' -> 1
    | '*' -> 2
    | '/' -> 2
    | '%' -> 2
    | '^' -> 3
    | _   -> 0

let isOperator (a : string) = 
    if a.Length = 1 && priority(a.[0]) > 0 then true else false

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

let apply a b operator =
    let rec pow elem p =
        match p with
        | 0 -> 1.0
        | 1 -> elem
        | p -> elem * (pow elem (p - 1))
    match operator with
    | "+" -> a + b
    | "-" -> b - a
    | "*" -> a * b
    | "/" -> b / a
    | "%" -> b % a 
    | "^" ->
        if a >= 0.0 then pow b (System.Convert.ToInt32(a))
        else 1.0 / (pow b (-System.Convert.ToInt32(a)))
    | _   -> failwith "Incorrect operator"

type Calculator<'A> (expression : string) =
    class
        //converts expression to reverse polish notation
        let convert () =
            let stack = new Stack<char>()
            let result = new Stack<string>()
            let mutable temp = ""
            
            for i = 0 to expression.Length - 1 do
                let t = expression.[i]
                if System.Char.IsDigit(t) then temp <- temp + t.ToString()
                else
                    match t with
                    | ' ' ->
                        if System.Char.IsDigit(expression.[i - 1]) then
                            result.push(temp)
                            temp <- ""
                    | '(' -> stack.push(t)
                    | ')' ->
                        if temp.Length > 0 then
                            result.push(temp)
                            temp <- "" 
                        while stack.top() <> '(' && stack.length > 0 do
                            result.push(stack.pop().ToString())
                        ignore(stack.pop())
                    | _   ->
                        if t = '-' && System.Char.IsDigit(expression.[i + 1]) then
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
            while stack.length > 0 do 
                result.push(stack.pop().ToString())
            result
        
        //computes value of expression
        member this.compute () =
            let stack = convert()

            let rec app op =
                let mutable a = 0.0
                let mutable b = 0.0
                let mutable temp = stack.pop()

                if isOperator(temp) then a <- app temp
                    else a <- stringToFloat temp
                temp <- stack.pop()
                if isOperator(temp) then b <- app temp
                    else b <- stringToFloat temp
                apply a b op
            
            app (stack.pop())
    end

[<TestCase ("5 + (-5)", Result = 0.0)>]
[<TestCase ("555 % 10", Result = 5.0)>]
[<TestCase ("2 + 2 * 2", Result = 6.0)>]
[<TestCase ("1 + (-101)", Result = -100.0)>]
[<TestCase ("123456789 ^ 0", Result = 1.0)>]
[<TestCase ("999999999 + 1", Result = 1000000000.0)>]
[<TestCase ("((1 + 2) * 3) ^ 4", Result = 6561.0)>]
[<TestCase ("1 + 1 + 1 + 1 + 1 + 1 * 0", Result = 5.0)>]
[<TestCase ("3 + 4 * 2 / (1 - 5) ^ 2", Result = 3.5)>]
[<TestCase ("((1 + 2) * 3) ^ (-1)", Result = 0.11111111111111111)>]
[<TestCase ("(34 + 81) * 59 / 134 - (35 - 31) ^ 3", Result = -13.365671641791046)>]
let ``Test 01-Calculator`` expression =
    let calc = new Calculator<int> (expression)
    calc.compute()


//the 36th task
type CalculatorWithVariables<'A> (expression : string, context : float array) =
    class
        let isVariable t =
            if t = 'p' then true else false
        
        //converts expression to reverse polish notation
        let convert () =
            let stack = new Stack<char>()
            let result = new Stack<string>()
            let mutable temp = ""

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

            for i = 0 to str.Length - 1 do
                let t = str.[i]
                if System.Char.IsDigit(t) || isVariable(t) then 
                    temp <- temp + t.ToString()
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
            while stack.length > 0 do 
                result.push(stack.pop().ToString())
            
            result

        //computes value of expression
        member this.compute () =
            let stack = convert()
            
            let getIndex (str : string) =
                let mutable temp = 0
                for i = 2 to str.Length - 2 do
                    temp <- temp * 10 + System.Convert.ToInt32(str.[i]) - 48
                temp

            let rec app op =
                let mutable a = 0.0
                let mutable b = 0.0
                let mutable temp = stack.pop()

                if isOperator(temp) then a <- app temp
                    else 
                        if isVariable(temp.[0]) then a <- context.[getIndex temp]
                        else 
                            if temp.[0] = '-' && isVariable(temp.[1]) then
                                a <- -1.0 * context.[getIndex (temp.TrimStart('-'))]
                            else a <- stringToFloat temp
                temp <- stack.pop()
                if isOperator(temp) then b <- app temp
                    else 
                        if isVariable(temp.[0]) then b <- context.[getIndex temp]
                        else 
                            if temp.[0] = '-' && isVariable(temp.[1]) then
                                b <- -1.0 * context.[getIndex (temp.TrimStart('-'))]
                            else b <- stringToFloat temp
                apply a b op
            
            app (stack.pop())
    end

[<TestCase ("555 % p[0]", [|10.0;|], Result = 5.0)>]
[<TestCase ("1 + 101p[0]", [|-1.0;|], Result = -100.0)>]
[<TestCase ("p[0] ^ 0", [|123456789.0;|], Result = 1.0)>]
[<TestCase ("5p[0] + (-5p[1])", [|1.0; 1.0;|], Result = 0.0)>]
[<TestCase ("((1 + 2) * 3) ^ (-p[0])", [|-1.0;|], Result = 9.0)>]
[<TestCase ("p[0] + p[1] * p[2]", [|2.0; 2.0; 2.0;|], Result = 6.0)>]
[<TestCase ("1000000000 + p[0]", [|999999999.0;|], Result = 1999999999.0)>]
[<TestCase ("p[0] ^ 2 + p[1] ^ 2 + p[2] ^ 2", [|3.0; 4.0; 0.0;|], Result = 25.0)>]
[<TestCase ("3p[0] + 4 * 2 / (1 - 5) ^ 2", [|4.998997;|], Result = 15.496991000000001)>]
[<TestCase ("(34p[0] + 81) * 59 / 134p[1] - (35 - 31) ^ 3", [|1.0; 2.0;|], Result = -38.682835820895522)>]
let ``Test 01-CalculatorWithVariables`` expression context =
    let calc = new CalculatorWithVariables<int> (expression, context)
    calc.compute()

[<Test>]
let ``Test 02-CalculatorWithVariables: no variables`` () =
    let calc = new CalculatorWithVariables<int> ("((1 + 2) * 3) ^ 4", [||])
    Assert.AreEqual(6561.0, calc.compute())

//Tests are cover 96.05% of code

[<EntryPoint>]
let main argv =
    0