// פונקציה לקריאה ל-API לקבלת ספרים עם הדירוג הגבוה ביותר
function GetTopRatedBooks() {
    let api = "https://localhost:7063/api/Book/TopRated"; // ה-URL המתאים ל-API שלך
    ajaxCall("GET", api, null, GetTopRatedBooksSCB, GetTopRatedBooksECB);
}

function GetTopRatedBooksSCB(topRatedBooks) {
    let swiperWrapper = document.querySelector('.swiper-wrapper');
    swiperWrapper.innerHTML = ''; // ניקוי תוכן קודם
    
    topRatedBooks.forEach(book => {
        let slideHTML = `
            <div class="swiper-slide">
                <div class="recommend-slide">
                    <div class="tour-slide__box">
                        <a href="${book.previewLink}">
                            <img src="${book.thumbnail}" alt="${book.title}" />
                        </a>
                    </div>
                </div>
            </div>
        `;
        swiperWrapper.innerHTML += slideHTML;
    });

    // לאחר עדכון התוכן, ייתכן שיהיה צורך לאתחל את ה-Swiper מחדש
    // swiper.update();  // אם אתה משתמש ב-Swiper JS, השורה הזו תאתחל את הסליידים מחדש
}

// פונקציה callback לשגיאה בקריאה ל-API לקבלת ספרים עם הדירוג הגבוה ביותר
function GetTopRatedBooksECB(ERROR) {
    alert("ERROR: " + ERROR);
}

// קריאה לפונקציה לקבלת ספרים עם הדירוג הגבוה ביותר
GetTopRatedBooks();
