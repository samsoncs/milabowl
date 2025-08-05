import PositionDelta from '../../components/core/PositionDelta';
import type { GameWeekResult } from '../../game_state/gameState';
import './style.css';
import { useMemo, useState, type MouseEventHandler } from 'react';
import TeamDetailPanel from './TeamDetailPanel';

type OptimizedImage = {
  src: string;
  avif: string[];
  webp: string[];
  sizes: string;
};

interface OverviewTable2Props {
  data: GameWeekResult[];
  avatars: OptimizedImage[];
  currentGameWeekResults: GameWeekResult[] | undefined;
}

interface SortToggleProps {
  sortDirection?: 'asc' | 'desc';
  onClick?: MouseEventHandler<HTMLButtonElement>;
}

function useSortedTable<T>(
  data: T[],
  initialProp: keyof T,
  initialOrder: 'asc' | 'desc' = 'desc'
) {
  const [sortOrder, setSortOrder] = useState<'asc' | 'desc'>(initialOrder);
  const [sortProp, setSortProp] = useState<keyof T>(initialProp);

  const handleSort = (prop: keyof T) => {
    if (sortProp === prop) {
      setSortOrder(sortOrder === 'asc' ? 'desc' : 'asc');
    } else {
      setSortProp(prop);
      setSortOrder('desc');
    }
  };

  const sortedData: T[] = useMemo(() => {
    return [...data].sort((a, b) => {
      const aVal = a[sortProp];
      const bVal = b[sortProp];
      if (typeof aVal === 'number' && typeof bVal === 'number') {
        return sortOrder === 'asc' ? aVal - bVal : bVal - aVal;
      }
      if (typeof aVal === 'string' && typeof bVal === 'string') {
        return sortOrder === 'asc'
          ? aVal.localeCompare(bVal)
          : bVal.localeCompare(aVal);
      }
      return 0;
    });
  }, [data, sortProp, sortOrder]);

  return { sortedData, sortOrder, sortProp, handleSort };
}

const SortToggle = ({ sortDirection, onClick }: SortToggleProps) => (
  <>
    <button onClick={onClick}>
      {sortDirection === 'asc' && (
        <svg
          xmlns="http://www.w3.org/2000/svg"
          fill="none"
          viewBox="0 0 24 24"
          strokeWidth="1.5"
          stroke="currentColor"
          className="h-5 w-5"
        >
          <path
            strokeLinecap="round"
            strokeLinejoin="round"
            d="M3 4.5h14.25M3 9h9.75M3 13.5h5.25m5.25-.75L17.25 9m0 0L21 12.75M17.25 9v12"
          />
        </svg>
      )}
      {sortDirection === 'desc' && (
        <svg
          xmlns="http://www.w3.org/2000/svg"
          fill="none"
          viewBox="0 0 24 24"
          strokeWidth="1.5"
          stroke="currentColor"
          className="h-5 w-5"
        >
          <path
            strokeLinecap="round"
            strokeLinejoin="round"
            d="M3 4.5h14.25M3 9h9.75M3 13.5h9.75m4.5-4.5v12m0 0-3.75-3.75M17.25 21 21 17.25"
          />
        </svg>
      )}
      {!sortDirection && (
        <svg
          xmlns="http://www.w3.org/2000/svg"
          fill="none"
          viewBox="0 0 24 24"
          strokeWidth="1.5"
          stroke="currentColor"
          className="h-5 w-5 text-slate-400 dark:text-slate-500"
        >
          <path
            strokeLinecap="round"
            strokeLinejoin="round"
            d="M3 7.5 7.5 3m0 0L12 7.5M7.5 3v13.5m13.5 0L16.5 21m0 0L12 16.5m4.5 4.5V7.5"
          />
        </svg>
      )}
    </button>
  </>
);

const Chevron = () => (
  <svg
    className={`h-4 w-4 transition-transform duration-200`}
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
);

const OverviewTable = ({
  data,
  avatars,
  currentGameWeekResults,
}: OverviewTable2Props) => {
  const { sortedData, sortOrder, sortProp, handleSort } =
    useSortedTable<GameWeekResult>(data, 'cumulativeMilaPoints', 'desc');
  const [expandedTeam, setExpandedTeam] = useState<string | null>(null);

  return (
    <div className="text-sm">
      <div className="flex gap-3 pb-2 pl-2 font-bold">
        <div className="w-4">#</div>
        <div className="flex-grow">Team</div>
        <div className="flex gap-1">
          GW
          <SortToggle
            sortDirection={sortProp === 'gwScore' ? sortOrder : undefined}
            onClick={() => handleSort('gwScore')}
          />
        </div>
        <div className="flex gap-1">
          Total
          <SortToggle
            sortDirection={
              sortProp === 'cumulativeMilaPoints' ? sortOrder : undefined
            }
            onClick={() => handleSort('cumulativeMilaPoints')}
          />
        </div>
        <div className="w-4" />
      </div>
      {sortedData.map((result) => {
        const optimizedImage = avatars.find((a) =>
          a.src.includes(
            result.teamName.replace('$', 's').toLowerCase().replaceAll(' ', '_')
          )
        )!;

        const deltaPosition =
          result.milaRankLastWeek === null ||
          result.milaRankLastWeek === undefined
            ? 0
            : result.milaRankLastWeek - result.milaRank;

        const isExpanded = expandedTeam === result.teamName;

        return (
          <div className="flex flex-col" key={result.teamName}>
            <div
              className={`flex gap-2 overflow-hidden p-[0.4rem] pr-0 ${!isExpanded ? 'border-b border-slate-200 dark:border-slate-700' : ''}`}
            >
              <div className="w-4">
                <PositionDelta pos={result.milaRank} delta={deltaPosition} />
              </div>

              <div
                className={`flex h-12 flex-grow items-center rounded-l-full sm:h-12 rank-${result.milaRank}`}
              >
                <div>
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
                      className={`rounded-full rank-${result.milaRank}-avatar`}
                      alt={`${result.teamName} avatar`}
                    />
                  </picture>
                </div>
                <div className="flex-grow p-2">
                  <div className={result.milaRank < 4 ? 'font-bold' : ''}>
                    {result.teamName}
                  </div>
                  <div className="flex justify-end gap-4">
                    <div>{result.gwScore}</div>
                    <div className="font-bold text-indigo-900 dark:text-orange-200">
                      {result.cumulativeMilaPoints}
                    </div>
                  </div>
                </div>
              </div>
              <div className="flex items-center justify-end">
                <button
                  onClick={() =>
                    setExpandedTeam(isExpanded ? null : result.teamName)
                  }
                  className="flex items-center justify-center rounded transition-colors hover:bg-slate-100 dark:hover:bg-slate-700"
                  aria-label={'Expand details'}
                >
                  <div className={isExpanded ? 'rotate-180' : ''}>
                    <Chevron />
                  </div>
                </button>
              </div>
            </div>
            <div
              className={`${isExpanded ? 'max-h-[500px]' : 'max-h-0'} overflow-hidden border-b
      border-slate-200 opacity-100 transition-all duration-200
      ease-in-out [will-change:max-height,opacity] dark:border-slate-700`}
            >
              <TeamDetailPanel
                row={result}
                currentGameWeekResults={currentGameWeekResults}
              />
            </div>
          </div>
        );
      })}
    </div>
  );
};

export default OverviewTable;
