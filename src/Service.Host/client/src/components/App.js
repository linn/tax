import React from 'react';
import List from '@material-ui/core/List';
import ListItem from '@material-ui/core/ListItem';
import Typography from '@material-ui/core/Typography';
import Page from '../containers/Page';
import config from '../config';

function App() {
    return (
        <Page>
            <Typography variant="h6">Tax</Typography>
            <List>
                <a href={`${config.appRoot}/tax/auth`}>
                    <ListItem button>
                        <Typography color="primary">VAT Returns</Typography>
                    </ListItem>
                </a>
            </List>
        </Page>
    );
}

export default App;
