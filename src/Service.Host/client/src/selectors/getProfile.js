const getProfile = state => {
    if (!state.oidc.user || !state.oidc.user.profile) {
        return null;
    }

    return state.oidc.user.profile;
};

export default getProfile;
