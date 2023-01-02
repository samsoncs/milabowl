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
import { Line as LineChart } from "react-chartjs-2";

const chartColors: string[] = [
  "#003f5c",
  "#2f4b7c",
  "#665191",
  "#a05195",
  "#d45087",
  "#f95d6a",
  "#ff7c43",
  "#ffa600",
  "#488f31"
];

interface PlayerStandingsChartProps {
  results: GameWeekResult[] | undefined;
  teamname: string;
}

const PlayerStandingsChart: React.FC<PlayerStandingsChartProps> = ({
  results,
  teamname
}: PlayerStandingsChartProps) => {
  const weeks = results?.map((r, i) => r.gameWeek);
  const datasets = [
    {
      label: teamname,
      borderColor: chartColors[0],
      backgroundColor: chartColors[0],
      fill: false,
      lineTension: 0,
      data: results?.map((x) => x.cumulativeAverageMilaPoints)
    },
    {
      label: "Average",
      borderColor: chartColors[7],
      backgroundColor: chartColors[7],
      fill: false,
      lineTension: 0,
      data: results?.map((x) => x.totalCumulativeAverageMilaPoints)
    }
  ];

  const chartData = {
    labels: weeks,
    datasets
  };

  const options = {};

  return (
    <Card style={{ height: "100%" }}>
      <CardHeader title="Avg Points" />
      <CardContent>
        <LineChart data={chartData} options={options} />
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
      <Grid item md={4}>
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
      <Grid item md={8}>
        <PlayerStandingsChart results={results} teamname={teamname ?? ""} />
      </Grid>
    </Grid>
  );
};

export default MilaPlayerPage;
