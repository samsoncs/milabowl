import { ResponsiveLine } from '@nivo/line';
import { useEffect, useState } from 'react';
import colors from 'tailwindcss/colors';
import type { GameWeekResult } from '../../../game_state/gameState';

interface PlayerStandingsChartProps {
  results: GameWeekResult[];
  teamname: string;
}

type ChartSerie = {
  id: string;
  data: { x: number; y: number }[];
};

const PlayerStandingsChart: React.FC<PlayerStandingsChartProps> = ({
  results,
  teamname,
}: PlayerStandingsChartProps) => {
  const [isDarkTheme, setIsDarkTheme] = useState(false);

  useEffect(() => {
    const storedTheme = localStorage.getItem('theme');
    if (storedTheme === 'dark') {
      setIsDarkTheme(true);
    }
  }, []);

  const data: ChartSerie[] = [
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

  const minValue = Math.min.apply(
    null,
    data[0].data.map((d) => d.y as number)
  );

  return (
    <div className="h-full">
      <div className="text-content-secondary text-sm">
        Line shows your performance vs avg. Above the line means above average.
      </div>
      <div className="h-72 lg:h-1/2">
        <ResponsiveLine
          data={data}
          margin={{ top: 20, right: 40, bottom: 50, left: 60 }}
          xScale={{ type: 'linear', min: 1 }}
          xFormat=" >-"
          yScale={{
            type: 'linear',
            min: minValue > 0 ? 0 : minValue,
            max: 'auto',
            stacked: false,
            reverse: false,
          }}
          axisTop={null}
          axisRight={null}
          enableArea
          axisBottom={{
            tickSize: 5,
            tickPadding: 5,
            tickRotation: 0,
            tickValues: 10,
            legend: 'Game Week',
            legendOffset: 36,
            legendPosition: 'middle',
          }}
          axisLeft={{
            tickSize: 5,
            tickPadding: 5,
            tickRotation: 0,
            legend: 'Your Avg points - Total Avg points',
            legendOffset: -40,
            legendPosition: 'middle',
          }}
          enableGridX={false}
          colors={[colors.indigo[500], colors.orange[400]]}
          lineWidth={4}
          pointSize={5}
          pointColor={{ theme: 'background' }}
          theme={{
            text: {
              fill: isDarkTheme ? colors.slate[300] : colors.slate[700],
            },
            grid: {
              line: {
                stroke: `${isDarkTheme ? colors.slate[700] : colors.slate[200]}`,
              },
            },
            tooltip: {
              container: {
                background: isDarkTheme ? colors.slate[700] : colors.white,
              },
            },
            crosshair: {
              line: {
                stroke: `${isDarkTheme ? colors.slate[500] : colors.slate[400]}`,
              },
            },
            axis: {
              ticks: {
                line: {
                  stroke: `${isDarkTheme ? colors.slate[500] : colors.slate[400]}`,
                },
              },
            },
          }}
          pointSymbol={(props) => (
            <g>
              <circle r={props.size} fill={props.borderColor}></circle>
            </g>
          )}
          pointBorderWidth={3}
          pointBorderColor={colors.indigo[500]}
          pointLabelYOffset={-12}
          useMesh={true}
        />
      </div>
    </div>
  );
};

export default PlayerStandingsChart;
