namespace MasFinalProj.API.Extensions;

/// <summary>
/// Rozszerzenia dla przeglądarki API Scalar.
/// </summary>
public static class ScalarExtensions
{
    /// <summary>
    /// Mapuje stronę API Scalar.
    /// </summary>
    /// <param name="endpoints"></param>
    /// <returns></returns>
    public static IEndpointConventionBuilder MapScalarUi(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/scalar/{documentName}", (string documentName) => Results.Content($$"""
              <!doctype html>
              <html>
              <head>
                  <title>Scalar API Reference -- {{documentName}}</title>
                  <meta charset="utf-8" />
                  <meta
                  name="viewport"
                  content="width=device-width, initial-scale=1" />
              </head>
              <body>
                  <script
                  id="api-reference"
                  data-url="/swagger/v1/{{documentName}}.json"></script>
                  <script>
                  var configuration = {
                      theme: 'purple',
                  }
              
                  document.getElementById('api-reference').dataset.configuration =
                      JSON.stringify(configuration)
                  </script>
                  <script src="https://cdn.jsdelivr.net/npm/@scalar/api-reference"></script>
              </body>
              </html>
              """, "text/html")).ExcludeFromDescription();
    }
}