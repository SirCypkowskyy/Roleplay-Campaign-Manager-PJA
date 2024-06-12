import {type ClassValue, clsx} from "clsx"
import {twMerge} from "tailwind-merge"
import {LogLevel} from "@/shared/types.ts";
import {siteConfig} from "@/config/site.ts";

export function cn(...inputs: ClassValue[]) {
    return twMerge(clsx(inputs))
}

const appLogLevel = siteConfig.logLevel;

/**
 * Moja autorska metoda do logowania z uwzględnieniem poziomu logowania
 * @param log poziom logowania 
 * @param args argumenty do zalogowania
 */
export function myLog(log: LogLevel = LogLevel.info, ...args: any[]) {
    const origin = new Error().stack?.split('\n')[2].trim();
    switch (appLogLevel) {
        case LogLevel.silent:
            break;
        case LogLevel.info:
            if (log === LogLevel.info)
                console.info(`[${origin}]`, ...args);
            break;
        case LogLevel.warn:
            if (log === LogLevel.warn || log === LogLevel.error)
                console.info(`[${origin}]`, ...args);
            break;
        case LogLevel.error:
            if (log === LogLevel.error || log === LogLevel.warn || log === LogLevel.info)
                console.info(`[${origin}]`, ...args);
            break;
        case LogLevel.debug:
            if (log === LogLevel.debug || log === LogLevel.error || log === LogLevel.warn || log === LogLevel.info)
                console.info(`[${origin}]`, ...args);
            break;
        default:
            console.info(`[${origin}]`, ...args);
            break;
    }
}
