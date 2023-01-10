import React, { useEffect, useState } from "react";
import { Buffer } from "buffer";
import {
  Box,
  Card,
  CardContent,
  CardHeader,
  Chip,
  Container,
  Grid,
  Typography,
  useMediaQuery
} from "@mui/material";
import theme from "../theme";

interface Nominations {
  personNominated: string;
  gw: number;
  date: string;
  reason: string;
  category: string;
}

const GetChipColor = (
  category: string
):
  | "default"
  | "primary"
  | "secondary"
  | "error"
  | "info"
  | "success"
  | "warning" => {
  switch (category) {
    case "SKILL":
      return "info";
    case "SHAME":
      return "error";
    case "DERP":
      return "warning";
    case "LUCK":
      return "success";
    default:
      return "default";
  }
};

const GetIcon = (category: string): string => {
  switch (category) {
    case "SKILL":
      return "⭐";
    case "SHAME":
      return "🤦";
    case "DERP":
      return "🥴";
    case "LUCK":
      return "🍀";
    default:
      return "";
  }
};

interface FilterNominationChipProps {
  category: string;
  filters: string[];
  setFilter: (value: React.SetStateAction<string[]>) => void;
}

const FilterNominationChip: React.FC<FilterNominationChipProps> = ({
  category,
  setFilter,
  filters
}) => (
  <Chip
    size="small"
    icon={<div style={{ fontSize: 13 }}>{GetIcon(category)}</div>}
    label={<Typography variant="body2">{category}</Typography>}
    color={GetChipColor(category)}
    style={{
      opacity: filters.length > 0 && !filters.includes(category) ? 0.38 : 1
    }}
    onClick={() => {
      setFilter((of) => {
        if (of.includes(category)) {
          return of.filter((item) => item !== category);
        }

        if (of.length > 2) {
          return [];
        }

        return [...of, category];
      });
    }}
  />
);

interface NominationChipProps {
  category: string;
}

const NominationChip: React.FC<NominationChipProps> = ({ category }) => (
  <Chip
    size="small"
    icon={<div style={{ fontSize: 13 }}>{GetIcon(category)}</div>}
    label={<Typography variant="body2">{category}</Typography>}
    color={GetChipColor(category)}
  />
);

const NominationsPage: React.FC<{}> = () => {
  const [nominations, setNominations] = useState<Nominations[]>([]);
  const [filters, setFilters] = useState<string[]>([]);
  const matchesXs = useMediaQuery(theme.breakpoints.down("sm"));

  useEffect(() => {
    const fetchNominations: () => Promise<void> = async () => {
      const response = await fetch(
        "https://api.github.com/repos/samsoncs/milabowl/contents/nominations.json?ref=content"
      );
      const json = await response.json();
      const nomsString = Buffer.from(json.content, "base64").toString();
      const noms: Nominations[] = JSON.parse(nomsString);
      setNominations(noms);
    };

    fetchNominations().catch((e) => console.log(e));
  }, []);
  return (
    <Container
      maxWidth="md"
      disableGutters
      style={{ marginLeft: "auto", marginRight: "auto" }}
    >
      <Grid container spacing={2}>
        <Box
          paddingLeft="16px"
          paddingTop="20px"
          display="flex"
          flexWrap="wrap"
          gap="8px"
        >
          <Typography>Filter:</Typography>
          <FilterNominationChip
            filters={filters}
            setFilter={setFilters}
            category="SKILL"
          />
          <FilterNominationChip
            filters={filters}
            setFilter={setFilters}
            category="SHAME"
          />
          <FilterNominationChip
            filters={filters}
            setFilter={setFilters}
            category="DERP"
          />
          <FilterNominationChip
            filters={filters}
            setFilter={setFilters}
            category="LUCK"
          />
        </Box>
        {nominations
          .filter((f) => filters.length === 0 || filters.includes(f.category))
          .map((n) => (
            <Grid
              item
              xs={12}
              key={`${n.personNominated} - GW ${n.gw} - ${n.category}`}
            >
              <Card>
                <CardHeader
                  title={
                    <Box display="flex" alignItems="center">
                      <Typography
                        fontWeight={700}
                        variant={matchesXs ? "body1" : "h5"}
                        style={{ marginRight: "10px", flexGrow: 1 }}
                      >{`GW ${n.gw} - ${n.personNominated}`}</Typography>
                      <div style={{ marginRight: "10px" }}>
                        <NominationChip category={n.category} />
                      </div>
                      <Typography variant="body2" color="text.secondary">
                        {n.date}
                      </Typography>
                    </Box>
                  }
                />
                <CardContent>{n.reason}</CardContent>
              </Card>
            </Grid>
          ))}
      </Grid>
    </Container>
  );
};

export default NominationsPage;
