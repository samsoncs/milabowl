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
      <div className="text-slate-600 dark:text-slate-300 mb-2 p-4 bg-slate-200 dark:bg-slate-700 rounded-xl shadow-sm">
        <div className="mb-3">
          <h4 className="font-semibold">
            Current GW
          </h4>
        </div>
        
        <div className="space-y-0">
          {
            (currentGwTeamData?.rules || row.rules)
              .filter(r => r.points !== 0)
              .map((r, index, filteredRules) => {
                const isPositive = r.points > 0;
                const isNegative = r.points < 0;
                const isLast = index === filteredRules.length - 1;
                
                return (
                  <div 
                    key={r.ruleShortName} 
                    className={`flex justify-between items-center py-2 ${
                      !isLast ? 'border-b border-slate-200 dark:border-slate-600' : ''
                    }`}
                  >
                    <span className="text-sm font-medium">{r.ruleShortName}</span>
                    
                    <div className="flex items-center gap-2">
                      <div className={`px-2 py-1 rounded-full text-xs font-bold ${
                        isPositive ? 'bg-green-100 dark:bg-green-900 text-green-700 dark:text-green-200' :
                        isNegative ? 'bg-red-100 dark:bg-red-900 text-red-700 dark:text-red-200' :
                        'bg-gray-100 dark:bg-gray-700'
                      }`}>
                        {isPositive && '+'}{r.points} pts
                      </div>
                    </div>
                  </div>
                );
              })
          }
          
          <div className="mt-3 pt-3 border-t border-slate-200 dark:border-slate-600">
            <div className="flex justify-between items-center">
              <span className="text-sm font-bold">Total Score</span>
              <div className="text-sm font-bold">
                {currentGwTeamData?.gwScore || row.gwScore} pts
              </div>
            </div>
          </div>
        </div>
      </div>

        <h5 className="font-medium text-slate-700 dark:text-slate-300">History</h5>

      <div className="grid grid-cols-2 md:grid-cols-2 lg:grid-cols-3 gap-4">
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
