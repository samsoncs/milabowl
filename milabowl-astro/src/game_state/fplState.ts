export interface FplResults{
    results: FplUserGameweekResult[];
}

export interface FplUserGameweekResult{
    gameWeek: number;
    teamName: string;
    lineup: FplPlayerResult[];
}

export interface FplPlayerResult{
    lastName: string;
    points: number;
    position: string;
    isCap: boolean;
    isViceCap: boolean;
    teamName: string;
    isBench: boolean;
}