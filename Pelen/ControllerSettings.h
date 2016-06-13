#pragma once

#define DEVICE_TYPE_COMMON_CONTROLLER 0x02

#define SLAVE_ID_DEVICE_TYPE DEVICE_TYPE_COMMON_CONTROLLER // Indicator
#define SLAVE_ID_DEVICE_SUB_TYPE 0x01
#define SLAVE_ID_DEVICE_REVISION 0x01
#define SLAVE_ID_DEVICE_NUMBER 0x00


#define DEVICE_NEED_TIME_SET 1

#define VENDOR_NAME "Arduino"
#define PRODUCT_CODE "Nano"
#define MAJOR_MINOR_REVISION "1.01"
#define VENDOR_URL "http://arduino.cc"
#define PRODUCT_NAME "Pelenalnyi stol"
#define MODEL_NAME "MMM"
#define USER_APPLICATION_NAME "MyController"
/*
extern "C"
{
const uint8_t SLAVE_ID_DEVICE_TYPE = DEVICE_TYPE_COMMON_CONTROLLER; // Indicator
const uint8_t SLAVE_ID_DEVICE_SUB_TYPE = 0x01;
const uint8_t SLAVE_ID_DEVICE_REVISION = 0x01;
const uint8_t SLAVE_ID_DEVICE_NUMBER = 0x00;




const char* const VENDOR_NAME = "Arduino";
const char* const PRODUCT_CODE = "Nano";
const char* const MAJOR_MINOR_REVISION = "1.01";
const char* const VENDOR_URL = "http://arduino.cc";
const char* const PRODUCT_NAME = "Pelenalnyi stol";
const char* const MODEL_NAME = "MMM";
const char* const USER_APPLICATION_NAME = "MyController";
}*/