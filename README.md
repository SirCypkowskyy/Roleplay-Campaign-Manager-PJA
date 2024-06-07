# MAS Final Project

Kolekcja instrukcji do projektu końcowego z MAS.

## Instrukcje uruchomienia

Uruchamianie aplikacji bez całkowitej konteneryzacji:

1. Zmień connection string w pliku `appsettings.json` na swój, lub utwórz kontener Docker z bazą danych, korzystając z pliku docker-compose.yml (użyj komendy `docker compose up -d db` w folderze, gdzie znajduje się docker-compose).
2. Uruchom migracje, wpisując w konsoli `dotnet ef database update -p ../MasFinalProj.Persistence -c DatabaseContext -s .`.
3. Uruchom aplikację.

Uruchamianie aplikacji z konteneryzacją:

1. Uruchom kontenery, wpisując w konsoli `docker compose up -d`.
2. Uruchom migracje, wpisując w konsoli `dotnet ef database update -p ../MasFinalProj.Persistence -c DatabaseContext -s .`.


## Źródła

- Zdjęcia: https://www.optionalrule.com/2021/03/10/imagery-art-resources-for-rpg-creators/