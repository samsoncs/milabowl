import { Bar } from '@nivo/bar';
import { useEffect, useState } from 'react';
import colors from 'tailwindcss/colors';
import type { GameWeekResult } from '../game_state/gameState';

interface Props {
    results: GameWeekResult[];
    teamname: string;
    height: number;
}

const LastWeeksChart: React.FC<Props> = ({
    results,
    teamname,
    height,
}: Props) => {
    const [isDarkTheme, setIsDarkTheme] = useState(false);

    useEffect(() => {
        const storedTheme = localStorage.getItem('theme');
        if (storedTheme === 'dark') {
            setIsDarkTheme(true);
        }
    }, []);

    return (
        <>
            <Bar
                height={height}
                width={140}
                keys={['score', 'score_neg']}
                indexBy="gameWeek"
                data={results?.map((rr) => ({
                    gameWeek: rr.gameWeek,
                    score: rr.milaPoints.total >= 0 ? rr.milaPoints.total : 0,
                    score_neg:
                        rr.milaPoints.total < 0 ? rr.milaPoints.total : 0,
                }))}
                layout="vertical"
                borderRadius={2}
                axisTop={null}
                axisRight={null}
                axisBottom={null}
                axisLeft={null}
                enableGridX={false}
                enableGridY={false}
                labelSkipHeight={10}
                padding={0.15}
                colors={[colors.indigo[300], colors.orange[300]]}
                valueFormat={(v) => `${v !== 0 ? (v < 0 ? v * -1 : v) : ''}`}
                isInteractive={false}
            />
        </>
    );
};

export default LastWeeksChart;
