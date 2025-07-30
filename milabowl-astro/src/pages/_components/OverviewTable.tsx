import {
  createColumnHelper,
  type RowData,
  type ExpandedState,
} from '@tanstack/react-table';
import type { GameWeekResult } from '../../game_state/gameState';
import SortableTable from '../../components/core/Table/SortableTable';
import { useMemo, useState } from 'react';
import PositionDelta from '../../components/core/PositionDelta';
import '@tanstack/react-table';
import TrendChart from './TrendChart';
import type { ResultsForTeams } from './types';
import TeamDetailPanel from './TeamDetailPanel';
import './style.css';

declare module '@tanstack/react-table' {
  // eslint-disable-next-line @typescript-eslint/no-unused-vars
  interface ColumnMeta<TData extends RowData, TValue> {
    align?: 'right' | 'left';
    classNames?: string;
  }
}

const columnHelper = createColumnHelper<GameWeekResult>();

type OptimizedImage = {
  src: string;
  src40Avif: string;
  src60Avif: string;
  src40Webp: string;
  src60Webp: string;
  srcSetAvif: string;
  srcSetWebp: string;
  sizes: string;
}

interface OverviewTableProps {
  data: GameWeekResult[];
  teams: ResultsForTeams[];
  avatars: OptimizedImage[];
  currentGameWeekResults?: GameWeekResult[];
}

const OverviewTable: React.FC<OverviewTableProps> = ({
  data,
  avatars,
  teams,
  currentGameWeekResults,
}) => {
  const [expanded, setExpanded] = useState<ExpandedState>({});

  const renderSubComponent = ({
    row,
  }: {
    row: { original: GameWeekResult };
  }) => (
    <TeamDetailPanel
      row={row.original}
      teams={teams}
      currentGameWeekResults={currentGameWeekResults}
    />
  );

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
        cell: (props) => {
          const optimizedImage = avatars.find((a) =>
            a.src.includes(
              props.row.original.teamName
                .replace('$', 's')
                .toLowerCase()
                .replaceAll(' ', '_')
            )
          )!;
          return(
          <span
            className={`flex items-center gap-2 rounded-l-full rank-${props.row.original.milaRank}`}
          >
            <picture className="h-10 w-10 sm:h-12 sm:w-12 rounded-full">
              <source
                srcSet={optimizedImage.srcSetAvif}
                sizes={optimizedImage.sizes}
                type="image/avif"
              />
              <source
                srcSet={optimizedImage.srcSetWebp}
                sizes={optimizedImage.sizes}
                type="image/webp"
              />
              <img
                src={optimizedImage.src40Webp}
                className="rounded-full"
                alt={`${props.row.original.teamName} avatar`}
              />
            </picture>
            <a
              className={`max-w-[75px] truncate transition-all duration-200 hover:underline sm:max-w-[250px] ${props.row.original.milaRank < 4 ? 'font-bold' : ''}`}
              href={`/fpl/players/${props.row.original.teamName.replaceAll(' ', '-')}/gw/${lastGameWeek}`}
            >
              {props.cell.getValue()}
            </a>
          </span>
        )},
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
                className="flex items-center justify-center rounded p-1 transition-colors hover:bg-slate-100 dark:hover:bg-slate-700"
                aria-label={
                  row.getIsExpanded() ? 'Collapse details' : 'Expand details'
                }
              >
                <svg
                  className={`h-4 w-4 transition-transform duration-200 ${
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
