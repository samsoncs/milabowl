import React from 'react';
import AppBar from '@material-ui/core/AppBar';
import Toolbar from '@material-ui/core/Toolbar';
import { Link, withRouter, RouteComponentProps } from 'react-router-dom';
import { Container, Typography, Button, IconButton, Menu, MenuItem } from '@material-ui/core';
import theme from './theme';

const Navbar = ({
  location
}: RouteComponentProps) => {
  return (
    <AppBar position="static" style={{ background: theme.palette.background.default, boxShadow: 'none', borderBottom: 'none' }}>
      <Container maxWidth="xl">
        <Toolbar disableGutters>
            <Typography color="textPrimary" style={{ textDecoration: 'none' }} component={Link} to="/" variant="h5">
              Milabowl
            </Typography>
            <section style={{ marginLeft: 'auto' }}>
              <Typography color="textPrimary" style={{
                textDecoration: 'none',
                fontWeight: 700,
                padding: '10px',
                borderRadius: '10px',
                background: location.pathname === '/standings' ? 'rgba(0,0,0,0.05)' : 'none',
              }} component={Link} to="standings">
                Standings
              </Typography>      
            </section>
        </Toolbar>
      </Container>
  </AppBar>
  );
};

export default withRouter(Navbar);
