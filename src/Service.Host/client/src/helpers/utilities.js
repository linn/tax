// eslint-disable-next-line import/prefer-default-export
export const toQueryString = object =>
    object ? Object.keys(object).reduce((a, n) => `${a}${n}=${object[n]}&`, '?') : null;
