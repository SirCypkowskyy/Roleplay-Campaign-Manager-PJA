# Aplikacja webowa

## Spis treści

- [Opis](#opis)
- [Uruchomienie](#uruchomienie)
- [Użyte technologie](#użyte-technologie)

## Opis

Implementacja aplikacji webowej, wykonanej z React.js z TypeScriptem w ramach Vite.js. Aplikacja komunikuje się z API (ASP.NET Core Web API) za pomocą zapytań HTTP, korzystając z natywnej funkcji fetch.

## Uruchomienie

Aplikacja powinna uruchomić się automatycznie przy uruchomieniu API (w konfiguracji `http`). W przeciwnym wypadku, należy uruchomić aplikację ręcznie za pomocą komendy `npm run dev`.

## Użyte technologie

- React.js - biblioteka do tworzenia interfejsów użytkownika
- TypeScript - nadzbiór JavaScriptu
- Vite.js - narzędzie do budowania aplikacji webowych
- Tailwind CSS - framework CSS (w celu szybkiego stylowania aplikacji)
- Shad.cn - biblioteka gotowych komponentów React/TailwindCSS (w celu szybkiego tworzenia interfejsu użytkownika)
- Zustand - biblioteka do zarządzania stanem aplikacji
- React Router - biblioteka do zarządzania nawigacją i routingiem w aplikacji
- microsoft/signalr - biblioteka do obsługi komunikacji z serwerem za pomocą protokołu SignalR (w celu obsługi komunikatów w czasie rzeczywistym)
- RadixUI - biblioteka gotowych komponentów React, dependency Shad.cn (w celu szybkiego tworzenia interfejsu użytkownika)
- Cookies - biblioteka do zarządzania ciasteczkami w przeglądarce