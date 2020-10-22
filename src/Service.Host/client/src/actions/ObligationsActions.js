import { RSAA } from 'redux-api-middleware';
import config from '../config';
import * as actionTypes from './index';

const requestObligations = item => ({
    [RSAA]: {
        endpoint: `${config.appRoot}/tax/obligations`,
        method: 'GET',
        options: { requiresAuth: true },
        headers: {
            Accept: 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(item),
        types: [
            {
                type: actionTypes.REQUEST_OBLIGATIONS,
                payload: {}
            },
            {
                type: actionTypes.RECEIVE_OBLIGATIONS,
                payload: async (action, state, res) => ({ data: await res.json() })
            },
            {
                type: actionTypes.FETCH_ERROR,
                payload: async (action, state, res) =>
                    res
                        ? {
                              error: {
                                  details: await res.json()
                              }
                          }
                        : `Network request failed`
            }
        ]
    }
});

export default requestObligations;
