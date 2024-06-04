/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    "./src/**/*.{html,ts}",
  ],
  daisyui: {
    themes: ["lemonade", "night", ""],
  },
  theme: {
    extend: {},
  },
  plugins:
    [
      require('daisyui'),
    ],
}
