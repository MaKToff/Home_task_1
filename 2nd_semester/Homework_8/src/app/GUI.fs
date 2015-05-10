(*
Homework 8 (20.04.2015)
Graphical user interface for calculator

Author: Mikhail Kita, group 171
*)

open System.Windows.Forms
open System.Drawing
open Calc
open Plotter

let mutable expression = "0"
let mutable memory = "0"
let mutable previous = "!"

let pi = "3.14159265358979323846"

let setupMenu =
    let menu = new MenuStrip()
    menu.BackColor <- Color.Gainsboro
    
    let fileMenuItem    = new ToolStripMenuItem("File")
    let helpMenuItem    = new ToolStripMenuItem("Help")
    let exitMenuItem    = new ToolStripMenuItem("Exit")
    let aboutMenuItem   = new ToolStripMenuItem("About")
    let plotterMenuItem = new ToolStripMenuItem("Run plotter")
 
    exitMenuItem.Click.Add    (fun e -> Application.Exit())
    plotterMenuItem.Click.Add (fun e -> plotterForm.ShowDialog() |> ignore)
    aboutMenuItem.Click.Add   (fun e ->
        MessageBox.Show(" 
            Homework 8 (20.04.2015)

            Author: Mikhail Kita", "About") |> ignore
    )

    fileMenuItem.DropDown.Items.Add(plotterMenuItem) |> ignore
    fileMenuItem.DropDown.Items.Add(exitMenuItem)    |> ignore
    helpMenuItem.DropDown.Items.Add(aboutMenuItem)   |> ignore
    
    menu.Items.Add(fileMenuItem)    |> ignore
    menu.Items.Add(helpMenuItem)    |> ignore
    menu

let exprTextBox =
    let textBox = new TextBox()
    textBox.Font      <- new Font("Calibri", 14.0f)
    textBox.TextAlign <- HorizontalAlignment.Right
    textBox.Multiline <- true
    textBox.Size      <- Size(325, 55)
    textBox.Location  <- Point(40, 35)
    textBox.Text      <- "0"
    textBox.ReadOnly  <- true
    textBox

let memoryTextBox =
    let textBox = new TextBox()
    textBox.Font      <- new Font("Calibri", 12.0f)
    textBox.TextAlign <- HorizontalAlignment.Center
    textBox.MaxLength <- 1
    textBox.Size      <- Size(25, 25)
    textBox.Location  <- Point(10, 35)
    textBox.ReadOnly  <- true
    textBox

let groupBox =
    let box = new GroupBox()
    box.Size     <- Size(175, 40)
    box.Location <- Point(10, 95)
    box.TabIndex <- 0
    
    let radioButton value x isChecked index =
        let button = new RadioButton()
        button.Text     <- value
        button.Location <- Point(x, 10)
        button.Size     <- Size(54, 25)
        button.Checked  <- isChecked
        button.Click.Add (fun e -> (box.TabIndex <- index))
        button
    
    box.Controls.Add (radioButton "Deg" 10 true 0)
    box.Controls.Add (radioButton "Rad" 64 false 1)
    box.Controls.Add (radioButton "Grad" 118 false 2)
    box

let numberButton value x y =
    let button = new Button()
    if value = "0" then button.Size <- Size(85, 35)
    else button.Size <- Size(40, 35)
    button.Font      <- new Font("Arial", 12.0f)
    button.Text      <- value
    button.Location  <- Point(x, y)
    button.BackColor <- Color.Lavender
    
    button.MouseMove.Add  (fun e -> (button.BackColor <- Color.Orange))
    button.MouseLeave.Add (fun e -> (button.BackColor <- Color.Lavender))
    button.Click.Add (fun e ->
        if expression.Length < 60 then
            match previous with
            | "xⁿ" | "mod" | "(" | ")" | "/" | "*" | "-" | "+" | "." -> 
                expression <- expression + value
            
            | "←" | "CE" | "pi" ->
                if expression <> "0" then expression <- expression + value
                else expression <- value
            
            | "1/x" -> expression <- value
            |  _    ->
                if isNumber previous && expression <> "0" then 
                    expression  <- expression + value
                else expression <- value
        previous <- value
        exprTextBox.Text <- expression
    )
    button

let operationButton value x y =

    //computes trigonometrical function with correct angle 
    let findCorrect value =
        let temp = value + " " + expression
        match groupBox.TabIndex with 
        | 0 -> expression <- tryToCompute (temp + " * " + pi + " / 180")
        | 1 -> expression <- tryToCompute temp
        | _ -> expression <- tryToCompute (temp + " * " + pi + " / 200")
    
    let button = new Button()
    if value = "=" then button.Size <- Size(40, 75)
    else button.Size <- Size(40, 35)
    button.Text      <- value
    button.Location  <- Point(x, y)
    button.BackColor <- Color.LightSteelBlue
    
    button.MouseMove.Add  (fun e -> (button.BackColor <- Color.Orange))
    button.MouseLeave.Add (fun e -> (button.BackColor <- Color.LightSteelBlue))
    button.Click.Add (fun e ->
        match value with
        | "MC" ->
            memory <- "0"
            memoryTextBox.Text <- ""
        
        | "M+" | "M-" ->
            memory <- tryToCompute (memory + " " + value.Remove(0, 1) + " " +  expression)
            if memory <> "0" then memoryTextBox.Text <- "M"

        | _ ->
            match value with
            | "sin" | "cos" | "tg" | "ctg" -> findCorrect value
            | "n!"  -> expression <- tryToCompute (expression + " !")
            | "x²"  -> expression <- tryToCompute (expression + " ^ 2")
            | "x³"  -> expression <- tryToCompute (expression + " ^ 3")
            | "ln"  -> expression <- tryToCompute ("ln " + expression)
            | "lg"  -> expression <- tryToCompute ("lg " + expression)
            | "10ⁿ" -> expression <- tryToCompute ("10 ^ " + expression)
            | "←"   ->
                if expression.Length > 0 then
                    expression <- expression.Remove(expression.Length - 1, 1)
                if expression.Length = 0 then expression <- "0"
            
            | "CE"  -> 
                let index = expression.LastIndexOf(" ")
                if index > 0 then expression <- expression.Remove(index)
                else expression <- "0"
            
            | "C"   -> expression <- "0"
            | "√"   -> expression <- tryToCompute ("sqrt " + expression)
            | "1/x" -> expression <- tryToCompute ("1 / " + expression)
            | "±"   -> expression <- tryToCompute ("-1 * " + expression)
            | "="   -> expression <- tryToCompute expression
            |  _    -> 
                if expression.Length < 60 then
                    match value with
                    | "MR"  -> 
                        if isNumber expression then expression <- memory
                        else expression <- expression + memory
                    
                    | "π"   -> 
                        if isNumber expression then expression <- pi
                        else expression <- expression + " " + pi
                    
                    | "e"   -> 
                        if isNumber expression then expression <- "2.71828182845904523536"
                        else expression <- expression + " 2.71828182845904523536"
                    
                    | "xⁿ"  -> expression <- expression + " ^ "
                    | "mod" -> expression <- expression + " % "
                    | "("   -> 
                        if isNumber expression then expression <- "("
                        else expression <- expression + "("
                    
                    | ")"   -> 
                        if not (isNumber expression) then 
                            expression <- expression + ")"
                    
                    | "."   ->
                        let index = expression.IndexOf(".")
                        if index < 0 then expression <- expression + "."
                    
                    |  _    -> expression <- expression + " " + value + " "
            exprTextBox.Text <- expression
        previous <- value
    )
    button

let mainForm =
    let form = new Form(Visible = false)
    form.MaximizeBox <- false
    form.MinimumSize <- Size(390, 385)
    form.MaximumSize <- Size(390, 385)
    form.Text        <- "Calculator"
    form.Font        <- new Font("Arial", 10.0f)
    form.BackColor   <- Color.LightGray

    form.Controls.Add setupMenu
    form.Controls.Add exprTextBox
    form.Controls.Add memoryTextBox
    form.Controls.Add groupBox

    let x = [|10; 55; 100; 145; 190; 235; 280; 325;|]
    for i = 0 to 8 do
        form.Controls.Add (numberButton ((i + 1).ToString()) x.[i % 3 + 3] (260 - i / 3 * 40))
    form.Controls.Add (numberButton "0" x.[3] 300)
    form.Controls.Add (operationButton "." x.[5] 300)
    let op = [|
        "MC"; "MR"; "M+"; "M-"; 
        "π"; "sin"; "cos"; "tg"; "ctg"; 
        "("; "x²"; "xⁿ"; "ln"; "lg"; 
        ")"; "x³"; "n!"; "e"; "10ⁿ"; 
        "←"; "CE"; "C"; 
        "±"; "√"; "/"; "mod"; "*"; "1/x"; "-"; "="; "+";
    |]
    for i = 0 to 3 do
        form.Controls.Add (operationButton op.[i] x.[i + 4] 100)
    for i = 0 to 2 do
        for j = 0 to 4 do
            form.Controls.Add (operationButton op.[j + 4 + 5 * i] x.[i] (j * 40 + 140))
    for i = 0 to 2 do
        form.Controls.Add (operationButton op.[i + 19] x.[i % 3 + 3] 140)
    for i = 0 to 8 do
        form.Controls.Add (operationButton op.[i + 22] x.[i % 2 + 6] (i / 2 * 40 + 140))
    form

[<EntryPoint>]
let main argv = 
    mainForm.Visible <- true
    Application.Run()
    0