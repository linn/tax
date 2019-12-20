import { connect } from 'react-redux';
import Obligations from '../components/Obligations';
import requestObligations from '../actions/ObligationsActions';
import getProfile from '../selectors/getProfile';

const mapStateToProps = state => ({
    loading: state.vatReturn?.loading,
    snackbarVisible: state.vatReturn.snackbarVisible,
    errorMessage: state.vatReturn?.error?.details?.message,
    obligations: state.obligations.list,
    profile: getProfile(state)
});

const mapDispatchToProps = {
    requestObligations
};

export default connect(mapStateToProps, mapDispatchToProps)(Obligations);
