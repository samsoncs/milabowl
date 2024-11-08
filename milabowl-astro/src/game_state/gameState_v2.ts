export interface GameWeekResult {
    gw: string;
    gameWeek: number;
    gwPosition: number;
    gwScore: number;
    milaRank: number;
    milaRankLastWeek?: number | null;
    teamName: string;
    userName: string;
    userId: number;
    cumulativeMilaPoints: number;
    cumulativeAverageMilaPoints: number;
    totalCumulativeAverageMilaPoints: number;

    rules: MilaRulePoints[];
}

// export interface MilaRules {
//     rules: MilaRulePoints[];
// }

export interface MilaRulePoints {
    points: number;
    ruleShortName: string;
}

export interface ResultsByWeek {
    results: GameWeekResult[];
    gameWeek: number;
}

export interface ResultsByUser {
    results: GameWeekResult[];
    teamName: string;
}

export interface MilaResultsDTO {
    resultsByWeek: ResultsByWeek[];
    resultsByUser: ResultsByUser[];
    overallScore: GameWeekResult[];
}
