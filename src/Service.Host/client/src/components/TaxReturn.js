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
        periodKey: null,
        vatDueSales: null,
        vatDueAcquisitions: null,
        totalVatDue: null,
        vatReclaimedCurrPeriod: null,
        netVatDue: null,
        totalValueSalesExVAT: null,
        totalValuePurchasesExVAT: null,
        totalValueGoodsSuppliedExVAT: null,
        totalAcquisitionsExVAT: null,
        finalised: null
    });

    const handleFieldChange = (propertyName, newValue) => {
        setVatReturn({ ...vatReturn, [propertyName]: newValue });
    };

    const inputInvalid = () => Object.keys(vatReturn).some(k => !vatReturn[k]);
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
                        value={vatReturn.vrn}
                        label=" 9 Digit VAT Registration Number"
                        type="number"
                        maxLength={9}
                        required
                        onChange={handleFieldChange}
                        propertyName="vrn"
                    />
                </Grid>
                <Grid item xs={12}>
                    <InputField
                        fullWidth
                        value={vatReturn.periodKey}
                        label="Period Key"
                        maxLength={4}
                        required
                        onChange={handleFieldChange}
                        propertyName="periodKey"
                    />
                </Grid>
                <Grid item xs={12}>
                    <InputField
                        fullWidth
                        value={vatReturn.vatDueSales}
                        label="VAT Due On Sales"
                        type="number"
                        required
                        onChange={handleFieldChange}
                        propertyName="vatDueSales"
                    />
                </Grid>
                <Grid item xs={12}>
                    <InputField
                        fullWidth
                        type="number"
                        value={vatReturn.vatDueAcquisitions}
                        label="VAT Due On Acquisitions"
                        required
                        onChange={handleFieldChange}
                        propertyName="vatDueAcquisitions"
                    />
                </Grid>
                <Grid item xs={12}>
                    <InputField
                        fullWidth
                        type="number"
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
                        type="number"
                        value={vatReturn.vatReclaimedCurrPeriod}
                        label="VAT Reclaimed in Current Period"
                        required
                        onChange={handleFieldChange}
                        propertyName="vatReclaimedCurrPeriod"
                    />
                </Grid>
                <Grid item xs={12}>
                    <InputField
                        fullWidth
                        type="number"
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
                        type="number"
                        value={vatReturn.totalValueSalesExVAT}
                        label="Total Value of Sales Excluding VAT"
                        required
                        onChange={handleFieldChange}
                        propertyName="totalValueSalesExVAT"
                    />
                </Grid>
                <Grid item xs={12}>
                    <InputField
                        fullWidth
                        type="number"
                        value={vatReturn.totalValuePurchasesExVAT}
                        label="Total nullalue of Purchases Excluding VAT"
                        required
                        onChange={handleFieldChange}
                        propertyName="totalValuePurchasesExVAT"
                    />
                </Grid>
                <Grid item xs={12}>
                    <InputField
                        fullWidth
                        type="number"
                        value={vatReturn.totalValueGoodsSuppliedExVAT}
                        label="Total Value of Goods Suppled Excluding VAT"
                        required
                        onChange={handleFieldChange}
                        propertyName="totalValueGoodsSuppliedExVAT"
                    />
                </Grid>
                <Grid item xs={12}>
                    <InputField
                        fullWidth
                        type="number"
                        value={vatReturn.totalAcquisitionsExVAT}
                        label="Total Acquisitions Excluding VAT"
                        required
                        onChange={handleFieldChange}
                        propertyName="totalAcquisitionsExVAT"
                    />
                </Grid>
                <Grid item xs={12}>
                    {/* todo - true false */}
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
                        disabled={inputInvalid()}
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
