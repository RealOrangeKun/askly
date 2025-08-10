/** @type {import('tailwindcss').Config} */
module.exports = {
  content: ["./**.{html,js}"],
  darkMode: 'class', // Enable class-based dark mode
  theme: {
    extend: {
      transitionProperty: {
        'colors': 'color, background-color, border-color, text-decoration-color, fill, stroke',
      }
    },
  },
  plugins: [],
}
