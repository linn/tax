import { connect } from 'react-redux';
import { getItemError } from '@linn-it/linn-form-components-library';
import TaxReturn from '../components/TaxReturn';
import add from '../actions/vatReturnActions';

const mapStateToProps = () => ({
    //loading:
    //snackbarVisible: 
    //itemError: 
});

const mapDispatchToProps = {
    submitVatReturn: add
};

export default connect(mapStateToProps, mapDispatchToProps)(TaxReturn);
