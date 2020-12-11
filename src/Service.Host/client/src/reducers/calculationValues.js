import * as actionTypes from '../actions';

const calculationValues = (
    state = {
        loading: false,
        item: null,
        receipt: null,
        error: null
    },
    action
) => {
    switch (action.type) {
        case actionTypes.REQUEST_CALCULATION_VALUES:
            return {
                ...state,
                loading: true
            };
        case actionTypes.RECEIVE_CALCULATION_VALUES:
            return {
                ...state,
                loading: false,
                item: action.payload.data
            };
        case actionTypes.FETCH_ERROR:
            return {
                ...state,
                loading: false,
                error: action.payload.error
            };

        default:
            return state;
    }
};

export default calculationValues;
