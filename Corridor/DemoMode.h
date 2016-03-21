#include <EEPROM.h>

byte _demoCode[256];
byte _demoLastBytePos;
bool _demoLoaded;

// РЈРєР°Р·Р°С‚РµР»СЊ РЅР° СЃР»РµРґСѓСЋС‰СѓСЋ РёРЅСЃС‚СЂСѓРєС†РёСЋ РґР»СЏ РёСЃРїРѕР»РЅРµРЅРёСЏ
byte _nextStepPointer;
unsigned long _lactDemoCommandTime;
int _delayToNextCommand;

bool LoadDemo()
{
  // Р§РёС‚Р°РµРј EEPROM 
  // Р’ РїРµСЂРІРѕРј Р±Р°Р№С‚Рµ - СЃРёРіРЅР°С‚СѓСЂР°
  byte value = EEPROM.read(0);
  if(value != 0x22)
    return false;
  // Р”Р»РёРЅР° РїСЂРѕРіСЂР°РјРјС‹ (+1)
  _demoLastBytePos = EEPROM.read(1);
  if(_demoLastBytePos == 0)
    return false;

  for(int i = 0; i <= _demoLastBytePos; i++)
  {
    _demoCode[i] = EEPROM[2 + i];
  }
  _demoLoaded = true;
  return true;
}

void StartDemo()
{
  if(!_demoLoaded)
    return;
  _lactDemoCommandTime = 0;
  //_demoLastBytePos = 0;
  _delayToNextCommand = 0;
  
}

// РџРµСЂРµС…РѕРґ РЅР° СѓРєР°Р·Р°С‚РµР»СЊ СЃР»РµРґСѓСЋС‰РµР№РіРѕ Р±Р°Р№С‚Р°
// Р•СЃР»Рё РµС‰С‘ РЅРµ РєРѕРЅРµС†, С‚Рѕ РІРѕР·РІСЂР°С‰Р°РµРј false
// Р•СЃР»Рё СѓРєР°Р·Р°С‚РµР»СЊ РѕР±РЅСѓР»РёР»СЃСЏ, С‚Рѕ true
bool IncDemoPointer()
{
  if(_nextStepPointer < _demoLastBytePos)
  {
    _nextStepPointer++;
    return false;
  }
  _nextStepPointer = 0;
  return true;
}


void NextDemoStep()
{
  if(!_demoLoaded)
    return;
  if(millis() - _lactDemoCommandTime < _delayToNextCommand)
    return;
  // Р•СЃР»Рё СѓРєР°Р·С‹РІР°РµС‚ РЅР° РїРѕСЃР»РµРґРЅРёР№ Р±Р°Р№С‚, С‚Рѕ СЌС‚Рѕ СЏРІРЅР°СЏ РѕС€РёР±РєР°, 
  // С‚Р°Рє РєР°Рє РЅР°РґРѕ СЃС‡РёС‚С‹РІР°С‚СЊ РїРѕСЃР»РµСѓСЋС‰СѓСЋ Р·Р°РґРµСЂР¶РєСѓ
  if(_nextStepPointer == _demoLastBytePos)
  {
    _nextStepPointer = 0;
  }
  byte ledData = _demoCode[_nextStepPointer];
  IncDemoPointer();
  byte delayData = _demoCode[_nextStepPointer];
  bool jumpToZero = IncDemoPointer();
  int delayMs = 0;
  if(delayData == 0xFF)
  { 
    // Р•СЃР»Рё СЌС‚Рѕ Р±С‹Р» РїРѕСЃР»РµРґРЅРёР№ Р±Р°Р№С‚, Рё РѕРЅ 0xFF, С‚Рѕ РЅР°РґРѕ РµС‰С‘ СЃС‡РёС‚Р°С‚СЊ Р·Р°РґРµСЂР¶РєСѓ РІ СЃРµРєРЅРґР°С…
    // Р° РЅР° РЅРµС‘ РјРµСЃС‚Р° РЅРµ С…РІР°С‚РёР»Рѕ
    if(jumpToZero)
    {
      delayMs = 0;
    }
    else
    {
      delayMs = _demoCode[_nextStepPointer] * 1000;
      IncDemoPointer();
    }
  }
  else
  {
    delayMs = delayData * 10;
  }

  byte ledNum = ledData >> 4;
  byte bright = ledData << 4;
  if(bright > 0)
    bright += 0xf;

  if(ledNum & 0x1 > 0)
    analogWrite(led1Pin, bright);
  if(ledNum & 0x2 > 0)
    analogWrite(led2Pin, bright);
  if(ledNum & 0x4 > 0)
    analogWrite(led3Pin, bright);
  if(ledNum & 0x8 > 0)
    analogWrite(led4Pin, bright);
    
  _delayToNextCommand = delayMs;
  _lactDemoCommandTime = millis();
}


