import { Grid, Card, CardContent, CardHeader, Typography } from "@mui/material";
import React from "react";
import { blogEntries } from "./blogEntries";

const BlogPage: React.FC<{}> = () => (
  <Grid
    container
    spacing={2}
    maxWidth="md"
    style={{ marginLeft: "auto", marginRight: "auto" }}
  >
    {blogEntries.map((b) => (
      <Grid item xs={12} key={b.title}>
        <Card>
          <CardHeader title={<Typography variant="h5">{b.title}</Typography>} />
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
);

export default BlogPage;
