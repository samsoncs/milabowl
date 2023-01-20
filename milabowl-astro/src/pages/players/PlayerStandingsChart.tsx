import { ResponsiveLine, Serie } from "@nivo/line";
import type { GameWeekResult } from "../../../src/game_state/gameState";

interface PlayerStandingsChartProps {
  results: GameWeekResult[];
  teamname: string;
}

const PlayerStandingsChart: React.FC<PlayerStandingsChartProps> = ({
  results,
  teamname
}: PlayerStandingsChartProps) => {
  const data: Serie[] = [
    {
      id: teamname,
      data:
        results?.map((rr) => ({
          x: rr.gameWeek,
          y: rr.cumulativeAverageMilaPoints
        })) ?? []
    },
    {
      id: "Average",
      data:
        results?.map((rr) => ({
          x: rr.gameWeek,
          y: rr.totalCumulativeAverageMilaPoints
        })) ?? []
    }
  ];

  return (
  <div style={{height: "55vh"}} className="h-screen">
    <ResponsiveLine
        data={data}
        margin={{ top: 50, right: 150, bottom: 50, left: 60 }}
        xScale={{ type: "point" }}
        xFormat=" >-"
        yScale={{
        type: "linear",
        min: "auto",
        max: "auto",
        stacked: false,
        reverse: false
        }}
        axisTop={null}
        axisRight={null}
        axisBottom={{
        tickSize: 5,
        tickPadding: 5,
        tickRotation: 0,
        legend: "Game Week",
        legendOffset: 36,
        legendPosition: "middle"
        }}
        axisLeft={{
        tickSize: 5,
        tickPadding: 5,
        tickRotation: 0,
        legend: "Avg. Points",
        legendOffset: -40,
        legendPosition: "middle"
        }}
        enableGridX={false}
        colors={["rgba(31, 119, 180, 1)", "rgba(255, 127, 14, 0.3)"]}
        lineWidth={4}
        pointSize={8}
        pointColor={{ theme: "background" }}
        pointBorderWidth={3}
        pointBorderColor={{ from: "serieColor" }}
        pointLabelYOffset={-12}
        useMesh={true}
        legends={[
        {
            anchor: "bottom-right",
            direction: "column",
            justify: false,
            translateX: 100,
            translateY: 0,
            itemsSpacing: 0,
            itemDirection: "left-to-right",
            itemWidth: 80,
            itemHeight: 20,
            itemOpacity: 0.75,
            symbolSize: 12,
            symbolShape: "circle",
            symbolBorderColor: "rgba(0, 0, 0, .5)",
            effects: [
            {
                on: "hover",
                style: {
                itemBackground: "rgba(0, 0, 0, .03)",
                itemOpacity: 1
                }
            }
            ]
        }
        ]}
    />
  </div>
  );
};

export default PlayerStandingsChart;