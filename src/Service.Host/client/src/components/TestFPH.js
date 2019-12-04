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

function TestFPH() {
    const [fraudPreventionHeaders, setFraudPreventionHeaders] = useState({
        doNotTrack: !!navigator?.doNotTrack,
        windowWidth: window.innerWidth,
        windowHeight: window.innerHeight,
        browserPlugins: null,
        userAgentString: navigator.userAgent,
        localIps: null,
        screenWidth: window.screen.width,
        screenHeight: window.screen.height,
        scalingFactor: window.devicePixelRatio,
        colourDepth: window.screen.colorDepth,
        timezoneOffset: new Date().getTimezoneOffset(),
        username: 'username'
    });

    const submitTestRequest = () => {
        const xhr = new XMLHttpRequest();
        xhr.open('POST', 'http://localhost:61798/test/fraud-prevention-headers');
        xhr.setRequestHeader('Content-Type', 'application/json');
        xhr.setRequestHeader('Accept', 'application/json',)

        xhr.send(JSON.stringify(fraudPreventionHeaders));
    };

    useEffect(() => {
        const plugins = [];
        for (let i = 0; i < navigator.plugins.length; i += 1) {
            plugins.push(navigator.plugins[i]);
        }
        setFraudPreventionHeaders(r => ({ ...r, browserPlugins: plugins.map(p => p.name) }));
        getLocalIPs(ipList => setFraudPreventionHeaders(r => ({ ...r, localIps: ipList })));
    }, [setFraudPreventionHeaders]);

    const handleFieldChange = (propertyName, newValue) => {
        setFraudPreventionHeaders({ ...fr, [propertyName]: newValue });
    };
    return (
        <Page width="m">
            <Grid container spacing={3}>
                <Grid item xs={12}>
                    <Title text="Fraud Prevention Headers" />
                </Grid>
                <Grid item xs={12}>
                    <InputField
                        fullWidth
                        value={fraudPreventionHeaders.userAgentString}
                        label="javascript user agent string"
                        maxLength={9}
                        required
                        onChange={handleFieldChange}
                        propertyName="userAgentString"
                    />
                </Grid>
                <Grid item xs={12}>
                    <InputField
                        fullWidth
                        value={fraudPreventionHeaders.doNotTrack}
                        label="Browser Do Not Track Settings"
                        maxLength={9}
                        required
                        onChange={handleFieldChange}
                        propertyName="doNotTrack"
                    />
                </Grid>
                <Grid item xs={12}>
                    <InputField
                        fullWidth
                        value={fraudPreventionHeaders.windowWidth}
                        label="Window Width"
                        maxLength={9}
                        required
                        onChange={handleFieldChange}
                        propertyName="windowWidth"
                    />
                </Grid>
                <Grid item xs={12}>
                    <InputField
                        fullWidth
                        value={fraudPreventionHeaders.windowHeight}
                        label="Window Height"
                        maxLength={9}
                        required
                        onChange={handleFieldChange}
                        propertyName="windowHeight"
                    />
                </Grid>
                <Grid item xs={12}>
                    <InputField
                        fullWidth
                        value={JSON.stringify(fraudPreventionHeaders.browserPlugins)}
                        label="List of Browser Plugins"
                        maxLength={9}
                        required
                        onChange={handleFieldChange}
                        propertyName="browserPlugins"
                    />
                </Grid>
                <Grid item xs={12}>
                    <InputField
                        fullWidth
                        value={JSON.stringify(fraudPreventionHeaders.localIps)}
                        label="List of Available Local IP addresses"
                        maxLength={9}
                        required
                        onChange={handleFieldChange}
                        propertyName="localIPs"
                    />
                </Grid>
                <Grid item xs={12}>
                    <InputField
                        fullWidth
                        value={fraudPreventionHeaders.screenWidth}
                        label="Screen Width"
                        maxLength={9}
                        required
                        onChange={handleFieldChange}
                        propertyName="screenWidth"
                    />
                </Grid>
                <Grid item xs={12}>
                    <InputField
                        fullWidth
                        value={fraudPreventionHeaders.screenHeight}
                        label="Screen Width"
                        maxLength={9}
                        required
                        onChange={handleFieldChange}
                        propertyName="screenHeight"
                    />
                </Grid>
                <Grid item xs={12}>
                    <InputField
                        fullWidth
                        value={fraudPreventionHeaders.scalingFactor}
                        label="Screen Scaling Factor"
                        maxLength={9}
                        required
                        onChange={handleFieldChange}
                        propertyName="scalingFactor"
                    />
                </Grid> 
                <Grid item xs={12}>
                    <InputField
                        fullWidth
                        value={fraudPreventionHeaders.colourDepth}
                        label="Screen Colour Depth"
                        maxLength={9}
                        required
                        onChange={handleFieldChange}
                        propertyName="colourDepth"
                    />
                </Grid>
                <Grid item xs={12}>
                    <InputField
                        fullWidth
                        value={fraudPreventionHeaders.timezoneOffset}
                        label="Timezone Offset in Minutes"
                        maxLength={9}
                        required
                        onChange={handleFieldChange}
                        propertyName="timezoneOffset"
                    />
                </Grid>
                <Grid item xs={12}>
                    <InputField
                        fullWidth
                        value={fraudPreventionHeaders.username}
                        label="Username"
                        maxLength={9}
                        required
                        onChange={handleFieldChange}
                        propertyName="username"
                    />
                </Grid>
                <Grid item xs={2}>
                    <Button
                        className={{ float: 'right' }}
                        variant="outlined"
                        color="primary"
                        onClick={() => {
                            submitTestRequest(fraudPreventionHeaders);
                        }}
                    >
                        Test Request
                    </Button>
                </Grid>
            </Grid>
        </Page>
    );
}

export default TestFPH;
