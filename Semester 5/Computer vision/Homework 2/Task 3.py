"""
Homework 2. Task 3.

Author: Mikhail Kita, group 371
"""

import cv2
from common import *

reduced = [scale(elem, 1.0 / 32.0, 8) for elem in array]
result = [scale(elem, 1.0 / 8.0, 32) for elem in reduced]

draw_map(reduced, "fig4_1.png")
draw_map(result, "fig4_2.png")

temp = [cv2.norm(result[i], array[i]) for i in range(0, 32)]

with open(path + "norm_image.txt", 'w') as f:
    f.write(str(cv2.norm(np.array(temp))))


filtered = [low_pass_filter(elem) for elem in array]
reduced = [scale(elem, 1.0 / 32.0, 8) for elem in filtered]
result = [scale(elem, 1.0 / 8.0, 32) for elem in reduced]

draw_map(reduced, "fig5_1.png")
draw_map(result, "fig5_2.png")

temp = [cv2.norm(result[i], filtered[i]) for i in range(0, 32)]

with open(path + "norm_image_filt.txt", 'w') as f:
    f.write(str(cv2.norm(np.array(temp))))
