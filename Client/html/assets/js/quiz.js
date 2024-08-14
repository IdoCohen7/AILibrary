function GetRandomQuestions() {
  let api = "https://localhost:7063/api/Question";
  ajaxCall("GET", api, null, GetRandomQuestionsSCB, AjaxECB);
}

function GetRandomQuestionsSCB(questions) {
  let currentQuestionIndex = 0;
  let score = 0;
  let outerDiv = document.createElement("div");
  document.getElementById("quizContainer").appendChild(outerDiv);

  function shuffle(array) {
    for (let i = array.length - 1; i > 0; i--) {
      let j = Math.floor(Math.random() * (i + 1));
      [array[i], array[j]] = [array[j], array[i]];
    }
    return array;
  }

  function displayQuestion(index) {
    outerDiv.innerHTML = ""; // Clear previous question

    let question = questions[index];

    let header = document.createElement("h2");
    header.innerHTML = `Question ${index + 1}`;
    outerDiv.appendChild(header);

    let text = document.createElement("h3");
    text.innerHTML = question.questionText;
    outerDiv.appendChild(text);

    // Collect and shuffle answers
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

    // Display shuffled answers
    answers.forEach((answer) => {
      let p = document.createElement("a");
      p.classList.add("game-card__title");
      p.innerHTML = answer.text;
      p.addEventListener("click", function () {
        if (answer.isCorrect) {
          score++;
        }
        currentQuestionIndex++;
        if (currentQuestionIndex < questions.length) {
          displayQuestion(currentQuestionIndex);
        } else {
          showResults();
        }
      });
      outerDiv.appendChild(p);
    });
  }

  function showResults() {
    let message = document.createElement("p");

    if (score <= 2) {
      message = "Better luck next time, ";
    } else if (score <= 4) {
      message = "Nicely done, ";
    } else if (score == 5) {
      message = "Perfect! ";
    }

    if (user != null) {
      message += user.name + ", ";
    }

    outerDiv.innerHTML = message;

    outerDiv.innerHTML += `You scored ${score} out of ${questions.length}`;
  }

  displayQuestion(currentQuestionIndex);
}

GetRandomQuestions();
