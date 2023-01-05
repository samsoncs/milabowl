import React from "react";
import "./index.css";
import { createHashRouter, Outlet } from "react-router-dom";
import DashboardPage from "./Dashboard/DashboardPage";
import Container from "@mui/material/Container";
import Header from "./Header";
import StandingsPage from "./Standings/StandingsPage";
import MilaPlayerPage from "./Players/MilaPlayerPage";
import { ThemeProvider } from "@mui/material";
import AccoladesPage from "./Accolades/AccoladesPage";
import BlogPage from "./Blog/BlogPage";
import theme from "./theme";
import NominationsPage from "./Nominations/NominationsPage";

const Layout: React.FC<{}> = () => (
  <ThemeProvider theme={theme}>
    <Container maxWidth="xl" disableGutters>
      <Header />
      <Outlet />
    </Container>
  </ThemeProvider>
);

const router = createHashRouter([
  {
    element: <Layout />,
    children: [
      {
        path: "/",
        element: <DashboardPage />
      },
      {
        path: "/standings",
        element: <StandingsPage />
      },
      {
        path: "/mila-player/:teamname",
        element: <MilaPlayerPage />
      },
      {
        path: "/accolades",
        element: <AccoladesPage />
      },
      {
        path: "/blog",
        element: <BlogPage />
      },
      {
        path: "/nominations",
        element: <NominationsPage />
      }
    ]
  }
]);

export default router;
