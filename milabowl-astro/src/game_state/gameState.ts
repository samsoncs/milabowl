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

export interface Rule {
    name: string;
    shortName: string;
    description: string;
}

// export interface MilaRules {
//     rules: MilaRulePoints[];
// }

export interface MilaRulePoints {
    points: number;
    ruleShortName: string;
    ruleName: string;
    reasoning?: string | null;
    description: string;
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
    rules: Rule[];
}

export interface OverallResult {
    gwScore: number;
    teamName: string;
    userName: string;
    userId: number;
    gwPosition: number;
    gameWeek: number;
    cumulativeMilaPoints: number;
    cumulativeAverageMilaPoints: number;
    totalCumulativeAverageMilaPoints: number;
    milaRank: number;
    milaRankLastWeek: number;
    rules: MilaRulePoints[];
}
