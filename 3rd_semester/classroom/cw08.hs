module F where

import Data.Ratio

type R     = Ratio Integer -- ������������ ����� �������������� ��������
type Point = (R, R)        -- ��������� ������������ �����

data F = -- ��������� �������������� ������
  C Point R   | -- ����������: �����, ������
  S Point R   | -- �������: �����, ������ �������
  R Point R R | -- �������������: �����, ������ �� �����������, 
                -- ������ �� ���������
  M [F]         -- �������� ������ �����

boundingBox :: F -> (Point, R, R) -- ��������� �������������� �������������, 
                                  -- ���������� ����������� ������ �������� ����,
                                  -- �������� �� �����������, �������� �� ���������

boundingBox (C (a, b) c) = ((a - c, b + c), 2 * c, 2 * c)
boundingBox (S (a, b) c) = ((a - c / 2, b + c / 2), c, c)
boundingBox (R (a, b) c d) = ((a - c / 2, b + d / 2), c, d)
boundingBox (M (l:ls)) = let ((x, y), h, v) = boundingBox l in getRectangle ((x, y), h + x, y - v) ls where
    getRectangle ((a, b), c, d) [] = ((a, b), c - a, b - d)
    getRectangle ((a, b), c ,d) (w:ws) = let ((a', b'), c', d') = boundingBox w
                                         in getRectangle ((min a a', max b b'), max c (c' + a'), min d (b' - d')) ws