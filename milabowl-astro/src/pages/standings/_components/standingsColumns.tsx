import { createColumnHelper, type ColumnDef } from '@tanstack/react-table';
import type { GameWeekResult } from '../../../game_state/gameState';
import PositionDelta from '../../../components/core/PositionDelta';
import type { OptimizedImage } from './types';

const columnHelper = createColumnHelper<GameWeekResult>();

export function getStandingsColmns(
  data: GameWeekResult[],
  avatars: OptimizedImage[],
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
      cell: (props) => {
        const optimizedImage = avatars.find((a) =>
          a.src.includes(
            props.row.original.userName.toLowerCase().replaceAll(' ', '_')
          )
        )!;
        return (
          <span className="flex items-center gap-2">
            <picture className="h-12 w-12 rounded-full sm:h-12 sm:w-12">
              <source
                srcSet={optimizedImage.avif.join(', ')}
                sizes={optimizedImage.sizes}
                type="image/avif"
              />
              <source
                srcSet={optimizedImage.webp.join(', ')}
                sizes={optimizedImage.sizes}
                type="image/webp"
              />
              <img
                src={optimizedImage.src}
                className={`rounded-full rank-${props.row.original.milaRank}-avatar`}
                alt={`${props.row.original.teamName} avatar`}
              />
            </picture>
            <a
              className="max-w-[130px] truncate underline sm:max-w-[300px]"
              href={`/fpl/players/${props.row.original.teamName.replaceAll(' ', '-')}/gw/${lastGameWeek}`}
            >
              {props.cell.getValue()}
            </a>
          </span>
        );
      },
      enableSorting: false,
      size: 230,
      maxSize: 400,
    }),
    ...data[0].rules.map((r, i) =>
      columnHelper.accessor((r) => r.rules[i].points, {
        id: r.ruleShortName,
        size: 70,
      })
    ),
    columnHelper.accessor('cumulativeMilaPoints', {
      id: 'totmp',
      header: 'Total MP',
      size: 120,
    }),
  ];
}
