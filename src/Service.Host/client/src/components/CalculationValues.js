import React, { useState, useEffect, Fragment } from 'react';
import Typography from '@material-ui/core/Typography';
import Button from '@material-ui/core/Button';

import PropTypes from 'prop-types';
import { InputField, Title, ErrorCard, Loading } from '@linn-it/linn-form-components-library';
import Grid from '@material-ui/core/Grid';

import Page from '../containers/Page';

function CalculationValues({ item, errorMessage, loading }) {
    const [calculationValues, setcalculationValues] = useState({
        SalesGoodsTotal: item?.salesGoodsTotal
    });

    useEffect(() => {
        setcalculationValues({ ...item });
    }, [item]);

    const handleFieldChange = (propertyName, newValue) => {
        setcalculationValues({ ...calculationValues, [propertyName]: newValue });
    };

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
                    <Title text="VAT Return Calculation Breakdown" />
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
                        Check these numbers against manual calculation and ammend if necessary.
                    </Typography>
                </Grid>
                <Grid item xs={12}>
                    <InputField
                        fullWidth
                        value={calculationValues?.salesGoodsTotal}
                        label="Sales - Goods"
                        type="number"
                        required
                        onChange={handleFieldChange}
                        propertyName="salesGoodsTotal"
                    />
                </Grid>
                <Grid item xs={2}>
                    <Button
                        className={{ float: 'right' }}
                        variant="outlined"
                        color="primary"
                        onClick={() => {}}
                    >
                        Submit
                    </Button>
                </Grid>
            </Grid>
        </Page>
    );
}

CalculationValues.propTypes = {
    //submitcalculationValues: PropTypes.func.isRequired,
    loading: PropTypes.bool,
    receipt: PropTypes.shape({}),
    item: PropTypes.shape({ salesGoodsTotal: PropTypes.number }),
    errorMessage: PropTypes.string
};

CalculationValues.defaultProps = {
    errorMessage: null,
    loading: false,
    receipt: null,
    item: null
};

export default CalculationValues;
