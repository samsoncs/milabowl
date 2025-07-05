import { Line } from '@nivo/line';
import colors from 'tailwindcss/colors';
import type { TeamResultData } from './types';

interface TrendChartProps {
  results: TeamResultData[];
  teamName: string;
  height: number;
}

type ChartSerie = {
  id: string;
  data: { x: number; y: number }[];
};

const TrendChart: React.FC<TrendChartProps> = ({
  results,
  teamName,
  height,
}: TrendChartProps) => {
  const minValue = Math.min.apply(
    null,
    results?.map(
      (d) =>
        Math.round(
          (d.cumulativeAverageMilaPoints - d.totalCumulativeAverageMilaPoints) *
            100
        ) / 100
    )
  );

  const maxValue = Math.max.apply(
    null,
    results?.map(
      (d) =>
        Math.round(
          (d.cumulativeAverageMilaPoints - d.totalCumulativeAverageMilaPoints) *
            100
        ) / 100
    )
  );

  const buffer =
    Math.round(
      (results[0].cumulativeAverageMilaPoints -
        results[0].totalCumulativeAverageMilaPoints) *
        100
    ) / 100;

  const data: ChartSerie[] = [
    {
      id: teamName,
      data:
        results?.map((rr) => {
          return {
            x: rr.gameWeek,
            y:
              Math.round(
                (rr.cumulativeAverageMilaPoints -
                  rr.totalCumulativeAverageMilaPoints) *
                  100
              ) /
                100 -
              buffer,
          };
        }) ?? [],
    },
  ];

  const minX = Math.min.apply(
    null,
    data[0].data.map((d) => d.x?.valueOf() as number)
  );
  const maxX = Math.max.apply(
    null,
    data[0].data.map((d) => d.x?.valueOf() as number)
  );

  return (
    <Line
      height={height}
      width={120}
      data={data}
      margin={{ top: 4, right: 0, bottom: 4, left: 0 }}
      xScale={{ type: 'linear', min: minX, max: maxX }}
      yScale={{
        type: 'linear',
        min: minValue - buffer,
        max: maxValue - buffer,
        stacked: false,
        reverse: false,
      }}
      curve="natural"
      enableArea
      enablePoints={false}
      areaOpacity={0.1}
      axisTop={null}
      axisRight={null}
      axisBottom={null}
      axisLeft={null}
      enableGridX={false}
      enableGridY={false}
      colors={[colors.indigo[300], colors.orange[300]]}
      lineWidth={2}
      pointColor={{ theme: 'background' }}
      useMesh={false}
      isInteractive={false}
    />
  );
};

export default TrendChart;
