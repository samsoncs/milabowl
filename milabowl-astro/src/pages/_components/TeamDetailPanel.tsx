import type { GameWeekResult } from '../../game_state/gameState';
import type { ResultsForTeams } from './types';
import TrendChart from './TrendChart';

interface TeamDetailPanelProps {
  row: GameWeekResult;
  teams?: ResultsForTeams[];
  currentGameWeekResults?: GameWeekResult[];
}

const TeamDetailPanel: React.FC<TeamDetailPanelProps> = ({
  row,
  teams,
  currentGameWeekResults
}) => {
  const teamData = teams?.find(t => t.teamName === row.teamName);
  
  const currentGwTeamData = currentGameWeekResults?.find(
    result => result.teamName === row.teamName && result.gameWeek === row.gameWeek
  );

  return (
    <div className="p-2">
      <div className="mb-6 p-4 pt-3 bg-white dark:bg-slate-700 rounded-lg transition-all duration-200 ease-in-out hover:shadow-md">
        <div className="font-medium text-slate-900 dark:text-slate-100 mb-3">
          Current GW
        </div>
        <div className="space-y-2 text-sm">
          {
            (currentGwTeamData?.rules || row.rules)
              .filter(r => r.points !== 0)
              .map(r => (
                <div key={r.ruleShortName} className="flex justify-between items-center py-1 border-b border-slate-100 dark:border-slate-600">
                  <span className="text-slate-700 dark:text-slate-300">{r.ruleShortName}</span>
                  <span className="font-semibold text-slate-900 dark:text-slate-100">{r.points} pts</span>
                </div>
              ))
          }
          <div className="flex justify-between items-center py-1 font-medium text-slate-900 dark:text-slate-100 pt-2">
            <span>Total</span>
            <span>{currentGwTeamData?.gwScore || row.gwScore}</span>
          </div>
        </div>
      </div>

      <div className="grid grid-cols-2 md:grid-cols-2 lg:grid-cols-3 gap-4">
        <div className="space-y-2 transition-transform duration-200 ease-in-out hover:scale-[1.02]">
          <h5 className="font-medium text-slate-700 dark:text-slate-300">Performance Trend</h5>
          <div className="h-24">
            {teamData && (
              <TrendChart
                results={teamData.results.slice(-10)}
                teamName={row.teamName}
                height={96}
              />
            )}
          </div>
        </div>
        <div className="space-y-2 transition-transform duration-200 ease-in-out hover:scale-[1.02]">
          <h5 className="font-medium text-slate-700 dark:text-slate-300">Recent History</h5>
          <div className="text-sm space-y-1 text-slate-600 dark:text-slate-400">
            {teamData?.results.slice(-3).map((result) => (
              <div key={result.gameWeek} className="flex justify-between">
                <span>GW {result.gameWeek}:</span>
                <span className="font-medium">#{result.milaRank} ({result.cumulativeAverageMilaPoints} pts)</span>
              </div>
            ))}
          </div>
        </div>
      </div>
    </div>
  );
};

export default TeamDetailPanel;
