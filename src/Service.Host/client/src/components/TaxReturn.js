import React, { useState, useEffect, Fragment } from 'react';
import Typography from '@material-ui/core/Typography';
import Button from '@material-ui/core/Button';
import PropTypes from 'prop-types';
import {
    SnackbarMessage,
    InputField,
    Title,
    ErrorCard,
    OnOffSwitch
} from '@linn-it/linn-form-components-library';
import Grid from '@material-ui/core/Grid';
import CalculationValues from '../containers/CalculationValues';
import Page from '../containers/Page';
import useFraudPreventionHeaders from '../hooks/useFraudPreventionHeaders';

function TaxReturn({
    submitVatReturn,
    errorMessage,
    snackbarVisible,
    hideSnackbar,
    receipt,
    match,
    profile,
    figures
}) {
    const [vatReturn, setVatReturn] = useState({
        periodKey: match?.params?.periodKey,
        vatDueSales: figures?.vatDueSales,
        vatDueAcquisitions: null,
        totalVatDue: null,
        vatReclaimedCurrPeriod: null,
        netVatDue: null,
        totalValueSalesExVat: null,
        totalValuePurchasesExVat: null,
        totalValueGoodsSuppliedExVat: null,
        totalAcquisitionsExVat: null,
        finalised: false
    });

    useEffect(() => {
        setVatReturn({ ...figures, periodKey: match?.params?.periodKey });
    }, [figures, match]);

    // eslint-disable-next-line camelcase
    const metadata = useFraudPreventionHeaders(profile?.preferred_username);

    const handleFieldChange = (propertyName, newValue) => {
        setVatReturn({ ...vatReturn, [propertyName]: newValue });
    };

    if (receipt) {
        return (
            <Page width="m">
                <SnackbarMessage
                    visible={snackbarVisible}
                    onClose={hideSnackbar}
                    message="Save Successful"
                />
                <Grid item xs={12}>
                    <Title text="VAT Return Confirmation" />
                </Grid>
                <Grid item xs={12}>
                    <InputField value={receipt?.processingDate} label="Processing Date" disabled />
                </Grid>
                <Grid item xs={12}>
                    <InputField
                        value={receipt?.formBundleNumber}
                        label="Form Bundle Number"
                        disabled
                    />
                </Grid>
                <Grid item xs={12}>
                    <InputField
                        value={receipt?.paymentIndicator}
                        label="Payment Indicator"
                        disabled
                    />
                </Grid>
                <Grid item xs={12}>
                    <InputField
                        value={receipt?.chargeRefNumber}
                        label="Charge Reference Number"
                        disabled
                    />
                </Grid>
                <Grid item xs={12}>
                    <Typography variant="subtitle" gutterBottom>
                        Take a note of this data for your records.
                    </Typography>
                </Grid>
            </Page>
        );
    }

    const inputInvalid = () =>
        Object.keys(vatReturn).some(
            k => vatReturn[k] === null || vatReturn[k] === '' || vatReturn[k] === false
        );
    return (
        <Page>
            <Grid container spacing={3}>
                <CalculationValues />
                {figures && (
                    <>
                        <Grid item xs={12}>
                            <Title text="VAT Return Form" />
                        </Grid>
                        {errorMessage ? (
                            <Grid item xs={12}>
                                <ErrorCard errorMessage={errorMessage} />
                            </Grid>
                        ) : (
                            // eslint-disable-next-line
                            <Fragment />
                        )}
                        <Grid item xs={12}>
                            <Typography variant="subtitle" gutterBottom>
                                When you submit this VAT information you are making a legal
                                declaration that the information is true and complete. A false
                                declaration can result in prosecution.
                            </Typography>
                        </Grid>
                        <Grid item xs={12}>
                            <InputField
                                value={vatReturn.vrn}
                                label=" 9 Digit VAT Registration Number"
                                type="number"
                                maxLength={9}
                                required
                                onChange={handleFieldChange}
                                propertyName="vrn"
                                disabled={vatReturn.finalised}
                            />
                        </Grid>
                        <Grid item xs={12}>
                            <InputField
                                value={vatReturn.vatDueSales}
                                label="VAT Due On Sales"
                                type="number"
                                required
                                onChange={handleFieldChange}
                                propertyName="vatDueSales"
                                disabled={vatReturn.finalised}
                            />
                        </Grid>
                        <Grid item xs={12}>
                            <InputField
                                type="number"
                                value={vatReturn.vatDueAcquisitions}
                                label="VAT Due On Acquisitions"
                                required
                                onChange={handleFieldChange}
                                propertyName="vatDueAcquisitions"
                                disabled={vatReturn.finalised}
                            />
                        </Grid>
                        <Grid item xs={12}>
                            <InputField
                                type="number"
                                value={vatReturn.totalVatDue}
                                label="Total VAT Due"
                                required
                                onChange={handleFieldChange}
                                propertyName="totalVatDue"
                                disabled={vatReturn.finalised}
                            />
                        </Grid>
                        <Grid item xs={12}>
                            <InputField
                                type="number"
                                value={vatReturn.vatReclaimedCurrPeriod}
                                label="VAT Reclaimed in Current Period"
                                required
                                onChange={handleFieldChange}
                                propertyName="vatReclaimedCurrPeriod"
                                disabled={vatReturn.finalised}
                            />
                        </Grid>
                        <Grid item xs={12}>
                            <InputField
                                type="number"
                                value={vatReturn.netVatDue}
                                label="Net VAT due"
                                required
                                onChange={handleFieldChange}
                                propertyName="netVatDue"
                                disabled={vatReturn.finalised}
                            />
                        </Grid>
                        <Grid item xs={12}>
                            <InputField
                                type="number"
                                value={vatReturn.totalValueSalesExVat}
                                label="Total Value of Sales Excluding VAT"
                                required
                                onChange={handleFieldChange}
                                propertyName="totalValueSalesExVat"
                                helperText="Omit decimal places for this box."
                                disabled={vatReturn.finalised}
                            />
                        </Grid>
                        <Grid item xs={12}>
                            <InputField
                                type="number"
                                value={vatReturn.totalValuePurchasesExVat}
                                label="Total Value of Purchases Excluding VAT"
                                required
                                onChange={handleFieldChange}
                                propertyName="totalValuePurchasesExVat"
                                helperText="Omit decimal places for this box."
                                disabled={vatReturn.finalised}
                            />
                        </Grid>
                        <Grid item xs={12}>
                            <InputField
                                type="number"
                                value={vatReturn.totalValueGoodsSuppliedExVat}
                                label="Total Value of Goods Suppled Excluding VAT"
                                required
                                onChange={handleFieldChange}
                                propertyName="totalValueGoodsSuppliedExVat"
                                helperText="Omit decimal places for this box."
                                disabled={vatReturn.finalised}
                            />
                        </Grid>
                        <Grid item xs={12}>
                            <InputField
                                type="number"
                                value={vatReturn.totalAcquisitionsExVat}
                                label="Total Acquisitions Excluding VAT"
                                required
                                helperText="Omit decimal places for this box."
                                onChange={handleFieldChange}
                                propertyName="totalAcquisitionsExVat"
                                disabled={vatReturn.finalised}
                            />
                        </Grid>
                        <Grid item xs={10}>
                            <OnOffSwitch
                                label="Finalised? (Double check the figures before marking as finalised.)"
                                value={vatReturn.finalised}
                                onChange={() =>
                                    setVatReturn(r => ({ ...r, finalised: !r.finalised }))
                                }
                                propertyName="monthly"
                            />
                        </Grid>
                        <Grid item xs={2}>
                            <Button
                                className={{ float: 'right' }}
                                variant="outlined"
                                disabled={inputInvalid()}
                                color="primary"
                                onClick={() => {
                                    metadata.localIpsTimestamp = new Date().toISOString();
                                    submitVatReturn({ ...vatReturn, ...metadata });
                                }}
                            >
                                Submit
                            </Button>
                        </Grid>
                    </>
                )}
            </Grid>
        </Page>
    );
}

TaxReturn.propTypes = {
    submitVatReturn: PropTypes.func.isRequired,
    profile: PropTypes.shape({ preferred_username: PropTypes.string }),
    errorMessage: PropTypes.string,
    snackbarVisible: PropTypes.bool,
    hideSnackbar: PropTypes.func.isRequired,
    receipt: PropTypes.shape({
        chargeRefNumber: PropTypes.string,
        paymentIndicator: PropTypes.string,
        processingDate: PropTypes.string,
        formBundleNumber: PropTypes.string
    }),
    match: PropTypes.shape({ params: PropTypes.shape({ periodKey: PropTypes.string }) }),
    figures: PropTypes.shape({ vatDueSales: PropTypes.number })
};

TaxReturn.defaultProps = {
    profile: null,
    errorMessage: null,
    snackbarVisible: false,
    receipt: null,
    match: null,
    figures: null
};

export default TaxReturn;
