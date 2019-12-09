import React from 'react';
import { Link } from 'react-router-dom';
import List from '@material-ui/core/List';
import ListItem from '@material-ui/core/ListItem';
import Typography from '@material-ui/core/Typography';
import Page from '../containers/Page';

function App() {
    return (
        <Page>
            <Typography variant="h6">Tax</Typography>
            <List>
                <a href="http://localhost:61798/tax/auth">
                    <ListItem button>
                        <Typography color="primary">Submit a tax return</Typography>
                    </ListItem>
                </a>
            </List>
        </Page>
    );
}

export default App;
