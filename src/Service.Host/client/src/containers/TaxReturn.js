import { connect } from 'react-redux';
import { getItemError, initialiseOnMount } from '@linn-it/linn-form-components-library';
import TaxReturn from '../components/TaxReturn';
//import ateFaultCodeActions from '../../actions/ateFaultCodeActions';
//import * as itemTypes from '../../itemTypes';

const mapStateToProps = () => ({
    //editStatus: ateFaultCodeSelectors.getEditStatus(state),
    //loading: ateFaultCodeSelectors.getLoading(state),
    //snackbarVisible: ateFaultCodeSelectors.getSnackbarVisible(state),
    //itemError: getItemError(state, itemTypes.ateFaultCode.item)
});

const mapDispatchToProps = {};

export default connect(mapStateToProps, mapDispatchToProps)(TaxReturn);
