// Reset Command 01 64 7F 00 00 00 00 00 00 00 FC 64

#include <Wire.h>
#include <Adafruit_BMP085.h>
#include <dht.h>
#include <Time.h>
#include <DS1302RTC.h>
#include <avr/wdt.h>
#include <EEPROM.h>
#include "ControllerSettings.h"
#include <ModbusRtu.h>

#define TXEN 4  //!!! (Забит в bootloader)
// A4 - SDA
// A5 - SCL
#define DHTPIN  2   // номер входа, подключенный к датчику влажности
#define DHTTYPE DHT22
// расположен на плате Arduino

#define EXCEPTION_STATUS_ERROR_DHT_BIT 7
#define EXCEPTION_STATUS_ERROR_BMP180_BIT 6

#define MEASURING_INTERVAL_MS 2000 // Интервал между измерениями

#define INPUT_REG_RESULT_TEMP 1 // Конечная температура после преобразований * 10
#define INPUT_REG_BMP180_TEMP 2 // Температура с датчика давления * 10
#define INPUT_REG_DHT_TEMP 3 // Температура с датчика влажности * 10
#define INPUT_REG_HYMIDITY 4 // Влажность * 10
#define INPUT_REG_PRESSURE 5 // Давление (мм рт ст * 10)

#include <MyModbus/ModbusRtu.h>
#include <DHTlib/dht.h>

//Задаём ведомому адрес, последовательный порт, выход управления TX

boolean led;
int8_t state = 0;
unsigned long tempus;

// массив данных modbus
#define modbusInputBufLen 6
//#define modbusHoldingBufLen 0
uint16_t _MODBUSCoils;
uint16_t _MODBUSDiscreteInputs;
uint16_t _MODBUSInputRegs[modbusInputBufLen];
//uint16_t _MODBUSHoldingRegs[11];

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

Adafruit_BMP085 bmp;
dht DHT;

bool _dhtError = false;

void setup() 
{
	Modbus(0, TXEN, &myDeviceInfo);
	// настраиваем входы и выходы
	io_setup();
	// настраиваем последовательный порт ведомого
	ModbusBegin(9600);

	if (!bmp.begin()) 
	{
		_dhtError = true;
		ModbusSetExceptionStatusBit(EXCEPTION_STATUS_ERROR_BMP180_BIT, true);
		//Serial.println("Could not find a valid BMP085 sensor, check wiring!");
	}
}

void io_setup() {


}

//void(* resetFunc) (void) = 0x7800; // Reset MC function

unsigned long _readSetsorsNextTime = 0;

void loop() {
	auto curMs = millis();
	// Защита от переполнения millis
	if(curMs > _readSetsorsNextTime || _readSetsorsNextTime - curMs > MEASURING_INTERVAL_MS)
	{
		_readSetsorsNextTime = curMs + MEASURING_INTERVAL_MS;
		auto bmpTemp = 88.;
		if(!_dhtError)
		{
			bmpTemp = bmp.readTemperature();
			_MODBUSInputRegs[INPUT_REG_PRESSURE] = bmp.readSealevelPressure() * 0.07501875468867216804201050262566;// / 133.3 * 10;
		}
		else
		{
			_MODBUSInputRegs[INPUT_REG_PRESSURE] = 6666;
		}
		auto dhtTemp = bmpTemp;
		_MODBUSInputRegs[INPUT_REG_BMP180_TEMP] = bmpTemp * 10;
		

		auto chk = DHT.read(DHTPIN);

		if (chk == DHTLIB_OK)
		{
			ModbusSetExceptionStatusBit(EXCEPTION_STATUS_ERROR_DHT_BIT, false);
			_MODBUSInputRegs[INPUT_REG_HYMIDITY] = DHT.humidity * 10;
			dhtTemp = DHT.temperature;
			_MODBUSInputRegs[INPUT_REG_DHT_TEMP] = dhtTemp * 10;
		}
		else
		{
			ModbusSetExceptionStatusBit(EXCEPTION_STATUS_ERROR_DHT_BIT, true);
		}
		// Просто выбираем наименьшую температуру
		if(dhtTemp > bmpTemp)
		{
			dhtTemp = bmpTemp;
		}
		_MODBUSInputRegs[INPUT_REG_RESULT_TEMP] = dhtTemp * 10;
	}




	//-----------------------------------------------------
	// обработка сообщений
	state = ModbusPoll(_MODBUSDiscreteInputs, &_MODBUSCoils, _MODBUSInputRegs, modbusInputBufLen, NULL, 0);

	io_poll();
}



void io_poll() {

}

