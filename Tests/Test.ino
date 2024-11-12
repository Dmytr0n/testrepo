#include <ArduinoUnit.h>

// Підключення вихідного коду для тестування
int counter1 = 0;
int counter2 = 0;

void resetCounters() {
  counter1 = 0;
  counter2 = 0;
}

// Тестові функції, які симулюють ігровий процес
void playGame(int number1, int number2) {
  if (number1 == 0) {
    resetCounters();
  }
  if (number1 == 1 && number2 == 4) {
    Serial.println("It's a tie. Player1 and Player2 select rock");
  }
  else if (number1 == 2 && number2 == 5) {
    Serial.println("It's a tie. Player1 and Player2 select paper");
  }
  else if (number1 == 3 && number2 == 6) {
    Serial.println("It's a tie. Player1 and Player2 select scissors");
  }
  else if (number1 == 1 && number2 == 6) {
    Serial.println("Player1 Win!. Player1 select rock and Player2 select scissors");
    counter1++;
  }
  else if (number1 == 3 && number2 == 5) {
    Serial.println("Player1 Win!. Player1 select scissors and Player2 select paper");
    counter1++;
  }
  else if (number1 == 2 && number2 == 4) {
    Serial.println("Player1 Win!. Player1 select paper and Player2 select rock");
    counter1++;
  }
  else if (number1 == 3 && number2 == 4) {
    Serial.println("Player2 Win!. Player1 select scissors and Player2 select rock");
    counter2++;
  }
  else if (number1 == 2 && number2 == 6) {
    Serial.println("Player2 Win!. Player1 select paper and Player2 select scissors");
    counter2++;
  }
  else if (number1 == 1 && number2 == 5) {
    Serial.println("Player2 Win!. Player1 select rock and Player2 select paper");
    counter2++;
  }
}

// Тести на нічию
test(tie_rock) {
  resetCounters();
  playGame(1, 4);
  assertEqual(counter1, 0);
  assertEqual(counter2, 0);
}

test(tie_paper) {
  resetCounters();
  playGame(2, 5);
  assertEqual(counter1, 0);
  assertEqual(counter2, 0);
}

test(tie_scissors) {
  resetCounters();
  playGame(3, 6);
  assertEqual(counter1, 0);
  assertEqual(counter2, 0);
}

// Тести на перемогу Player1
test(player1_wins_rock_scissors) {
  resetCounters();
  playGame(1, 6);
  assertEqual(counter1, 1);
  assertEqual(counter2, 0);
}

test(player1_wins_scissors_paper) {
  resetCounters();
  playGame(3, 5);
  assertEqual(counter1, 1);
  assertEqual(counter2, 0);
}

test(player1_wins_paper_rock) {
  resetCounters();
  playGame(2, 4);
  assertEqual(counter1, 1);
  assertEqual(counter2, 0);
}

// Тести на перемогу Player2
test(player2_wins_scissors_rock) {
  resetCounters();
  playGame(3, 4);
  assertEqual(counter1, 0);
  assertEqual(counter2, 1);
}

test(player2_wins_paper_scissors) {
  resetCounters();
  playGame(2, 6);
  assertEqual(counter1, 0);
  assertEqual(counter2, 1);
}

test(player2_wins_rock_paper) {
  resetCounters();
  playGame(1, 5);
  assertEqual(counter1, 0);
  assertEqual(counter2, 1);
}

// Тест на скидання лічильників
test(reset_counters_test) {
  counter1 = 5;
  counter2 = 3;
  resetCounters();
  assertEqual(counter1, 0);
  assertEqual(counter2, 0);
}

void setup() {
  Serial.begin(9600);
  Test::begin();
}

void loop() {
  Test::run();
}
