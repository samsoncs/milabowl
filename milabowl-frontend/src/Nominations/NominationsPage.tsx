import React, {useEffect, useState} from "react";
import { Buffer } from "buffer";
import { Card, CardContent, CardHeader, Container, Grid, Typography } from "@mui/material";

interface Nominations{
    personNominated: string;
    gw: number;
    date: string;
    reason: string;
}

const NominationsPage: React.FC<{}> = () => {
    const [nominations, setNominations] = useState<Nominations[]>([]);
    useEffect(() => {
        const fetchNominations: () => Promise<void> = async () => {
            const response = await fetch("https://api.github.com/repos/samsoncs/milabowl/contents/nominations.json?ref=content");
            const json = await response.json();
            const nomsString = Buffer.from(json.content, 'base64').toString();
            const noms: Nominations[] = JSON.parse(nomsString);
            setNominations(noms);
        };

        fetchNominations().catch(e => console.log(e));
    }, [])
    return (
        <Container
            maxWidth="md"
            disableGutters
            style={{ marginLeft: "auto", marginRight: "auto" }}
        >
            <Grid container spacing={2}>
            {nominations.map((n) => (
                <Grid item xs={12} key={`${n.personNominated} - GW ${n.gw}`}>
                <Card>
                    <CardHeader
                    title={<Typography variant="h5">{`GW ${n.gw} - ${n.personNominated}`}</Typography>}
                    />
                    <CardContent>
                        {n.reason}
                    </CardContent>
                </Card>
                </Grid>
            ))}
            </Grid>
        </Container>
)};

export default NominationsPage;