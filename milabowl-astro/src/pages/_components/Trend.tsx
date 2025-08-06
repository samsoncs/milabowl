import { animated, useSpring } from 'react-spring';
import {
  type BumpPoint,
  type BumpDatum,
  type BumpSerieExtraProps,
  ResponsiveBump,
} from '@nivo/bump';
import { useEffect, useState } from 'react';
import colors from 'tailwindcss/colors';
import type { ResultsForTeams } from './types';

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
        className={'fill-white dark:fill-slate-900'}
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

const PlayerStandingsChart: React.FC<TrendProps> = ({ teams }) => {
  const [isDarkTheme, setIsDarkTheme] = useState(false);

  useEffect(() => {
    const storedTheme = localStorage.getItem('theme');
    if (storedTheme === 'dark') {
      setIsDarkTheme(true);
    }
  }, []);

  const chartData = teams.map((team) => ({
    id: team.teamName,
    data: team.results.map((result) => ({
      x: `GW ${result.gameWeek}`,
      y: result.milaRank,
      points: result.cumulativeAverageMilaPoints,
    })),
  }));

  return (
    <div style={{ height: '55vh' }}>
      <ResponsiveBump
        data={chartData}
        xOuterPadding={0.3}
        theme={{
          text: {
            fontSize: 12,
            fill: isDarkTheme ? colors.slate[300] : colors.slate[700],
          },
          grid: {
            line: {
              stroke: `${isDarkTheme ? colors.slate[600] : colors.slate[300]}`,
              strokeWidth: 1.5,
            },
          },
          tooltip: {
            container: {
              background: isDarkTheme ? colors.slate[700] : colors.white,
            },
          },
        }}
        colors={[
          '#3B82F6', // blue-500
          '#F59E0B', // amber-500
          '#10B981', // emerald-500
          '#EF4444', // red-500
          '#8B5CF6', // purple-500
          '#22C55E', // green-500
          '#EC4899', // pink-500
          '#62748e', // violet-500
          '#F97316', // orange-500
        ]}
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

interface TrendProps {
  teams: ResultsForTeams[];
}

const Trend = ({ teams }: TrendProps) => {
  return <PlayerStandingsChart teams={teams} />;
};

export default Trend;
