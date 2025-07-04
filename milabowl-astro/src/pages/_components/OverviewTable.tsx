import { createColumnHelper, type RowData } from '@tanstack/react-table';
import type { GameWeekResult } from '../../game_state/gameState';
import type { ResultsForTeams } from './types';
import SortableTable from '../../components/core/Table/SortableTable';
import { useMemo } from 'react';
import PositionDelta from '../../components/core/PositionDelta';
import TrendChart from './TrendChart';
import '@tanstack/react-table';

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
  teams,
  avatars,
}) => {
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
              : props.row.original.milaRankLastWeek - props.row.original.milaRank;

          return (
            <div className="flex flex-col items-center justify-between gap-1">
              <PositionDelta
                pos={props.row.original.milaRank}
                delta={deltaPosition}
              />
            </div>
          );
        },
        meta: {
          align: 'right',
        },
      }),
      columnHelper.display({
        id: 'teamName',
        header: 'Team',
        cell: (props) => {
          const avatar = avatars.find((a) =>
            a.src.includes(
              props.row.original.teamName.toLowerCase().replace(/\s/g, '_')
            )
          );
          return (
            <div className="flex items-center space-x-3">
              {avatar && (
                <img
                  src={avatar.src}
                  alt={props.row.original.teamName}
                  className="h-8 w-8 rounded-full object-cover"
                />
              )}
              <div className="max-w-[10rem] truncate text-sm font-medium">
                {props.row.original.teamName}
              </div>
            </div>
          );
        },
      }),
      columnHelper.accessor('cumulativeAverageMilaPoints', {
        header: 'Pts',
        cell: (props) => (
          <div className="text-right text-sm font-semibold">
            {props.getValue()}
          </div>
        ),
        meta: {
          align: 'right',
        },
      }),
      columnHelper.display({
        id: 'trend',
        header: 'Trend',
        cell: (props) => {
          const teamResults = teams?.find(
            (t) => t.teamName === props.row.original.teamName
          )?.results;

          const filteredResults = teamResults?.slice(-5) ?? [];

          return (
            <TrendChart
              results={filteredResults}
              teamName={props.row.original.teamName}
              height={40}
            />
          );
        },
        meta: {
          classNames: 'w-32',
        },
      }),
    ],
    [avatars, teams]
  );

  return (
    <SortableTable
      data={data}
      columns={columns}
      initialColumnPinnings={['milaRank', 'teamName']}
    />
  );
};

export default OverviewTable;