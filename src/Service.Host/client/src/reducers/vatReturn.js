import * as actionTypes from '../actions';

const vatReturn = (
    state = {
        loading: false,
        figures: null,
        receipt: null,
        error: null,
        snackbarVisible: false
    },
    action
) => {
    switch (action.type) {
        case actionTypes.REQUEST_VAT_RETURN:
            return {
                ...state,
                loading: true
            };
        case actionTypes.RECEIVE_VAT_RETURN:
            return {
                ...state,
                loading: false,
                figures: action.payload.data,
                snackbarVisible: true
            };
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
        case actionTypes.HIDE_SNACKBAR:
            return {
                ...state,
                snackbarVisible: false
            };

        case actionTypes.FETCH_ERROR:
            return {
                ...state,
                loading: false,
                error: action.payload.error,
                snackbarVisible: false
            };

        default:
            return state;
    }
};

export default vatReturn;
