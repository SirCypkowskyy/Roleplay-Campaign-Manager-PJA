/**
 * DTO odpowiedzi z danymi użytkownika
 * @param id - identyfikator użytkownika
 * @param username - nazwa użytkownika
 * @param email - adres email użytkownika
 * @param role - rola użytkownika
 * */
export type User = {
    id: number;
    username: string;
    email: string;
    role?: string;
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

/**
 * DTO odpowiedzi z wyzwaniem autoryzacyjnym
 * @param username - nazwa użytkownika
 * @param email - adres email użytkownika
 * @param role - rola użytkownika
 */
export type AuthChallengeResponse = {
    username: string;
    email: string;
    role: string;
}


/**
 * DTO odpowiedzi z danymi do wyświetlenia na dashboardzie użytkownika
 */
export type DashboardCampaignResponse = {
    campaignGuid: string;
    campaignHost: string;
    campaignHostGuid: number;
    campaignName: string;
    lastMessageDate: string;
}

/**
 * DTO odpowiedzi z danymi do wyświetlenia na dashboardzie użytkownika
 * @param numberOfActiveCampaignsTotal - liczba aktywnych kampanii
 * @param numberOfActiveCampaignsInLast30Days - liczba aktywnych kampanii w ciągu ostatnich 30 dni
 * @param messagesSentTotal - liczba wysłanych wiadomości
 * @param messagesSentInLast30Days - liczba wysłanych wiadomości w ciągu ostatnich 30 dni
 * @param createdCharactersTotal - liczba stworzonych postaci
 * @param activeCampaigns - lista aktywnych kampanii
 */
export type UserDashboardDataResponse = {
    numberOfActiveCampaignsTotal: number;
    numberOfActiveCampaignsInLast30Days: number;
    messagesSentTotal: number;
    messagesSentInLast30Days: number;
    createdCharactersTotal: number;
    activeCampaigns: DashboardCampaignResponse[];
}

/**
 * Format odpowiedzi z SignalR
 * @param type - typ wiadomości
 * @param target - cel wiadomości
 * @param arguments - argumenty wiadomości
 */
export type SignalRResponseMessage = {
    type: number;
    target: string;
    arguments: any[];
}

/**
 * Typ argumentów wiadomości otrzymanej z SignalR
 * @param message - treść wiadomości
 * @param author - autor wiadomości
 * @param character - avatar autora
 * @param authorAppRole - rola autora
 * @param authorCampaignRole - rola autora w kampanii
 * @param avatarResourceId - identyfikator zasobu avatara
 */
export type SignalRChatMessageReceivedArgumentsType = {
    message: string;
    author?: string;
    character?: string;
    authorAppRole?: string;
    authorCampaignRole?: string;
    avatarResourceId?: number;
}
