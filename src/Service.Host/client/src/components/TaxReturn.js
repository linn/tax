import React, { useState } from 'react';
import Typography from '@material-ui/core/Typography';
import Button from '@material-ui/core/Button';
import PropTypes from 'prop-types';
import Page from '../containers/Page';

function TaxReturn({ submitVatReturn }) {
    const [vatReturn, setVatReturn] = useState({
        vrn: 267836612,
        periodKey: 'A001',
        vatDueSales: 105.5,
        vatDueAcquisitions: -100.45,
        totalVatDue: 5.05,
        vatReclaimedCurrPeriod: 105.15,
        netVatDue: 100.1,
        totalValueSalesExVAT: 300,
        totalValuePurchasesExVAT: 300,
        totalValueGoodsSuppliedExVAT: 3000,
        totalAcquisitionsExVAT: 3000,
        finalised: true
    });

    return (
        <Page>
            <Typography variant="h6">Tax Return Form </Typography>
            <Button onClick={() => submitVatReturn(vatReturn)}> Submit </Button>
        </Page>
    );
}

TaxReturn.propTypes = {
    submitVatReturn: PropTypes.func.isRequired
};

export default TaxReturn;
