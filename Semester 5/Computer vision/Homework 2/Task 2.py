"""
Homework 2. Task 2.

Author: Mikhail Kita, group 371
"""

import cv2
from common import *

filtered = low_pass_filter(line)
reduced = scale(filtered, 1.0 / 32.0, 8)
result = scale(reduced, 1.0 / 8.0, 32)

draw_plot(reduced, "fig3_1.png")
draw_plot(result, "fig3_2.png")

with open(path + "norm_row_filt.txt", 'w') as f:
    f.write(str(cv2.norm(result, filtered)))
