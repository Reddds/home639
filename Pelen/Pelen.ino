/*
Ver 2.1
*/

//#include "Calendar.h"
#include <dht.h>
#include <LiquidCrystal.h>
#include <DS1302RTC.h>
#include <Streaming.h>
#include <Time.h>
#include <Wire.h>
#include <EEPROM.h>
#include <IRremote.h>
#include "ModbusRtu.h"
#include <avr/wdt.h>

//!!!!!!!! ��������� ������� ��� ������ � ����
//2015,12,24,19,54,00,

#define RESET_PIN 7 // A4
#define TXEN 4  //!!! (����� � bootloader)
#define ID   2      // ����� ��������


// Set your own pins with these defines !
#define DS1302_SCLK_PIN   A2    // Arduino pin for the Serial Clock
#define DS1302_IO_PIN     A1    // Arduino pin for the Data I/O
#define DS1302_CE_PIN     A0    // Arduino pin for the Chip Enable


//������, ���������� ����� ����������
char compileTime[] = __TIME__; //"hh:mm:ss"
char compileDate[] = __DATE__; //"hh:mm:ss"

#define DHTPIN 6
#define DHTTYPE DHT22
#define KAKA_PIN 8
#define CALL_PIN 10 // ������ ������

#define RECV_PIN 11 // �� �������

#define CALL_LED_PIN 13

#define KAKA_EEPROM_ADDRESS 10

#define RESET_COIL 1 // ����� : 17. ������� MODBUS: [slaveID] 05 0011 FF00 [CRC]
#define RESET_KAKA_COIL 2 // ����� 18
#define NEW_KAKA_COIL 3 // ���� ������ ������, ��������������� �������� 1, ���� ���������� �� �������
#define CALL_COIL 4 // ������ ������. ����� ��������� ��������
#define WASH_BANNER_TEST_COIL 5 // �������� ��������� �� ��������

#define KAKA_COUNT_INPUT_REG 0
#define ERROR_CODE_INPUT_REG 1
// ����������� - ���������
#define TEMP_INPUT_REG 2
#define HUM_INPUT_REG 3

// -------- ��������� ������� -----------------------------
#define HOUR_MIN_HOLDING_REG 0
#define DAY_SEC_HOLDING_REG 1
#define MONTH_YEAR_HOLDING_REG 2

#define SET_KAKA_COUNT_HOLDING_REG 3 // ���� �������� � ���� ������� �� ������ ����� ����������� ��� ���������� ���������

							   // Init the DS1302
							   // Set pins:  CE, IO,CLK
DS1302RTC RTC(DS1302_CE_PIN, DS1302_IO_PIN, DS1302_SCLK_PIN);
//DHT dht(DHTPIN, DHTTYPE);
dht DHT;
LiquidCrystal lcd(A4, A5, 5, 9, 3, 2);//4

									  //--------  MODBUS  -----------------------------
									  //����� �������� �����, ���������������� ����, ����� ���������� TX
Modbus slave(ID, 0, TXEN);
// ������ ������ modbus
#define modbusInputBufLen 5
#define modbusHoldingBufLen 5
uint16_t _MODBUSDiscreteInputs;
uint16_t _MODBUSCoils;
uint16_t _MODBUSInputRegs[modbusInputBufLen];
uint16_t _MODBUSHoldingRegs[modbusHoldingBufLen];
int8_t state = 0;
//--------  END MODBUS  -----------------------------
unsigned long oldMillis;
int kakaCounter = 0;


// ------------ function definitions -----------------
void ResetKaka();
void ShowWashMessage();
void io_setup();
void io_poll();
void SetKakaKount(uint8_t newVal, bool writeEeprom);
void InitRtc();
void ResetArduino();
void print2digits(int number);

void printTime(time_t t);
void printDate(time_t t);
void printI00(int val, char delim);
//-----------------------------------------------------


IRrecv irrecv(RECV_PIN);

void setup() {
	// ����������� ����� � ������
	io_setup();
	// ����������� ���������������� ���� ��������
	slave.begin(9600);



	// put your setup code here, to run once:
	Serial.begin(9600);
	//Serial.println("DHTxx test!");

	pinMode(KAKA_PIN, INPUT_PULLUP);
	pinMode(CALL_PIN, INPUT_PULLUP);

	//������ ������� �� EEPROM:
	SetKakaKount(EEPROM.read(KAKA_EEPROM_ADDRESS), false);

	//dht.begin();
	lcd.begin(16, 2);
	lcd.print("Starting...");
	// Check clock oscillation

	InitRtc();
	delay(1000);
	lcd.clear();

	irrecv.enableIRIn(); // Start the receiver

	oldMillis = millis();
}

void InitRtc()
{
	lcd.clear();
	
	if (RTC.haltRTC())
		lcd.print("Clock stpd! Set Time!");
	else
		lcd.print("Clock working.");
	//delay(1000);
	// Setup Time library
	lcd.setCursor(0, 1);
	lcd.print("RTC Sync");
	setSyncProvider(RTC.get); // the function to get the time from the RTC
	int timeStat = timeStatus();
	if (timeStat == timeSet)
		lcd.print(" Ok!");
	else if (timeStat == timeNotSet)
		lcd.print(" Need set!");
	else
		lcd.print(" NeedsSync!");
}

void io_setup() {
	digitalWrite(RESET_PIN, HIGH);

	pinMode(RESET_PIN, OUTPUT);

	pinMode(CALL_LED_PIN, OUTPUT);
}



bool kakaPressed;
unsigned long kakaPressedTime;



int buttonKakState = HIGH;             // the current reading from the input pin
int buttonCallState = HIGH;             // the current reading from the input pin

int lastKakPinState = LOW;   // the previous reading from the input pin
int lastCallPinState = LOW;

							 // the following variables are long's because the time, measured in miliseconds,
							 // will quickly become a bigger number than can be stored in an int.
long lastDebounceTime = 0;  // the last time the output pin was toggled
unsigned long debounceDelay = 50;    // the debounce time; increase if the output flickers

// ����, ��� �������� ��� �������� ����� ��������� � ���������
uint8_t resetDay = 0;

// ����, ��� �������� ��� �������� ��������� ���� �������
uint8_t washDay = 0;
// ����� ������ ��������� �� �����. ������������ ��� ������� ������ ����
bool isBannerMode = false;

decode_results irResults;



void loop() {
	if (Serial.available()) {
		auto inByte = Serial.peek();
		if (inByte == 0xFB) //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
		{
			ResetArduino();
			return;
		}
	}

	if (irrecv.decode(&irResults)) {
		if ((irResults.value & 0xFF) == 0x10)
			bitWrite(_MODBUSCoils, CALL_COIL, 1);
		irrecv.resume(); // Receive the next value
	}

	// ��������� ���������
	state = slave.poll(_MODBUSDiscreteInputs, _MODBUSCoils, _MODBUSInputRegs, modbusInputBufLen, _MODBUSHoldingRegs, modbusHoldingBufLen);

	//��������� ������ � ��������� Modbus � � ���������������� ���������
	io_poll();
	// ------------------------------------------------------------
	auto curMillis = millis();
	//----------------------------------------------------------------------------
	// read the state of the switch into a local variable:
	auto kakPinCurState = digitalRead(KAKA_PIN);
	auto callPinCurState = digitalRead(CALL_PIN);

	// check to see if you just pressed the button
	// (i.e. the input went from LOW to HIGH),  and you've waited
	// long enough since the last press to ignore any noise:

	// If the switch changed, due to noise or pressing:
	if (kakPinCurState != lastKakPinState)
	{
		// reset the debouncing timer
		lastDebounceTime = millis();
		lastKakPinState = kakPinCurState;
	}

	if (callPinCurState != lastCallPinState)
	{
		// reset the debouncing timer
		lastDebounceTime = millis();
		lastCallPinState = callPinCurState;
	}


	if ((millis() - lastDebounceTime) > debounceDelay)
	{
		// whatever the reading is at, it's been there for longer
		// than the debounce delay, so take it as the actual current state:

		// if the button state has changed:
		if (kakPinCurState != buttonKakState) {
			buttonKakState = kakPinCurState;

			//Serial.println(kakPinCurState);
			// only toggle the LED if the new button state is LOW
			if (buttonKakState == LOW) 
			{
				if (isBannerMode)
				{
					isBannerMode = false;
					lcd.clear();
				}
				else
					SetKakaKount(kakaCounter + 1, true);
			}
		}
		if (callPinCurState != buttonCallState) {
			buttonCallState = callPinCurState;

			//Serial.println(kakPinCurState);
			// only toggle the LED if the new button state is LOW
			if (buttonCallState == LOW) {
				bitWrite(_MODBUSCoils, CALL_COIL, 1);
			}
		}
	}
	//----------------------------------------------------------------------------
	if (curMillis - oldMillis < 1000)
		return;
	oldMillis = curMillis;

	// put your main code here, to run repeatedly:
	double h = 0;
	double t = 0;
	auto chk = DHT.read(DHTPIN);
	if (chk == DHTLIB_OK)
	{
		h = DHT.humidity;
		t = DHT.temperature;

		_MODBUSInputRegs[TEMP_INPUT_REG] = t;
		_MODBUSInputRegs[HUM_INPUT_REG] = h;

	}

	// Display time centered on the upper line
	auto ho = hour();
	auto mi = minute();
	auto d = day();
	if (ho == 0 && mi == 0 && resetDay != d)
	{
		resetDay = d;
		ResetKaka();
	}

	if (ho == 10 && mi == 0 && washDay != d)
	{
		washDay = d;
		ShowWashMessage();
	}

	if (!isBannerMode)
	{
		lcd.setCursor(0, 1);
		print2digits(ho);
		lcd.print(":");
		print2digits(mi);
		lcd.print(":");
		print2digits(second());

		lcd.setCursor(9, 1);
		lcd.print("KAK ");
		lcd.print(kakaCounter);

		if (isnan(t) || isnan(h)) {
			//Serial.println("Failed to read from DHT");
			lcd.setCursor(0, 0);
			lcd.print("Error read DHT!");
		}
		else {

			lcd.setCursor(0, 0);
			//lcd.print("Hum: ");
			lcd.print(h);
			lcd.print(" %");
			lcd.setCursor(8, 0);
			//lcd.print("Temp: ");
			lcd.print(t);
			lcd.print(" *C");

			//Serial.print("");
			//Serial.print(h);
			//Serial.print(" %");

			//Serial.print("Temperature: ");
			//Serial.print(t);
			//Serial.println(" *C");
		}
	}
	/*
	//RTC.set()
	time_t t1;
	tmElements_t tm;

	//check for input to set the RTC, minimum length is 12, i.e. yy,m,d,h,m,s
	if (Serial.available() >= 12) {
	//note that the tmElements_t Year member is an offset from 1970,
	//but the RTC wants the last two digits of the calendar year.
	//use the convenience macros from Time.h to do the conversions.
	int y = Serial.parseInt();
	if (y >= 100 && y < 1000)
	Serial.println(F("Error: Year must be two digits or four digits!"));
	else {
	if (y >= 1000)
	tm.Year = CalendarYrToTm(y);
	else    //(y < 100)
	tm.Year = y2kYearToTm(y);
	tm.Month = Serial.parseInt();
	tm.Day = Serial.parseInt();
	tm.Hour = Serial.parseInt();
	tm.Minute = Serial.parseInt();
	tm.Second = Serial.parseInt();
	t1 = makeTime(tm);
	//use the time_t value to ensure correct weekday is set
	if(RTC.set(t1) == 0) { // Success
	setTime(t1);
	Serial.print(F("RTC set to: "));
	printDateTime(t1);
	//Serial << endl;
	}
	else
	Serial.println(F("RTC set failed!")); //<< endl;
	//dump any extraneous input
	while (Serial.available() > 0) Serial.read();
	}
	}*/
}

void SetKakaKount(uint8_t newVal, bool writeEeprom)
{
	kakaCounter = newVal;
	if (writeEeprom)
	{
		EEPROM.write(KAKA_EEPROM_ADDRESS, kakaCounter);
		bitWrite(_MODBUSCoils, NEW_KAKA_COIL, 1);
	}
	_MODBUSInputRegs[KAKA_COUNT_INPUT_REG] = kakaCounter;
}

void ResetKaka()
{
	kakaCounter = 0;
	EEPROM.write(KAKA_EEPROM_ADDRESS, 0);
}

void ShowWashMessage()
{
	isBannerMode = true;
	lcd.clear();
	lcd.print("Wash face!");
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
	digitalWrite(RESET_PIN, LOW);
}

void io_poll() {
	// reset ���� � RESET_COIL ���� �������� ��������
	if (bitRead(_MODBUSCoils, RESET_COIL))
	{
		ResetArduino();
		return;
	}

	if (bitRead(_MODBUSCoils, CALL_COIL))
	{
		digitalWrite(CALL_LED_PIN, HIGH);
	}
	else
	{
		digitalWrite(CALL_LED_PIN, LOW);
	}

	if (bitRead(_MODBUSCoils, RESET_KAKA_COIL))
	{
		ResetKaka();
		bitWrite(_MODBUSCoils, RESET_KAKA_COIL, 0);
	}

	if (bitRead(_MODBUSCoils, WASH_BANNER_TEST_COIL))
	{
		ShowWashMessage();
		bitWrite(_MODBUSCoils, WASH_BANNER_TEST_COIL, 0);
	}



	if (_MODBUSHoldingRegs[SET_KAKA_COUNT_HOLDING_REG] != 0)
	{
		kakaCounter = _MODBUSHoldingRegs[SET_KAKA_COUNT_HOLDING_REG];
		EEPROM.write(KAKA_EEPROM_ADDRESS, kakaCounter);
		_MODBUSHoldingRegs[SET_KAKA_COUNT_HOLDING_REG] = 0;
	}

	// --------------- SET TIME -----------------------
	auto hourMin = _MODBUSHoldingRegs[HOUR_MIN_HOLDING_REG];
	if (hourMin != 0)
	{
		auto daySec = _MODBUSHoldingRegs[DAY_SEC_HOLDING_REG];
		auto monthYear = _MODBUSHoldingRegs[MONTH_YEAR_HOLDING_REG];
		time_t t1;
		tmElements_t tm;
		tm.Year = y2kYearToTm(monthYear & 0xFF);
		tm.Month = monthYear >> 8;
		tm.Day = daySec >> 8;
		tm.Hour = hourMin >> 8;
		tm.Minute = hourMin & 0xFF;
		tm.Second = daySec & 0xFF;
		t1 = makeTime(tm);

		if (RTC.set(t1) == 0)
		{ // Success
			_MODBUSInputRegs[ERROR_CODE_INPUT_REG] = 0xFFFF;
			InitRtc();
		}
		else
			_MODBUSInputRegs[ERROR_CODE_INPUT_REG] = 0x0101;


		_MODBUSHoldingRegs[HOUR_MIN_HOLDING_REG] = 0;
		_MODBUSHoldingRegs[DAY_SEC_HOLDING_REG] = 0;
		_MODBUSHoldingRegs[MONTH_YEAR_HOLDING_REG] = 0;
	}




	/*  if (bitRead( au16data[1], MODE_DEMO_COIL ))
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
	}*/

	//�������� Coil[1] � Discrete[0]
	//au16data[0] = au16data[1];
	//������� �������� �������� 1.3 �� ���������
	//digitalWrite( ledPin, bitRead( au16data[1], 3 ));
	//��������� ��������� ������ � ������� 0.3
	//bitWrite( au16data[0], 3, _moveDetected);//digitalRead( sensorPin )
	//�������� Holding[5,6,7] � Input[2,3,4]

	//au16data[3] = au16data[6];
	//au16data[4] = au16data[7];
	//��������� � �������� ���������� ����������
	//au16data[8] = slave.getInCnt();
	//au16data[9] = slave.getOutCnt();
	//au16data[10] = slave.getErrCnt();
}
void print2digits(int number) {
	// Output leading zero
	if (number >= 0 && number < 10) {
		lcd.write('0');
	}
	lcd.print(number);
}

char getInt(const char* string, int startIndex)
{
	return int(string[startIndex] - '0') * 10 + int(string[startIndex + 1]) - '0';
}
/*
char getMonth(const char* string, int startIndex)
{
char dec[] = "Dec";

bool equal = true;
for(int i = 0; i < 3; i++)
{
if(string[startIndex + i] != dec[i];
{
equal = false;
break;
}
}
if(equal)
return 12;
}*/

//print date and time to Serial
void printDateTime(time_t t)
{
	printDate(t);
	Serial << ' ';
	printTime(t);
}

//print time to Serial
void printTime(time_t t)
{
	printI00(hour(t), ':');
	printI00(minute(t), ':');
	printI00(second(t), ' ');
}

//print date to Serial
void printDate(time_t t)
{
	printI00(day(t), 0);
	Serial << monthShortStr(month(t)) << _DEC(year(t));
}

//Print an integer in "00" format (with leading zero),
//followed by a delimiter character to Serial.
//Input value assumed to be between 0 and 99.
void printI00(int val, char delim)
{
	if (val < 10) Serial << '0';
	Serial << _DEC(val);
	if (delim > 0) Serial << delim;
	return;
}