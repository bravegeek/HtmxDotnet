import { populateControllerList } from "../utils/controller_detect";

export function InitGamePage()
{    
    populateControllerList("gamepad-select");
    setUpListeners();
}

function setUpListeners()
{
    /**
     * Controller refresh button
     *
     * @type {HTMLButtonElement}
     */
    const btn = document.getElementById('refresh-gamepad-select');

    btn.addEventListener('click', refreshGamePadList)

    window.addEventListener("gamepadconnected", (event) => {
        console.log("Gamepad connected:", event.gamepad);
      });
}

function refreshGamePadList()
{
    populateControllerList("gamepad-select");
}