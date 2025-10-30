// // ==============================
// Theme Toggle
// ==============================
const themeToggleBtn = document.getElementById("themeToggle");
const themeIcon = document.getElementById("themeIcon");

(function () {
    const savedTheme = localStorage.getItem("theme");
    if (savedTheme === "dark") {
        document.body.classList.add("dark-theme");
        themeIcon.textContent = "🌙 Dark";
    } else {
        document.body.classList.remove("dark-theme");
        themeIcon.textContent = "🌞 Light";
    }
})();

if (themeToggleBtn) {
    themeToggleBtn.addEventListener("click", () => {
        document.body.classList.toggle("dark-theme");
        if (document.body.classList.contains("dark-theme")) {
            localStorage.setItem("theme", "dark");
            themeIcon.textContent = "🌙 Dark";
        } else {
            localStorage.setItem("theme", "light");
            themeIcon.textContent = "🌞 Light";
        }
    });
}

// ==============================
// Sidebar Toggle (Mobile)
// ==============================
const sidebar = document.querySelector(".sidebar");
const sidebarToggleBtn = document.getElementById("sidebarToggle");

if (sidebar && sidebarToggleBtn) {
    sidebarToggleBtn.addEventListener("click", () => {
        sidebar.classList.toggle("sidebar-open");
    });
}

document.addEventListener("click", (e) => {
    if (
        window.innerWidth < 768 &&
        sidebar &&
        sidebar.classList.contains("sidebar-open") &&
        !sidebar.contains(e.target) &&
        e.target !== sidebarToggleBtn
    ) {
        sidebar.classList.remove("sidebar-open");
    }
});

// ==============================
// Init
// ==============================
document.addEventListener("DOMContentLoaded", () => {
    console.log("✅ Customer Dashboard ready");
});
