import React, { useState } from 'react';
import AppBar from '@material-ui/core/AppBar';
import Toolbar from '@material-ui/core/Toolbar';
import { Link, withRouter, RouteComponentProps } from 'react-router-dom';
import { Container, Typography, Button, IconButton, Menu, MenuItem } from '@material-ui/core';
import theme from './theme';
import { login, logout, isMilabowlAdmin } from './Api/AuthService';
import Person from '@material-ui/icons/Person'

interface NavbarProps{
  setAccessToken: React.Dispatch<React.SetStateAction<string | undefined>>;
  accessToken: string | undefined;
}

interface MenuButtonProps{
  accessToken: string;
}

const MenuButton = ({ accessToken }: MenuButtonProps) => {
  const [anchorEl, setAnchorEl] = useState<Element | ((element: Element) => Element) | null | undefined>(null);
  return(
    <>
      <IconButton onClick={(e) => { setAnchorEl(e.currentTarget);}}>
        <Person color="secondary"/>
      </IconButton>
      <Menu
        anchorEl={anchorEl}
        open={Boolean(anchorEl)}
        keepMounted
        onClose={() => setAnchorEl(null)}
      >
        <MenuItem onClick={() => setAnchorEl(null)} component={Link} to="/profile">Profile</MenuItem>
        { isMilabowlAdmin(accessToken) && <MenuItem onClick={() => setAnchorEl(null)} component={Link} to="/admin">Admin</MenuItem> }
        <MenuItem onClick={() => logout()}>Log Out</MenuItem>
      </Menu>
    </>
)};

const Navbar = ({
  location, history, setAccessToken, accessToken,
}: NavbarProps & RouteComponentProps) => {
  const [isLoading, setIsLoading] = useState<boolean>(false);

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
              {
                !accessToken && !isLoading
                && <Button onClick={async () => {
                  setIsLoading(true);
                  const token = await login();
                  setAccessToken(token);
                  setIsLoading(false);
                  history.push('/');
                }}>
                    Log in
                </Button>
              }
              {
                accessToken && !isLoading
                && (
                  <MenuButton accessToken={accessToken}/> 
                )
              }
            </section>
        </Toolbar>
      </Container>
  </AppBar>
  );
};

export default withRouter(Navbar);
