import { reducers as sharedLibraryReducers } from '@linn-it/linn-form-components-library';
import { combineReducers } from 'redux';
import { reducer as oidc } from 'redux-oidc';
import vatReturn from './vatReturn';

const rootReducer = combineReducers({
    oidc,
    vatReturn,
    ...sharedLibraryReducers
});

export default rootReducer;
