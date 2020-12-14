import React, { useState, useEffect } from 'react';
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
        <Page>
            <Grid container spacing={3}>
                <Grid item xs={12}>
                    <Title text="VAT Return Calculation Breakdown" />
                </Grid>
                {errorMessage ? (
                    <Grid item xs={12}>
                        <ErrorCard errorMessage={errorMessage} />
                    </Grid>
                ) : (
                    <></>
                )}
                <Grid item xs={12}>
                    <Typography variant="subtitle" gutterBottom>
                        Check these numbers against manual calculation and ammend if necessary.
                    </Typography>
                </Grid>
                <Grid item xs={12}>
                    <InputField
                        value={calculationValues?.salesGoodsTotal}
                        label="Sales - Goods"
                        type="number"
                        onChange={handleFieldChange}
                        propertyName="salesGoodsTotal"
                    />
                </Grid>
                <Grid item xs={12}>
                    <InputField
                        value={calculationValues?.salesVatTotal}
                        label="Sales - VAT"
                        type="number"
                        onChange={handleFieldChange}
                        propertyName="salesVatTotal"
                    />
                </Grid>
                <Grid item xs={12}>
                    <InputField
                        value={calculationValues?.canteenGoodsTotal}
                        label="Canteen - Goods"
                        type="number"
                        onChange={handleFieldChange}
                        propertyName="canteenGoodsTotal"
                    />
                </Grid>
                <Grid item xs={12}>
                    <InputField
                        value={calculationValues?.canteenVatTotal}
                        label="Canteen - VAT"
                        type="number"
                        onChange={handleFieldChange}
                        propertyName="canteenVatTotal"
                    />
                </Grid>
                <Grid item xs={12}>
                    <InputField
                        value={calculationValues?.purchasesGoodsTotal}
                        label="Purchases - Goods"
                        type="number"
                        onChange={handleFieldChange}
                        propertyName="purchasesGoodsTotal"
                    />
                </Grid>
                <Grid item xs={12}>
                    <InputField
                        value={calculationValues?.purchasesVatTotal}
                        label="Purchases - VAT"
                        type="number"
                        onChange={handleFieldChange}
                        propertyName="purchasesVatTotal"
                    />
                </Grid>
                <Grid item xs={12}>
                    <InputField
                        value={calculationValues?.cashbookAndOtherTotal}
                        label="Cashbook/Other"
                        type="number"
                        onChange={handleFieldChange}
                        propertyName="cashbookAndOtherTotal"
                    />
                </Grid>
                <Grid item xs={12}>
                    <InputField
                        value={calculationValues?.instrastatDispatchesGoodsTotal}
                        label="Intrastat Dispatches - Goods"
                        type="number"
                        onChange={handleFieldChange}
                        propertyName="instrastatDispatchesGoodsTotal"
                    />
                </Grid>
                <Grid item xs={12}>
                    <InputField
                        value={calculationValues?.intrastatArrivalsGoodsTotal}
                        label="Intrastat Arrivals - Goods"
                        type="number"
                        onChange={handleFieldChange}
                        propertyName="intrastatArrivalsGoodsTotal"
                    />
                </Grid>
                <Grid item xs={12}>
                    <InputField
                        value={calculationValues?.intrastatArrivalsVatTotal}
                        label="Intrastat Arrivals - VAT"
                        type="number"
                        onChange={handleFieldChange}
                        propertyName="intrastatArrivalsVatTotal"
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
