#include <avr/wdt.h>
#include "ModbusRtu.h"


#define TXEN 4  //!!! (����� � bootloader)
//#define RESET_PIN 17 // A3
#define ID   3      // ����� ��������
#define DOOR_INPUT 0
// ���� �������
#define LIGHT_INPUT 1

#define DOOR_PIN  2   // ����� �����, ������������ � ������
#define RELAY_PIN 7

// ������ ������ modbus
uint16_t au16data[11];
int8_t state = 0;

// the following variables are long's because the time, measured in miliseconds,
// will quickly become a bigger number than can be stored in an int.
long lastDebounceTime = 0;  // the last time the output pin was toggled
long debounceDelay = 500;    // the debounce time; increase if the output flickers
bool lastDoorPinState;
bool buttonDoorState;

bool isEnterInBath = false;

void io_setup();
void io_poll();

//����� �������� �����, ���������������� ����, ����� ���������� TX
Modbus slave(ID, 0, TXEN);

void setup()
{
	// put your setup code here, to run once:
	// ����������� ����� � ������
	io_setup();
	// ����������� ���������������� ���� ��������
	slave.begin(9600);
}

void io_setup()
{
	//digitalWrite(RESET_PIN, HIGH);
	digitalWrite(RELAY_PIN, HIGH);


	//pinMode(RESET_PIN, OUTPUT);
	pinMode(RELAY_PIN, OUTPUT);

	pinMode(DOOR_PIN, INPUT_PULLUP);
}

void softwareReset(uint8_t prescaller) {
	// start watchdog with the provided prescaller
	wdt_enable(prescaller);
	// wait for the prescaller time to expire
	// without sending the reset signal by using
	// the wdt_reset() method
	while (1) {}
}

void ResetArduino()
{
	// restart in 60 milliseconds
	softwareReset(WDTO_60MS);
	//digitalWrite(RESET_PIN, LOW);
}


void LightOn()
{
	digitalWrite(RELAY_PIN, LOW);
	digitalWrite(13, HIGH);
	bitWrite(au16data[0], LIGHT_INPUT, 1);
}

void LightOff()
{
	digitalWrite(RELAY_PIN, HIGH);
	digitalWrite(13, LOW);
	bitWrite(au16data[0], LIGHT_INPUT, 0);
}

void loop()
{
	// put your main code here, to run repeatedly:
	if (Serial.available()) {
		auto inByte = Serial.peek();
		if (inByte == 0xFC) // ����, ������������ ��� ������ � �������� � ����������
		{
			ResetArduino();
			return;
		}
	}

	// ------------------------------------------------------------
	//auto curMillis = millis();
	// read the state of the switch into a local variable:
	auto doorPinCurState = digitalRead(DOOR_PIN);
	// check to see if you just pressed the button
	// (i.e. the input went from LOW to HIGH),  and you've waited
	// long enough since the last press to ignore any noise:

	if (doorPinCurState != lastDoorPinState)
	{
		// reset the debouncing timer
		lastDebounceTime = millis();
		lastDoorPinState = doorPinCurState;
	}

	if ((millis() - lastDebounceTime) > debounceDelay)
	{
		// whatever the reading is at, it's been there for longer
		// than the debounce delay, so take it as the actual current state:

		// if the button state has changed:
		if (doorPinCurState != buttonDoorState) 
		{
			buttonDoorState = doorPinCurState;

			//Serial.println(kakPinCurState);
			// only toggle the LED if the new button state is LOW
			if (buttonDoorState == HIGH) // ����� ��������� 
			{
				bitWrite(au16data[0], DOOR_INPUT, 0);
				// ��� ����� �� ������
				if(!isEnterInBath)
				{
					isEnterInBath = true;
					LightOn();
				}
				else
				{
					// ���-�� ������� �� ������
					// ���� ���� ������ ����������
					isEnterInBath = false; 
				}
				//bitWrite(au16data[1], CALL_COIL, 1);
			}
			else // ����� ���������
			{
				bitWrite(au16data[0], DOOR_INPUT, 1);
				if(!isEnterInBath)
				{
					LightOff();
				}
			}
		}
	}
	// ------------------------------------------------


	// ��������� ���������
	state = slave.poll(au16data, 11);
	//��������� ������ � ��������� Modbus � � ���������������� ���������
	io_poll();




}

void io_poll()
{

	//�������� Coil[1] � Discrete[0]
	//au16data[0] = au16data[1];
}
