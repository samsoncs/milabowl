import {
  Grid,
  Card,
  CardContent,
  CardHeader,
  Typography,
  Container,
  Box
} from "@mui/material";
import React from "react";
import { blogEntries } from "./blogEntries";

const BlogPage: React.FC<{}> = () => (
  <Container
    maxWidth="md"
    disableGutters
    style={{ marginLeft: "auto", marginRight: "auto" }}
  >
    <Grid container spacing={2}>
      {blogEntries.map((b) => (
        <Grid item xs={12} key={b.title}>
          <Card>
            <CardHeader
              title={
                <Box display="flex" alignItems="center">
                  <Typography variant="h5" style={{ flexGrow: 1 }}>
                    {b.title}
                  </Typography>
                  <Typography color="text.secondary" variant="body2">
                    {b.date}
                  </Typography>
                </Box>
              }
            />
            <CardContent>
              {b.paragraphs.map((p, i) => (
                <Typography
                  style={{ marginTop: i !== 0 ? "1rem" : "" }}
                  component="p"
                  key={p}
                  variant="body1"
                >
                  {p}
                </Typography>
              ))}
            </CardContent>
          </Card>
        </Grid>
      ))}
    </Grid>
  </Container>
);

export default BlogPage;
