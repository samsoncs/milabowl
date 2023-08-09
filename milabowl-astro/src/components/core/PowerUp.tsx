import iconBomb from "./../../assets/icon_bomb.svg";
import React, { useState } from "react";

export interface PowerUpProps {
    type: string;  // One of 3xc, wildcard, freehit, bboost
    gwPlayed?: string | null; // Make this nullable?
}

interface iTypeToColor {
    [key: string]: string;
}

const typeToColor: iTypeToColor = {
    "3xc": "purple",
    "wildcard": "yellow",
    "freehit": "green",
    "bboost": "red"
};

const icon_envelope =
    <svg className="relative w-3 h-3 z-10" aria-hidden="true" xmlns="http://www.w3.org/2000/svg" fill="currentColor" viewBox="0 0 20 16">
        <path d="m10.036 8.278 9.258-7.79A1.979 1.979 0 0 0 18 0H2A1.987 1.987 0 0 0 .641.541l9.395 7.737Z" />
        <path d="M11.241 9.817c-.36.275-.801.425-1.255.427-.428 0-.845-.138-1.187-.395L0 2.6V14a2 2 0 0 0 2 2h16a2 2 0 0 0 2-2V2.5l-8.759 7.317Z" />
    </svg>;

const PowerUp: React.FC<PowerUpProps> = ({ gwPlayed, type }) => {
    let color = typeToColor[type];
    let playedWeekIndicator;

    // Set chip shadow based on whether it is played or not
    let classShadow: string;

    // Set hover effect based on whether it is played or not
    let classHover: string;

    // If chip is not played, then use default text:
    if (gwPlayed === undefined || gwPlayed == null || gwPlayed == "") {
        gwPlayed = "-"

        classShadow = " shadow-lg shadow-" + color + "-500/50 ";
        classHover = " hover:bg-" + color + "-800 focus:ring-4 focus:outline-none focus:ring-" + color + "-300 dark:hover:bg-" + color + "-700 dark:focus:ring-" + color + "-800 ";
        playedWeekIndicator = <div className={"relative shadow-lg shadow-" + color + "-500/50 inline-block text-end p-1 " + (gwPlayed == "-" ? "pr-3" : "pr-2") + " align-middle w-11 h-7 text-xs font-bold text-white bg-blue-800 hover:bg-blue-900 border-2 border-white rounded-br-full rounded-tr-full right-3 z-1 dark:border-gray-900"}>{gwPlayed} </div>;
    } else {
        // If chip is used, set the GW label in a gray-tone
        color = "stone";
        classShadow = "";
        classHover = " grayscale-[80%]";
        playedWeekIndicator = <div className={"relative inline-block text-end p-1 " + (gwPlayed == "-" ? "pr-3" : "pr-2") + " align-middle w-11 h-7 text-xs font-bold text-gray bg-" + color + "-800 hover:bg-" + color + "-900 border-2 border-white rounded-br-full rounded-tr-full right-3 z-1 dark:border-gray-900"}>{gwPlayed} </div>;
    }
    let powerUpIcon;
    if (type == "3xc") {
        powerUpIcon = "🥊";
    } else if (type == "wildcard") {
        powerUpIcon = "🍌";
    } else if (type == "freehit") {
        powerUpIcon = "🐢";
    } else if (type == "bboost") {
        powerUpIcon = "🍄";
    }

    /* By including all dynamically used colors in a comment, Tailwind will pull the styling for that class. See: https://stackoverflow.com/a/74959709
    bg-blue-800 hover:bg-blue-900 dark:border-gray-900
    shadow-yellow-500/50 text-white bg-yellow-700 hover:bg-yellow-800 focus:ring-yellow-300 dark:bg-yellow-600 dark:hover:bg-yellow-700 dark:focus:ring-yellow-800
    shadow-red-500/50 text-white bg-red-700 hover:bg-red-800 focus:ring-red-300 dark:bg-red-600 dark:hover:bg-red-700 dark:focus:ring-red-800
    shadow-green-500/50 text-white bg-green-700 hover:bg-green-800 focus:ring-green-300 dark:bg-green-600 dark:hover:bg-green-700 dark:focus:ring-green-800
    shadow-purple-500/50 text-white bg-purple-700 hover:bg-purple-800 focus:ring-purple-300 dark:bg-purple-600 dark:hover:bg-purple-700 dark:focus:ring-purple-800
    shadow-stone-500/50 text-white bg-stone-700 hover:bg-stone-800 focus:ring-stone-300 dark:bg-stone-600 dark:hover:bg-stone-700 dark:focus:ring-stone-800
    */

    const [hover, setHover] = useState(false);

    return (
        <div>
            <div className="items-center justify-center align-middle" >
                <button type="button" className={classShadow + " relative z-10 inline-flex p-2 text-sm font-medium text-center text-white bg-" + color + "-700 rounded-full " + classHover + " dark:bg-" + color + "-600 dark:border-" + color + "-900"}>
                    {powerUpIcon}
                </button>
                {playedWeekIndicator}
            </div>
        </div >
    )
};

export default PowerUp;