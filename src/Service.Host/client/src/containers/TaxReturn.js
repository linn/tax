import { connect } from 'react-redux';
import TaxReturn from '../components/TaxReturn';
import { add, hideSnackbar } from '../actions/vatReturnActions';

const mapStateToProps = state => ({
    loading: state.vatReturn?.loading,
    snackbarVisible: state.vatReturn.snackbarVisible,
    errorMessage: state.vatReturn?.error?.details?.message,
    receipt: state.vatReturn.receipt
});

const mapDispatchToProps = {
    submitVatReturn: add,
    hideSnackbar
};

export default connect(mapStateToProps, mapDispatchToProps)(TaxReturn);
