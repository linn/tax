import React, { useState, Fragment, useEffect } from 'react';
import Typography from '@material-ui/core/Typography';
import Button from '@material-ui/core/Button';
import { makeStyles } from '@material-ui/styles';

import PropTypes from 'prop-types';
import {
    SnackbarMessage,
    InputField,
    Title,
    ErrorCard,
    Loading,
    OnOffSwitch
} from '@linn-it/linn-form-components-library';
import Grid from '@material-ui/core/Grid';

import Page from '../containers/Page';

function getLocalIPs(callback) {
    const ipList = [];
    const RTCPeerConnection =
        window.RTCPeerConnection || window.webkitRTCPeerConnection || window.mozRTCPeerConnection;

    const connection = new RTCPeerConnection({
        iceServers: []
    });
    connection.createDataChannel('');

    connection.onicecandidate = e => {
        if (!e.candidate) {
            connection.close();
            callback(ipList);
            return;
        }
        const ip = /^candidate:.+ (\S+) \d+ typ/.exec(e.candidate.candidate)[1];
        ipList.push(ip);
    };

    connection.createOffer(
        sdp => {
            connection.setLocalDescription(sdp);
        },
        function onerror() {}
    );
}

function TaxReturn({
    submitVatReturn,
    errorMessage,
    snackbarVisible,
    loading,
    hideSnackbar,
    receipt,
    profile
}) {
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
        finalised: false
    });

    const [metadata, setMetadata] = useState({
        doNotTrack: !!navigator?.doNotTrack,
        windowWidth: window.innerWidth,
        windowHeight: window.innerHeight,
        browserPlugins: null,
        userAgentString: navigator.userAgent,
        localIps: null,
        screenWidth: window.screen.width,
        screenHeight: window.screen.height,
        scalingFactor: window.devicePixelRatio,
        timezoneOffset: new Date().getTimezoneOffset(),
        username: profile?.preferred_username
    });

    useEffect(() => {
        const plugins = [];
        for (let i = 0; i < navigator.plugins.length; i += 1) {
            plugins.push(navigator.plugins[i]);
        }
        setMetadata(r => ({ ...r, browserPlugins: plugins.map(p => p.name) }));
        getLocalIPs(ipList => setMetadata(r => ({ ...r, localIps: ipList })));
    }, [setMetadata]);

    const handleFieldChange = (propertyName, newValue) => {
        setVatReturn({ ...vatReturn, [propertyName]: newValue });
    };

    if (loading) {
        return (
            <Page width="m">
                <Loading />
            </Page>
        );
    }

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
                    <InputField
                        fullWidth
                        value={receipt.processingDate}
                        label="Processing Date"
                        disabled
                    />
                </Grid>
                <Grid item xs={12}>
                    <InputField
                        fullWidth
                        value={receipt.formBundleNumber}
                        label="Form Bundle Number"
                        disabled
                    />
                </Grid>
                <Grid item xs={12}>
                    <InputField
                        fullWidth
                        value={receipt.paymentIndicator}
                        label="Payment Indicator"
                        disabled
                    />
                </Grid>
                <Grid item xs={12}>
                    <InputField
                        fullWidth
                        value={receipt.chargeRefNumber}
                        label="Charge Reference Number"
                        disabled
                    />
                </Grid>
            </Page>
        );
    }

    const inputInvalid = () =>
        Object.keys(vatReturn)
            .filter(k => k !== 'finalised')
            .some(k => !vatReturn[k]);
    return (
        <Page width="m">
            <Grid container spacing={3}>
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
                        label="Total Value of Purchases Excluding VAT"
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
                <Grid item xs={4}>
                    <OnOffSwitch
                        label="Finalised?"
                        value={vatReturn.finalised}
                        onChange={() => setVatReturn(r => ({ ...r, finalised: !r.finalised }))}
                        propertyName="monthly"
                    />
                </Grid>
                <Grid item xs={6} />
                <Grid item xs={2}>
                    <Button
                        className={{ float: 'right' }}
                        variant="outlined"
                        disabled={inputInvalid()}
                        color="primary"
                        onClick={() => {
                            submitVatReturn({ ...vatReturn, ...metadata });
                        }}
                    >
                        Submit
                    </Button>
                </Grid>
            </Grid>
        </Page>
    );
}

TaxReturn.propTypes = {
    submitVatReturn: PropTypes.func.isRequired,
    profile: PropTypes.shape({})
};

TaxReturn.defaultProps = {
    profile: null
};

export default TaxReturn;
