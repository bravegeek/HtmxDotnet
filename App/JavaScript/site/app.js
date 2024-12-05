import htmx from "htmx.org";
import { InitMutiTabForm } from "./components/multiTabForm";
import { InitGamePage } from "./pages/game";

/**
 * Creates a debounced function that delays invoking the provided function
 * until after `wait` milliseconds have passed since the last time the
 * debounced function was invoked.
 *
 * @template T
 * @param {(...args: any[]) => T} func - The function to debounce. It should return a value of type T.
 * @param {number} wait - The number of milliseconds to delay.
 * @returns {(...args: any[]) => T} - Returns the new debounced function, which returns the same type as `func`.
 *
 * @example
 * // Usage:
 * const debouncedFunc = debounce(() => "hello", 200);
 * let result = debouncedFunc(); // returns a string after 200ms
 */
function debounce(func, wait) {
    let timeout;

    return function(...args) {
        const context = this;
        clearTimeout(timeout);
        timeout = setTimeout(() => {
            func.apply(context, args);
        }, wait);
    };
}

document.addEventListener("DOMContentLoaded", function () {
    const location = window.location;
    // Check if the URL indicates we're on the "game" page
    if (location.pathname.match('/Game')) {
        // Call the InitPage function to initialize the game page
        InitGamePage();
    }

    if(location.pathname.match('/KitchenSink'))
    {
        InitMutiTabForm();
    }
});

