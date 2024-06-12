/**
 * DTO odpowiedzi z danymi użytkownika
 * @param id - identyfikator użytkownika
 * @param username - nazwa użytkownika
 * @param email - adres email użytkownika
 */
export type User = {
    id: number;
    username: string;
    email: string;
}

/**
 * DTO odpowiedzi z tokenem JWT do autoryzacji
 * @param token - token JWT
 * @param refreshToken - token odświeżający
 * @param expires - data wygaśnięcia tokenu
 * @param username - nazwa użytkownika
 */
export type JwtResponse = {
    token: string;
    refreshToken: string;
    expires: Date | string;
    username: string;
}


/**
 * DTO odpowiedzi z błędem 400
 * @param type - typ błędu
 * @param title - tytuł błędu
 * @param status - status błędu
 * @param errors - lista błędów
 * @param traceId - identyfikator śladu
 */
export type BadRequestResponse = {
    type: string;
    title: string;
    status: number;
    errors: Record<string, string[]>;
    traceId: string;
}

/**
 * Wiadomość w czacie
 * @param text - treść wiadomości
 * @param sender - nadawca wiadomości
 * @param character - avatar nadawcy
 * @param time - czas wysłania wiadomości
 */
export type Message = {
    text: string;
    sender: string;
    character: string;
    time: string
}