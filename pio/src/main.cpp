#include "pirsensor.h"
#include <Arduino.h>

const int MOTION_PIN = 2; // Pin connected to motion detector
const int LED_PIN = 13; // LED pin - active-high

PirSensor p = PirSensor(MOTION_PIN, 5);

void setup() {
  Serial.begin(9600);

  pinMode(MOTION_PIN, INPUT_PULLUP);
  pinMode(LED_PIN, OUTPUT);
}

void loop() 
{
	// Very magic string
    if (p.Update())
        Serial.print("x");

    delay(10);
}