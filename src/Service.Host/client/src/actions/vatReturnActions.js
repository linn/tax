import { RSAA } from 'redux-api-middleware';
import config from '../config';
import * as actionTypes from './index';

const queryString = object =>
    object ? Object.keys(object).reduce((a, n) => `${a}${n}=${object[n]}&`, '?') : null;

export const add = item => ({
    [RSAA]: {
        endpoint: `${config.appRoot}/tax/return`,
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

export const getCalculationValues = () => ({
    [RSAA]: {
        endpoint: `${config.appRoot}/tax/return/calculation-values`,
        method: 'GET',
        options: { requiresAuth: true },
        headers: {
            Accept: 'application/json',
            'Content-Type': 'application/json'
        },
        types: [
            {
                type: actionTypes.REQUEST_CALCULATION_VALUES,
                payload: {}
            },
            {
                type: actionTypes.RECEIVE_CALCULATION_VALUES,
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

export const getFigures = item => ({
    [RSAA]: {
        endpoint: `${config.appRoot}/tax/return${queryString(item)}`,
        method: 'GET',
        options: { requiresAuth: false },
        headers: {
            Accept: 'application/json',
            'Content-Type': 'application/json'
        },
        types: [
            {
                type: actionTypes.REQUEST_VAT_RETURN,
                payload: {}
            },
            {
                type: actionTypes.RECEIVE_VAT_RETURN,
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

export const hideSnackbar = () => ({
    type: 'HIDE_SNACKBAR'
});
