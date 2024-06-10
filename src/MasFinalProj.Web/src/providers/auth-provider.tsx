import React, { createContext, useContext, useState, useEffect, ReactNode } from 'react';
import Cookies from 'js-cookie';

const LOGIN_API = 'api/v1/user/auth';
const REFRESH_TOKEN_API = 'api/v1/user/auth/refresh';

type AuthChallengeResponse = {
    username: string;
    email: string;
    role: string;
}

interface AuthContextType {
    authToken: string | null;
    loginUser: (username: string, password: string) => Promise<boolean>;
    refreshToken: () => Promise<void>;
    getBearerToken: () => string;
    logoutUser: () => void;
    loading: boolean;
    challenge: () => Promise<AuthChallengeResponse | undefined>;
    lastChallenge?: AuthChallengeResponse;
    lastChallengeTime?: number;
    isAuthenticated: () => boolean;
    setAuthToken: (token: string) => void;
    setRefreshToken: (token: string) => void;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

interface AuthProviderProps {
    children: ReactNode;
}

export const AuthProvider: React.FC<AuthProviderProps> = ({ children }) => {
    const [authToken, setAuthToken] = useState<string | null>(null);
    const [loading, setLoading] = useState(true);
    const [lastChallenge, setLastChallenge] = useState<AuthChallengeResponse | undefined>(undefined);
    const [lastChallengeTime, setLastChallengeTime] = useState<number | undefined>(undefined);

    useEffect(() => {
        const token = Cookies.get('token');
        if (token)
            setAuthToken(token);
        setLoading(false);
    }, []);

    const loginUser = async (email: string, password: string): Promise<boolean> => {
        try {
            const response = await fetch(LOGIN_API, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ email, password })
            });
            if (!response.ok)
                throw new Error('Email or password is incorrect');
            const data = await response.json();
            const { token, refreshToken } = data;
            Cookies.set('token', token);
            Cookies.set('refreshToken', refreshToken);
            Cookies.set('email', email);
            setAuthToken(token);
            return true;
        } catch (error) {
            console.error('Login error:', error);
            return false;
        }
    };

    const refreshToken = async (): Promise<void> => {
        try {
            const refreshToken = Cookies.get('refreshToken');
            if (refreshToken) {
                const response = await fetch(REFRESH_TOKEN_API + `?refreshToken=${refreshToken}&email=${Cookies.get('email')}`, {
                    method: 'GET',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                });
                if (!response.ok) {
                    throw new Error('Token refresh failed');
                }
                const data = await response.json();
                const { token, newRefreshToken } = data;
                Cookies.set('token', token);
                Cookies.set('refreshToken', newRefreshToken);
                setAuthToken(token);
            }
        } catch (error) {
            console.error('Token refresh error:', error);
        }
    };

    const setRefreshToken = (token: string): void => {
        Cookies.set('refreshToken', token);
    }

    const logoutUser = (): void => {
        Cookies.remove('token');
        Cookies.remove('refreshToken');
        Cookies.remove('email');
        setAuthToken(null);
    };

    const getBearerToken = (): string => {
        return `Bearer ${authToken}`;
    };

    /**
     * Zwraca informację, czy użytkownik jest zalogowany
     * @returns true, jeśli ostatni challenge zwrócił poprawne dane wcześniej niż 1 minutę temu
     * @returns false, jeśli ostatni challenge zwrócił błędne dane lub nie został wykonany
     */
    const isAuthenticated = (): boolean => {
        if (!authToken)
            return false;
        if (!lastChallenge)
            return false;
        if (!lastChallengeTime)
            return false;
        return lastChallengeTime >= Date.now() - 60 * 1000;
    }

    const challenge = async (): Promise<AuthChallengeResponse | undefined> => {
        const response = await fetch('api/v1/user/auth/self', {
            method: 'GET',
            headers: {
                'Authorization': getBearerToken()
            }
        });
        if (!response.ok)
            return undefined;

        const data = await response.json();
        setLastChallenge(data);
        setLastChallengeTime(Date.now());
        return data;
    }

    return (
        <AuthContext.Provider value={{ authToken, loginUser, refreshToken, logoutUser, loading, getBearerToken, challenge, setAuthToken, setRefreshToken, isAuthenticated, lastChallenge, lastChallengeTime }}>
            {children}
        </AuthContext.Provider>
    );
};

export const useAuth = (): AuthContextType => {
    const context = useContext(AuthContext);
    if (!context) {
        throw new Error('useAuth must be used within an AuthProvider');
    }
    return context;
};
