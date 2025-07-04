import { createColumnHelper, type RowData, type ExpandedState } from '@tanstack/react-table';
import type { GameWeekResult } from '../../game_state/gameState';
import SortableTable from '../../components/core/Table/SortableTable';
import { useMemo, useState } from 'react';
import PositionDelta from '../../components/core/PositionDelta';
import '@tanstack/react-table';
import TrendChart from './TrendChart';
import type { ResultsForTeams } from './types';

declare module '@tanstack/react-table' {
  // eslint-disable-next-line @typescript-eslint/no-unused-vars
  interface ColumnMeta<TData extends RowData, TValue> {
    align?: 'right' | 'left';
    classNames?: string;
  }
}

const columnHelper = createColumnHelper<GameWeekResult>();

interface OverviewTableProps {
  data: GameWeekResult[];
  teams: ResultsForTeams[];
  avatars: ImageMetadata[];
  currentGameWeekResults?: GameWeekResult[];
}

const OverviewTable: React.FC<OverviewTableProps> = ({
  data,
  avatars,
  teams,
  currentGameWeekResults
}) => {
  const [expanded, setExpanded] = useState<ExpandedState>({});

  const renderSubComponent = ({ row }: { row: { original: GameWeekResult } }) => {
    const teamData = teams?.find(t => t.teamName === row.original.teamName);
    
    // Find the current game week specific data for this team
    const currentGwTeamData = currentGameWeekResults?.find(
      result => result.teamName === row.original.teamName && result.gameWeek === row.original.gameWeek
    );
    
    return (
      <div className="p-2">
       
        
        <div className="mb-6 p-4 pt-3 bg-white dark:bg-slate-700 rounded-lg">
            <div className="font-medium text-slate-900 dark:text-slate-100 mb-3">
            Current GW
            </div>
          <div className="space-y-2 text-sm">
            {
              (currentGwTeamData?.rules || row.original.rules)
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
              <span>{currentGwTeamData?.gwScore || row.original.gwScore}</span>
            </div>
          </div>
        </div>

        <div className="grid grid-cols-2 md:grid-cols-2 lg:grid-cols-3 gap-4">
          <div className="space-y-2">
            <h5 className="font-medium text-slate-700 dark:text-slate-300">Performance Trend</h5>
            <div className="h-24">
              {teamData && (
                <TrendChart
                  results={teamData.results.slice(-10)}
                  teamName={row.original.teamName}
                  height={96}
                />
              )}
            </div>
          </div>
          <div className="space-y-2">
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

  const lastGameWeek = data[data.length - 1].gameWeek;
  const columns = useMemo(
    () => [
      columnHelper.display({
        id: 'milaRank',
        header: '#',
        cell: (props) => {
          const deltaPosition =
            props.row.original.milaRankLastWeek === null ||
            props.row.original.milaRankLastWeek === undefined
              ? 0
              : props.row.original.milaRankLastWeek -
                props.row.original.milaRank;

          return (
            <div className="flex flex-col items-center justify-between gap-1">
              <PositionDelta
                pos={props.row.original.milaRank}
                delta={deltaPosition}
              />
            </div>
          );
        },
      }),
      columnHelper.accessor('teamName', {
        id: 'teamName',
        header: 'Team',
        cell: (props) => (
          <span className="flex items-center gap-2">
            <img
              src={
                avatars.find((a) =>
                  a.src.includes(
                    props.row.original.teamName
                      .replace('$', 's')
                      .toLowerCase()
                      .replaceAll(' ', '_')
                  )
                )?.src
              }
              className="h-10 w-10 rounded-full sm:h-12 sm:w-12"
            />
            <a
              className="max-w-[130px] truncate underline sm:max-w-[300px]"
              href={`/fpl/players/${props.row.original.teamName.replaceAll(' ', '-')}/gw/${lastGameWeek}`}
            >
              {props.cell.getValue()}
            </a>
          </span>
        ),
        enableSorting: false,
      }),
      columnHelper.display({
        id: 'trend',
        header: 'Trend',
        cell: (props) => (
          <>
            <TrendChart
              height={30}
              teamName={props.row.original.teamName}
              results={teams
                .find((r) => r.teamName === props.row.original.teamName)!
                .results.slice(-5)}
            />
          </>
        ),
        meta: {
          classNames: 'hidden sm:table-cell lg:hidden xl:table-cell',
        },
      }),
      columnHelper.accessor('gwScore', {
        id: 'gwScore',
        header: 'GW',
        meta: {
          align: 'right',
        },
      }),
      columnHelper.accessor('cumulativeMilaPoints', {
        id: 'cumulativeMilaPoints',
        header: 'Total',
        cell: (props) => (
          <span className="font-bold text-indigo-900 dark:text-orange-200">
            {props.getValue()}
          </span>
        ),
        meta: {
          align: 'right',
          classNames: 'pl-0 md:pl-3',
        },
      }),
      columnHelper.display({
        id: 'expand',
        header: '',
        cell: ({ row }) => {
          return row.getCanExpand() ? (
            <div className="flex justify-end">
              <button
                onClick={row.getToggleExpandedHandler()}
                className="flex items-center justify-center p-1 hover:bg-slate-100 dark:hover:bg-slate-700 rounded transition-colors"
                aria-label={row.getIsExpanded() ? 'Collapse details' : 'Expand details'}
              >
                <svg
                  className={`w-4 h-4 transition-transform duration-200 ${
                    row.getIsExpanded() ? 'rotate-180' : ''
                  }`}
                  fill="none"
                  stroke="currentColor"
                  viewBox="0 0 24 24"
                >
                  <path
                    strokeLinecap="round"
                    strokeLinejoin="round"
                    strokeWidth={2}
                    d="M19 9l-7 7-7-7"
                  />
                </svg>
              </button>
            </div>
          ) : null;
        },
        meta: {
          align: 'right',
          classNames: 'w-12',
        },
      }),
    ],
    [teams, avatars, lastGameWeek]
  );

  return (
    <SortableTable
      data={data}
      columns={columns}
      initialSort={[{ id: 'cumulativeMilaPoints', desc: true }]}
      expandedState={expanded}
      onExpandedChange={setExpanded}
      getRowCanExpand={() => true}
      renderSubComponent={renderSubComponent}
    />
  );
};

export default OverviewTable;
