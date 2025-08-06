// wwwroot/js/theme.js
const toggle = document.getElementById("theme-toggle");
toggle.addEventListener("click", () => {
  document.documentElement.classList.toggle("dark");
  const theme = document.documentElement.classList.contains("dark") ? "dark" : "light";
  localStorage.setItem("theme", theme);
});

// Load saved theme on page load
if (localStorage.getItem("theme") === "dark") {
  document.documentElement.classList.add("dark");
}
