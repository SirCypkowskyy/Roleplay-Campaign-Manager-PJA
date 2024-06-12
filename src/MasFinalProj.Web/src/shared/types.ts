/**
 * Enum dla poziomów logowania
 * @export LogLevel - poziomy logowania
 * @enum silent - brak logowania
 * @enum info - logowanie informacji
 * @enum warn - logowanie ostrzeżeń
 * @enum error - logowanie błędów
 * @enum debug - logowanie debugowania
 */
export enum LogLevel {silent = 'silent', info = 'info', warn = 'warn', error = 'error', debug = 'debug'}

/**
 * Typ dla odpowiedzi z API
 * @export ApiResponse - odpowiedź z API
 * @template T - typ danych w odpowiedzi
 * @property statusCode - kod statusu odpowiedzi
 * @property isSuccess - czy odpowiedź jest udana
 * @property errorMessage - wiadomość błędu
 * @property errorDetails - szczegóły błędu
 * @property data - dane odpowiedzi (jeśli odpowiedź jest udana, inaczej null)
 */
export type ApiResponse<T> = {
    statusCode: number;
    isSuccess: boolean;
    errorMessage?: string | null;
    errorDetails?: string | null;
    data?: T | null;
}