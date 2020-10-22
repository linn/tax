import { connect } from 'react-redux';
import TestFPH from '../components/TestFPH';
import getProfile from '../selectors/getProfile';

const mapStateToProps = state => ({
    profile: getProfile(state)
});

const mapDispatchToProps = {};

export default connect(mapStateToProps, mapDispatchToProps)(TestFPH);
