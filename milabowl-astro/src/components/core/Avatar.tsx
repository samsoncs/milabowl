import styles from "./Avatar.module.css";
import party from "party-js";

export interface AvatarProps {
    team_name: string;
    avatar_size: string;
}

function avatar_mapping(team_name: string): string {
    team_name = team_name.toLowerCase();
    //.replaceAll(" ", "")
    //.replaceAll("'", "")
    //.replaceAll("’", "");
    let path = "avatars_2022_23/";
    if (team_name.includes("boris")) {
        return path + "avatar_henrik2.png";
    } else if (team_name.includes("dollar")) {
        return path + "avatar_mikkel.png";
    } else if (team_name.includes("veni")) {
        return path + "avatar_anders.png";
    } else if (team_name.includes("haalandaise")) {
        return path + "avatar_eivind.png";
    } else if (team_name.includes("maguire")) {
        return path + "avatar_malte.png";
    } else if (team_name.includes("premier")) {
        return path + "avatar_markus.png";
    } else if (team_name.includes("filip")) {
        return path + "avatar_martin.png";
    } else if (team_name.includes("haalandslaget")) {
        return path + "avatar_sam2.png";
    } else if (team_name.includes("emila")) {
        return path + "avatar_simen.png";
    }
    return "https://www.nicepng.com/png/detail/186-1869910_ic-question-mark-roblox-question-mark-avatar.png";
}

interface UserProfileInfo {
    info: string | null;
    followers: number | null;
    following: number | null;
    name: string;
    avatar_src: string;
}

interface UserProfiles {
    [key: string]: UserProfileInfo
}

const image_root = "avatars_2022_23/";
const user_profiles: UserProfiles = {
    "Haalandslaget": {
        "info": "Mila-source founder and evangelist",
        "followers": 42,
        "following": 69,
        "name": "sam",
        "avatar_src": ""
    },
    "Milaion dollar squad": {
        "info": "Mila-source contributor",
        "followers": 0,
        "following": 0,
        "name": "mikkel",
        "avatar_src": ""
    },
    "Haalandaise Saus": {
        "info": "Mila try-hard",
        "followers": 0,
        "following": 0,
        "name": "eivind",
        "avatar_src": ""
    },
    "WeDidn'tStartMaguire": {
        "info": "",
        "followers": 0,
        "following": 0,
        "name": "malte",
        "avatar_src": ""
    },
    "eMILA Smith Rowe": {
        "info": "",
        "followers": 0,
        "following": 0,
        "name": "simen",
        "avatar_src": ""
    },
    "Veni Vidi Vici Mila": {
        "info": "",
        "followers": 0,
        "following": 0,
        "name": "anders",
        "avatar_src": ""
    },
    "Premier Lag": {
        "info": "",
        "followers": 0,
        "following": 0,
        "name": "markus",
        "avatar_src": ""
    },
    "Filip Coutinho": {
        "info": "",
        "followers": 0,
        "following": 0,
        "name": "martin",
        "avatar_src": ""
    },
    "Boris’ party boys": {
        "info": "",
        "followers": 0,
        "following": 0,
        "name": "henrik",
        "avatar_src": ""
    }

};

const Avatar: React.FC<AvatarProps> = ({ team_name, avatar_size }) => {
    let user: UserProfileInfo = {
        "info": "Nobody has no name",
        "name": "John Doe",
        "followers": -1,
        "following": -1,
        "avatar_src": "https://www.nicepng.com/png/detail/186-1869910_ic-question-mark-roblox-question-mark-avatar.png"
    };
    if (team_name in user_profiles) {
        user["info"] = user_profiles[team_name]["info"];
        user["name"] = user_profiles[team_name]["name"];
        user["followers"] = user_profiles[team_name]["followers"];
        user["following"] = user_profiles[team_name]["following"];
        user["avatar_src"] = image_root + "avatar_" + user_profiles[team_name]["name"] + ".png";
    }
    return (
        <div className={styles.avatarDiv}>
            <img
                className={styles.avatar}
                src={user["avatar_src"]}
                //onMouseOver={(e) => console.log(e)}
                width={avatar_size}
                data-popover-target={"popover-user-profile" + team_name.replaceAll(" ", "")}
            />
            <div data-popover id={"popover-user-profile" + team_name.replaceAll(" ", "")} role="tooltip" className="absolute z-10 invisible inline-block w-64 text-sm text-gray-500 transition-opacity duration-300 bg-white border border-gray-200 rounded-lg shadow-sm opacity-0 dark:text-gray-400 dark:bg-gray-800 dark:border-gray-600">
                <div className="p-3">
                    <div className="flex items-center justify-between mb-2">
                        <a href="#">
                            <img className="w-20 h-20 rounded-full" src={user["avatar_src"]} alt="Jese Leos" />
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
                        <a href="#">{team_name}</a>
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