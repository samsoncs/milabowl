import React, { useState, useEffect } from 'react';
import {
  BrowserRouter as Router, Switch, Route, Redirect, RouteComponentProps
} from 'react-router-dom';
import Container from '@material-ui/core/Container';
import DashboardPage from './Pages/DashboardPage';
import ProfilePage from './Pages/ProfilePage';
import MilaPlayerPage from './Pages/MilaPlayerPage';
import StandingsPage from './Pages/StandingsPage';
import LoginPage from './Pages/LoginPage';
import Navbar from './Navbar';
import theme from './theme';
import { getAccessTokenFromCache } from './Api/AuthService';
import AdminPage from './Pages/AdminPage';
import useMediaQuery from '@material-ui/core/useMediaQuery';

interface PrivateRouteProps {
  component: React.ComponentType<RouteComponentProps<any>> | React.ComponentType<any>;
  path: string;
  exact?: boolean;
  accessToken: string | undefined;
}

const PrivateRoute = ({
  component, path, exact, accessToken,
}: PrivateRouteProps) => (
    <>
      {accessToken && <Route path={path} component={component} exact={exact} />}
      {!accessToken && <Redirect to="login" />}
    </>
  );

function App() {
  const [accessToken, setAccessToken] = useState<string | undefined>();
  const [isLoading, setIsLoading] = useState(true);
  const isXsScreen = useMediaQuery(theme.breakpoints.down('xs'));

  useEffect(() => {
    const init = async (): Promise<void> => {
      setIsLoading(true);
      const tokenFromCache = await getAccessTokenFromCache();
      setAccessToken(tokenFromCache);
      setIsLoading(false);
    };

    init();
  }, []);

  return (
    <Router>
      <div className="App" style={{ background: theme.palette.background.default }}>
        <Navbar setAccessToken={setAccessToken} accessToken={accessToken} />
        <Container maxWidth="xl" disableGutters={isXsScreen} style={{ marginTop: '20px' }}>
          <Switch>
            <Route accessToken={accessToken} exact path="/login" component={LoginPage} />

            {!isLoading && (
              <>
                <PrivateRoute accessToken={accessToken} exact path="/" component={DashboardPage} />
                <PrivateRoute accessToken={accessToken} path="/standings" component={StandingsPage} />
                <PrivateRoute accessToken={accessToken} path="/profile" component={ProfilePage} />
                <PrivateRoute accessToken={accessToken} path="/admin" component={AdminPage} />
                <PrivateRoute accessToken={accessToken} path="/mila-player/:teamname" component={MilaPlayerPage} />
              </>
            )}
          </Switch>
        </Container>
      </div>
    </Router>
  );
}

export default App;
