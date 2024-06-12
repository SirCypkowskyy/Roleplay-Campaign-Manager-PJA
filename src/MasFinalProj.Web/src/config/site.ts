import {LogLevel} from "@/shared/types.ts";

/**
 * Ustawienia konfiguracyjne strony
 * @property name - nazwa strony
 * @property url - adres strony
 * @property ogImage - obrazek do wyświetlenia w social media
 * @property author - autor strony
 * @property authorWebsite - strona autora
 * @property description - opis strony
 * @property keywords - słowa kluczowe
 * @property logLevel - poziom logowania
 */
export const siteConfig = {
    name: "Roleplay Master",
    url: "https://roleplaymaster.com",
    ogImage: "https://roleplaymaster.com/og-image.png",
    author: "Cyprian Gburek s24759",
    authorWebsite: "https://cg-personal.vercel.app/",
    description: "Roleplay Master is a platform for roleplayers to connect, collaborate, and create amazing roleplay experiences together.",
    keywords: "roleplay, roleplay master, roleplay platform, roleplay community, roleplay collaboration, roleplay creation",
    logLevel: "info" as LogLevel
}

export type SiteConfig = typeof siteConfig