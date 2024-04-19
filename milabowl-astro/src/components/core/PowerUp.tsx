import iconBomb from './../../assets/icon_bomb.svg'
import PowerUpModal from './PowerUpModal'
import React, { useState } from 'react'

export interface PowerUpProps {
    type: string // One of 3xc, wildcard, freehit, bboost
    gwPlayed?: string | null // Make this nullable?
    playerName: string // To know which user card to create modal on
}

interface iTypeToColor {
    [key: string]: string
}

const typeToColor: iTypeToColor = {
    '3xc': 'purple',
    wildcard: 'yellow',
    freehit: 'green',
    bboost: 'red',
}
interface PowerUpInfo {
    color: string
    icon: string
    name: string
    explanation: string
}

interface TypeDetails {
    [key: string]: PowerUpInfo
}

const num_points = '3'

const powerUpDetails: TypeDetails = {
    '3xc': {
        color: 'purple',
        icon: '🥊',
        name: 'Red Shell',
        explanation:
            '-' +
            num_points +
            " to the playah' in front of you (in MilaPts) overall. If you're in the lead, targets 2nd place",
    },
    wildcard: {
        color: 'yellow',
        icon: '🍌',
        name: 'Banana',
        explanation:
            '-' + num_points + ' to the playah behind you (in MilaPts) overall',
    },
    freehit: {
        color: 'green',
        icon: '🐢',
        name: 'Green Shell',
        explanation:
            '-' +
            num_points +
            " to the playah' in front of you (in MilaPts) this GW. If you're in the lead, targets 2nd place",
    },
    bboost: {
        color: 'red',
        icon: '🍄',
        name: 'Mushroom',
        explanation:
            '+50% MilaPts this GW, excluding effects from 💣 and other power ups',
    },
}

const icon_envelope = (
    <svg
        className="relative z-10 h-3 w-3"
        aria-hidden="true"
        xmlns="http://www.w3.org/2000/svg"
        fill="currentColor"
        viewBox="0 0 20 16"
    >
        <path d="m10.036 8.278 9.258-7.79A1.979 1.979 0 0 0 18 0H2A1.987 1.987 0 0 0 .641.541l9.395 7.737Z" />
        <path d="M11.241 9.817c-.36.275-.801.425-1.255.427-.428 0-.845-.138-1.187-.395L0 2.6V14a2 2 0 0 0 2 2h16a2 2 0 0 0 2-2V2.5l-8.759 7.317Z" />
    </svg>
)

const PowerUp: React.FC<PowerUpProps> = ({ gwPlayed, type, playerName }) => {
    const powerUp: PowerUpInfo = powerUpDetails[type]
    //let color = typeToColor[type];
    let color = powerUp.color
    let playedWeekIndicator

    // Set chip shadow based on whether it is played or not
    let classShadow: string

    // Set hover effect based on whether it is played or not
    let classHover: string

    // If chip is not played, then use default text:
    if (gwPlayed === undefined || gwPlayed == null || gwPlayed == '') {
        gwPlayed = '-'

        classShadow = ' shadow-lg shadow-' + color + '-500/50 '
        classHover =
            ' hover:bg-' +
            color +
            '-800 focus:ring-4 focus:outline-none focus:ring-' +
            color +
            '-300 dark:hover:bg-' +
            color +
            '-700 dark:focus:ring-' +
            color +
            '-800 '
        playedWeekIndicator = (
            <div
                className={
                    'shadow- relative shadow-lg' +
                    color +
                    '-500/50 inline-block p-1 text-end ' +
                    (gwPlayed == '-' ? 'pr-3' : 'pr-2') +
                    ' z-1 right-3 h-7 w-11 rounded-br-full rounded-tr-full border-2 border-white bg-blue-800 align-middle text-xs font-bold text-white hover:bg-blue-900 dark:border-gray-900'
                }
            >
                {gwPlayed}{' '}
            </div>
        )
    } else {
        // If chip is used, set the GW label in a gray-tone
        color = 'stone'
        classShadow = ''
        classHover = ' grayscale-[60%]'
        playedWeekIndicator = (
            <div
                className={
                    'relative inline-block p-1 text-end ' +
                    (gwPlayed == '-' ? 'pr-3' : 'pr-2') +
                    ' text-gray bg- h-7 w-11 align-middle text-xs font-bold' +
                    color +
                    '-800 hover:bg-' +
                    color +
                    '-900 z-1 right-3 rounded-br-full rounded-tr-full border-2 border-white dark:border-gray-900'
                }
            >
                {gwPlayed}{' '}
            </div>
        )
    }
    let powerUpIcon
    powerUpIcon = powerUp.icon

    /* By including all dynamically used colors in a comment, Tailwind will pull the styling for that class. See: https://stackoverflow.com/a/74959709
    bg-blue-800 hover:bg-blue-900 dark:border-gray-900
    shadow-yellow-500/50 text-white bg-yellow-700 hover:bg-yellow-800 focus:ring-yellow-300 dark:bg-yellow-600 dark:hover:bg-yellow-700 dark:focus:ring-yellow-800
    shadow-red-500/50 text-white bg-red-700 hover:bg-red-800 focus:ring-red-300 dark:bg-red-600 dark:hover:bg-red-700 dark:focus:ring-red-800
    shadow-green-500/50 text-white bg-green-700 hover:bg-green-800 focus:ring-green-300 dark:bg-green-600 dark:hover:bg-green-700 dark:focus:ring-green-800
    shadow-purple-500/50 text-white bg-purple-700 hover:bg-purple-800 focus:ring-purple-300 dark:bg-purple-600 dark:hover:bg-purple-700 dark:focus:ring-purple-800
    shadow-stone-500/50 text-white bg-stone-700 hover:bg-stone-800 focus:ring-stone-300 dark:bg-stone-600 dark:hover:bg-stone-700 dark:focus:ring-stone-800
    */

    const [hover, setHover] = useState(false)

    const modal_id = 'popup-modal-' + type + playerName

    return (
        <div>
            <div className="items-center justify-center align-middle">
                <button
                    type="button"
                    data-modal-target={modal_id}
                    data-modal-toggle={modal_id}
                    className={
                        classShadow +
                        ' bg- relative z-10 inline-flex p-2 text-center text-sm font-medium text-white' +
                        color +
                        '-700 rounded-full ' +
                        classHover +
                        ' dark:bg-' +
                        color +
                        '-600 dark:border-' +
                        color +
                        '-900'
                    }
                >
                    {powerUpIcon}
                </button>
                {playedWeekIndicator}
            </div>
            <PowerUpModal
                modal_id={modal_id}
                icon={powerUp.icon}
                info={powerUp.explanation}
                name={powerUp.name}
            />
        </div>
    )
}

export default PowerUp
