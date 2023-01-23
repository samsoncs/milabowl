import { useState } from "react";
import styles from "./Award.module.css";
import Particles from "react-tsparticles";
import type { Engine } from "tsparticles-engine";
import { useCallback } from "react";
import { loadConfettiPreset } from "tsparticles-preset-confetti";

export interface AwardProps {
    title: string;
    firstPlace: string;
    seccondPlace: string;
    thirdPlace: string;
}

const particleOptions = {
    preset: "confetti",
    particles:{
        color: {
            value: ["#BD10E0", "#B8E986", "#50E3C2", "#FFD300", "#E86363"],
        },
    },
};

interface TrophyProps{
    fill: string;
    stroke: string;
    h?: string;
    w?: string;
}

const Trophy: React.FC<TrophyProps> = ({fill, stroke, h, w}) => (
    <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor" className={`${h} ${w} ${stroke} ${fill}`}>
        <path fillRule="evenodd" d="M5.166 2.621v.858c-1.035.148-2.059.33-3.071.543a.75.75 0 00-.584.859 6.753 6.753 0 006.138 5.6 6.73 6.73 0 002.743 1.346A6.707 6.707 0 019.279 15H8.54c-1.036 0-1.875.84-1.875 1.875V19.5h-.75a2.25 2.25 0 00-2.25 2.25c0 .414.336.75.75.75h15a.75.75 0 00.75-.75 2.25 2.25 0 00-2.25-2.25h-.75v-2.625c0-1.036-.84-1.875-1.875-1.875h-.739a6.706 6.706 0 01-1.112-3.173 6.73 6.73 0 002.743-1.347 6.753 6.753 0 006.139-5.6.75.75 0 00-.585-.858 47.077 47.077 0 00-3.07-.543V2.62a.75.75 0 00-.658-.744 49.22 49.22 0 00-6.093-.377c-2.063 0-4.096.128-6.093.377a.75.75 0 00-.657.744zm0 2.629c0 1.196.312 2.32.857 3.294A5.266 5.266 0 013.16 5.337a45.6 45.6 0 012.006-.343v.256zm13.5 0v-.256c.674.1 1.343.214 2.006.343a5.265 5.265 0 01-2.863 3.207 6.72 6.72 0 00.857-3.294z" clipRule="evenodd" />
    </svg>  
);

const Award: React.FC<AwardProps> = ({ title, firstPlace, seccondPlace, thirdPlace }) => {
    const [isClicked, setIsClicked] = useState(false);
    const particlesInit = useCallback(async (engine: Engine) => {
        await loadConfettiPreset(engine);
    }, []);

    return(
        <div>
            <div className="text-center text-xl font-bold mb-4">
                {title}
            </div>
            <div className={["h-64", "w-64", styles.flipCard, isClicked ? styles.flipped : ""].join(" ")} onClick={() => setIsClicked(!isClicked)}>
                <div className={styles.flipCardInner}>
                    <div className={["flex items-center justify-center text-white text-9xl bg-indigo-900", styles.flipCardFront].join(" ") }>
                        ?
                    </div>
                    <div className={["flex", "items-center", "justify-center", "bg-slate-100", , styles.flipCardBack].join(" ")}>
                        <div className="flex flex-col items-center w-full gap-5">
                           <div className="text-xl">
                                <div className="flex items-center gap-2">
                                    <span className="font-bold">Winner:</span> {firstPlace}
                                    <Trophy h="h-8" w="h-8" fill="fill-yellow-100" stroke="stroke-yellow-500"/>
                                </div>                                    
                           </div>
                           <div className="space-y-1">
                                <div className="flex gap-10">
                                        <div className="flex items-center gap-2"> 
                                            <span className="font-bold">1 runner up:</span> {seccondPlace}
                                            <Trophy h="h-5" w="h-5" fill="fill-zinc-100" stroke="stroke-zinc-500"/>
                                        </div>                               
                                </div>
                                <div className="flex gap-10">
                                        <div className="flex items-center gap-2">
                                            <span className="font-bold">2 runner up:</span> {thirdPlace}
                                            <Trophy h="h-5" w="h-5" fill="fill-amber-100" stroke="stroke-amber-600"/>
                                        </div>
                                </div>
                            </div>
                        </div>                         
                    </div>
                </div>
                {
                    isClicked && (
                        <Particles 
                            id="tsparticles" 
                            init={particlesInit} 
                            options={particleOptions}
                        />
                    )
                }            
            </div>
        </div>
    )
};

export default Award;