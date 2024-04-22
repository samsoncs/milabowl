import { animated, useSpring } from 'react-spring';
import {
    type BumpPoint,
    type BumpDatum,
    type BumpSerieExtraProps,
    ResponsiveBump,
} from '@nivo/bump';
import { useEffect, useState } from 'react';
import game_state from '../game_state/game_state.json';
import type { MilaResultsDTO } from '../game_state/gameState';
import colors from 'tailwindcss/colors';

interface PlayerStandingsChartProps {
    results: MilaResultsDTO;
}

interface CustomBumpDatum extends BumpDatum {
    points: number;
}

const CustomPoint: React.FC<{
    point: BumpPoint<CustomBumpDatum, BumpSerieExtraProps>;
}> = ({ point }) => {
    const animatedProps = useSpring<{
        x: number;
        y: number;
        radius: number;
        color: string;
        borderColor: string;
        borderWidth: number;
        backgroundRadius: number;
    }>({
        x: point.x,
        y: point.y,
        radius: point.size / 2,
        backgroundRadius: point.size / 2 - point.borderWidth,
        color: point.color,
        borderColor: point.borderColor,
        borderWidth: point.borderWidth,
        config: { mass: 1, tension: 170, friction: 26, clamp: true },
    });

    return (
        <animated.g
            transform={`translate(${point.x}, ${point.y ?? 0})`}
            style={{ pointerEvents: 'none' }}
        >
            <animated.circle
                r={animatedProps.radius}
                className={'fill-white dark:fill-slate-800'}
                stroke={animatedProps.borderColor}
                strokeWidth={animatedProps.borderWidth}
            />
            {point.size !== 0 && (
                <animated.text
                    textAnchor="middle"
                    fontSize="10px"
                    fill={animatedProps.borderColor}
                    className={'dark:fill-slate-100'}
                    dy=".3em"
                >
                    {point.data.points}
                </animated.text>
            )}
        </animated.g>
    );
};

const GetFriendlyName = (name: string, isXs: boolean): string => {
    return isXs ? `${name.substring(0, 10)}..` : name;
};

const PlayerStandingsChart: React.FC<PlayerStandingsChartProps> = ({
    results,
}: PlayerStandingsChartProps) => {
    const [isDarkTheme, setIsDarkTheme] = useState(false);

    useEffect(() => {
        const storedTheme = localStorage.getItem('theme');
        if (storedTheme === 'dark') {
            setIsDarkTheme(true);
        }
    }, []);

    const [week, setWeek] = useState<number[]>([
        results.resultsByWeek.length < 5 ? 0 : results.resultsByWeek.length - 5,
        results.resultsByWeek.length,
    ]);

    const data = results.resultsByUser.map((r, i) => ({
        id: r.teamName,
        data: r.results.slice(week[0], week[1]).map((rr) => ({
            x: `GW ${rr.gameWeek}`,
            y: rr.milaRank,
            points: rr.cumulativeAverageMilaPoints,
        })),
    }));

    return (
        <div style={{ height: '55vh' }}>
            <ResponsiveBump
                data={data}
                xOuterPadding={0.3}
                theme={{
                    fontSize: 12,
                    textColor: isDarkTheme
                        ? colors.slate[300]
                        : colors.slate[700],
                    grid: {
                        line: {
                            stroke: `${isDarkTheme ? colors.slate[600] : colors.slate[300]}`,
                            strokeWidth: 1.5,
                        },
                    },
                    tooltip: {
                        container: {
                            background: isDarkTheme
                                ? colors.slate[700]
                                : colors.white,
                        },
                    },
                }}
                colors={{ scheme: 'category10' }}
                lineWidth={5}
                activeLineWidth={7}
                inactiveLineWidth={5}
                inactiveOpacity={0.15}
                startLabel={false}
                pointSize={28}
                activePointSize={31}
                inactivePointSize={0}
                pointColor={{ theme: 'background' }}
                pointBorderWidth={4}
                activePointBorderWidth={4}
                pointBorderColor={{ from: 'serie.color' }}
                pointComponent={CustomPoint}
                enableGridY={false}
                axisTop={{
                    tickSize: 5,
                    tickPadding: 5,
                    tickRotation: 0,
                    legend: '',
                    legendPosition: 'middle',
                    legendOffset: -36,
                }}
                axisBottom={{
                    tickSize: 5,
                    tickPadding: 5,
                    tickRotation: 0,
                    legend: '',
                    legendPosition: 'middle',
                    legendOffset: 32,
                }}
                axisLeft={null}
                margin={{
                    top: 40,
                    right: 150, // 90 for SM,
                    bottom: 40,
                    left: 10,
                }}
                axisRight={null}
            />
        </div>
    );
};

const Trend = () => {
    const milaResults: MilaResultsDTO = game_state;

    return <PlayerStandingsChart results={milaResults} />;
};

export default Trend;
