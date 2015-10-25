data TL a = N | O (TL a) a (TL a) | E (TL a) (TL a) deriving (Show)

push :: a -> TL a -> (TL a, a)
push y (O N a N) = (O N y N, a)
push y (O first a second) = (O (fst t1) (snd t1) (fst t2), snd t2) where 
    t1 = push y first
    t2 = push a second
push y (E first second) = (E (fst t1) (fst t2), snd t2) where
    t1 = push y first
    t2 = push (snd t1) second

shup :: a -> TL a -> (a, TL a)
shup y (O N x N) = (x, O N y N)
shup y (O h x t) = let (m, t') = shup y t
                       (l, h') = shup x h in (l, O h' m t')
shup y (E h   t) = let (m, t') = shup y t
                       (l, h') = shup m h in (l, E h' t')

takeT :: TL a -> (a, TL a)
takeT (O N x N) = (x, N)
takeT (O h x t) = let (t', l) = push x t in (l, E h t')
takeT (E h   t) = let (m, h') = takeT h
                      (l, t') = takeT t in (l, O h' m t')

takeH :: TL a -> (a, TL a)
takeH (O N x N) = (x, N)
takeH (O h x t) = let (l, h') = shup x h in (l, E h' t)
takeH (E h   t) = let (l, h') = takeH h
                      (m, t') = takeH t in (l, O h' m t')

reverse' N = N
reverse' (O h x t) = O (reverse' t) x $ reverse' h
reverse' (E h   t) = E (reverse' t) $ reverse' h

cons :: a -> TL a -> TL a
cons y N = O N y N
cons y (O h x t) = E (cons y h) (cons x t)
cons y (E h   t) = let (h', m) = push y h in O h' m t

snoc :: TL a -> a -> TL a
snoc N y = O N y N
snoc (O h x t) y = E (snoc h x) (snoc t y)
snoc (E h   t) y = let (m, t') = shup y t in O h m t'

of_list [] = N
of_list (x:xs) = cons x $ of_list xs

length' N = 0
length' (O h _ t) = 1 + length' h + length' t
length' (E h   t) = length' h + length' t

append' N ys = ys
append' xs N = xs
append' xs ys = move p (abs delta) where
    move (f, t) 0 = let (f', t') = swap (,) f t in E f' t'
    move (f, t) 1 = let (m, f') = get f
                        (f'', t') = swap (,) f' t in O f'' m t'
    move (f, t) d = let (m, f') = get f in move (f', put m t) (d - 2)

    delta = length' xs - length' ys
    swap = if delta > 0 then id else flip
    p = swap (,) xs ys
    
    (put, get) = if delta > 0 then (cons, takeT)
                              else (flip snoc, takeH)
