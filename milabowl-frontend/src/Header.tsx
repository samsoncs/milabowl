import React from "react";
import { Link, useLocation } from "react-router-dom";
import { AppBar, Toolbar, Typography, useMediaQuery } from "@mui/material";
import theme from "./theme";

const Header: React.FC<{}> = () => {
  const location = useLocation();
  const matchesXs = useMediaQuery(theme.breakpoints.down("sm"));

  return (
    <AppBar
      position="static"
      style={{
        boxShadow: "none",
        borderBottom: "none",
        paddingLeft: "0px",
        paddingRight: "0px"
      }}
    >
      <Toolbar disableGutters>
        <Typography
          color="textPrimary"
          style={{ textDecoration: "none" }}
          component={Link}
          to="/"
          variant="h5"
        >
          Milabowl
        </Typography>
        <section style={{ marginLeft: "auto" }}>
          <Typography
            color="textPrimary"
            style={{
              textDecoration: "none",
              fontWeight: 700,
              padding: "10px",
              borderRadius: "10px",
              background:
                location.pathname === "/blog" ? "rgba(0,0,0,0.05)" : "none"
            }}
            component={Link}
            to="blog"
          >
            Blog
          </Typography>
          {!matchesXs && (
            <Typography
              color="textPrimary"
              style={{
                textDecoration: "none",
                fontWeight: 700,
                padding: "10px",
                borderRadius: "10px",
                background:
                  location.pathname === "/accolades"
                    ? "rgba(0,0,0,0.05)"
                    : "none"
              }}
              component={Link}
              to="accolades"
            >
              Accolades
            </Typography>
          )}
          <Typography
            color="textPrimary"
            style={{
              textDecoration: "none",
              fontWeight: 700,
              padding: "10px",
              borderRadius: "10px",
              background:
                location.pathname === "/standings" ? "rgba(0,0,0,0.05)" : "none"
            }}
            component={Link}
            to="standings"
          >
            Standings
          </Typography>
        </section>
      </Toolbar>
    </AppBar>
  );
};

export default Header;
