import {
  Box,
  Card,
  CardContent,
  CardHeader,
  Grid,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  Toolbar,
  Link,
  Slider
} from "@mui/material";
import React, { useState, useEffect } from "react";
import { MilaResultsDTO } from "../GameState/DTOs/MilaResultDTOs";
import { GetMilaResults } from "../GameState/MilaResults";
import { Line } from "react-chartjs-2";
import "chart.js/auto";
import PositionDelta from "../Components/PositionDelta";
import Podium from "../Components/Podium";
import type { ChartOptions } from "chart.js";

interface PlayerStandingsProps {
  results: MilaResultsDTO;
}

const PlayerStandings: React.FC<PlayerStandingsProps> = ({
  results
}: PlayerStandingsProps) => (
  <Card style={{ height: "100%" }}>
    <Toolbar disableGutters>
      <CardHeader title="Standings" />

      <section style={{ marginLeft: "auto", marginRight: "20px" }}>
        <Link style={{ justifySelf: "flex-end" }} href="#/standings">
          See More
        </Link>
      </section>
    </Toolbar>

    <CardContent>
      <TableContainer>
        <Table aria-label="simple table">
          <TableHead>
            <TableRow>
              <TableCell>Rank</TableCell>
              <TableCell>Team</TableCell>
              <TableCell align="right">GW</TableCell>
              <TableCell align="right">Avg</TableCell>
              <TableCell align="right">Total Score</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {results.resultsByWeek[
              results.resultsByWeek.length - 1
            ].results.map((r, i) => {
              const deltaPosition =
                r.milaRankLastWeek === null || r.milaRankLastWeek === undefined
                  ? 0
                  : r.milaRankLastWeek - r.milaRank;
              return (
                <TableRow key={r.teamName}>
                  <TableCell component="th" scope="row">
                    <Box display="flex" alignItems="center">
                      {`${i + 1}.`}
                      <PositionDelta delta={deltaPosition} />
                    </Box>
                  </TableCell>
                  <TableCell>
                    <Link href={`#/mila-player/${r.teamName}`}>
                      {r.teamName}
                    </Link>
                  </TableCell>
                  <TableCell align="right">{r.milaPoints.total}</TableCell>
                  <TableCell align="right">
                    {r.cumulativeAverageMilaPoints}
                  </TableCell>
                  <TableCell align="right">{r.cumulativeMilaPoints}</TableCell>
                </TableRow>
              );
            })}
          </TableBody>
        </Table>
      </TableContainer>
    </CardContent>
  </Card>
);

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

const options: ChartOptions<"line"> = {
  maintainAspectRatio: false,
  scales: {
    y: {
      reverse: true,
      title: {
        text: "Position",
        display: true
      }
    }
  }
};

interface PlayerStandingsChartProps {
  results: MilaResultsDTO;
}

const PlayerStandingsChart2: React.FC<PlayerStandingsChartProps> = ({
  results
}: PlayerStandingsChartProps) => {
  const [week, setWeek] = useState<number[]>([
    results.resultsByWeek.length - 5,
    results.resultsByWeek.length
  ]);
  const weeks = results.resultsByWeek
    .slice(week[0], week[1])
    .map((r) => r.gameWeek);
  const datasets = results.resultsByUser.map((r, i) => ({
    label: r.teamName.toString(),
    borderColor: chartColors[i],
    backgroundColor: chartColors[i],
    fill: false,
    lineTension: 0,
    data: r.results.slice(week[0], week[1]).map((x) => x.milaRank)
  }));
  const chartData = {
    labels: weeks,
    datasets
  };

  return (
    <Card style={{ height: "100%" }}>
      <CardHeader
        title={
          <div style={{ display: "flex", flexDirection: "row" }}>
            <div style={{ width: "350px" }}>
              Trend - GW {week[0] + 1} - {week[1]}
            </div>
            <Slider
              getAriaLabel={() => "Week"}
              valueLabelDisplay="auto"
              value={week}
              min={0}
              max={results.resultsByWeek.length}
              step={1}
              onChange={(evt, val) => setWeek(val as number[])}
            />
          </div>
        }
      />
      <CardContent style={{ height: "55vh" }}>
        <Line data={chartData} options={options} />
      </CardContent>
    </Card>
  );
};

const DashboardPage: React.FC<{}> = () => {
  const [milaResults, setMilaResults] = useState<MilaResultsDTO | undefined>();

  useEffect(() => {
    function getMilaResults(): void {
      const results = GetMilaResults();
      setMilaResults(results);
    }

    getMilaResults();
  }, []);

  return (
    <>
      {milaResults === undefined && <div>loading...</div>}
      {milaResults !== undefined && (
        <>
          <Podium
            firstPlaceName={
              milaResults.resultsByWeek[milaResults.resultsByWeek.length - 1]
                .results[0].teamName
            }
            secondPlaceName={
              milaResults.resultsByWeek[milaResults.resultsByWeek.length - 1]
                .results[1].teamName
            }
            thirdPlaceName={
              milaResults.resultsByWeek[milaResults.resultsByWeek.length - 1]
                .results[2].teamName
            }
          />
          <Grid container spacing={2}>
            <Grid item lg={5} xs={12}>
              <PlayerStandings results={milaResults} />
            </Grid>
            <Grid item lg={7} xs={12}>
              <PlayerStandingsChart2 results={milaResults} />
            </Grid>
          </Grid>
        </>
      )}
    </>
  );
};

export default DashboardPage;
