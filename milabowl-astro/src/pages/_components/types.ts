export interface TeamResultData {
  gameWeek: number;
  milaRank: number;
  cumulativeAverageMilaPoints: number;
  totalCumulativeAverageMilaPoints: number;
}

export interface ResultsForTeams {
  teamName: string;
  results: TeamResultData[];
}
