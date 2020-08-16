# Pokemon

This application has been developed using:
- Visual Studio 2019
- .NET Core 3.1

# Requirements
In order to run the application ***docker*** (and ***docker-compose***) must be installed.

It has been tested with the following versions:
 - Docker: version 19.03.8, build afacb8b
 - Docker-compose: version 1.25.4, build 8d51620a


# Instructions

In order to build and start the application run the following script located in the project root:

- ***On Windows***
    ```
    > start.bat
    ```
    
- ***On Linux***
    ```sh
    $ start.sh
    ```

# Use the API
The web API is available at http://localhost:5000/pokemon/{pokemonName} (or at any available local network interfaces).

E.g.

Requesting the description of "ditto"

> http http://localhost:5000/pokemon/ditto

should return a response like this

```
HTTP/1.1 200 OK
Content-Type: application/json; charset=utf-8
Date: Sat, 15 Aug 2020 08:48:41 GMT
Server: Kestrel
Transfer-Encoding: chunked

{
    "description": "'t transforms into thither's few or none will entertain it 't sees. If 't be true the thing 't’s transforming into isn’t right in front of 't,  ditto relies on its memory—so oft 't fails.",
    "name": "ditto"
}

```

It's also possible to specify the client's preferred language.

For example we may want to get "ditto's" pokemon description in spanish ("es"):

> http http://localhost:5000/pokemon/ditto Accept-Language:es

this should return
```
HTTP/1.1 200 OK
Content-Type: application/json; charset=utf-8
Date: Sat, 15 Aug 2020 08:59:52 GMT
Server: Kestrel
Transfer-Encoding: chunked

{
    "description": "Puede transformarse en cualquier cosa que vea,\npero, si intenta hacerlo de memoria, habrá\ndetalles que se le escapen.",
    "name": "ditto"
}
```

please note that in this case, not Shakespearean translation will be done, since it can only translate from english ("en" language).
