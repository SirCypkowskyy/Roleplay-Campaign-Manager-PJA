import {create} from "zustand";
import {persist} from "zustand/middleware";
import Cookies from "js-cookie";
import {Endpoints} from "@/lib/api/endpoints.ts";
import {myLog} from "@/lib/utils.ts";
import {LogLevel} from "@/shared/types.ts";
import {AuthChallengeResponse} from "@/lib/api/types.ts";

interface AuthStore {
    challengeAsync: () => Promise<void>;
    challengeIfUserDataMissingAsync: () => Promise<void>;
    authorizeAsync: () => Promise<void>;
    refreshTokenAsync: () => Promise<void>;
    loginAsync: (email: string, password: string) => Promise<boolean>;
    loginWithOAuthRetrieveCodeAsync: (code: string) => Promise<boolean>;
    isLoggedIn: boolean;
    logout: () => void;
    getBearerToken: () => string;
    userData?: AuthChallengeResponse;
}

export const useAuthStore = create(
    persist<AuthStore>(
        (set) => ({
            isLoggedIn: false,
            authorizeAsync: async () => {
                const token = Cookies.get('token');
                const refreshToken = Cookies.get('refreshToken');
                const email = Cookies.get('email');
                if (!token || !refreshToken || !email) {
                    set({isLoggedIn: false});
                    return;
                }
                const response = await Endpoints.User.REFRESH_TOKEN_API(refreshToken, email);
                if (!response.isSuccess) {
                    set({isLoggedIn: false});
                    return;
                }
                Cookies.set('token', response.data?.token || '');
                Cookies.set('refreshToken', response.data?.refreshToken || '');
                set({isLoggedIn: true});
            },
            loginAsync: async (email: string, password: string) => {
                const response = await Endpoints.User.LOGIN_API(email, password);
                if (!response.isSuccess) {
                    set({isLoggedIn: false});
                    return false;
                }
                Cookies.set('token', response.data?.token || '');
                Cookies.set('refreshToken', response.data?.refreshToken || '');
                Cookies.set('email', email);
                set({isLoggedIn: true});
                return true;
            },
            loginWithOAuthRetrieveCodeAsync: async (code: string) => {
                const response = await Endpoints.User.LOGIN_WITH_OAUTH_RETRIEVE_CODE_API(code);
                if (!response.isSuccess) {
                    set({isLoggedIn: false});
                    return false;
                }
                Cookies.set('token', response.data?.token || '');
                Cookies.set('refreshToken', response.data?.refreshToken || '');
                set({isLoggedIn: true});
                return true;
            },
            logout: () => {
                Cookies.remove('token');
                Cookies.remove('refreshToken');
                Cookies.remove('email');
                set({isLoggedIn: false});
            },
            refreshTokenAsync: async () => {
                myLog(LogLevel.debug, '[AuthStore/refreshTokenAsync] Refreshing token');
                const refreshToken = Cookies.get('refreshToken');
                const email = Cookies.get('email');
                if (!refreshToken || !email) {
                    set({isLoggedIn: false});
                    return;
                }
                const response = await Endpoints.User.REFRESH_TOKEN_API(refreshToken, email);
                if (!response.isSuccess) {
                    set({isLoggedIn: false});
                    return;
                }
                Cookies.set('token', response.data?.token || '');
                Cookies.set('refreshToken', response.data?.refreshToken || '');
                set({isLoggedIn: true});
            },
            getBearerToken: () => {
                return Cookies.get('token') || '';
            },
            email: null,
            role: null,
            username: null,
            challengeAsync: async () => {
                const token = Cookies.get('token');
                if (!token) {
                    myLog(LogLevel.error, '[AuthStore/challengeAsync] No token, so cannot challenge');
                    set({isLoggedIn: false});
                    return;
                }
                const response = await Endpoints.User.CHALLENGE_API(token);
                if (!response.isSuccess) {
                    myLog(LogLevel.error, '[AuthStore/challengeAsync] Challenge failed');
                    set({isLoggedIn: false});
                    return;
                }
                set({isLoggedIn: true, userData: response.data as AuthChallengeResponse});
            },
            challengeIfUserDataMissingAsync: async () => {
                if (!useAuthStore.getState().userData)
                    await useAuthStore.getState().challengeAsync();             
            }
        }),
        {
            name: 'auth-store'
        }
    )
);