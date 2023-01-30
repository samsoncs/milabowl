import { ResponsiveLine, Serie } from "@nivo/line";
import { useEffect, useState } from "react";
import colors from "tailwindcss/colors";
import type { GameWeekResult } from "../../../src/game_state/gameState";

interface PlayerStandingsChartProps {
  results: GameWeekResult[];
  teamname: string;
}

const PlayerStandingsChart: React.FC<PlayerStandingsChartProps> = ({
  results,
  teamname
}: PlayerStandingsChartProps) => {
  const [isDarkTheme, setIsDarkTheme] = useState(false);

  useEffect(() => {
    const storedTheme = localStorage.getItem("theme");
    if(storedTheme === "dark"){
      setIsDarkTheme(true);
    }
  }, []);

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
        margin={{ top: 50, right: 40, bottom: 80, left: 60 }}
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
        colors={[colors.indigo[500], colors.orange[400]]}
        lineWidth={4}
        pointSize={5}
        pointColor={{ theme: "background" }}
        theme={{ 
          textColor: isDarkTheme ? colors.slate[300] : colors.slate[700], 
          grid: {line: { stroke: `${isDarkTheme ? colors.slate[700] : colors.slate[200]}`}},
          tooltip: {
              container:{
                  background: isDarkTheme ? colors.slate[700] : colors.white
              }
          },
          crosshair:{
            line: { stroke: `${isDarkTheme ? colors.slate[500] : colors.slate[400]}`}
          }
        }}
        pointSymbol={(props) => (
            <g>
              <circle
                r={props.size}
                fill={props.borderColor}
              >
              </circle>
            </g>
        )}
        pointBorderWidth={3}
        pointBorderColor={{ from: "serieColor" }}
        pointLabelYOffset={-12}
        useMesh={true}
        legends={[
        {
            anchor: "bottom-left",
            direction: "row",
            justify: false,
            translateX: 0,
            translateY: 70,
            itemsSpacing: 0,
            itemDirection: "left-to-right",
            itemWidth: 80,
            itemHeight: 20,
            itemTextColor: isDarkTheme ? colors.slate[300] : colors.slate[700],
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