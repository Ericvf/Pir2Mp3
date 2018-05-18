#include "pirsensor.h"

PirSensor::PirSensor(int p_sensorPin, int p_threshold)
: sensorPin(p_sensorPin), threshold(p_threshold)
{
    pirValue = 0;
}

bool PirSensor::Update() {
    int currentValue = digitalRead(this->sensorPin);
    
    pirValue += currentValue == LOW ? 1 : -1;
    pirValue = max(0, pirValue);
    pirValue = min(25, pirValue);

    if (pirValue > threshold){
        pirValue = 0;
        return true;
    }
    else
        return false;
}