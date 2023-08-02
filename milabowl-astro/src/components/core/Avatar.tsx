import styles from "./Avatar.module.css";
import party from "party-js";

export interface AvatarProps {
    teamName: string;
    size: string; // currently not used
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
    "Haalandslaget": {
        "info": "Mila-source founder and evangelist",
        "followers": 42,
        "following": 69,
        "name": "sam",
        "avatarSrc": imageRoot + "sam.png"
    },
    "Milaion dollar squad": {
        "info": "Mila-source contributor",
        "followers": 0,
        "following": 0,
        "name": "mikkel",
        "avatarSrc": imageRoot + "mikkel.png"
    },
    "Haalandaise Saus": {
        "info": "Mila try-hard",
        "followers": 0,
        "following": 0,
        "name": "eivind",
        "avatarSrc": imageRoot + "eivind.png"
    },
    "WeDidn'tStartMaguire": {
        "info": "",
        "followers": 0,
        "following": 0,
        "name": "malte",
        "avatarSrc": imageRoot + "malte.png"
    },
    "eMILA Smith Rowe": {
        "info": "Mila-source founder",
        "followers": 0,
        "following": 0,
        "name": "simen",
        "avatarSrc": imageRoot + "simen.png"
    },
    "Veni Vidi Vici Mila": {
        "info": "He came",
        "followers": 0,
        "following": 0,
        "name": "anders",
        "avatarSrc": imageRoot + "anders.png"
    },
    "Premier Lag": {
        "info": "Legendary BieUgle",
        "followers": 0,
        "following": 0,
        "name": "markus",
        "avatarSrc": imageRoot + "markus.png"
    },
    "Filip Coutinho": {
        "info": "",
        "followers": 0,
        "following": 0,
        "name": "martin",
        "avatarSrc": imageRoot + "martin.png"
    },
    "Boris’ party boys": {
        "info": "💩",
        "followers": 0,
        "following": 0,
        "name": "henrik",
        "avatarSrc": imageRoot + "henrik.png"
    }

};

const Avatar: React.FC<AvatarProps> = ({ teamName, size }) => {
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
    return (
        <div className={styles.avatarDiv}>
            <img
                className={styles.avatar + " w-5 md:w-7 lg:w-9"}
                src={user["avatarSrc"]}
                //onMouseOver={(e) => console.log(e)}
                //width={size}
                data-popover-target={"popover-user-profile" + teamName.replaceAll(" ", "")}
            />
            <div data-popover id={"popover-user-profile" + teamName.replaceAll(" ", "")} role="tooltip" className="absolute z-10 invisible inline-block w-64 text-sm text-gray-500 transition-opacity duration-300 bg-white border border-gray-200 rounded-lg shadow-sm opacity-0 dark:text-gray-400 dark:bg-gray-800 dark:border-gray-600">
                <div className="p-3">
                    <div className="flex items-center justify-between mb-2">
                        <a href="#">
                            <img className="w-24 h-24 rounded-full" src={user["avatarSrc"]} alt="Avatar pic" />
                        </a>
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
                    <ul className="flex text-sm">
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