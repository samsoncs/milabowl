export interface FplResults{
    results: FplUserGameweekResult[];
}

export interface FplUserGameweekResult{
    gameWeek: number;
    teamName: string;
    totalScore: number;
    lineup: FplPlayerResult[];
}

export interface FplPlayerResult{
    webName: string;
    points: number;
    position: string;
    isCap: boolean;
    isViceCap: boolean;
    teamName: string;
    isBench: boolean;
}