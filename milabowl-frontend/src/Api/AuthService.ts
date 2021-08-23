import { UserAgentApplication } from 'msal';
import jwt_decode from 'jwt-decode';

const BASE_WEB_URI: string = process.env.REACT_APP_BASE_WEB_URI as string;
const MILABOWL_ADMIN = "MilabowlAdmin";

const agent = new UserAgentApplication({
    auth: {
        clientId: 'aa6ccf30-b294-41fd-b522-995d5347e21b',
        redirectUri: BASE_WEB_URI,
        authority: 'https://login.microsoftonline.com/f5c98e5d-b164-4b19-8f0b-3ff85b5fff50',
    },
    cache: {
        cacheLocation: 'localStorage',
        storeAuthStateInCookie: true,
    },
});

const getAccessTokenFromCache = async (): Promise<string | undefined> => {
    try {
        const accessToken = await agent.acquireTokenSilent({
            scopes: ['https://milab2c.onmicrosoft.com/883ff110-4bf9-40e7-b6a4-60f018ec9f22/MilabowlApi.Read'],
        });

        return accessToken.accessToken;
    } catch (e) {
        return undefined;
    }
};

const login = async (): Promise<string> => {
    const resp = await agent.loginPopup({
        scopes: ['openid', 'profile', 'user.read'],
        prompt: 'select_account',
    });

    const accessToken = await agent.acquireTokenSilent({
        scopes: ['https://milab2c.onmicrosoft.com/883ff110-4bf9-40e7-b6a4-60f018ec9f22/MilabowlApi.Read', 'user.read'],
    });

    return accessToken.accessToken;
};

const logout = async (): Promise<void> => {
    await agent.logout();
};

const getDecodedToken = (token: string | undefined): DecodedToken | undefined => {
    if(!token){
        return undefined;
    }

    const decodedToken = jwt_decode<DecodedToken>(token);
    return decodedToken;
}

const isMilabowlAdmin = (token: string | undefined): boolean => {
    const decodedToken = getDecodedToken(token);
    if(!decodedToken){
        return false;
    }

    return decodedToken.roles?.includes(MILABOWL_ADMIN);
}

interface DecodedToken{
    roles: string[];
    preferred_username: string;

}

export { login, logout, getAccessTokenFromCache, isMilabowlAdmin, getDecodedToken };
