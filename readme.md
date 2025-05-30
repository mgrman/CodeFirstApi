﻿# CodeFirstApi

Code generator project, to create REST API and appropriate interfaces for easy working in Blazor InteractiveAuto render mode.
- Based on interface definition in Shared project
  - Create Controller based REST API, to be used in Backend (respecting Authorize attribute).
  - Create a client to be used in Razor Components in WebAssembly Render mode, which calls the REST API above.
  - Add wrapper around the service implementation in Backend for use in ServerSide Render mode, so it is evaluating the same Authorize attribute definition as the REST API is.
  - For the wrappers used when running on the server and in browser, to automatically implement State Persistance for use in Prerendering (to remove the flicker on mode change with prerendering)

This means, that you define the interface in Shared project, and the generator will create the necessary code for you.
Handling different render modes as needed and abstracting away the details from Razor Components pages.

Adding or updating the API, means adding a method any of the existing marked interfaces or creating new interfaces in the shared project. 
And of course implementing this interface in the backend and adding it to the DI container.


# Supported attributes

In projects that reference the CodeFirstApi.Generator project, you can use the following attributes.
Reference CodeFirstApi.Core project to get the attributes.

- GenerateHttpClients 
  - Assembly attribute, generates the clients for the services, to be used in Client project.
- GenerateHttpControllers
  - Assembly attribute, generates the controllers and service wrappers, to be used in Backend project.
- GenerateServices
  - Interface attribute, marks the interface as a service, to be used by the generator.
- PersistForPrerendering
  - Generates the code for Persisting the pre-rendered state of the component, based on https://learn.microsoft.com/en-us/aspnet/core/blazor/components/prerender?view=aspnetcore-9.0#persist-prerendered-state
- Authorize
  - Interface or method attribute, marks the interface or method as requiring authorization (uses the same definition as the built-in version of this attribute but is for use with interfaces).


# Usage examples

See the project in the `sample` folder.

Or see the [Demo app](https://codefirstapi.azurewebsites.net/).


# Usage notice

This is only pre-alpha version for evaluating the idea, and not yet ready for production use.
Nuget package will not be released later on when the code is a bit more stable.


## Next TODOs

1. release as Nuget package
2. add diagram of generated code 
3. add exact steps to add this to new projects


### Rest of TODOs
 
- add route attribute support
  - use arguments that are mentioned in route direct instead of JSON
- better handling of state persistance key for arguments (using JSON of arguments now, which is not optimal)
- add Verb attribute (can be combined with route)
- add allow anonymous attribute
- add Unity HTTP Client generator support


## Similar

- https://github.com/reactiveui/refit
  - Generate a REST client from code
  - ie code first but only for REST Client, not for server side
  - Seems alternative to Swagger somehow

- https://github.com/canton7/RestEase
  - Inspired by Refit
  - Seems generally the same, generate clients

- https://github.com/bpawluk/EasyApi
  - Handles both client and server side
  - But each request is a separate class
  - A bit too verbose due to that
  - And due to the class per method, quite a lot needs to be injected to use adjecent functionality
  - the `@inject ICall<AddComment, Guid> AddComment` is a good example of that
    - you need to know what request class you want to use and what it should return. 
    - This seems unnecessary.

- https://github.com/RicoSuter/NSwag
  - Can create Clients from Server side APIs
  - But needs an extra build step to generate OpenAPI specification, from which the Clients can be generated
  - And needs extra configuration to share the types themselves, as there is an in between format instead of using code itself for definition.