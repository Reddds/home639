// Reset 03 64 7F 00 00 00 00 00 00 00 5D AE 

#include <avr/wdt.h>
#include "ControllerSettings.h"
#include <MyTime.h>
#include <Time.h>
#include <EEPROM.h>
#include <DS1302RTC.h>
#include <Arduino.h>
#include <ModbusRtu.h>


#define TXEN 4  //!!! (Забит в bootloader)
//#define RESET_PIN 17 // A3
//#define ID   3      // адрес ведомого
#define DOOR_INPUT 0
// Свет включён
#define LIGHT_INPUT 1

#define DOOR_PIN  7   // номер входа, подключенный к кнопке
#define RELAY_PIN 2
#define LED_1_PIN 10

// Игнорировать открытие двери, всегда держать включёным свет
#define COMMAND_IGNORE_DOOR 10 // Data 0 - Нормальная работа 0xff - игнорирование двери
#define COMMAND_SET_LIGHT 11 // Включение-выключение света 0 - выключение 0xff - включение

// массив данных modbus
#define modbusInputBufLen 5
#define modbusHoldingBufLen 5
#include <MyModbus/ModbusRtu.h>
uint16_t _MODBUSDiscreteInputs;
uint16_t _MODBUSCoils;
uint16_t _MODBUSInputRegs[modbusInputBufLen];
uint16_t _MODBUSHoldingRegs[modbusHoldingBufLen];
int8_t state = 0;

// the following variables are long's because the time, measured in miliseconds,
// will quickly become a bigger number than can be stored in an int.
long lastDebounceTime = 0;  // the last time the output pin was toggled
long debounceDelay = 500;    // the debounce time; increase if the output flickers
bool lastDoorPinState;
bool buttonDoorState;
// Не проверять, открыта ли дверь. Всегда держать свет включённым
bool _ignoreDoor = false;


bool isEnterInBath = false;

void io_setup();
void io_poll();

MyDeviceInfoTag myDeviceInfo =
{
	DEVICE_NEED_TIME_SET,
	SLAVE_ID_DEVICE_TYPE,
	SLAVE_ID_DEVICE_SUB_TYPE,
	SLAVE_ID_DEVICE_REVISION,
	SLAVE_ID_DEVICE_NUMBER,
	VENDOR_NAME,
	PRODUCT_CODE,
	MAJOR_MINOR_REVISION,
	VENDOR_URL,
	PRODUCT_NAME,
	MODEL_NAME,
	USER_APPLICATION_NAME
};
void setup()
{
	Modbus(0, TXEN, &myDeviceInfo);
	// put your setup code here, to run once:
	// настраиваем входы и выходы
	io_setup();
	// настраиваем последовательный порт ведомого
	ModbusBegin(9600);
}

void io_setup()
{
	//digitalWrite(RESET_PIN, HIGH);
	digitalWrite(RELAY_PIN, HIGH);


	//pinMode(RESET_PIN, OUTPUT);
	pinMode(RELAY_PIN, OUTPUT);
	pinMode(LED_1_PIN, OUTPUT);

	pinMode(DOOR_PIN, INPUT_PULLUP);
}

//void softwareReset(uint8_t prescaller) {
//	// start watchdog with the provided prescaller
//	wdt_enable(prescaller);
//	// wait for the prescaller time to expire
//	// without sending the reset signal by using
//	// the wdt_reset() method
//	while (1) {}
//}
//
//void ResetArduino()
//{
//	// restart in 60 milliseconds
//	softwareReset(WDTO_60MS);
//	//digitalWrite(RESET_PIN, LOW);
//}


void LightOn()
{
	if(!_ignoreDoor)
		digitalWrite(RELAY_PIN, LOW);
	digitalWrite(LED_1_PIN, HIGH);
	bitWrite(_MODBUSDiscreteInputs, LIGHT_INPUT, 1);
}

void LightOff()
{
	if (!_ignoreDoor)
		digitalWrite(RELAY_PIN, HIGH);
	digitalWrite(LED_1_PIN, LOW);
	bitWrite(_MODBUSDiscreteInputs, LIGHT_INPUT, 0);
}

void loop()
{
	// put your main code here, to run repeatedly:
//	if (Serial.available()) {
//		auto inByte = Serial.peek();
//		if (inByte == 0xFC) // Байт, передаваемый для сброса и перехода к загрузчику
//		{
//			ResetArduino();
//			return;
//		}
//	}

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
			if (buttonDoorState == HIGH) // Дверь открылась 
			{
				bitWrite(_MODBUSDiscreteInputs, DOOR_INPUT, 1);
				// Ещё никто не входил
				if(!isEnterInBath)
				{
					isEnterInBath = true;
					LightOn();
				}
				else
				{
					// Кто-то выходит из ванной
					// Свет пока держим включённым
					isEnterInBath = false; 
				}
				//bitWrite(au16data[1], CALL_COIL, 1);
			}
			else // Дверь закрылась
			{
				bitWrite(_MODBUSDiscreteInputs, DOOR_INPUT, 0);
				if(!isEnterInBath)
				{
					LightOff();
				}
			}
		}
	}
	// ------------------------------------------------


	// обработка сообщений
	state = ModbusPoll(_MODBUSDiscreteInputs, &_MODBUSCoils, _MODBUSInputRegs, modbusInputBufLen, _MODBUSHoldingRegs, modbusHoldingBufLen);
	//обновляем данные в регистрах Modbus и в пользовательской программе
	io_poll();
}

void io_poll()
{
	uint16_t lastAddress;
	uint16_t lastEndAddress;
	uint8_t lastCommand;
	//uint16_t lastCount;
	uint8_t *lastFunction = ModbusGetLastCommand(&lastAddress, &lastEndAddress, &lastCommand);
	if (*lastFunction == MB_FC_NONE)
		return;
	lastEndAddress += lastAddress - 1;

	uint8_t v1;
	if (*lastFunction == MB_FC_USER_COMMAND)
	{
		if (*ModbusGetUserCommandId() == COMMAND_IGNORE_DOOR)
		{
			if(*ModbusGetUserCommandData() == MODBUS_ON)
			{
				LightOn();
				_ignoreDoor = true;
			}
			else
			{
				_ignoreDoor = false;
				isEnterInBath = false;
				LightOff();
				//bitRead(_MODBUSDiscreteInputs, LIGHT_INPUT) == 1 ? LightOn() : LightOff();
			}
			ModbusSetExceptionStatusBit(MB_EXCEPTION_LAST_COMMAND_STATE, 1);
		}
		else if (*ModbusGetUserCommandId() == COMMAND_SET_LIGHT)
		{
			if(*ModbusGetUserCommandData() == MODBUS_ON)
			{
				LightOn();
			}
			else
			{
				LightOff();
			}
			ModbusSetExceptionStatusBit(MB_EXCEPTION_LAST_COMMAND_STATE, 1);
		}
		return;
	}
}

uint8_t MyTimeSet(time_t newTime)
{
	return 0;
}
