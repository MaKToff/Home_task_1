"""
Homework 3.
Performs recognition of test images.

Author: Mikhail Kita, group 371
"""

import os
import re
import cv2


# Gets all images located in given directory
def get_images(path):
    result = []

    for current_path, dirs, files in os.walk(path):
        for file in files:
            image = cv2.imread(current_path + "/" + file)
            gray = cv2.cvtColor(image, cv2.COLOR_BGR2GRAY)
            result.append({"filename": file, "image": gray})

    return result


ok = 0
cascade = cv2.CascadeClassifier("cascade.xml")
images = get_images("./data/tests")

for img in images:
    squares = cascade.detectMultiScale(img["image"], 1.3, 5)
    answer = re.sub(r"(.*)-.*", r"\1", img["filename"])

    if ((len(squares) == 0) & (answer == "triangle")) | ((len(squares) > 0) & (answer == "square")):
        ok += 1
        print("OK")
    else:
        print("Image {} recognized incorrectly.".format(img["filename"]))

print("Accuracy: ", str(round(float(ok) / float(len(images)) * 100, 2)) + "%")
