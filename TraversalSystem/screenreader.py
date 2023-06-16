import os
import pytesseract
import cv2
import pyscreenshot as ImageGrab
import reshandler as res

pytesseract.pytesseract.tesseract_cmd = "tesseract\\tesseract.exe"


def time_until_jump():
    try:
        os.remove("h.png")
        os.remove("h_processed.png")
        os.remove("m1.png")
        os.remove("m1_processed.png")
        os.remove("m2.png")
        os.remove("m2_processed.png")
        os.remove("s1.png")
        os.remove("s1_processed.png")
        os.remove("s2.png")
        os.remove("s2_processed.png")
    except FileNotFoundError:
        pass
        
    y1 = res.timeUpperY
    y2 = res.timeLowerY

    h = ImageGrab.grab(bbox=(res.hourX1, y1, res.hourX2, y2))
    m1 = ImageGrab.grab(bbox=(res.min1x1, y1, res.min1x2, y2))
    m2 = ImageGrab.grab(bbox=(res.min2x1, y1, res.min2x2, y2))
    s1 = ImageGrab.grab(bbox=(res.sec1x1, y1, res.sec1x2, y2))
    s2 = ImageGrab.grab(bbox=(res.sec2x1, y1, res.sec2x2, y2))

    h.save("h.png")
    m1.save("m1.png")
    m2.save("m2.png")
    s1.save("s1.png")
    s2.save("s2.png")
    
    f = ""
    custom_config = r'--oem 0 --psm 10 -c tessedit_char_whitelist=0123456789'

    tessImage = cv2.imread("h.png")
    tessImage = cv2.cvtColor(tessImage, cv2.COLOR_BGR2GRAY)
    tessImage = cv2.copyMakeBorder(tessImage, 7, 7, 7, 7, cv2.BORDER_CONSTANT, value=[1, 0, 0])
    tessImage = cv2.resize(tessImage, (100, 100))
    cv2.imwrite("h_processed.png", tessImage)
    
    f += pytesseract.image_to_string(tessImage, config=custom_config) + ":"
    
    tessImage = cv2.imread("m1.png")
    tessImage = cv2.cvtColor(tessImage, cv2.COLOR_BGR2GRAY)
    tessImage = cv2.copyMakeBorder(tessImage, 7, 7, 7, 7, cv2.BORDER_CONSTANT, value=[1, 0, 0])
    tessImage = cv2.resize(tessImage, (100, 100))
    cv2.imwrite("m1_processed.png", tessImage)
    
    f += pytesseract.image_to_string(tessImage, config=custom_config)
    
    tessImage = cv2.imread("m2.png")
    tessImage = cv2.cvtColor(tessImage, cv2.COLOR_BGR2GRAY)
    tessImage = cv2.copyMakeBorder(tessImage, 7, 7, 7, 7, cv2.BORDER_CONSTANT, value=[1, 0, 0])
    tessImage = cv2.resize(tessImage, (100, 100))
    cv2.imwrite("m2_processed.png", tessImage)
    
    f += pytesseract.image_to_string(tessImage, config=custom_config) + ":"
    
    tessImage = cv2.imread("s1.png")
    tessImage = cv2.cvtColor(tessImage, cv2.COLOR_BGR2GRAY)
    tessImage = cv2.copyMakeBorder(tessImage, 7, 7, 7, 7, cv2.BORDER_CONSTANT, value=[1, 0, 0])
    tessImage = cv2.resize(tessImage, (100, 100))
    cv2.imwrite("s1_processed.png", tessImage)
    
    f += pytesseract.image_to_string(tessImage, config=custom_config)
    
    tessImage = cv2.imread("s2.png")
    tessImage = cv2.cvtColor(tessImage, cv2.COLOR_BGR2GRAY)
    tessImage = cv2.copyMakeBorder(tessImage, 7, 7, 7, 7, cv2.BORDER_CONSTANT, value=[1, 0, 0])
    tessImage = cv2.resize(tessImage, (100, 100))
    cv2.imwrite("s2_processed.png", tessImage)
    
    f += pytesseract.image_to_string(tessImage, config=custom_config)
    
    return f.replace("\n", "")
