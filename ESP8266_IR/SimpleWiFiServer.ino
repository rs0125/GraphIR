#include <ESP8266WiFi.h>
#include <WiFiUdp.h>

const char* ssid = "";            // Your WiFi SSID
const char* password = "";      // Your WiFi password

WiFiUDP udp;
const char* host = "";     // Replace with your laptop's local IP
const int port = 5053;                  // Must match Python's bind port

const int irPin = D2;                   // IR sensor pin

void setup() {
  pinMode(irPin, INPUT);
  Serial.begin(115200);

  WiFi.begin(ssid, password);
  Serial.print("Connecting to WiFi");

  while (WiFi.status() != WL_CONNECTED) {
    delay(500);
    Serial.print(".");
  }

  Serial.println("\nWiFi connected. IP: " + WiFi.localIP().toString());
  udp.begin(4210);  // Optional: start UDP on random local port
}

void loop() {
  int irState = digitalRead(irPin);
  udp.beginPacket(host, port);
  udp.write(irState ? "1" : "0");
  udp.endPacket();

  Serial.println(irState);  // Optional: monitor IR state
  delay(100);               // 10Hz update rate
}
