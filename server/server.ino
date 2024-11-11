// Оголошення глобальних змінних
int counter1 = 0;
int counter2 = 0;

void setup() {
  Serial.begin(9600);
}

void loop() {
  if (Serial.available() >= 2) {
    int number1 = Serial.parseInt();
    int number2 = Serial.parseInt();
    if (number1 == 0)
    {
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
      counter1++; // Збільшуємо глобальний лічильник для Player1
    } 
    else if (number1 == 3 && number2 == 5) {
      Serial.println("Player1 Win!. Player1 select scissors and Player2 select paper");
      counter1++; // Збільшуємо глобальний лічильник для Player1
    } 
    else if (number1 == 2 && number2 == 4) {
      Serial.println("Player1 Win!. Player1 select paper and Player2 select rock");
      counter1++; // Збільшуємо глобальний лічильник для Player1
    } 
    else if (number1 == 3 && number2 == 4) {
      Serial.println("Player2 Win!. Player1 select scissors and Player2 select rock");
      counter2++; // Збільшуємо глобальний лічильник для Player2
    } 
    else if (number1 == 2 && number2 == 6) {
      Serial.println("Player2 Win!. Player1 select paper and Player2 select scissors");
      counter2++; // Збільшуємо глобальний лічильник для Player2
    } 
    else if (number1 == 1 && number2 == 5) {
      Serial.println("Player2 Win!. Player1 select rock and Player2 select paper");
      counter2++; // Збільшуємо глобальний лічильник для Player2
    }

    // Виводимо поточні значення лічильників
    Serial.print("Player1 Wins: ");
    Serial.println(counter1);
    Serial.print("Player2 Wins: ");
    Serial.println(counter2);
  }
}
void resetCounters() {
  counter1 = 0;
  counter2 = 0;
}