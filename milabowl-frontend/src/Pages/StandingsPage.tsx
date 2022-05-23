import React, { useState, useEffect } from 'react';
import {
    Card, CardContent, CardHeader, Table, TableHead,
    TableBody, TableCell, TableRow, TableContainer, Box, Select, MenuItem, Toolbar, Tooltip
} from '@material-ui/core';
import { MilaResultsDTO } from '../Api/Dtos/ApiDtos';
import { GetMilaResults } from '../Api/api';
import { withStyles } from '@material-ui/core/styles';
import { ArrowDropDownOutlined, ArrowDropUpOutlined } from '@material-ui/icons';
import theme from '../theme';

const LightToolTip = withStyles((theme) => ({
    tooltip: {
        //backgroundColor: theme.palette.common.white,
        backgroundColor: theme.palette.warning.light,
        color: 'rgba(0, 0, 0, 0.87)',
        boxShadow: theme.shadows[1],
        fontSize: 11,
    },
}))(Tooltip);

const StandingsPage = () => {
    const [milaResults, setMilaResults] = useState<MilaResultsDTO | undefined>();
    const [currentGameWeek, setCurrentGameweek] = useState(0);
    const results = currentGameWeek === 0
        ? milaResults?.overallScore
        : milaResults?.resultsByWeek[currentGameWeek - 1]?.results;

    results?.sort((n1, n2) => n2.cumulativeMilaPoints - n1.cumulativeMilaPoints);

    useEffect(() => {
        async function getMilaResults(): Promise<void> {
            const res = await GetMilaResults();
            setMilaResults(res);
        }

        getMilaResults();
    }, []);

    return (
        <Card style={{ height: '100%' }}>
            <Toolbar disableGutters>
                <CardHeader title={`Standings - ${currentGameWeek === 0 ? 'Overall' : (`Game Week ${currentGameWeek}`)}`} />

                <section style={{ marginLeft: 'auto', marginRight: '20px' }}>
                    <Select
                        value={currentGameWeek}
                        style={{ marginBottom: '10px' }}
                        onChange={(event: React.ChangeEvent<{
                            name?: string | undefined;
                            value: unknown;
                        }>): void => setCurrentGameweek(Number(event.target.value))}
                    >
                        <MenuItem value={0}>Overall</MenuItem>
                        {
                            milaResults?.resultsByWeek
                                .map((m) => (
                                    <MenuItem key={m.gameWeek} value={m.gameWeek}>Game Week {m.gameWeek}</MenuItem>
                                ))
                        }
                    </Select>
                </section>
            </Toolbar>

            <CardContent>
                <TableContainer>
                    <Table aria-label="simple table">
                        <TableHead>
                            <TableRow>
                                <TableCell>Rank</TableCell>
                                <TableCell>Team</TableCell>
                                <TableCell align="center"><LightToolTip title="Sweet 69: 6.9 pts if GW Score is 69" placement="top"><div>69</div></LightToolTip></TableCell>
                                <TableCell align="center"><LightToolTip title="Yellow Card: 1 pt each" placement="top"><div>YC</div></LightToolTip></TableCell>
                                <TableCell align="center"><LightToolTip title="Red Card: 3 pts each" placement="top"><div>RC</div></LightToolTip></TableCell>
                                <TableCell align="center"><LightToolTip title="Bench Fail: -1 pt per 5 pts on bench" placement="top"><div>BF</div></LightToolTip></TableCell>
                                <TableCell align="center"><LightToolTip title="Cap Fail: -1 pt if captain scores less than 5 pts" placement="top"><div>CF</div></LightToolTip></TableCell>
                                <TableCell align="center"><LightToolTip title="Cap GoalKeeper: 2 pts" placement="top"><div>CK</div></LightToolTip></TableCell>
                                <TableCell align="center"><LightToolTip title="Cap Defender: 1 pt" placement="top"><div>CD</div></LightToolTip></TableCell>
                                <TableCell align="center"><LightToolTip title="Minus is Plus: every negative point is positive" placement="top"><div>MiP</div></LightToolTip></TableCell>
                                <TableCell align="center"><LightToolTip title="Increase Streak: 1 pt if higher score than previous GW" placement="top"><div>IS</div></LightToolTip></TableCell>
                                <TableCell align="center"><LightToolTip title="Equal Streak: 6.9 pts if same score as previous GW" placement="top"><div>ES</div></LightToolTip></TableCell>
                                <TableCell align="center"><LightToolTip title="GW Position Score: 4 pts FPL GW winner, then decreasing with 0.5" placement="top"><div>GW PS</div></LightToolTip></TableCell>
                                <TableCell align="center"><LightToolTip title="Head 2 Head Meta: 2 point for winning H2H with 2 or less points" placement="top"><div>H2H M</div></LightToolTip></TableCell>
                                <TableCell align="center"><LightToolTip title="Sixty Nine Sub: 2.69 points if at least one player played 69 minutes " placement="top"><div>69 Sub</div></LightToolTip></TableCell>
                                <TableCell align="center"><LightToolTip title="Unique Cap: 2 pts if you have a captain no one else had" placement="top"><div>Unq Cap</div></LightToolTip></TableCell>
                                <TableCell align="center"><LightToolTip title="Trendy Bitch: -1 point if you bought most transferred in player and -1 point if you sold most transferred out player." placement="top"><div>Trnd</div></LightToolTip></TableCell>
                                <TableCell align="center"><LightToolTip title="GW Mila Points" placement="top"><div>GW MP</div></LightToolTip></TableCell>
                                <TableCell align="right"><LightToolTip title="Total Mila Points" placement="top"><div>Total MP</div></LightToolTip></TableCell>
                            </TableRow>
                        </TableHead>
                        <TableBody>
                            {
                                results
                                && results.map((r, i) => {
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
                                                {r.teamName}
                                            </TableCell>
                                            <TableCell align="center">
                                                {r.milaPoints.gW69}
                                            </TableCell>
                                            <TableCell align="center">
                                                {r.milaPoints.yellowCard}
                                            </TableCell>
                                            <TableCell align="center">
                                                {r.milaPoints.redCard}
                                            </TableCell>
                                            <TableCell align="center">
                                                {r.milaPoints.benchFail}
                                            </TableCell>
                                            <TableCell align="center">
                                                {r.milaPoints.capFail}
                                            </TableCell>
                                            <TableCell align="center">
                                                {r.milaPoints.capKeep}
                                            </TableCell>
                                            <TableCell align="center">
                                                {r.milaPoints.capDef}
                                            </TableCell>
                                            <TableCell align="center">
                                                {r.milaPoints.minusIsPlus}
                                            </TableCell>
                                            <TableCell align="center">
                                                {r.milaPoints.increaseStreak}
                                            </TableCell>
                                            <TableCell align="center">
                                                {r.milaPoints.equalStreak}
                                            </TableCell>
                                            <TableCell align="center">
                                                {r.milaPoints.gwPositionScore}
                                            </TableCell>
                                            <TableCell align="center">
                                                {r.milaPoints.headToHeadMeta}
                                            </TableCell>
                                            <TableCell align="center">
                                                {r.milaPoints.sixtyNineSub}
                                            </TableCell>
                                            <TableCell align="center">
                                                {r.milaPoints.uniqueCap}
                                            </TableCell>
                                            <TableCell align="center">
                                                {r.milaPoints.trendyBitch}
                                            </TableCell>
                                            <TableCell align="center">
                                                {r.milaPoints.total}
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
};

export default StandingsPage;
