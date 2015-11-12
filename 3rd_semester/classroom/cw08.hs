module F where

import Data.Ratio

type R     = Ratio Integer -- рациональное число неограниченной точности
type Point = (R, R)        -- двумерная рациональная точка

data F = -- двумерная геометрическая фигура
  C Point R   | -- окружность: центр, радиус
  S Point R   | -- квадрат: центр, размер стороны
  R Point R R | -- прямоугольник: центр, размер по горизонтали, 
                -- размер по вертикали
  M [F]         -- непустой список фигур

boundingBox :: F -> (Point, R, R) -- вычисляет ограничивающий прямоугольник, 
                                  -- задаваемый координатой левого верхнего угла,
                                  -- размером по горизонтали, размером по вертикали

boundingBox (C (a, b) c) = ((a - c, b + c), 2 * c, 2 * c)
boundingBox (S (a, b) c) = ((a - c / 2, b + c / 2), c, c)
boundingBox (R (a, b) c d) = ((a - c / 2, b + d / 2), c, d)
boundingBox (M (l:ls)) = let ((x, y), h, v) = boundingBox l in getRectangle ((x, y), h + x, y - v) ls where
    getRectangle ((a, b), c, d) [] = ((a, b), c - a, b - d)
    getRectangle ((a, b), c ,d) (w:ws) = let ((a', b'), c', d') = boundingBox w
                                         in getRectangle ((min a a', max b b'), max c (c' + a'), min d (b' - d')) ws
