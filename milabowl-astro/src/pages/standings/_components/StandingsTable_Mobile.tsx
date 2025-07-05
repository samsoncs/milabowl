import type { GameWeekResult } from '../../../game_state/gameState';
import { useMemo } from 'react';
import SortableTable from '../../../components/core/Table/SortableTable';
import { getStandingsColmns } from './standingsColumns';

interface Props {
  data: GameWeekResult[];
  lastGameWeek: number;
  avatars: ImageMetadata[];
}

const StandingsTableMobile: React.FC<Props> = ({
  data,
  lastGameWeek,
  avatars,
}) => {
  const columns = useMemo(
    () => getStandingsColmns(data, avatars, lastGameWeek),
    [data, lastGameWeek, avatars]
  );

  return (
    <SortableTable data={data} columns={columns} initialColumnPinnings={[]} />
  );
};

export default StandingsTableMobile;
