import { Avatar, Card, CardContent, Typography } from "@mui/material";
import React from "react";
import Grid from "@mui/material/Unstable_Grid2";
import { useTheme } from "@mui/material/styles";
import useMediaQuery from "@mui/material/useMediaQuery";

interface PodiumColumnProps {
  height: string;
}

const PodiumColumn: React.FC<PodiumColumnProps> = ({
  height
}: PodiumColumnProps) => <Card style={{ height }} />;

interface PodiumProps {
  title?: string;
  firstPlaceName: string;
  secondPlaceName: string;
  thirdPlaceName: string;
}

const getDisplayName = (matchesXs: boolean, name: string): string => {
  if (matchesXs && !name.includes(" ") && name.length > 15) {
    return (
      name.substring(0, name.length / 2) +
      " " +
      name.substring(name.length / 2, name.length)
    );
  }

  return name;
};

const Podium: React.FC<PodiumProps> = ({
  title,
  firstPlaceName,
  secondPlaceName,
  thirdPlaceName
}: PodiumProps) => {
  const theme = useTheme();
  const matchesXs = useMediaQuery(theme.breakpoints.down("sm"));
  const gridSize = title !== undefined ? 3 : 4;
  return (
    <Grid key={title} container spacing={2} style={{ marginTop: "10px" }}>
      {title !== undefined && (
        <Grid xs={gridSize}>
          <Card style={{ height: "100%", backgroundColor: "#394473" }}>
            <CardContent>
              <Typography
                variant="h5"
                style={{ color: "white", textAlign: "center" }}
                sx={{
                  fontSize: { xs: "0.9rem", md: "2rem" }
                }}
              >
                {title}
              </Typography>
            </CardContent>
          </Card>
        </Grid>
      )}

      <Grid style={{ marginTop: "auto" }} xs={gridSize}>
        <div
          style={{
            display: "flex",
            flexDirection: "column",
            alignItems: "center"
          }}
        >
          <Avatar alt="Remy Sharp" />
          <Typography
            color="textPrimary"
            style={{ fontWeight: 700, textAlign: "center" }}
            sx={{
              fontSize: { xs: "0.9rem", md: "1rem" }
            }}
          >
            2. {getDisplayName(matchesXs, secondPlaceName)}
          </Typography>
        </div>
        <PodiumColumn height={matchesXs ? "40px" : "100px"} />
      </Grid>
      <Grid xs={gridSize}>
        <div
          style={{
            display: "flex",
            flexDirection: "column",
            alignItems: "center"
          }}
        >
          <Avatar alt="Remy Sharp" />

          <Typography
            color="textPrimary"
            style={{ fontWeight: 700, textAlign: "center" }}
            sx={{
              fontSize: { xs: "0.9rem", md: "1rem" }
            }}
          >
            1. {getDisplayName(matchesXs, firstPlaceName)}
          </Typography>
        </div>
        <PodiumColumn height={matchesXs ? "80px" : "150px"} />
      </Grid>
      <Grid style={{ marginTop: "auto" }} xs={gridSize}>
        <div
          style={{
            display: "flex",
            flexDirection: "column",
            alignItems: "center"
          }}
        >
          <Avatar alt="Remy Sharp" />
          <Typography
            color="textPrimary"
            style={{ fontWeight: 700, textAlign: "center" }}
            sx={{
              fontSize: { xs: "0.9rem", md: "1rem" }
            }}
          >
            3. {getDisplayName(matchesXs, thirdPlaceName)}
          </Typography>
        </div>
        <PodiumColumn height={matchesXs ? "10px" : "50px"} />
      </Grid>
    </Grid>
  );
};

export default Podium;
