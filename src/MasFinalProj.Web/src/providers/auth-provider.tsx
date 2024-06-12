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
    authToken: () => string | undefined;
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
    setUsername(username: string): void;
    retriveJwtDataFromCode(code: string): Promise<boolean>;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

interface AuthProviderProps {
    children: ReactNode;
}

export const AuthProvider: React.FC<AuthProviderProps> = ({ children }) => {
    const [loading, setLoading] = useState(true);
    const [lastChallengeTime, setLastChallengeTime] = useState<number | undefined>(undefined);
    const [lastChallenge, setLastChallenge] = useState<AuthChallengeResponse | undefined>(undefined);
    
    useEffect(() => {
        setLoading(false);
        setLastChallenge(Cookies.get('lastChallenge') ? JSON.parse(Cookies.get('lastChallenge') as string) : undefined);
        setLastChallengeTime(Cookies.get('lastChallengeTime') ? parseInt(Cookies.get('lastChallengeTime') as string) : undefined);
    }, []);
    
    const authToken = () => Cookies.get('token');
    
    const setAuthToken = (token: string | null): void => {
        if (token)
            Cookies.set('token', token);
        else
        {
            Cookies.remove('token');
            Cookies.remove('refreshToken');
            Cookies.remove('email');
        }
    };

    const loginUser = async (email: string, password: string): Promise<boolean> => {
        console.log('[AuthProvider] Called for login', email, password)
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
            console.log('Login success:', response);
            const data = await response.json();
            const { token, refreshToken } = data;
            Cookies.set('token', token);
            Cookies.set('refreshToken', refreshToken);
            Cookies.set('email', email);
            setAuthToken(token);
            await challenge();
            return true;
        } catch (error) {
            console.error('Login error:', error);
            return false;
        }
    };

    const refreshToken = async (): Promise<void> => {
        console.log('[AuthProvider] Called for token refresh');
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
        const token = authToken();
        return `Bearer ${token}`;
    };

    /**
     * Zwraca informację, czy użytkownik jest zalogowany
     * @returns true, jeśli ostatni challenge zwrócił poprawne dane wcześniej niż 1 minutę temu
     * @returns false, jeśli ostatni challenge zwrócił błędne dane lub nie został wykonany
     */
    const isAuthenticated = (): boolean => {
        console.log('[AuthProvider][isAuthenticated] Called for token validation');
        if (!authToken())
        {
            console.log('[AuthProvider][isAuthenticated] Token is not set');
            return false;
        }
        if (!lastChallenge)
        {
            console.log('[AuthProvider][isAuthenticated] lastChallenge object is not set');
            return false;
        }
        if (!lastChallengeTime)
        {
            console.log('[AuthProvider][isAuthenticated] lastChallengeTime is not set');
            return false;
        }
        if(lastChallengeTime < Date.now() - 60 * 1000) {
            challenge();
            return false;
        }
        return true;
    }

    const challenge = async (): Promise<AuthChallengeResponse | undefined> => {
        console.log('[AuthProvider][Challenge] Called for token challenge');
        const response = await fetch('api/v1/user/auth/self', {
            method: 'GET',
            headers: {
                'Authorization': getBearerToken()
            }
        });
        if (!response.ok)
        {
            console.error('[AuthProvider][Challenge] Challenge failed:', response);
            return undefined;
        }

        const data = await response.json();
        setLastChallenge(data);
        Cookies.set('lastChallenge', JSON.stringify(data));
        setLastChallengeTime(Date.now());
        Cookies.set('lastChallengeTime', Date.now().toString());
        return data;
    }
    
    const setUsername = (username: string) => {
        setLastChallenge({
            username,
            email: '',
            role: ''
        });
    }
    
    const retriveJwtDataFromCode = async (code: string): Promise<boolean> => {
        try {
            const response = await fetch('api/v1/user/auth/discord/retrieve?code=' + code, {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json'
                },
            });
            if (!response.ok)
                throw new Error('Discord OAuth failed');
            const data = await response.json();
            const { token, refreshToken, username } = data;
            console.log('Discord OAuth success:', data);
            Cookies.set('token', token);
            Cookies.set('refreshToken', refreshToken);
            Cookies.set('username', username);
            setAuthToken(token);
            setRefreshToken(refreshToken);
            return true;
        } catch (error) {
            console.error('Discord OAuth error:', error);
            return false;
        }
    }

    return (
        <AuthContext.Provider value={{ authToken, loginUser, refreshToken, logoutUser, loading, getBearerToken, challenge, setAuthToken, setRefreshToken, isAuthenticated, lastChallenge, lastChallengeTime, setUsername, retriveJwtDataFromCode }}>
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
