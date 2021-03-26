import React, { useState, Fragment } from 'react';
import Button from '@material-ui/core/Button';

import PropTypes from 'prop-types';
import { InputField, Title, ErrorCard, Loading } from '@linn-it/linn-form-components-library';
import Grid from '@material-ui/core/Grid';
import { Link } from 'react-router-dom';
import Page from '../containers/Page';
import useFraudPreventionHeaders from '../hooks/useFraudPreventionHeaders';

function TaxReturn({ requestObligations, obligations, errorMessage, loading, profile }) {
    const [vrn, setVrn] = useState();

    // eslint-disable-next-line camelcase
    const metadata = useFraudPreventionHeaders(profile?.preferred_username);

    const handleFieldChange = (propertyName, newValue) => {
        setVrn(newValue);
    };

    if (obligations) {
        return (
            <Page width="m">
                <Grid item xs={12}>
                    <Title text="Outstanding VAT Returns" />
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
                        <Link to={`/tax/submit-return/${o.periodKey}`}>Resolve</Link>
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
    obligations: PropTypes.arrayOf(PropTypes.shape),
    loading: PropTypes.bool,
    requestObligations: PropTypes.func.isRequired,
    profile: PropTypes.shape({ preferred_username: PropTypes.string }),
    errorMessage: PropTypes.string
};

TaxReturn.defaultProps = {
    profile: null,
    obligations: null,
    loading: false,
    errorMessage: ''
};

export default TaxReturn;
