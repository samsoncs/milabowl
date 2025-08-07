import type { GameWeekResult } from '../../../game_state/gameState';
import { useMemo } from 'react';
import SortableTable from '../../../components/core/Table/SortableTable';
import { getStandingsColmns } from './standingsColumns';
import type { OptimizedImage } from './types';

interface Props {
  data: GameWeekResult[];
  lastGameWeek: number;
  avatars: OptimizedImage[];
}

const StandingsTable: React.FC<Props> = ({ data, lastGameWeek, avatars }) => {
  const columns = useMemo(
    () => getStandingsColmns(data, avatars, lastGameWeek),
    []
  );

  return (
    <SortableTable
      data={data}
      columns={columns}
      initialColumnPinnings={['rank', 'teamName']}
    />
  );
};

export default StandingsTable;
