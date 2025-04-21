# CodeFirstApi

Mainly a generator project, to create REST API and appropriate interfaces for easy working in Blazor InteractiveAuto render mode.
- Based on interface definition in Shared project
  - Create Controller based REST API, to be used in Backend (respecting Authorize attribute).
  - Create a client to be used in Razor Components in WebAssembly Render mode, which calls the REST API above.
  - Add wrapper around the service implmentation in Backend for use in ServerSide Render mode, so it is protected as the REST API is.
  - For the wrappers used when running on the server and in browser, to automatically implement State Persistance for use in Prerendering (to remove the flicker on mode change with prerendering)

# Usage
See the project in the `sample` folder.

# Version info
This is only pre-alpha version for evaluating the idea, and not yet ready for production use.
Nuget package will not be released later on when the code is a bit more stable.


## Next TODOs

- add route attribute support
  - use arguments that are mentioned in route direct instead of JSON
- better handling of state persistance key for arguments (using JSON of arguments now, which is not optimal)
- add Verb attribute (can be combined with route)
- add allow anonymous attribute
- release as Nuget package



## Similar

# https://github.com/reactiveui/refit
- Generate a REST client from code
- ie code first but only for REST Client, not for server side
- Seems alternative to Swagger somehow

# https://github.com/canton7/RestEase
- Inspired by Refit
- Seems generally the same, generate clients
 

https://github.com/bpawluk/EasyApi
- Handles both client and server side
- But each request is a separate class
- A bit too verbose due to that
- And due to the class per method, quite a lot needs to be injected to use adjecent functionality
- the `@inject ICall<AddComment, Guid> AddComment` is a good example of that
  - you need to know what request class you want to use and what it should return. 
  - This seems unnecessary.