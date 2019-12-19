import * as actionTypes from '../actions';

const vatReturn = (state = { loading: false, obligations: null, error: null }, action) => {
    switch (action.type) {
        case actionTypes.REQUEST_OBLIGATIONS:
            return {
                ...state,
                loading: true
            };
        case actionTypes.RECEIVE_OBLIGATIONS:
            return {
                ...state,
                loading: false,
                list: action.payload.data.obligations,
                snackbarVisible: true
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
