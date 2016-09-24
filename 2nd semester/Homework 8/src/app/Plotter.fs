(*
Homework 8 (20.04.2015)
Plot the graph of the function.

Author: Mikhail Kita, group 171
*)

module Plotter

open System.Windows.Forms
open System.Drawing
open Calc

let funcGroupBox =
    let box = new GroupBox()
    box.Text     <- "Function"
    box.Size     <- Size(485, 65)
    box.Location <- Point(10, 10)
    box.TabIndex <- 0

    let funcTextBox =
        let textBox = new TextBox()
        textBox.Font      <- new Font("Calibri", 12.0f)
        textBox.MaxLength <- 100
        textBox.Text      <- "sqrt (49 - x ^ 2) + 1.5 * sin x + (cos (2 * x)) ^ 2 - 3! + 2"
        textBox.Size      <- Size(465, 25)
        textBox.Location  <- Point(10, 25)
        textBox

    box.Controls.Add(funcTextBox)
    box

let inputGroupBox =
    let box = new GroupBox()
    box.Text     <- "Input data"
    box.Size     <- Size(165, 170)
    box.Location <- Point(330, 85)
    box.TabIndex <- 0

    let inputTextBox text y =
        let textBox = new TextBox()
        textBox.Font      <- new Font("Calibri", 12.0f)
        textBox.MaxLength <- 6
        textBox.Text      <- text
        textBox.Size      <- Size(60, 25)
        textBox.Location  <- Point(95, y)
        textBox

    let label text y =
        let lbl = new Label()
        lbl.Text     <- text
        lbl.Location <- Point(10, y) 
        lbl

    box.Controls.Add(inputTextBox "-7" 25)
    box.Controls.Add(inputTextBox "7" 60)
    box.Controls.Add(inputTextBox "0.001" 95)
    box.Controls.Add(inputTextBox "20" 130)
    box.Controls.Add(label "Left border" 32)
    box.Controls.Add(label "Right border" 67)
    box.Controls.Add(label "Precision" 102)
    box.Controls.Add(label "Scale" 137)
    box

let statusGroupBox =
    let box = new GroupBox()
    box.Text     <- "Status"
    box.Size     <- Size(165, 65)
    box.Location <- Point(330, 265)
    box.TabIndex <- 0

    let resultTextBox =
        let textBox = new TextBox()
        let textBox = new TextBox()
        textBox.TextAlign <- HorizontalAlignment.Center
        textBox.Font      <- new Font("Calibri", 12.0f)
        textBox.Size      <- Size(145, 25)
        textBox.Location  <- Point(10, 25)
        textBox.ReadOnly  <- true
        textBox

    box.Controls.Add(resultTextBox)
    box

let graphPictureBox =
    let pictureBox = new PictureBox()
    pictureBox.Size      <- Size(310, 310)
    pictureBox.Location  <- Point(10, 85)
    pictureBox.BackColor <- Color.White
    pictureBox

// Clears a Bitmap
let clear (image : Bitmap)=
    for i = 0 to 309 do
        for j = 0 to 309 do
            image.SetPixel(i, j, Color.White)

// Draws graph using input data
let drawGraph (image : Bitmap) =
    let handler sender (event : PaintEventArgs) =
        event.Graphics.DrawImage(image, 0, 0)

    graphPictureBox.Paint.AddHandler(PaintEventHandler(handler))
    graphPictureBox.Refresh()    

let clearButton =
    let button = new Button()
    button.Text      <- "Clear"
    button.Size      <- Size(75, 50)
    button.Location  <- Point(330, 345)
    button.BackColor <- Color.LightSteelBlue
    
    button.MouseMove.Add  (fun e -> (button.BackColor <- Color.Orange))
    button.MouseLeave.Add (fun e -> (button.BackColor <- Color.LightSteelBlue))
    button.Click.Add(fun e ->
        funcGroupBox.Controls.Item(0).Text   <- ""
        inputGroupBox.Controls.Item(0).Text  <- ""
        inputGroupBox.Controls.Item(1).Text  <- ""
        inputGroupBox.Controls.Item(2).Text  <- ""
        inputGroupBox.Controls.Item(3).Text  <- ""
        statusGroupBox.Controls.Item(0).Text <- ""

        let image = new Bitmap(310, 310)
        clear(image)
        drawGraph(image)
    )
    button

let startButton =
    let button = new Button()
    button.Text      <- "Start"
    button.Size      <- Size(75, 50)
    button.Location  <- Point(420, 345)
    button.BackColor <- Color.LightSteelBlue
    
    button.MouseMove.Add  (fun e -> (button.BackColor <- Color.Orange))
    button.MouseLeave.Add (fun e -> (button.BackColor <- Color.LightSteelBlue))
    button.Click.Add(fun e ->

        // Tries to compute value of the expression
        let tryToCompute expr value =
            try
                let dict           = new Dictionary()
                dict.add "x" (float value)
                float32 (compute expr dict)
            with
            | :? ListIsEmpty | :? InvalidOperation | :? KeyNotFound
            | :? System.OverflowException ->
                statusGroupBox.Controls.Item(0).Text <- "Error"
                0.0f

        // Tries to convert value into float32
        let tryToConvert value =
            try
                float32 value
            with
            | :? System.FormatException ->
                statusGroupBox.Controls.Item(0).Text <- "Incorrect input"
                0.0f

        let image = new Bitmap(310, 310)
        clear(image)
        statusGroupBox.Controls.Item(0).Text <- ""

        if (funcGroupBox.Controls.Item(0).Text).Length = 0
            || (inputGroupBox.Controls.Item(0).Text).Length = 0
            || (inputGroupBox.Controls.Item(1).Text).Length = 0
            || (inputGroupBox.Controls.Item(2).Text).Length = 0
            || (inputGroupBox.Controls.Item(3).Text).Length = 0
            then
                statusGroupBox.Controls.Item(0).Text <- "Incorrect input"
        else        
            let expr      = funcGroupBox.Controls.Item(0).Text
            let left      = tryToConvert (inputGroupBox.Controls.Item(0).Text)
            let right     = tryToConvert (inputGroupBox.Controls.Item(1).Text)
            let precision = tryToConvert (inputGroupBox.Controls.Item(2).Text)
            let scale     = tryToConvert (inputGroupBox.Controls.Item(3).Text)
            
            let x0 = 155.0f
            let y0 = 155.0f
            
            let drawPoint (graph : Graphics) pen x y =
                graph.DrawEllipse(pen, x + x0 - 1.5f, y0 - y - 1.5f, 3.0f, 3.0f)

            use graph = Graphics.FromImage(image) 

            // Draws axes
            let mutable t = scale
            graph.DrawLine(Pens.Black, x0, 0.0f, x0, 325.0f)
            graph.DrawLine(Pens.Black, 0.0f, y0, 325.0f, y0)
            while t < 325.0f do
                drawPoint graph Pens.Green t 0.0f
                drawPoint graph Pens.Green -t 0.0f
                drawPoint graph Pens.Green 0.0f t
                drawPoint graph Pens.Green 0.0f -t
                t <- t + scale
            
            // Draws a graph
            let timer = new System.Diagnostics.Stopwatch()
            timer.Start()
            t <- left
            if statusGroupBox.Controls.Item(0).Text = "" then
                while t < right do
                    if timer.ElapsedMilliseconds < 5000L then
                        drawPoint graph Pens.Red (t * scale) ((tryToCompute expr t) * scale)
                    else
                        t <- right
                        let message = "     Computation was interrupted due to timeout"
                        MessageBox.Show(message, "Warning") |> ignore
                    t <- t + precision
            timer.Reset()

            if statusGroupBox.Controls.Item(0).Text <> "" then clear(image)
            else statusGroupBox.Controls.Item(0).Text <- "OK"

        drawGraph(image)
    )
    button

let plotterForm =
    let form = new Form(Visible = false)
    form.MaximizeBox <- false
    form.MinimumSize <- Size(520, 445)
    form.MaximumSize <- Size(520, 445)
    form.Text        <- "Plotter"
    form.Font        <- new Font("Arial", 10.0f)
    form.BackColor   <- Color.LightGray

    form.Controls.Add(funcGroupBox)
    form.Controls.Add(inputGroupBox)
    form.Controls.Add(statusGroupBox)
    form.Controls.Add(clearButton)
    form.Controls.Add(startButton)
    form.Controls.Add(graphPictureBox)
    form