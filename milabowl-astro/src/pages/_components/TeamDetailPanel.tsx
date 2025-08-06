import type { GameWeekResult } from '../../game_state/gameState';

interface TeamDetailPanelProps {
  row: GameWeekResult;
  currentGameWeekResults?: GameWeekResult[];
}

const TeamDetailPanel: React.FC<TeamDetailPanelProps> = ({
  row,
  currentGameWeekResults,
}) => {
  const currentGwTeamData = currentGameWeekResults?.find(
    (result) =>
      result.teamName === row.teamName && result.gameWeek === row.gameWeek
  );

  return (
    <div className="p-2">
      <div className="mb-2 rounded-xl bg-slate-200 p-4 text-slate-600 shadow-sm dark:bg-slate-800 dark:text-slate-300">
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
                  className={`flex items-center justify-between py-1 ${
                    !isLast
                      ? 'border-b border-slate-200 dark:border-slate-600'
                      : ''
                  }`}
                >
                  <span className="text-sm font-medium">{r.ruleShortName}</span>

                  <div className="flex items-center gap-2">
                    <div
                      className={`rounded-full px-2 py-1 font-mono text-xs font-semibold ${
                        isPositive
                          ? 'bg-green-100 text-green-600 dark:bg-green-900/60 dark:text-green-200'
                          : isNegative
                            ? 'bg-red-100 text-red-600 dark:bg-red-900/60 dark:text-red-200'
                            : 'bg-gray-100 dark:bg-gray-700'
                      }`}
                    >
                      {isPositive && '+'}
                      {r.points.toFixed(2)} pts
                    </div>
                  </div>
                </div>
              );
            })}

          <div className="mt-3 border-t border-slate-200 pt-3 dark:border-slate-600">
            <div className="flex items-center justify-between">
              <span className="font-semibold">GW Score</span>
              <div className="px-2 font-mono font-bold">
                {currentGwTeamData?.gwScore?.toFixed(2) || row.gwScore} pts
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default TeamDetailPanel;
