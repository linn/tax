import React, { useState, useEffect } from 'react';
import Typography from '@material-ui/core/Typography';
import Button from '@material-ui/core/Button';
import PropTypes from 'prop-types';
import { InputField, Title, ErrorCard, Loading } from '@linn-it/linn-form-components-library';
import { DataGrid } from '@material-ui/data-grid';
import Grid from '@material-ui/core/Grid';
import { Decimal } from 'decimal.js';

function CalculationValues({ item, errorMessage, loading, fetchVatReturn }) {
    const [calculationValues, setcalculationValues] = useState({
        SalesGoodsTotal: item?.salesGoodsTotal
    });

    const [rows, setRows] = useState([]);

    const [selectedRows, setSelectedRows] = useState([]);

    useEffect(() => {
        setcalculationValues({
            ...item,
            cashbookAndOtherTotal: parseFloat(
                selectedRows.reduce(
                    (accumulator, currentValue) =>
                        new Decimal(accumulator).plus(new Decimal(currentValue.amount)),
                    0
                )
            )
        });
        setRows(
            item?.ledgerEntries?.map(s => ({
                ...s,
                id: s.tref
            }))
        );
    }, [item, selectedRows]);

    const handleFieldChange = (propertyName, newValue) => {
        setcalculationValues({ ...calculationValues, [propertyName]: newValue });
    };

    const columns = [
        { field: 'amount', headerName: 'Amount', width: 140 },
        { field: 'creditOrDebit', headerName: 'Debit/Credit', width: 140 },
        { field: 'datePosted', headerName: 'Date Posted', width: 200 },
        { field: 'narrative', headerName: 'Narrative', width: 140 },
        { field: 'description', headerName: 'Description', width: 140 },
        { field: 'comments', headerName: 'Comments', width: 400 }
    ];

    const handleSelectionChange = model => {
        setSelectedRows(rows.filter(r => model.selectionModel.includes(r.tref.toString())));
    };

    return (
        <>
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
            {loading ? (
                <Grid item xs={12}>
                    <Loading />
                </Grid>
            ) : (
                <>
                    <Grid item xs={12}>
                        <Typography variant="subtitle1" gutterBottom>
                            Values are calculated for the previous three month period. Check these
                            values against manual calculation and ammend if necessary.
                        </Typography>
                    </Grid>
                    <Grid item xs={3}>
                        <InputField
                            value={calculationValues?.salesGoodsTotal}
                            label="Sales - Goods"
                            type="number"
                            onChange={handleFieldChange}
                            propertyName="salesGoodsTotal"
                        />
                    </Grid>
                    <Grid item xs={3}>
                        <InputField
                            value={calculationValues?.salesVatTotal}
                            label="Sales - VAT"
                            type="number"
                            onChange={handleFieldChange}
                            propertyName="salesVatTotal"
                        />
                    </Grid>
                    <Grid item xs={6} />
                    <Grid item xs={3}>
                        <InputField
                            value={calculationValues?.canteenGoodsTotal}
                            label="Canteen - Goods"
                            type="number"
                            onChange={handleFieldChange}
                            propertyName="canteenGoodsTotal"
                        />
                    </Grid>
                    <Grid item xs={3}>
                        <InputField
                            value={calculationValues?.canteenVatTotal}
                            label="Canteen - VAT"
                            type="number"
                            onChange={handleFieldChange}
                            propertyName="canteenVatTotal"
                        />
                    </Grid>
                    <Grid item xs={6} />
                    <Grid item xs={3}>
                        <InputField
                            value={calculationValues?.purchasesGoodsTotal}
                            label="Purchases - Goods"
                            type="number"
                            onChange={handleFieldChange}
                            propertyName="purchasesGoodsTotal"
                        />
                    </Grid>
                    <Grid item xs={3}>
                        <InputField
                            value={calculationValues?.purchasesVatTotal}
                            label="Purchases - VAT"
                            type="number"
                            onChange={handleFieldChange}
                            propertyName="purchasesVatTotal"
                        />
                    </Grid>

                    <Grid item xs={6} />
                    <Grid item xs={12}>
                        <Typography variant="subtitle1" gutterBottom>
                            Selected the Nominal Ledger Entries you want to include in the
                            Cashbook/Other total below the table.
                        </Typography>
                    </Grid>
                    <Grid item xs={12}>
                        <div style={{ height: 500, width: '100%' }}>
                            {rows && (
                                <DataGrid
                                    rows={rows}
                                    columns={columns}
                                    density="standard"
                                    onSelectionModelChange={model => handleSelectionChange(model)}
                                    rowHeight={34}
                                    checkboxSelection
                                    hideFooter
                                />
                            )}
                        </div>
                    </Grid>
                    <Grid item xs={12}>
                        <InputField
                            value={calculationValues?.cashbookAndOtherTotal}
                            label="Cashbook/Other Total (Total of Ledger Entries Selected Above)"
                            type="number"
                            onChange={handleFieldChange}
                            propertyName="cashbookAndOtherTotal"
                        />
                    </Grid>
                    <Grid item xs={3}>
                        <InputField
                            value={calculationValues?.instrastatDispatchesGoodsTotal}
                            label="Intrastat Dispatches - Goods"
                            type="number"
                            onChange={handleFieldChange}
                            propertyName="instrastatDispatchesGoodsTotal"
                        />
                    </Grid>
                    <Grid item xs={3}>
                        <InputField
                            value={calculationValues?.intrastatArrivalsGoodsTotal}
                            label="Intrastat Arrivals - Goods"
                            type="number"
                            onChange={handleFieldChange}
                            propertyName="intrastatArrivalsGoodsTotal"
                        />
                    </Grid>
                    <Grid item xs={3}>
                        <InputField
                            value={calculationValues?.intrastatArrivalsVatTotal}
                            label="Intrastat Arrivals - VAT"
                            type="number"
                            onChange={handleFieldChange}
                            propertyName="intrastatArrivalsVatTotal"
                        />
                    </Grid>
                    <Grid item xs={3} />
                    <Grid item xs={4}>
                        <Button
                            variant="outlined"
                            disabled={loading}
                            color="primary"
                            onClick={() => {
                                fetchVatReturn(calculationValues);
                            }}
                        >
                            Calculate VAT Return Values
                        </Button>
                    </Grid>
                    <Grid item xs={8} />
                </>
            )}
        </>
    );
}

CalculationValues.propTypes = {
    fetchVatReturn: PropTypes.func.isRequired,
    loading: PropTypes.bool,
    receipt: PropTypes.shape({}),
    item: PropTypes.shape({
        salesGoodsTotal: PropTypes.number,
        ledgerEntries: PropTypes.arrayOf(PropTypes.shape({}))
    }),
    errorMessage: PropTypes.string
};

CalculationValues.defaultProps = {
    errorMessage: null,
    loading: false,
    receipt: null,
    item: null
};

export default CalculationValues;
