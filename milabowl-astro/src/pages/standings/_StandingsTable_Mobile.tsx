import { createColumnHelper } from '@tanstack/react-table';
import type { GameWeekResult } from '../../game_state/gameState';
import { useMemo } from 'react';
import PositionDelta from '../../components/core/PositionDelta';
import SortableTable from '../../components/core/Table/SortableTable';
import { getStandingsColmns } from './_standingsColumns';

const columnHelper = createColumnHelper<GameWeekResult>();

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
        () => getStandingsColmns(avatars, lastGameWeek),
        []
    );

    return (
        <SortableTable
            data={data}
            columns={columns}
            initialColumnPinnings={[]}
        />
    );
};

export default StandingsTableMobile;
