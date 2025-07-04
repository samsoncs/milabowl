import { createColumnHelper, type RowData } from '@tanstack/react-table';
import type { GameWeekResult } from '../../game_state/gameState';
import SortableTable from '../../components/core/Table/SortableTable';
import { useMemo } from 'react';
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
    ],
    []
  );

  return (
    <SortableTable
      data={data}
      columns={columns}
      initialSort={[{ id: 'cumulativeMilaPoints', desc: true }]}
    />
  );
};

export default OverviewTable;
