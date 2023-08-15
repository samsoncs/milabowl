import styles from "./Avatar.module.css";
import party from "party-js";
import PowerUp from "./PowerUp"
import game_state from "../../game_state/game_state.json";
import type { ResultsByUser, GameWeekResult } from "../../game_state/gameState";

const milaResultsByUser: ResultsByUser = game_state.resultsByUser;

export interface AvatarProps {
    teamName: string;
    children: any;
}

interface UserProfileInfo {
    info: string | null;
    followers: number | null;
    following: number | null;
    name: string;
    avatarSrc: string;
}

interface UserProfiles {
    [key: string]: UserProfileInfo
}

const imageRoot = "/avatars/";
const userProfileDict: UserProfiles = {
    "$jeik og betalt": {
        "info": "Mila-source founder and evangelist",
        "followers": 42,
        "following": 69,
        "name": "sam",
        "avatarSrc": imageRoot + "sam.png"
    },
    "Milady’s Cmilax": {
        "info": "Mila-source contributor",
        "followers": 0,
        "following": 0,
        "name": "mikkel",
        "avatarSrc": imageRoot + "mikkel.png"
    },
    "Henriks Håndjagere": {
        "info": "Mila try-hard",
        "followers": 1,
        "following": 1,
        "name": "eivind",
        "avatarSrc": imageRoot + "eivind.png"
    },
    "The KaneSaw Massacre": {
        "info": "",
        "followers": 0,
        "following": 0,
        "name": "malte",
        "avatarSrc": imageRoot + "malte.png"
    },
    "MilaysianBucketBoys": {
        "info": "Mila-source founder",
        "followers": 0,
        "following": 0,
        "name": "simen",
        "avatarSrc": imageRoot + "simen.png"
    },
    "SplitteMilaBramseil": {
        "info": "He came",
        "followers": 0,
        "following": 0,
        "name": "anders",
        "avatarSrc": imageRoot + "anders.png"
    },
    "Skodad Octavia": {
        "info": "Legendary BieUgle",
        "followers": 0,
        "following": 0,
        "name": "markus",
        "avatarSrc": imageRoot + "markus.png"
    },
    "AC Mila": {
        "info": "",
        "followers": 0,
        "following": 0,
        "name": "martin",
        "avatarSrc": imageRoot + "martin.png"
    },
    "Mast69urbinho": {
        "info": "💩",
        "followers": 0,
        "following": 0,
        "name": "henrik",
        "avatarSrc": imageRoot + "henrik.png"
    }

};

const Avatar: React.FC<AvatarProps> = ({ children, teamName }) => {
    let user: UserProfileInfo = {
        "info": "Nobody has no name",
        "name": "John Doe",
        "followers": -1,
        "following": -1,
        "avatarSrc": "https://www.nicepng.com/png/detail/186-1869910_ic-question-mark-roblox-question-mark-avatar.png"
    };
    if (teamName in userProfileDict) {
        user = userProfileDict[teamName];
    }

    // List of existing power ups
    let powerUpGWs = {
        "3xc": "",
        "wildcard": "",
        "freehit": "",
        "bboost": "",
    };
    // For each power up, determine if (and when) it has been used by the current player
    const userResults: GameWeekResult[] = milaResultsByUser.find(e => e.teamName == teamName).results;

    for (const [key, value] of Object.entries(powerUpGWs)) {
        let gw = userResults.find(e => e.milaPoints.activeChip == key)?.gameWeek;
        powerUpGWs[key] = gw;
    }

    return (
        <div className={styles.avatarDiv}>
            <div data-popover-target={"popover-user-profile" + teamName.replaceAll(" ", "")} className={styles.avatar + " w-6 md:w-7 lg:w-9"}>
                {children}
            </div>
            {/* <img
                className={styles.avatar + " w-5 md:w-7 lg:w-9"}
                src={user["avatarSrc"]}
                //onMouseOver={(e) => console.log(e)}
                //width={size}
                data-popover-target={"popover-user-profile" + teamName.replaceAll(" ", "")}
            /> */}
            <div data-popover id={"popover-user-profile" + teamName.replaceAll(" ", "")} role="tooltip" className="absolute z-10 invisible inline-block w-81 text-sm text-gray-500 transition-opacity duration-300 bg-white border border-gray-200 rounded-lg shadow-sm opacity-0 dark:text-gray-400 dark:bg-gray-800 dark:border-gray-600">
                <div className="p-3">
                    <div className="flex items-center justify-between mb-2">
                        {/* <a href="#">
                            <img className="w-24 h-24 rounded-full" src={user["avatarSrc"]} alt="Avatar pic" />
                        </a> */}
                        <div className={styles.avatar + " w-24 h-24 rounded-full"}>
                            {children}
                        </div>
                        <div>
                            <button
                                type="button"
                                // @ts-ignore
                                onClick={(e) => { party.confetti(e.target); }
                                } className="text-white bg-blue-700 hover:bg-blue-800 focus:ring-4 focus:ring-blue-300 font-medium rounded-lg text-xs px-3 py-1.5 dark:bg-blue-600 dark:hover:bg-blue-700 focus:outline-none dark:focus:ring-blue-800">Follow</button>
                        </div>
                    </div>
                    <p className="text-base font-semibold leading-none text-gray-900 dark:text-white">
                        <a href="#">{teamName}</a>
                    </p>
                    <p className="mb-3 text-sm font-normal">
                        <a href="#" className="hover:underline">@{user.name}</a>
                    </p>
                    <p className="mb-4 text-sm">{user.info}</p>

                    <div className="text-white pb-1"> Power ups: (played in gw)
                    </div>
                    <ul className="flex text-sm">
                        <li>
                            <PowerUp type="3xc" playerName={user.name.replaceAll(" ", "")} gwPlayed={powerUpGWs["3xc"]} />
                        </li>
                        <li>
                            <PowerUp type="wildcard" playerName={user.name.replaceAll(" ", "")} gwPlayed={powerUpGWs["wildcard"]} />
                        </li>
                        <li>
                            <PowerUp type="freehit" playerName={user.name.replaceAll(" ", "")} gwPlayed={powerUpGWs["freehit"]} />
                        </li>
                        <li>
                            <PowerUp type="bboost" playerName={user.name.replaceAll(" ", "")} gwPlayed={powerUpGWs["bboost"]} />
                        </li>
                    </ul>


                    <ul className="flex text-sm pt-2">
                        <li className="mr-2">
                            <a href="#" className="hover:underline">
                                <span className="font-semibold text-gray-900 dark:text-white">{user.following} </span>
                                <span>Following</span>
                            </a>
                        </li>
                        <li>
                            <a href="#" className="hover:underline">
                                <span className="font-semibold text-gray-900 dark:text-white">{user.followers} </span>
                                <span>Followers</span>
                            </a>
                        </li>
                    </ul>
                </div>
                <div data-popper-arrow></div>
            </div>
        </div >
    )
};

export default Avatar;