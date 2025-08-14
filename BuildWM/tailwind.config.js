/** @type {import('tailwindcss').Config} */
module.exports = {
  darkMode: 'class', // Enables switching using a class (e.g., .dark)
  content: [
    "./Views/**/*.cshtml",
    "./Pages/**/*.cshtml",
    "./wwwroot/js/**/*.js"
  ],
  theme: {
    extend: {
      fontFamily: {
        inter: ['Inter', 'sans-serif']
      }
    },
  },
  plugins: [],
}
