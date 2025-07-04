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
}

const OverviewTable: React.FC<OverviewTableProps> = ({
  data,
  avatars,
  teams
}) => {
  const [expanded, setExpanded] = useState<ExpandedState>({});

  const renderSubComponent = ({ row }: { row: { original: GameWeekResult } }) => {
    const teamData = teams?.find(t => t.teamName === row.original.teamName);
    
    return (
      <div className="p-4">

        <div className="mb-6 p-4 bg-white dark:bg-gray-700 rounded-lg border border-gray-200 dark:border-gray-600">
          <h5 className="font-medium text-gray-900 dark:text-gray-100 mb-3">
            Game Week {row.original.gameWeek} - Rules & Scores
          </h5>
          <div className="space-y-2 text-sm">

            {
                // This is using overall rules results, want to change to use from current gw
                row.original.rules.filter(r => r.points !== 0).map(r => <div key={r.ruleShortName}>
                    {r.ruleShortName} - {r.points} pts.
                    </div>)
            }

          </div>
        </div>

        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
          <div className="space-y-2">
            <h5 className="font-medium text-gray-700 dark:text-gray-300">Performance Trend (Last 10 GWs)</h5>
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
            <h5 className="font-medium text-gray-700 dark:text-gray-300">Recent History</h5>
            <div className="text-sm space-y-1 text-gray-600 dark:text-gray-400">
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
                className="flex items-center justify-center p-1 hover:bg-gray-100 dark:hover:bg-gray-700 rounded transition-colors"
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
