import os
import pytesseract
import cv2
import pyscreenshot as ImageGrab


def text_in_box(x1, y1, x2, y2, skipProcess=False):
    try:
        os.remove("ss.png")
        os.remove("ss_processed.png")
    except FileNotFoundError:
        print("File doesn't exist")

    im = ImageGrab.grab(bbox=(x1, y1, x2, y2))

    im.save("ss.png")

    tessImage = cv2.imread("ss.png")
    tessImage = cv2.cvtColor(tessImage, cv2.COLOR_BGR2GRAY)

    if not skipProcess:
        tessImage = cv2.threshold(tessImage, 0, 255, cv2.THRESH_BINARY + cv2.THRESH_OTSU)[1]
        scale_percent = 150  # percent of original size
        width = int(tessImage.shape[1] * scale_percent / 100)
        height = int(tessImage.shape[0] * scale_percent / 100)
        dim = (width, height)

        # resize image
        tessImage = cv2.resize(tessImage, dim, interpolation=cv2.INTER_AREA)

    cv2.imwrite("ss_processed.png", tessImage)

    custom_config = r'--oem 3 --psm 6'
    return pytesseract.image_to_string(tessImage, config=custom_config)


def time_until_jump():
    return text_in_box(1478, 1008, 1554, 1027, True)