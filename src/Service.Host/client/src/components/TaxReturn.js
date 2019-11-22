import React, { useState } from 'react';
import Typography from '@material-ui/core/Typography';
import Button from '@material-ui/core/Button';
import { makeStyles } from '@material-ui/styles';

import PropTypes from 'prop-types';
import { SnackbarMessage, InputField } from '@linn-it/linn-form-components-library';
import Grid from '@material-ui/core/Grid';

import Page from '../containers/Page';

function TaxReturn({ submitVatReturn }) {
    const [vatReturn, setVatReturn] = useState({
        vrn: 187045351,
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

    const handleFieldChange = (propertyName, newValue) => {
        setVatReturn({ ...vatReturn, [propertyName]: newValue });
    };

    return (
        <Page width="m">
            {/* <SnackbarMessage
                visible={snackbarVisible}
                onClose={() => setSnackbarVisible(false)}
                message="Save Successful"
            /> */}
            <Grid container spacing={3}>
                <Grid item xs={12}>
                    <Typography variant="h6">VAT Return Form </Typography>
                </Grid>
                <Grid item xs={12}>
                    <InputField
                        fullWidth
                        value={vatReturn.periodKey}
                        label="Period Key"
                        required
                        onChange={handleFieldChange}
                        propertyName="periodKey"
                    />
                </Grid>
                <Grid item xs={12}>
                    <InputField
                        fullWidth
                        value={vatReturn.vatDueSales}
                        label="VAT Due Sales"
                        required
                        onChange={handleFieldChange}
                        propertyName="vatDueSales"
                    />
                </Grid>
                <Grid item xs={12}>
                    <InputField
                        fullWidth
                        value={vatReturn.vatDueAcquisitions}
                        label="VAT Due Acquisitions"
                        required
                        onChange={handleFieldChange}
                        propertyName="vatDueAcquisitions"
                    />
                </Grid>
                <Grid item xs={12}>
                    <InputField
                        fullWidth
                        value={vatReturn.totalVatDue}
                        label="Total VAT Due"
                        required
                        onChange={handleFieldChange}
                        propertyName="totalVatDue"
                    />
                </Grid>
                <Grid item xs={12}>
                    <InputField
                        fullWidth
                        value={vatReturn.vatReclaimedCurrPeriod}
                        label="VAT Reclaimed Current Period"
                        required
                        onChange={handleFieldChange}
                        propertyName="vatReclaimedCurrPeriod"
                    />
                </Grid>
                <Grid item xs={12}>
                    <InputField
                        fullWidth
                        value={vatReturn.netVatDue}
                        label="Net VAT due"
                        required
                        onChange={handleFieldChange}
                        propertyName="netVatDue"
                    />
                </Grid>
                <Grid item xs={12}>
                    <InputField
                        fullWidth
                        value={vatReturn.totalValueSalesExVAT}
                        label="Total Value Sales Ex VAT"
                        required
                        onChange={handleFieldChange}
                        propertyName="totalValueSalesExVAT"
                    />
                </Grid>
                <Grid item xs={12}>
                    <InputField
                        fullWidth
                        value={vatReturn.totalValuePurchasesExVAT}
                        label="Total value Purchases Ex VAT"
                        required
                        onChange={handleFieldChange}
                        propertyName="totalValuePurchasesExVAT"
                    />
                </Grid>
                <Grid item xs={12}>
                    <InputField
                        fullWidth
                        value={vatReturn.totalValueGoodsSuppliedExVAT}
                        label="Total Value Goods Suppled Ex VAT"
                        required
                        onChange={handleFieldChange}
                        propertyName="totalValueGoodsSuppliedExVAT"
                    />
                </Grid>
                <Grid item xs={12}>
                    <InputField
                        fullWidth
                        value={vatReturn.totalAcquisitionsExVAT}
                        label="Total Acquisitions Ex VAT"
                        required
                        onChange={handleFieldChange}
                        propertyName="totalAcquisitionsExVAT"
                    />
                </Grid>
                <Grid item xs={12}>
                    <InputField
                        fullWidth
                        value={vatReturn.finalised}
                        label="Finalised"
                        required
                        onChange={handleFieldChange}
                        propertyName="finalised"
                    />
                </Grid>
                <Grid item xs={10} />
                <Grid item xs={2}>
                    <Button
                        className={{ float: 'right' }}
                        variant="outlined"
                        color="primary"
                        onClick={() => submitVatReturn(vatReturn)}
                    >
                        Submit
                    </Button>
                </Grid>
            </Grid>
        </Page>
    );
}

TaxReturn.propTypes = {
    submitVatReturn: PropTypes.func.isRequired
};

export default TaxReturn;
