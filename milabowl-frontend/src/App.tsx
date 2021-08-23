import React from 'react';
import {
  BrowserRouter as Router, Switch, Route, RouteComponentProps
} from 'react-router-dom';
import Container from '@material-ui/core/Container';
import DashboardPage from './Pages/DashboardPage';
import MilaPlayerPage from './Pages/MilaPlayerPage';
import StandingsPage from './Pages/StandingsPage';
import Navbar from './Navbar';
import theme from './theme';
import useMediaQuery from '@material-ui/core/useMediaQuery';

interface PrivateRouteProps {
  component: React.ComponentType<RouteComponentProps<any>> | React.ComponentType<any>;
  path: string;
  exact?: boolean;
  accessToken: string | undefined;
}
function App() {
  const isXsScreen = useMediaQuery(theme.breakpoints.down('xs'));

  return (
    <Router>
      <div className="App" style={{ background: theme.palette.background.default }}>
        <Navbar />
        <Container maxWidth="xl" disableGutters={isXsScreen} style={{ marginTop: '20px' }}>
          <Switch>
            <Route exact path="/" component={DashboardPage} />
            <Route path="/standings" component={StandingsPage} />
            <Route path="/mila-player/:teamname" component={MilaPlayerPage} />
          </Switch>
        </Container>
      </div>
    </Router>
  );
}

export default App;
