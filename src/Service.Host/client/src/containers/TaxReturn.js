import { connect } from 'react-redux';
import { initialiseOnMount } from '@linn-it/linn-form-components-library';
import TaxReturn from '../components/TaxReturn';
import { add, hideSnackbar, getFigures } from '../actions/vatReturnActions';
import getProfile from '../selectors/getProfile';

const mapStateToProps = state => ({
    loading: state.vatReturn?.loading,
    snackbarVisible: state.vatReturn.snackbarVisible,
    errorMessage: state.vatReturn?.error?.details?.message,
    receipt: state.vatReturn.receipt,
    profile: getProfile(state),
    figures: state.vatReturn.figures
});

const initialise = () => dispatch => {
    dispatch(getFigures());
};

const mapDispatchToProps = {
    initialise,
    submitVatReturn: add,
    hideSnackbar
};

export default connect(mapStateToProps, mapDispatchToProps)(initialiseOnMount(TaxReturn));
