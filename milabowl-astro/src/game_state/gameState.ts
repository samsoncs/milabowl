export interface GameWeekResult {
    milaPoints: MilaRulePoints;
    gw: string | null;
    gwScore: number;
    teamName: string;
    userName: string;
    gwPosition: number;
    gameWeek: number;
    cumulativeMilaPoints: number;
    cumulativeAverageMilaPoints: number;
    totalCumulativeAverageMilaPoints: number;
    milaRank: number;
    milaRankLastWeek?: number | null;
  }
  
  export interface MilaRulePoints {
    capFail: number;
    benchFail: number;
    capKeep: number;
    capDef: number;
    gW69: number;
    redCard: number;
    yellowCard: number;
    minusIsPlus: number;
    increaseStreak: number;
    equalStreak: number;
    gwPositionScore: number;
    headToHeadMeta: number;
    sixtyNineSub: number;
    uniqueCap: number;
    trendyBitch: number;
    total: number;
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
  