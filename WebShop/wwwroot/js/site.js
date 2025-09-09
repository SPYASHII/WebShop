// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


function GetCookie(name) {
    const value = `; ${document.cookie}`;
    const parts = value.split(`; ${name}=`);

    if (parts.length === 2) {
        return parts.pop().split(';').shift();
    }
    return null;
}

function SetHeaderColor() {
    const headerColor = GetCookie(`HeaderBackgroundColor`);

    if (headerColor != null) {
        const header = document.querySelector("header");

        header.style.backgroundColor = headerColor;
    }
}

document.addEventListener('DOMContentLoaded', SetHeaderColor);