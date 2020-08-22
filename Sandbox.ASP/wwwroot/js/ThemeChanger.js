function changeTheme(theme) {
    switch (theme) {
        case SiteTheme.Dark: setloadingIcon('🌮'); break;
        case SiteTheme.Light: setloadingIcon('🌯'); break;
    }

    activateLoader();
    clearTheme();
    setThemeClass(theme);
    setTimeout(function () {
        deactivateLoader();
    }, 3000);
}
function toggleDarkMode(checkbox) {
    if (checkbox.checked)
        changeTheme(SiteTheme.Dark);
    else
        changeTheme(SiteTheme.Light);
}
function clearTheme() {
    document.body.classList.remove('dark-theme');
    document.body.classList.remove('light-theme');
}
function setThemeClass(theme) {
    switch (theme) {
        case SiteTheme.Dark: document.body.classList.add('dark-theme'); break;
        case SiteTheme.Light: document.body.classList.add('light-theme'); break;
    }
}