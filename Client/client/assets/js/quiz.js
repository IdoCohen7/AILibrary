function GetRandomQuestions() {
  let api = "https://proj.ruppin.ac.il/cgroup75/test2/tar1/api/Question";
  ajaxCall("GET", api, null, GetRandomQuestionsSCB, AjaxECB);
}

function GetRandomQuestionsSCB(questions) {
  let currentQuestionIndex = 0;
  let score = 0;

  // קבלת אלמנט החידון
  let quizContainer = document.getElementById("quizContainer");

  // ניקוי התוכן הקודם לפני הוספת תוכן חדש
  quizContainer.innerHTML = "";

  let outerDiv = document.createElement("div");
  quizContainer.appendChild(outerDiv);

  let startTime = Date.now();
  let timeLimit = 60000; // 15 שניות במילישניות
  let timerElement = document.createElement("div");
  timerElement.id = "timer";
  timerElement.style.fontSize = "24px";
  timerElement.style.marginBottom = "20px";
  outerDiv.appendChild(timerElement);

  function shuffle(array) {
    for (let i = array.length - 1; i > 0; i--) {
      let j = Math.floor(Math.random() * (i + 1));
      [array[i], array[j]] = [array[j], array[i]];
    }
    return array;
  }

  function updateTimer() {
    let elapsedTime = Date.now() - startTime;
    let remainingTime = Math.max(
      0,
      Math.ceil((timeLimit - elapsedTime) / 1000)
    );
    timerElement.textContent = `Time Left: ${remainingTime}`;

    if (remainingTime <= 0) {
      showResults();
    }
  }

  function displayQuestion(index) {
    // בדוק אם הזמן נגמר
    if (Date.now() - startTime > timeLimit) {
      showResults();
      return;
    }

    outerDiv.innerHTML = ""; // ניקוי השאלה הקודמת
    outerDiv.appendChild(timerElement);

    let question = questions[index];

    let header = document.createElement("h2");
    header.innerHTML = `Question ${index + 1}`;
    outerDiv.appendChild(header);

    let text = document.createElement("h3");
    text.innerHTML = question.questionText;
    outerDiv.appendChild(text);

    // איסוף ומעורבות תשובות
    let answers = [
      {
        text: question.answer1,
        isCorrect: question.answer1 === question.correctAnswer,
      },
      {
        text: question.answer2,
        isCorrect: question.answer2 === question.correctAnswer,
      },
      {
        text: question.answer3,
        isCorrect: question.answer3 === question.correctAnswer,
      },
      {
        text: question.answer4,
        isCorrect: question.answer4 === question.correctAnswer,
      },
    ];
    shuffle(answers);

    // הצגת התשובות המעורבות
    answers.forEach((answer) => {
      let p = document.createElement("a");
      p.classList.add("game-card__title");
      p.innerHTML = answer.text;
      p.addEventListener("click", function () {
        if (answer.isCorrect) {
          score++;
        }
        currentQuestionIndex++;
        if (
          currentQuestionIndex < questions.length &&
          Date.now() - startTime <= timeLimit
        ) {
          displayQuestion(currentQuestionIndex);
        } else {
          showResults();
        }
      });
      outerDiv.appendChild(p);
    });
  }

  function showResults() {
    outerDiv.innerHTML = "";

    let message = document.createElement("p");
    if (score <= 2) {
      message.innerHTML = "Better luck next time, ";
    } else if (score <= 4) {
      message.innerHTML = "Nicely done, ";
    } else if (score == 5) {
      message.innerHTML = "Perfect! ";
    }

    if (user != null) {
      message.innerHTML += user.name + ", ";
    }

    message.innerHTML += `You scored ${score} out of ${questions.length}`;
    outerDiv.appendChild(message);
  }

  // התחל טיימר ועדכן כל שנייה
  setInterval(updateTimer, 1000);

  displayQuestion(currentQuestionIndex);
}
