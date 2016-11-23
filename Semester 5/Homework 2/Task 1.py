"""
Homework 2. Task 1.

Author: Mikhail Kita, group 371
"""

import cv2
from common import *

reduced = scale(line, 1.0 / 32.0, 8)
result = scale(reduced, 1.0 / 8.0, 32)

draw_plot(reduced, "fig2_1.png")
draw_plot(result, "fig2_2.png")

with open(path + "norm_row.txt", 'w') as f:
    f.write(str(cv2.norm(result, line)))
