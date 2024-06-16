import {
    AuthChallengeResponse,
    CharacterResponseDto,
    JwtResponse,
    Message,
    User,
    UserDashboardDataResponse
} from "@/lib/api/types.ts";
import {myLog} from "@/lib/utils.ts";
import {ApiResponse, LogLevel} from "@/shared/types.ts";

/**
 * @description Endpointy dla API
 */
export const Endpoints = {
    /**
     * @description Endpointy dla usera
     */
    User: {
        /**
         * @description Endpoint dla logowania użytkownika
         * @param email - email użytkownika
         * @param password - hasło użytkownika
         */
        LOGIN_API: async (email: string, password: string): Promise<ApiResponse<JwtResponse>> => {
            myLog(LogLevel.debug, '[User.LoginApi] Called for login')
            const response = await fetch('/api/v1/user/auth', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({email, password})
            });

            if (!response.ok)
                return {
                    isSuccess: false,
                    statusCode: response.status,
                    errorMessage: response.statusText,
                    errorDetails: await response.text(),
                }

            myLog(LogLevel.debug, 'Login success:', response);
            const data = await response.json();
            return {
                isSuccess: true,
                statusCode: response.status,
                data: data as JwtResponse,
            }
        },
        /**
         * @description Endpoint dla odświeżania tokena JWT
         * @param refreshToken - token odświeżający
         */
        REFRESH_TOKEN_API: async (refreshToken: string): Promise<ApiResponse<JwtResponse>> => {
            myLog(LogLevel.debug, '[User.RefreshTokenApi] Called for token refresh')
            const response = await fetch(`/api/v1/user/auth/refresh`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({refreshToken})
            });

            if (!response.ok)
            {
                return {
                    isSuccess: false,
                    statusCode: response.status,
                    errorMessage: 'Token refresh failed',
                    errorDetails: await response.text(),
                }
            }
                
            const data = await response.json();
            return {
                isSuccess: true,
                statusCode: response.status,
                data: data as JwtResponse,
            }
        },
        /**
         * @description Endpoint dla challenge'owania użytkownika z tokenem JWT
         * @param bearerToken - token JWT
         */
        CHALLENGE_API: async (bearerToken: string): Promise<ApiResponse<AuthChallengeResponse>> => {
            myLog(LogLevel.debug, '[User.ChallengeApi] Called for challenge')
            const response = await fetch('/api/v1/user/auth/self', {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${bearerToken}`
                }
            });
            if (!response.ok)
                return {
                    isSuccess: false,
                    statusCode: response.status,
                    errorMessage: 'Challenge failed',
                    errorDetails: await response.text(),
                }

            const data = await response.json();
            return {
                isSuccess: true,
                statusCode: response.status,
                data: data as AuthChallengeResponse,
            }
        },
        /**
         * @description Endpoint dla logowania z OAuth
         * @param code - kod OAuth
         */
        LOGIN_WITH_OAUTH_RETRIEVE_CODE_API: async (code: string): Promise<ApiResponse<JwtResponse>> => {
            const response = await fetch('/api/v1/user/auth/discord/retrieve?code=' + code, {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json'
                },
            });
            if (!response.ok)
                return {
                    isSuccess: false,
                    statusCode: response.status,
                    errorMessage: 'Failed to retrieve token data from Discord OAuth code',
                    errorDetails: await response.text(),
                }
            
            const data = await response.json();
            return {
                isSuccess: true,
                statusCode: response.status,
                data: data as JwtResponse,
            }
        },
        RETRIEVE_USER_DASHBOARD_DATA : async (bearerToken: string): Promise<ApiResponse<UserDashboardDataResponse>> => {
            const response = await fetch('/api/v1/user/dashboard', {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${bearerToken}`
                }
            });
            if (!response.ok)
                return {
                    isSuccess: false,
                    statusCode: response.status,
                    errorMessage: 'Failed to retrieve user dashboard data',
                    errorDetails: await response.text(),
                }

            const data = await response.json();
            return {
                isSuccess: true,
                statusCode: response.status,
                data: data as UserDashboardDataResponse,
            }
        },
        REGISTER_API: async (username: string, email: string, password: string): Promise<ApiResponse<User>> => {
            myLog(LogLevel.debug, '[User.RegisterApi] Called for registration')
            const response = await fetch('/api/v1/user/register', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({ username, email, password })
            });

            if (!response.ok) {
                return {
                    isSuccess: false,
                    statusCode: response.status,
                    errorMessage: response.statusText,
                    errorDetails: await response.text(),
                }
            }

            myLog(LogLevel.debug, 'Registration success:', response);
            const data = await response.json();
            return {
                isSuccess: true,
                statusCode: response.status,
                data: data as User,
            }
        },
        GET_CAMPAIGN_MESSAGES_API: async (bearerToken: string, campaignId: string, numToQuery: number, skip: number): Promise<ApiResponse<Message[]>> => {
            const response = await fetch(`/api/v1/campaign/${campaignId}/messages?numToQuery=${numToQuery}&skip=${skip}`, {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${bearerToken}`
                }
            });
            if (!response.ok)
                return {
                    isSuccess: false,
                    statusCode: response.status,
                    errorMessage: 'Failed to retrieve campaign messages',
                    errorDetails: await response.text(),
                }

            const data = await response.json();
            return {
                isSuccess: true,
                statusCode: response.status,
                data: data as Message[]
            }
        },
        GET_IMAGE_API: async (bearerToken: string, imageId: string): Promise<ApiResponse<Blob>> => {
            const response = await fetch(`/api/v1/image/${imageId}`, {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${bearerToken}`
                }
            });
            if (!response.ok)
                return {
                    isSuccess: false,
                    statusCode: response.status,
                    errorMessage: 'Failed to retrieve image',
                    errorDetails: await response.text(),
                }

            const data = await response.blob();
            return {
                isSuccess: true,
                statusCode: response.status,
                data: data
            }
        },
        GET_CONTROLLABLE_CHARACTERS_API: async (bearerToken: string, campaignId: string): Promise<ApiResponse<CharacterResponseDto>> => {
            const response = await fetch(`/api/v1/campaign/${campaignId}/controllable-characters`, {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${bearerToken}`
                }
            });
            if (!response.ok)
                return {
                    isSuccess: false,
                    statusCode: response.status,
                    errorMessage: 'Failed to retrieve controllable characters',
                    errorDetails: await response.text(),
                }
            myLog(LogLevel.debug, 'Controllable characters retrieved:', response);

            const data = await response.json();
            return {
                isSuccess: true,
                statusCode: response.status,
                data: data as CharacterResponseDto
            }
        }
    }
}

