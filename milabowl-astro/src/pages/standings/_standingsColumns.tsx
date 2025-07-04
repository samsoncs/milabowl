import { createColumnHelper, type ColumnDef } from '@tanstack/react-table';
import type { GameWeekResult } from '../../game_state/gameState';
import PositionDelta from '../../components/core/PositionDelta';

const columnHelper = createColumnHelper<GameWeekResult>();

export function getStandingsColmns(
  data: GameWeekResult[],
  avatars: ImageMetadata[],
  lastGameWeek: number
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
): ColumnDef<GameWeekResult, any>[] {
  return [
    columnHelper.display({
      id: 'rank',
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
      size: 40,
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
            className="h-9 w-9 rounded-full"
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
      size: 230,
      maxSize: 400,
    }),
    ...data[0].rules.map((r, i) =>
      columnHelper.accessor((r) => r.rules[i].points, {
        id: r.ruleShortName,
      })
    ),
    columnHelper.accessor('cumulativeMilaPoints', {
      id: 'totmp',
      header: 'Total MP',
      size: 120,
    }),
  ];
}
