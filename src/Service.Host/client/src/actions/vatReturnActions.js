import { RSAA } from 'redux-api-middleware';
import * as actionTypes from './index';

const add = item => ({
    [RSAA]: {
        endpoint: `http://localhost:61798/tax/return`, // todo - approots
        method: 'POST',
        options: { requiresAuth: true },
        headers: {
            Accept: 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(item),
        types: [
            {
                type: actionTypes.POST_VAT_RETURN,
                payload: {}
            },
            {
                type: actionTypes.RECEIVE_VAT_RETURN_RECEIPT,
                payload: async (action, state, res) => ({ data: await res.json() })
            },
            {
                type: 'FETCH_ERROR',
                payload: (action, state, res) => 'Network request failed'
            }
        ]
    }
});

export default add;
