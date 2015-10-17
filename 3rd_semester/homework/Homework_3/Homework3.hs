data L a = N | E (L a) (L a) | O (L a) a (L a) deriving (Show)

-- returns length of list
length' :: L a -> Int
length' N = 0
length' (E first second) = length' first + length' second
length' (O first a second) = 1 + length' first + length' second

-- checks that list is correct
wf :: L a -> Bool
wf N = True
wf (E first second) = length' first == length' second
wf (O first a second) = length' first == length' second

-- converts a standard list to list in new notation
of_list :: [a] -> L a
of_list [] = N
of_list list = divide (div (length list) 2) where 
     divide n = 
        if (n * 2 == length list) then 
             (E (of_list (take n list)) $ of_list $ drop n list)
        else (O (of_list (take n list)) (list !! n) $ of_list $ drop (n + 1) list)

-- converts list in new notation to a standard list
to_list :: L a -> [a]
to_list N = []
to_list (E first second) = (to_list first) ++ (to_list second)
to_list (O first a second) = (to_list first) ++ (a : (to_list second))

-- merge of two lists
append' :: L a -> L a -> L a
append' list1 list2 = of_list ((to_list list1) ++ (to_list list2))

-- reverse of list
reverse' :: L a -> L a
reverse' N = N
reverse' (E first second) = (E (reverse' second) $ reverse' first)
reverse' (O first a second) = (O (reverse' second) a $ reverse' first)

map' :: (a -> b) -> L a -> L b
map' f N = N
map' f (E first second) = (E (map' f first) $ map' f second)
map' f (O first a second) = (O (map' f first) (f a) $ map' f second)

foldl' :: (a -> b -> a) -> a -> L b -> a
foldl' f acc (O N a N) = f acc a
foldl' f acc (E first second) = foldl' f (foldl' f acc first) second
foldl' f acc (O first a second) = foldl' f (f (foldl' f acc first) a) second

foldr' :: (a -> b -> b) -> b -> L a -> b
foldr' f acc (O N a N) = f a acc
foldr' f acc (E first second) = foldr' f (foldr' f acc first) second
foldr' f acc (O first a second) = foldr' f (f a (foldr' f acc first)) second