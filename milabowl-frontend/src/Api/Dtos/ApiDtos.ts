export interface GameWeekResult {
  gw: string;
  teamName: string;
  userName: string;
  milaPoints: MilaRulePoints;
  gwPosition: number;
  gwScore: number;
  gameWeek: number;
  cumulativeMilaPoints: number;
  cumulativeAverageMilaPoints: number;
  totalCumulativeAverageMilaPoints: number;
  milaRank: number;
  milaRankLastWeek?: number;
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
  hit: number;
  gwPositionScore: number;
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
