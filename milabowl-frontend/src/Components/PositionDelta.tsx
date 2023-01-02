import React from "react";
import ArrowDropDownIcon from "@mui/icons-material/ArrowDropDown";
import ArrowDropUpIcon from "@mui/icons-material/ArrowDropUp";
import { Box } from "@mui/material";

interface PositionDeltaProps {
  delta: number;
}

const PositionDelta: React.FC<PositionDeltaProps> = (
  props: PositionDeltaProps
) => (
  <div>
    {props.delta < 0 && (
      <Box display="flex" alignItems="center" style={{ color: "red" }}>
        <ArrowDropDownIcon />({props.delta})
      </Box>
    )}
    {props.delta > 0 && (
      <Box display="flex" alignItems="center" style={{ color: "green" }}>
        <ArrowDropUpIcon />({props.delta})
      </Box>
    )}
  </div>
);

export default PositionDelta;
