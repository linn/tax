import * as actionTypes from '../actions';

const vatReturn = (state = { loading: false, receipt: null, snackbarVisible: false }, action) => {
    switch (action.type) {
        case actionTypes.POST_VAT_RETURN:
            return {
                ...state,
                loading: true
            };
        case actionTypes.RECEIVE_VAT_RETURN_RECEIPT:
            return {
                ...state,
                loading: false,
                receipt: action.payload.data,
                snackbarVisible: true
            };

        default:
            return state;
    }
};

export default vatReturn;
