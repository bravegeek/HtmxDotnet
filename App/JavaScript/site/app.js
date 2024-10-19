import htmx from "htmx.org";
import { InitMutiTabForm } from "./components/multiTabForm";

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


InitMutiTabForm();