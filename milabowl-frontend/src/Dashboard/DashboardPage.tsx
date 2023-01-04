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
  Slider,
  useMediaQuery
} from "@mui/material";
import React, { useState, useEffect } from "react";
import { MilaResultsDTO } from "../GameState/DTOs/MilaResultDTOs";
import { GetMilaResults } from "../GameState/MilaResults";
import PositionDelta from "../Components/PositionDelta";
import Podium from "../Components/Podium";
import {
  BumpPoint,
  ResponsiveBump,
  BumpDatum,
  BumpSerieExtraProps
} from "@nivo/bump";
import { useSpring, animated } from "@react-spring/web";
import { useMotionConfig } from "@nivo/core";
import theme from "../theme";

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

interface PlayerStandingsChartProps {
  results: MilaResultsDTO;
}

interface CustomBumpDatum extends BumpDatum {
  points: number;
}

const CustomPoint: React.FC<{
  point: BumpPoint<CustomBumpDatum, BumpSerieExtraProps>;
}> = ({ point }) => {
  const { animate, config: springConfig } = useMotionConfig();

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
    config: springConfig,
    immediate: !animate
  });

  return (
    <animated.g
      transform={`translate(${point.x}, ${point.y ?? 0})`}
      style={{ pointerEvents: "none" }}
    >
      <animated.circle
        r={animatedProps.radius}
        fill={animatedProps.borderColor}
        stroke={animatedProps.borderColor}
        strokeWidth={animatedProps.borderWidth}
      />
      <animated.circle r={animatedProps.backgroundRadius} fill="white" />
      {point.size !== 0 && (
        <animated.text
          textAnchor="middle"
          fontSize="10px"
          fill={animatedProps.borderColor}
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
  results
}: PlayerStandingsChartProps) => {
  const [week, setWeek] = useState<number[]>([
    results.resultsByWeek.length - 5,
    results.resultsByWeek.length
  ]);

  const matchesXs = useMediaQuery(theme.breakpoints.down("sm"));

  const data = results.resultsByUser.map((r, i) => ({
    id: GetFriendlyName(r.teamName, matchesXs),
    data: r.results.slice(week[0], week[1]).map((rr) => ({
      x: `GW ${rr.gameWeek}`,
      y: rr.milaRank,
      points: rr.cumulativeAverageMilaPoints
    }))
  }));

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
        <ResponsiveBump
          data={data}
          xOuterPadding={0.3}
          theme={{ fontSize: 12 }}
          colors={{ scheme: "category10" }}
          lineWidth={5}
          activeLineWidth={7}
          inactiveLineWidth={5}
          inactiveOpacity={0.15}
          startLabel={false}
          pointSize={28}
          activePointSize={31}
          inactivePointSize={0}
          pointColor={{ theme: "background" }}
          pointBorderWidth={3}
          activePointBorderWidth={3}
          pointBorderColor={{ from: "serie.color" }}
          pointComponent={CustomPoint}
          enableGridY={false}
          axisTop={{
            tickSize: 5,
            tickPadding: 5,
            tickRotation: 0,
            legend: "",
            legendPosition: "middle",
            legendOffset: -36
          }}
          axisBottom={{
            tickSize: 5,
            tickPadding: 5,
            tickRotation: 0,
            legend: "",
            legendPosition: "middle",
            legendOffset: 32
          }}
          axisLeft={null}
          margin={{
            top: 40,
            right: matchesXs ? 90 : 150,
            bottom: 40,
            left: 10
          }}
          axisRight={null}
        />
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
              <PlayerStandingsChart results={milaResults} />
            </Grid>
          </Grid>
        </>
      )}
    </>
  );
};

export default DashboardPage;
