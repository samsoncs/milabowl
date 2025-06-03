import type { GameWeekResult } from '../../game_state/gameState_v2';
import { useMemo } from 'react';
import SortableTable from '../../components/core/Table/SortableTable';
import { getStandingsColmns } from './_standingsColumns';

interface Props {
    data: GameWeekResult[];
    lastGameWeek: number;
    avatars: ImageMetadata[];
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
