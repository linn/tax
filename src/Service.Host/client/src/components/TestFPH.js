import PropTypes from 'prop-types';
import React from 'react';
import Button from '@material-ui/core/Button';

import { InputField, Title } from '@linn-it/linn-form-components-library';
import Grid from '@material-ui/core/Grid';
import config from '../config';

import Page from '../containers/Page';

import useFraudPreventionHeaders from '../hooks/useFraudPreventionHeaders';

function TestFPH({ profile }) {
    // eslint-disable-next-line camelcase
    const fraudPreventionHeaders = useFraudPreventionHeaders(profile?.preferred_username);
    const submitTestRequest = () => {
        const xhr = new XMLHttpRequest();
        xhr.open('POST', `${config.appRoot}/tax/test/fraud-prevention-headers`);
        xhr.setRequestHeader('Content-Type', 'application/json');
        xhr.setRequestHeader('Accept', 'application/json');

        xhr.send(JSON.stringify(fraudPreventionHeaders));
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
                        disabled
                        propertyName="userAgentString"
                    />
                </Grid>
                <Grid item xs={12}>
                    <InputField
                        fullWidth
                        value={fraudPreventionHeaders.deviceId}
                        label="Device Id"
                        maxLength={9}
                        disabled
                        propertyName="deviceId"
                    />
                </Grid>
                <Grid item xs={12}>
                    <InputField
                        fullWidth
                        value={fraudPreventionHeaders.doNotTrack}
                        label="Browser Do Not Track Settings"
                        maxLength={9}
                        disabled
                        propertyName="doNotTrack"
                    />
                </Grid>
                <Grid item xs={12}>
                    <InputField
                        fullWidth
                        value={fraudPreventionHeaders.windowWidth}
                        label="Window Width"
                        maxLength={9}
                        disabled
                        propertyName="windowWidth"
                    />
                </Grid>
                <Grid item xs={12}>
                    <InputField
                        fullWidth
                        value={fraudPreventionHeaders.windowHeight}
                        label="Window Height"
                        maxLength={9}
                        disabled
                        propertyName="windowHeight"
                    />
                </Grid>
                <Grid item xs={12}>
                    <InputField
                        fullWidth
                        value={JSON.stringify(fraudPreventionHeaders.browserPlugins)}
                        label="List of Browser Plugins"
                        maxLength={9}
                        disabled
                        propertyName="browserPlugins"
                    />
                </Grid>
                <Grid item xs={12}>
                    <InputField
                        fullWidth
                        value={JSON.stringify(fraudPreventionHeaders.localIps)}
                        label="List of Available Local IP addresses"
                        maxLength={9}
                        disabled
                        propertyName="localIPs"
                    />
                </Grid>
                <Grid item xs={12}>
                    <InputField
                        fullWidth
                        value={fraudPreventionHeaders.localIpsTimestamp}
                        label="Time when local Ips were detected"
                        maxLength={9}
                        disabled
                        propertyName="localIpsTimestamp"
                    />
                </Grid>
                <Grid item xs={12}>
                    <InputField
                        fullWidth
                        value={fraudPreventionHeaders.screenWidth}
                        label="Screen Width"
                        maxLength={9}
                        disabled
                        propertyName="screenWidth"
                    />
                </Grid>
                <Grid item xs={12}>
                    <InputField
                        fullWidth
                        value={fraudPreventionHeaders.screenHeight}
                        label="Screen Width"
                        maxLength={9}
                        disabled
                        propertyName="screenHeight"
                    />
                </Grid>
                <Grid item xs={12}>
                    <InputField
                        fullWidth
                        value={fraudPreventionHeaders.scalingFactor}
                        label="Screen Scaling Factor"
                        maxLength={9}
                        disabled
                        propertyName="scalingFactor"
                    />
                </Grid>
                <Grid item xs={12}>
                    <InputField
                        fullWidth
                        value={fraudPreventionHeaders.colourDepth}
                        label="Screen Colour Depth"
                        maxLength={9}
                        disabled
                        propertyName="colourDepth"
                    />
                </Grid>
                <Grid item xs={12}>
                    <InputField
                        fullWidth
                        value={fraudPreventionHeaders.timezoneOffset}
                        label="Timezone Offset in Minutes"
                        maxLength={9}
                        disabled
                        propertyName="timezoneOffset"
                    />
                </Grid>
                <Grid item xs={12}>
                    <InputField
                        fullWidth
                        value={fraudPreventionHeaders.username}
                        label="Username"
                        maxLength={9}
                        disabled
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

TestFPH.propTypes = {
    profile: PropTypes.shape({ preferred_username: PropTypes.string }).isRequired
};

export default TestFPH;
