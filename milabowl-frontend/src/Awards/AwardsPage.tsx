import { Container, Grid, Typography } from "@mui/material";
import { useSpring, a } from "@react-spring/web";
import React, { useState } from "react";
import { useReward } from "react-rewards";
import theme from "../theme";

interface AwardProps {
  awardTitle: string;
  winner: string;
}

const Award: React.FC<AwardProps> = ({ awardTitle, winner }) => {
  const [flipped, setFlipped] = useState(false);
  const [hover, setHover] = useState(false);
  const { reward } = useReward(awardTitle, "confetti", {
    lifetime: 100,
    decay: 0.94,
    spread: 40,
    startVelocity: 25
  });
  const { transform, opacity } = useSpring({
    opacity: flipped ? 0 : 1,
    transform: `perspective(1000px) rotateY(${flipped ? 360 : 0}deg)`,
    config: { mass: 5, tension: 500, friction: 80 }
  });

  const { rotateZ } = useSpring({
    from: {
      rotateZ: !flipped && hover ? -1 : 0
    },
    to: {
      rotateZ: !flipped && hover ? 1 : 0
    },
    loop: {
      reverse: true
    },
    config: { mass: 1, tension: 150, friction: 12, clamp: true }
  });

  return (
    <div>
      <Typography
        style={{ marginBottom: 10 }}
        align="center"
        variant="h5"
        color="text.primary"
      >
        {awardTitle}
      </Typography>
      <a.div
        onMouseOver={() => {
          setHover(true);
        }}
        onMouseLeave={() => {
          setHover(false);
        }}
        style={{
          position: "relative",
          height: "200px",
          width: "200px",
          display: "flex",
          justifyContent: "center",
          marginLeft: "auto",
          marginRight: "auto"
        }}
        onClick={
          !flipped
            ? () => {
                setFlipped(true);
                reward();
              }
            : undefined
        }
      >
        <a.div
          style={{
            opacity,
            transform,
            backgroundColor: theme.palette.text.primary,
            position: "absolute",
            width: "100%",
            height: "100%",
            cursor: "pointer",
            display: "flex",
            justifyContent: "center",
            alignItems: "center",
            willChange: "transform",
            rotateZ,
            boxShadow: `0 0 10px ${theme.palette.text.primary}`
          }}
        >
          <Typography color="white" fontSize={100}>
            ?
          </Typography>
        </a.div>
        <a.div
          style={{
            opacity: opacity.to((o) => 1 - o),
            transform,
            position: "absolute",
            width: "100%",
            height: "100%",
            cursor: !flipped ? "pointer" : "auto",
            display: "flex",
            justifyContent: "center",
            alignItems: "center",
            willChange: "transform"
          }}
        >
          <Typography variant="body1">{winner}</Typography>
          <span style={{ position: "absolute", bottom: 0 }} id={awardTitle} />
        </a.div>
      </a.div>
    </div>
  );
};

const AwardsPage: React.FC<{}> = () => (
  <Container
    maxWidth="md"
    disableGutters
    style={{ marginLeft: "auto", marginRight: "auto", minHeight: "90vh" }}
  >
    <Grid container spacing={2}>
      <Grid item xs={12}>
        <Award awardTitle="Overall FPL Winner" winner="Some person" />
      </Grid>
      <Grid item xs={12}>
        <Award awardTitle="Overall Milabowl Winner" winner="Some person" />
      </Grid>
      <Grid item xs={12}>
        <Award awardTitle="Coolest player" winner="Some person" />
      </Grid>
      <Grid item xs={12}>
        <Award awardTitle="Omg" winner="Some person" />
      </Grid>
    </Grid>
  </Container>
);

export default AwardsPage;
