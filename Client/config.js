// Import the functions you need from the SDKs you need
import { initializeApp } from "firebase/app";
import { getAnalytics } from "firebase/analytics";
// TODO: Add SDKs for Firebase products that you want to use
// https://firebase.google.com/docs/web/setup#available-libraries

// Your web app's Firebase configuration
// For Firebase JS SDK v7.20.0 and later, measurementId is optional
const firebaseConfig = {
  apiKey: "AIzaSyDo3gLMsVHj3pDFNX2CQbmSB7H0AD_3Fs4",
  authDomain: "ailibrary-656c3.firebaseapp.com",
  projectId: "ailibrary-656c3",
  storageBucket: "ailibrary-656c3.appspot.com",
  messagingSenderId: "38771178732",
  appId: "1:38771178732:web:817c8b3b606da9d422b9c9",
  measurementId: "G-R7LZZMMP0H",
  databaseURL: "https://ailibrary-656c3-default-rtdb.firebaseio.com/",
};

// Initialize Firebase
const app = initializeApp(firebaseConfig);
const analytics = getAnalytics(app);

firebase.initializeApp(firebaseConfig);
firebase.analytics();
