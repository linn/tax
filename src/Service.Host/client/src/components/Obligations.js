import React, { useState, Fragment, useEffect } from 'react';
import Button from '@material-ui/core/Button';

import PropTypes from 'prop-types';
import { InputField, Title, ErrorCard, Loading } from '@linn-it/linn-form-components-library';
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

function TaxReturn({ requestObligations, obligations, errorMessage, loading, profile }) {
    const [vrn, setVrn] = useState();

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
        setVrn(newValue);
    };

    if (obligations) {
        return (
            <Page width="m">
                <Grid item xs={12}>
                    <Title text="VAT Return Obligations" />
                </Grid>
                {obligations?.map?.(o => (
                    <div>
                        <Grid item xs={12}>
                            <InputField fullWidth value={o.start} label="Period Start" disabled />
                        </Grid>
                        <Grid item xs={12}>
                            <InputField fullWidth value={o.end} label="Period End" disabled />
                        </Grid>
                        <Grid item xs={12}>
                            <InputField fullWidth value={o.due} label="Due Date" disabled />
                        </Grid>
                        <Grid item xs={12}>
                            <InputField fullWidth value={o.periodKey} label="Period Key" disabled />
                        </Grid>
                    </div>
                ))}
            </Page>
        );
    }

    if (loading) {
        return (
            <Page width="m">
                <Loading />
            </Page>
        );
    }

    return (
        <Page width="m">
            <Grid container spacing={3}>
                <Grid item xs={12}>
                    <Title text="VAT Obligations" />
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
                        value={vrn}
                        label="9 Digit VAT Registration Number"
                        type="number"
                        maxLength={9}
                        required
                        onChange={handleFieldChange}
                        propertyName="vrn"
                    />
                </Grid>
                <Grid item xs={6} />
                <Grid item xs={2}>
                    <Button
                        className={{ float: 'right' }}
                        variant="outlined"
                        disabled={!vrn}
                        color="primary"
                        onClick={() => {
                            requestObligations({ ...metadata, vrn });
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
    requestObligations: PropTypes.func.isRequired,
    profile: PropTypes.shape({})
};

TaxReturn.defaultProps = {
    profile: null
};

export default TaxReturn;
