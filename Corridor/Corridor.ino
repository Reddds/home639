// Reset Command 01 64 7F 00 00 00 00 00 00 00 FC 64


#include "RS485.h"
#include "DemoMode.h"
#include <Time.h>
#include <DS1302RTC.h>
#include <avr/wdt.h>
#include "ControllerSettings.h"
#include <ModbusRtu.h>

//#define RESET_VECTOR() ((void(const *)(void))0x7800)()
#define RESET_PIN 18 // A4
#define TXEN 4  //!!! (Забит в bootloader)
//#define ID   1      // адрес ведомого
#define sensorPin  2   // номер входа, подключенный к кнопке
#define stlPin  13  // номер выхода индикатора работы
// расположен на плате Arduino
#define T_PIN 7 // Конрпус открыт

#define WATCHING_TIME_MS 5000

#define RESET_COIL 1 // Адрес : 17. Команда MODBUS: [slaveID] 05 0001 FF00 [CRC] / 01 05 00 01 FF 00 DD FA 
#define MODE_DEMO_COIL 2 // Адрес : 18
#include <MyModbus/ModbusRtu.h>

// В режиме Демо считывается EEPROM c адреса EE_DEMO_START
// Если в [0] 0x22 - идентификатор что демопрограмма загружена
// [1] - байт длины программы (максимум - 255 байт)
// [2]... код демопрограммы
// Команды:
// Команда состоит из 2 байтов
// 0 - Какой светодиод зажечь и на какю яркость
// Старшие 4 бита - маска светодиодов от 0
// Младшие - яркость 0 - не горит, F - на полную
// 2 - интервал в милисекундах * 10
// Если интервал = 0xFF, то Интервал - следующий байт в секундах
// После достижения последнего байта программа работает по кругу
enum WorkMode { ModeMoveSensor, ModeDemo };
WorkMode _currentMode = ModeMoveSensor;

//Задаём ведомому адрес, последовательный порт, выход управления TX

boolean led;
int8_t state = 0;
unsigned long tempus;

// массив данных modbus
uint16_t _MODBUSCoils;
uint16_t _MODBUSDiscreteInputs;
//uint16_t _MODBUSInputRegs[5];
//uint16_t _MODBUSHoldingRegs[11];

bool _moveDetected;
unsigned long _lastMove;

void io_setup();
void ResetArduino();
void LightOn();
void LightOff();
void io_poll();

void ResetMoveDetection()
{
	_moveDetected = false;
	_lastMove = 0;
}

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
	ResetMoveDetection();
	// настраиваем входы и выходы
	io_setup();
	// настраиваем последовательный порт ведомого
	ModbusBegin(9600);
	// зажигаем светодиод на 100 мс
	tempus = millis() + 100;
	digitalWrite(stlPin, HIGH);
	LoadDemo();
	//delay(2000);
}

void io_setup() {
	digitalWrite(RESET_PIN, HIGH);

	digitalWrite(stlPin, HIGH);

	digitalWrite(led1Pin, LOW);
	digitalWrite(led2Pin, LOW);
	digitalWrite(led3Pin, LOW);
	digitalWrite(led4Pin, LOW);


	pinMode(RESET_PIN, OUTPUT);
	pinMode(stlPin, OUTPUT);

	pinMode(led1Pin, OUTPUT);
	pinMode(led2Pin, OUTPUT);
	pinMode(led3Pin, OUTPUT);
	pinMode(led4Pin, OUTPUT);

	pinMode(sensorPin, INPUT_PULLUP);

}

//void(* resetFunc) (void) = 0x7800; // Reset MC function

int demoModeLastLed = -1;
unsigned long demoModeLastTime = 0;

void loop() {
//	if (Serial.available()) {
//		int inByte = Serial.peek();
//		if (inByte == 0xFA)
//		{
//			ResetArduino();
//			return;
//		}
//	}
	if (_currentMode == ModeMoveSensor)
	{
		//------- Обработка сенсора --------------------------------
		if (digitalRead(sensorPin) != LOW) // сенсор работает на размыкание
		{
			_lastMove = millis();
			if (!_moveDetected)
			{
				_moveDetected = true;
				// включаем свет
				LightOn();
			}
		}
		else
		{
			unsigned long curTime = millis();
			// защита от обнуления millis раз в 50 дней
			if (_lastMove > curTime)
				_lastMove = 0;

			// Если контрольное время без движений превышено, вырубаем свет
			if ((curTime - _lastMove) > WATCHING_TIME_MS)
			{
				_moveDetected = false;
				LightOff();
			}
		}
	}
	else
	{
		// Demo mode
		NextDemoStep();
	}
	//-----------------------------------------------------
	// обработка сообщений
	state = ModbusPoll(_MODBUSDiscreteInputs, &_MODBUSCoils, NULL, 0, NULL, 0);
	// если получили пакет без ошибок - зажигаем светодиод на 50 мс
	if (state > 4) {
		tempus = millis() + 50;
		digitalWrite(stlPin, HIGH);
	}
	if (millis() > tempus) digitalWrite(stlPin, LOW);
	//обновляем данные в регистрах Modbus и в пользовательской программе
	io_poll();
}

// Включение всех диодов
void LightOn()
{
	digitalWrite(led1Pin, HIGH);
	digitalWrite(led2Pin, HIGH);
	digitalWrite(led3Pin, HIGH);
	digitalWrite(led4Pin, HIGH);
}

// Выключение всех диодов
void LightOff()
{
	digitalWrite(led1Pin, LOW);
	digitalWrite(led2Pin, LOW);
	digitalWrite(led3Pin, LOW);
	digitalWrite(led4Pin, LOW);
}

//void softwareReset(uint8_t prescaller) {
//	// start watchdog with the provided prescaller
//	wdt_enable(prescaller);
//	// wait for the prescaller time to expire
//	// without sending the reset signal by using
//	// the wdt_reset() method
//	while (1) {}
//}

//void ResetArduino()
//{
//	// restart in 60 milliseconds
//	softwareReset(WDTO_60MS);
//	digitalWrite(RESET_PIN, LOW);
//}


void io_poll() {
	// reset если в RESET_COIL было записано значение
//	if (bitRead(_MODBUSCoils, RESET_COIL))
//	{
//		ResetArduino();
//		return;
//	}

	if (bitRead(_MODBUSCoils, MODE_DEMO_COIL))
	{
		if (_currentMode != ModeDemo)
		{
			demoModeLastLed = -1;
			ResetMoveDetection();
			_currentMode = ModeDemo;
			StartDemo();
		}
	}
	else
	{
		if (_currentMode != ModeMoveSensor)
		{
			LightOff();
			ResetMoveDetection();
			_currentMode = ModeMoveSensor;
		}
	}

	//Копируем Coil[1] в Discrete[0]
	//au16data[0] = au16data[1];
	//Выводим значение регистра 1.3 на светодиод
	//digitalWrite( ledPin, bitRead( au16data[1], 3 ));
	//Сохраняем состояние кнопки в регистр 0.3
	bitWrite(_MODBUSDiscreteInputs, 3, _moveDetected);//digitalRead( sensorPin )
											//Копируем Holding[5,6,7] в Input[2,3,4]
	//au16data[2] = au16data[5];
	//au16data[3] = au16data[6];
	//au16data[4] = au16data[7];
	//Сохраняем в регистры отладочную информацию
	//au16data[8] = slave.getInCnt();
	//au16data[9] = slave.getOutCnt();
	//au16data[10] = slave.getErrCnt();
}

