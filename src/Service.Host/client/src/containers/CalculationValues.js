import { connect } from 'react-redux';
import { initialiseOnMount } from '@linn-it/linn-form-components-library';
import CalculationValues from '../components/CalculationValues';
import { getCalculationValues } from '../actions/vatReturnActions';

const mapStateToProps = state => ({
    loading: state.calculationValues?.loading,
    errorMessage: state.calculationValues?.error?.details?.message,
    item: state.calculationValues.item
});

const initialise = () => dispatch => {
    dispatch(getCalculationValues());
};

const mapDispatchToProps = {
    initialise
};

export default connect(mapStateToProps, mapDispatchToProps)(initialiseOnMount(CalculationValues));
