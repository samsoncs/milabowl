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
  currentGameWeekResults,
}) => {
  const teamData = teams?.find((t) => t.teamName === row.teamName);

  const currentGwTeamData = currentGameWeekResults?.find(
    (result) =>
      result.teamName === row.teamName && result.gameWeek === row.gameWeek
  );

  return (
    <div className="p-2">
      <div className="mb-2 rounded-xl bg-slate-200 p-4 text-slate-600 shadow-sm dark:bg-slate-700 dark:text-slate-300">
        <div className="mb-3">
          <h4 className="font-semibold">Current GW</h4>
        </div>

        <div className="space-y-0">
          {(currentGwTeamData?.rules || row.rules)
            .filter((r) => r.points !== 0)
            .map((r, index, filteredRules) => {
              const isPositive = r.points > 0;
              const isNegative = r.points < 0;
              const isLast = index === filteredRules.length - 1;

              return (
                <div
                  key={r.ruleShortName}
                  className={`flex items-center justify-between py-2 ${
                    !isLast
                      ? 'border-b border-slate-200 dark:border-slate-600'
                      : ''
                  }`}
                >
                  <span className="text-sm font-medium">{r.ruleShortName}</span>

                  <div className="flex items-center gap-2">
                    <div
                      className={`rounded-full px-2 py-1 text-xs font-bold ${
                        isPositive
                          ? 'bg-green-100 text-green-700 dark:bg-green-900 dark:text-green-200'
                          : isNegative
                            ? 'bg-red-100 text-red-700 dark:bg-red-900 dark:text-red-200'
                            : 'bg-gray-100 dark:bg-gray-700'
                      }`}
                    >
                      {isPositive && '+'}
                      {r.points} pts
                    </div>
                  </div>
                </div>
              );
            })}

          <div className="mt-3 border-t border-slate-200 pt-3 dark:border-slate-600">
            <div className="flex items-center justify-between">
              <span className="text-sm font-bold">Total Score</span>
              <div className="text-sm font-bold">
                {currentGwTeamData?.gwScore || row.gwScore} pts
              </div>
            </div>
          </div>
        </div>
      </div>

      <h5 className="font-medium text-slate-700 dark:text-slate-300">
        History
      </h5>

      <div className="grid grid-cols-2 gap-4 md:grid-cols-2 lg:grid-cols-3">
        <div className="space-y-2 transition-transform duration-200 ease-in-out hover:scale-[1.02]">
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
          <div className="space-y-1 text-sm text-slate-600 dark:text-slate-400">
            {teamData?.results.slice(-3).map((result) => (
              <div key={result.gameWeek} className="flex justify-between">
                <span>GW {result.gameWeek}:</span>
                <span className="font-medium">
                  #{result.milaRank} ({result.cumulativeAverageMilaPoints} pts)
                </span>
              </div>
            ))}
          </div>
        </div>
      </div>
    </div>
  );
};

export default TeamDetailPanel;
