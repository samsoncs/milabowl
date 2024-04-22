import { Line, type Serie } from '@nivo/line';
import { useEffect, useState } from 'react';
import colors from 'tailwindcss/colors';
import type { GameWeekResult } from '../game_state/gameState';

interface TrendChartProps {
    results: GameWeekResult[];
    teamname: string;
    height: number;
}

const TrendChart: React.FC<TrendChartProps> = ({
    results,
    teamname,
    height,
}: TrendChartProps) => {
    const [isDarkTheme, setIsDarkTheme] = useState(false);

    useEffect(() => {
        const storedTheme = localStorage.getItem('theme');
        if (storedTheme === 'dark') {
            setIsDarkTheme(true);
        }
    }, []);

    const minValue = Math.min.apply(
        null,
        results?.map(
            (d) =>
                Math.round(
                    (d.cumulativeAverageMilaPoints -
                        d.totalCumulativeAverageMilaPoints) *
                        100
                ) / 100
        )
    );

    const maxValue = Math.max.apply(
        null,
        results?.map(
            (d) =>
                Math.round(
                    (d.cumulativeAverageMilaPoints -
                        d.totalCumulativeAverageMilaPoints) *
                        100
                ) / 100
        )
    );

    const data: Serie[] = [
        {
            id: teamname,
            data:
                results?.map((rr) => ({
                    x: rr.gameWeek,
                    y:
                        Math.round(
                            (rr.cumulativeAverageMilaPoints -
                                rr.totalCumulativeAverageMilaPoints) *
                                100
                        ) / 100,
                })) ?? [],
        },
    ];

    const areaBaselineValue = data[0].data[0].y as number;

    return (
        <Line
            height={height}
            width={120}
            data={data}
            margin={{ top: 4, right: 0, bottom: 4, left: -120 }}
            xScale={{ type: 'linear', min: 1 }}
            yScale={{
                type: 'linear',
                min: minValue,
                max: maxValue,
                stacked: false,
                reverse: false,
            }}
            curve="natural"
            enableArea
            areaOpacity={0.1}
            areaBaselineValue={areaBaselineValue}
            axisTop={null}
            axisRight={null}
            axisBottom={null}
            axisLeft={null}
            enableGridX={false}
            enableGridY={false}
            colors={[colors.indigo[300], colors.orange[300]]}
            lineWidth={2}
            enablePoints={false}
            pointColor={{ theme: 'background' }}
            useMesh={false}
            isInteractive={false}
        />
    );
};

export default TrendChart;
