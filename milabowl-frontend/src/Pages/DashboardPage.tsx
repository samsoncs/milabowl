import React, { useState, useEffect } from 'react';
import Grid from '@material-ui/core/Grid';
import {
    Card, Box, CardContent, Toolbar, CardHeader, Table,
    TableHead, TableBody, TableCell, TableRow, TableContainer, Typography, Avatar, Button,
} from '@material-ui/core';
import { Line as LineChart } from 'react-chartjs-2';
import { Link } from 'react-router-dom';
import { GetMilaResults } from '../Api/api';
import { MilaResultsDTO } from '../Api/Dtos/ApiDtos';
import theme from '../theme';
import { ArrowDropDownOutlined, ArrowDropUpOutlined } from '@material-ui/icons';

interface NewsCardProps {
    title: string;
    playerName: string;
    content: string;
    color: string;
}

const NewsCard = ({
    title, playerName, content, color,
}: NewsCardProps) => (
        <Card style={{ height: '130px' }}>
            <Grid container style={{ height: '100%' }}>
                <Grid item sm={4} xs={12} style={{ background: color, position: 'relative' }}>
                    <div style={{
                        position: 'absolute', left: 0, right: 0, bottom: 0, top: '55%', background: 'rgba(0,0,0,0.3)',
                    }}></div>
                    <Box
                        display="flex"
                        flexDirection="column"
                        alignItems="center"
                        justifyContent="center"
                        height="100%"
                        marginLeft="10px"
                        marginRight="10px"
                        textAlign="center"
                    >
                        <Avatar style={{ height: 75, width: 75 }} alt="Remy Sharp" src="https://www.gravatar.com/avatar/205e460b479e2e5b48aec07710c08d50" />
                        <Typography variant="h5" style={{
                            fontSize: '16px', fontWeight: 700, color: 'white', zIndex: 1,
                        }}>
                            {title}
                        </Typography>
                    </Box>
                </Grid>
                <Grid item sm={8} xs={12}>
                    <CardContent>
                        {playerName}
                        {content}
                    </CardContent>
                </Grid>
            </Grid>
        </Card>
    );

interface PlayerStandingsProps {
    results: MilaResultsDTO;
}

const PlayerStandings = ({ results }: PlayerStandingsProps) => (
    <Card style={{ height: '100%' }}>
        <Toolbar disableGutters>
            <CardHeader title="Standings" />

            <section style={{ marginLeft: 'auto', marginRight: '20px' }}>
                <Link style={{ justifySelf: 'flex-end' }} to="/standings">See More</Link>
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
                        {
                            results.resultsByWeek[results.resultsByWeek.length - 1]
                                .results.map((r, i) => {
                                    const deltaPosition = r.milaRankLastWeek === null || r.milaRankLastWeek === undefined ? 0 : (r.milaRankLastWeek - r.milaRank);
                                    return (
                                        <TableRow key={r.teamName}>
                                            <TableCell component="th" scope="row">
                                                <Box display="flex" alignItems="center">
                                                    {`${i + 1}.`}
                                                    {
                                                        deltaPosition < 0 &&
                                                        <Box display="flex" alignItems="center"
                                                            style={{ color: "red" }}>
                                                            <ArrowDropDownOutlined />
                                                            ({deltaPosition})
                                                        </Box>
                                                    }
                                                    {
                                                        deltaPosition > 0 &&
                                                        <Box display="flex" alignItems="center"
                                                            style={{ color: theme.palette.primary.dark }}>
                                                            <ArrowDropUpOutlined />
                                                            ({deltaPosition})
                                                        </Box>
                                                    }
                                                </Box>
                                            </TableCell>
                                            <TableCell>
                                                <Link style={{ color: theme.palette.text.primary }} to={`/mila-player/${r.teamName}`}>{r.teamName}</Link>
                                            </TableCell>
                                            <TableCell align="right">
                                                {r.milaPoints.total}
                                            </TableCell>
                                            <TableCell align="right">
                                                {r.cumulativeAverageMilaPoints}
                                            </TableCell>
                                            <TableCell align="right">
                                                {r.cumulativeMilaPoints}
                                            </TableCell>
                                        </TableRow>
                                    )
                                })
                        }
                    </TableBody>
                </Table>
            </TableContainer>
        </CardContent>
    </Card>
);

const chartColors: string[] = [
    '#003f5c', '#2f4b7c', '#665191', '#a05195', '#d45087',
    '#f95d6a', '#ff7c43', '#ffa600', '#488f31',
];

const options = {
    maintainAspectRatio: false,
};

interface PlayerStandingsChartProps {
    results: MilaResultsDTO;
}

const PlayerStandingsChart = ({ results }: PlayerStandingsChartProps) => {
    const weeks = results.resultsByWeek.map((r) => r.gameWeek);
    const datasets = results.resultsByUser.map((r, i) => ({
        label: r.teamName.toString(),
        borderColor: chartColors[i],
        backgroundColor: chartColors[i],
        fill: false,
        lineTension: 0,
        data: r.results.map((x) => x.cumulativeAverageMilaPoints),
    }));
    const chartData = {
        labels: weeks,
        datasets,
    };

    return (
        <Card style={{ height: '100%' }}>
            <CardHeader title="Rank Over Time" />
            <CardContent style={{height: "55vh"}}>
                <LineChart data={chartData} options={options} />
            </CardContent>
        </Card>
    );
};

const DashboardPage = () => {
    const [milaResults, setMilaResults] = useState<MilaResultsDTO | undefined>();

    useEffect(() => {
        async function getMilaResults() {
            const results = await GetMilaResults();

            console.log(results.resultsByWeek);

            setMilaResults(results);
        }

        getMilaResults();
    }, []);

    return (
        <>
            {!milaResults && (
                <div>loading...</div>
            )}
            {milaResults && (
                <Grid container spacing={2}>
                    <Grid item lg={3} xs={12}>
                        <NewsCard color="#1BDB6B" title="Play of the Week" playerName="" content="Formula Won leaving 27 points on the bench" />
                    </Grid>
                    <Grid item lg={3} xs={12}>
                        <NewsCard color="#6556A4" title="Current Leader" playerName="" content={milaResults.resultsByWeek[milaResults.resultsByWeek.length - 1].results[0].teamName} />
                    </Grid>
                    <Grid item lg={3} xs={12}>
                        <NewsCard color="#6556A4" title="Current Looser" playerName=""
                            content={
                                milaResults.resultsByWeek[milaResults.resultsByWeek.length - 1]
                                    .results[milaResults.resultsByWeek[milaResults.resultsByWeek.length - 1].results.length - 1].teamName} />
                    </Grid>
                    <Grid item lg={3} xs={12}>
                        <NewsCard color="#F2DE74" title="Reigning Champ" playerName="" content="Eivind won the first ever Milabowl season 19/20" />
                    </Grid>
                    <Grid item lg={5} xs={12}>
                        <PlayerStandings results={milaResults} />
                    </Grid>
                    <Grid item lg={7} xs={12}>
                        <PlayerStandingsChart results={milaResults} />
                    </Grid>
                </Grid>
            )}
        </>
    );
};

export default DashboardPage;
