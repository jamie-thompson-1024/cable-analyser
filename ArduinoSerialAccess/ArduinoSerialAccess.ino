
int testPinConnection(int pin, int testPins[], int testPinCount);
void resetTestPins();
bool isValidTestPin(int pin);
String popFirstArgument(String* inputString, char delimiter);

/* ========================
 * =  Command reference   =
 * ========================
 *
 * ---------------
 *  Command:
 *    "TestPinConnections [pin to test] [pins to test connection to]"
 *      returns   "Results [pin tested] [pins connected to]"
 *      on error  "Error [error code]"
 *   
 *  Examples:
 *    "TestPinConnections 3 4,5,6,7"
 *      returns   "Results 3 5,6"
 *
 *    "TestPinConnections 3 4,5,90"
 *      returns   "Error 1"
 * ---------------
 *  Command:
 *    "DeviceType"
 *      returns "CableAnalyser"
*/


// Error codes
#define INVALID_PIN 1

// Constants
#define BAUD_RATE 9600
#define MAX_TEST_PINS 60

int VALID_TEST_PINS[] = {
  48, 49, 50, 51, 52
};
const int VALID_TEST_PIN_COUNT = sizeof(VALID_TEST_PINS) / sizeof(int);

void setup() {
  Serial.begin(BAUD_RATE);
  resetTestPins();
}

void loop() {
  if(Serial.available()) {
    // Receive message and extract command
    String serialMessage = Serial.readString();
    serialMessage.trim();
    String command = popFirstArgument(&serialMessage, ' ');

    if(command.equals("DeviceType")) {
      Serial.println("CableAnalyser");
    }

    if(command.equals("TestPinConnections")) {
      String highPinArg = popFirstArgument(&serialMessage, ' ');
      int highPin = (int)highPinArg.toInt();

      String testPinsArg = popFirstArgument(&serialMessage, ' ');
      int testPins[MAX_TEST_PINS];
      int testPinCount;
      for(testPinCount = 0; testPinCount < MAX_TEST_PINS && testPinsArg.length() > 0; testPinCount++) {
        String pinArg = popFirstArgument(&testPinsArg, ',');
        testPins[testPinCount] = (int)pinArg.toInt();
      }

      // Run test and catch and send any error messages back to computer
      int errorCode = testPinConnection(highPin, testPins, testPinCount) ;
      if(errorCode != 0) {
        Serial.println(String("Error ") + String(errorCode, DEC));
      }

      resetTestPins();
    }
  }
}

int testPinConnection(int pin, int testPins[], int testPinCount) {
  
  // Configure pins
  for(int i = 0; i < testPinCount; i++) {
    int testPin = testPins[i];
    if(!isValidTestPin(testPin)) {
      return INVALID_PIN;
    }
    pinMode(testPin, INPUT_PULLUP);
  }

  if(!isValidTestPin(pin)) {
    return INVALID_PIN;
  }
  pinMode(pin, OUTPUT);
  digitalWrite(pin, LOW);
  
  int connectedPins[MAX_TEST_PINS];
  int connectedPinCount = 0;

  // Sample test pins
  for(int i = 0; i < testPinCount; i++) {
    int testPin = testPins[i];
    if(digitalRead(testPin) == LOW) {
      connectedPins[connectedPinCount++] = testPin;
    }
  }

  // Construct message to send back to computer
  String resultsMessage = String("Results ") + String(pin, DEC) + String(" ");

  if(connectedPinCount > 0) {
    for(int i = 0; i < connectedPinCount; i++) {
      resultsMessage += String(connectedPins[i], DEC);
      if(i < connectedPinCount - 1) {
        resultsMessage += String(',');
      }
    }
  } else {
    resultsMessage += "N/C";
  }

  Serial.println(resultsMessage);

  return 0;
}

void resetTestPins() {
  for(int i = 0; i < VALID_TEST_PIN_COUNT; i++) {
    int pin = VALID_TEST_PINS[i];
    pinMode(pin, INPUT);
    digitalWrite(pin, LOW);
  }
}

bool isValidTestPin(int pin) {
  for(int i = 0; i < VALID_TEST_PIN_COUNT; i++) {
    int validPin = VALID_TEST_PINS[i];
    if(pin == validPin) {
      return true;
    }
  }
  return false;
}

String popFirstArgument(String* inputString, char delimiter) {
  int breakPoint;

  for(breakPoint = 0; breakPoint < inputString->length(); breakPoint++) {
    if(inputString->charAt(breakPoint) == delimiter) {
      break;
    }
  }

  String argument = inputString->substring(0, breakPoint);
  inputString->remove(0, breakPoint);

  if(inputString->charAt(0) == delimiter) {
    inputString->remove(0, 1);
  } 

  if(inputString->charAt(inputString->length() - 1) == delimiter) {
    inputString->remove(inputString->length() - 1, 1);
  } 
  
  return argument;
}
