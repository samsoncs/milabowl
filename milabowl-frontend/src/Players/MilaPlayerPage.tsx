import React, { useState, useEffect } from "react";
import { GameWeekResult } from "../GameState/DTOs/MilaResultDTOs";
import { GetMilaResults } from "../GameState/MilaResults";
import { useParams } from "react-router-dom";
import {
  Card,
  CardContent,
  CardHeader,
  Grid,
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableRow
} from "@mui/material";
import { ResponsiveLine, Serie } from "@nivo/line";

interface PlayerStandingsChartProps {
  results: GameWeekResult[] | undefined;
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
    <Card style={{ height: "100%" }}>
      <CardHeader title="Avg Points" />
      <CardContent style={{ height: "55vh" }}>
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
      </CardContent>
    </Card>
  );
};

const MilaPlayerPage: React.FC<{}> = () => {
  const [results, setResults] = useState<GameWeekResult[] | undefined>();
  const { teamname } = useParams<{ teamname?: string }>();
  useEffect(() => {
    function getMilaResults(): void {
      const res = GetMilaResults();
      const resultsForPlayer = res?.resultsByUser.find(
        (r) => r.teamName === teamname
      )?.results;
      setResults(resultsForPlayer);
    }

    getMilaResults();
  }, []);

  return (
    <Grid container spacing={2}>
      <Grid item md={4} xs={12}>
        <Card style={{ height: "100%" }}>
          <CardHeader title={teamname} />
          <CardContent>
            <Table>
              <TableHead>
                <TableRow>
                  <TableCell>GW</TableCell>
                  <TableCell align="right">Pts</TableCell>
                  <TableCell align="right">Avg</TableCell>
                  <TableCell align="right">Tot</TableCell>
                </TableRow>
              </TableHead>
              <TableBody>
                {results
                  ?.concat()
                  .sort(
                    (a: GameWeekResult, b: GameWeekResult) =>
                      b.gameWeek - a.gameWeek
                  )
                  .map((r) => (
                    <TableRow key={r.gameWeek}>
                      <TableCell>{`Game Week ${r.gameWeek}`}</TableCell>
                      <TableCell align="right">{r.milaPoints.total}</TableCell>
                      <TableCell align="right">
                        {r.cumulativeAverageMilaPoints}
                      </TableCell>
                      <TableCell align="right">
                        {r.cumulativeMilaPoints}
                      </TableCell>
                    </TableRow>
                  ))}
              </TableBody>
            </Table>
          </CardContent>
        </Card>
      </Grid>
      <Grid item md={8} xs={12}>
        <PlayerStandingsChart results={results} teamname={teamname ?? ""} />
      </Grid>
    </Grid>
  );
};

export default MilaPlayerPage;
