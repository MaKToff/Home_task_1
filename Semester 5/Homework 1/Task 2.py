"""
Homework 1. Task 2.

Author: Mikhail Kita, group 371
"""

import cv2

file = cv2.imread("example.png")
gaussian = cv2.GaussianBlur(file, (3, 3), 0)
gray = cv2.cvtColor(gaussian, cv2.COLOR_BGR2GRAY)
laplacian = cv2.Laplacian(gray, cv2.CV_16S, 1, 7, 5, 0)
result = cv2.convertScaleAbs(laplacian)

cv2.imwrite("result.png", result)
cv2.imshow("Original picture", file)
cv2.imshow("Result", result)
cv2.waitKey()
