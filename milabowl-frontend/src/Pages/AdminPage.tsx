import React, { useState } from 'react';
import { Card, CardHeader, CardContent, Button, Grid, CircularProgress } from '@material-ui/core';
import { ImportData, ProcessData } from '../Api/api';

const AdminPage = () => {
    const [isImporting, setIsImporting] = useState(false);
    const [isProcessing, setIsProcessing] = useState(false);

    return (
        <Card>
            <CardHeader title="Admin Panel"></CardHeader>
            <CardContent>
                <Grid container spacing={2}>
                    <Grid item xs={12}>
                        <Button
                            onClick={async () => { setIsImporting(true); await ImportData(); setIsImporting(false) }}
                            color="secondary"
                            variant="contained"
                            disabled={isImporting}
                        >
                            Import Data
                        </Button>
                        {isImporting && <CircularProgress color="secondary" />}
                    </Grid>
                    <Grid item xs={12}>
                        <Button
                            onClick={async () => { setIsProcessing(true); await ProcessData(); setIsProcessing(false) }}
                            color="secondary"
                            variant="contained"
                            disabled={isProcessing}
                        >
                            Process Data
                        </Button>
                        {isProcessing && <CircularProgress color="secondary" />}
                    </Grid>
                </Grid>
            </CardContent>
        </Card>
    );
}

export default AdminPage;