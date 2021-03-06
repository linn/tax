﻿import { reducers as sharedLibraryReducers } from '@linn-it/linn-form-components-library';
import { combineReducers } from 'redux';
import { reducer as oidc } from 'redux-oidc';
import vatReturn from './vatReturn';
import calculationValues from './calculationValues';
import obligations from './obligations';

const rootReducer = combineReducers({
    oidc,
    calculationValues,
    vatReturn,
    obligations,
    ...sharedLibraryReducers
});

export default rootReducer;
