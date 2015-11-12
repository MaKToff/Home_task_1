data E = X String | C Integer | E :+: E | E :*: E | E :-: E

data Tree = Leaf String | Node String Tree Tree

to_string :: E -> String
to_string x = parse (getTree x) where
    getTree (X a) = Leaf a
    getTree (C a) = Leaf (show a)
    getTree (a :+: b) = Node " + " (getTree a) $ getTree b
    getTree (a :*: b) = Node " * " (getTree a) $ getTree b
    getTree (a :-: b) = Node " - " (getTree a) $ getTree b

    parse (Leaf a) = a
    parse (Node " * " f s) = (parse f) ++ " * " ++ (br s) where
        br (Leaf a) = parse (Leaf a)
        br (Node " * " f' s') = parse (Node " * " f' s')
        br (Node a f' s') = "(" ++ (parse (Node a f' s')) ++ ")"
    parse (Node a f s) = (parse f) ++ a ++ (parse s)

f a = 0

eval :: (String -> Integer) -> E -> Integer
eval f (C a) = a
eval f (X a) = f a
eval f (a :+: b) = (eval f a) + (eval f b)
eval f (a :*: b) = (eval f a) * (eval f b)
eval f (a :-: b) = (eval f a) - (eval f b) 