import { connect } from 'react-redux';
import { getItemError } from '@linn-it/linn-form-components-library';
import TaxReturn from '../components/TaxReturn';
import add from '../actions/vatReturnActions';

const mapStateToProps = () => ({
    //loading: ateFaultCodeSelectors.getLoading(state),
    //snackbarVisible: ateFaultCodeSelectors.getSnackbarVisible(state),
    //itemError: getItemError(state, itemTypes.ateFaultCode.item)
});

const mapDispatchToProps = {
    submitVatReturn: add
};

export default connect(mapStateToProps, mapDispatchToProps)(TaxReturn);
