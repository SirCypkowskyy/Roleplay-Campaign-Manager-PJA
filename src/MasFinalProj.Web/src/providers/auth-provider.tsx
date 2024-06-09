import React, { createContext, useContext, useState, useEffect, ReactNode } from 'react';
import Cookies from 'js-cookie';

// API endpoints (przykładowe, dostosuj do swoich potrzeb)
const LOGIN_API = 'api/v1/user/auth';
const REFRESH_TOKEN_API = 'api/v1/user/auth/refresh';

interface AuthContextType {
    authToken: string | null;
    loginUser: (username: string, password: string) => Promise<boolean>;
    refreshToken: () => Promise<void>;
    getBearerToken: () => string;
    logoutUser: () => void;
    loading: boolean;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

interface AuthProviderProps {
    children: ReactNode;
}

export const AuthProvider: React.FC<AuthProviderProps> = ({ children }) => {
    const [authToken, setAuthToken] = useState<string | null>(null);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const token = Cookies.get('token');
        if (token)
            setAuthToken(token);
        setLoading(false);
    }, []);

    /**
     * Logowanie użytkownika
     * @param email Adres email
     * @param password Hasło
     */
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
            Cookies.set('token', token, { httpOnly: true });
            Cookies.set('refreshToken', refreshToken, { httpOnly: true });
            Cookies.set('email', email, { httpOnly: true });
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
                Cookies.set('token', token, { httpOnly: true });
                Cookies.set('refreshToken', newRefreshToken, { httpOnly: true });
                setAuthToken(token);
            }
        } catch (error) {
            console.error('Token refresh error:', error);
        }
    };

    const logoutUser = (): void => {
        Cookies.remove('token');
        Cookies.remove('refreshToken');
        Cookies.remove('email');
        setAuthToken(null);
    };
    
    const getBearerToken = (): string => {
        return `Bearer ${authToken}`;
    };

    return (
        <AuthContext.Provider value={{ authToken, loginUser, refreshToken, logoutUser, loading, getBearerToken }}>
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
