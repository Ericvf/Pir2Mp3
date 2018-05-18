#include <Arduino.h>

class PirSensor
{
    private:
        int pirValue = 0;
        int threshold = 0;
        int sensorPin = 0;

    public:
        PirSensor(int p_sensorPin, int p_threshold);

        bool Update();
};
