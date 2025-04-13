import cv2
from cvzone2.HandTrackingModule import HandDetector
import socket


def dist(L1, L2):
    return ((L1[0] - L2[0]) * 2 + (L1[1] - L2[1]) * 2 + (L1[2] - L2[2]) * 2) * 0.5


def main():
    print("Starting hand tracking with IR control...")

    # Parameters
    width, height = 1280, 720

    # Webcam
    cap = cv2.VideoCapture(0)
    cap.set(3, width)
    cap.set(4, height)

    # Hand Detector
    detector = HandDetector(maxHands=1, detectionCon=0.8)

    # UDP to Unity
    sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
    serverAddressPort = ("127.0.0.1", 5052)
    IRPORT = ("127.0.0.1", 5055)

    # UDP from ESP8266 IR Sensor
    ir_sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
    ir_sock.bind(("0.0.0.0", 5053))
    ir_sock.setblocking(False)

    statekumar = 0
    ir_active = False

    while True:
        success, img = cap.read()
        flip_img = cv2.flip(img, 1)

        try:
            msg, _ = ir_sock.recvfrom(1024)
            decoded = msg.decode().strip()
            #print(f"Received from ESP: '{decoded}'")
            ir_active = decoded == '0'
        except:
            pass

        # Show IR status on screen
        color = (0, 255, 0) if ir_active else (0, 0, 255)
        cv2.putText(flip_img, f"IR Active: {ir_active}", (50, 50),
                    cv2.FONT_HERSHEY_SIMPLEX, 1, color, 2)

        # Detect hand if IR is active
        data = []
        hands, _ = detector.findHands(flip_img)
        if hands:
            hand = hands[0]
            lmList = hand['lmList']
            for lm in lmList:
                data.extend([-lm[0], height - lm[1], lm[2]])
            sock.sendto(str.encode(str(data)), serverAddressPort)  # Send to Unity
            if (ir_active):
                statekumar = 1
            else:
                statekumar = 0
            sock.sendto(str.encode(str(statekumar)), IRPORT)

        cv2.imshow("Hand Tracking", flip_img)
        cv2.waitKey(1)


if __name__ == "__main__":
    main()
