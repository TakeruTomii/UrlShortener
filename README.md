# UrlShortener

A Web API solution to shorten given URLs.

## Environment

- .NET Core 8.0
- Visual Studio 2022.

## How to Launch

1. Click UrlShortenerApp/UrlShortenApp.sln
2. Choose "Debug", "Any CPU", "http" and Start Debug.

## How to Use

I reccomend you to use HTTP Request tool such as Postman to request the endpoint.
You may refer to swagger UI when it shows after launching for reference.

## Endpoints

1. `POST /short-url`
   To shorten a long URL.
   It possess the mapping of the short and original URLs and notify it to the users.
   You may pass the short URL in the following endopoint to redirect.

2. `GET /{shortUrl}`
   To redirect from the short URL.
   Redirect user
