# Back-end Phase 3 - Pokemon API

This is an API that allow users to validate a pokemon from the official [pokeapi](pokeapi.co) and read their power (base experience). Users can add these pokemons to their team and set a nickname for each. 

Simply open it in Visual Studio and run, the swagger page with all the endpoints will showup. 

## Advanced Features List

- [x] Onion structure - clear separation of DB access layer and API layer
- [x] Usage of EF Core
- [ ] Usage of caching to speed up calls
- [x] End to end testing using Postman or JMeter
- [x] Comprehensive unit testing
- [ ] OAuth2 with PKCE login w/ at least ONE third party provider
- [ ] Implementation of websockets using SignalR
- [ ] Deployment using a CI/CD pipeline to the cloud
- [x] Usage of Fluent Validation / Fluent Assertions
- [ ] Demonstration of complex BE logic

## Onion structure - clear separation of DB access layer and API layer
The API has an onion structure. The `Domain` folder being the domain layer, it defines the fundamental `Pokemon` entity.
The `DBRepo` is the repository layer, it fetches and alter data stored in the database. The `PokemonController` is the service layer, because the API has a simple functionality of managing a pokemon team, the business logic is not much different from the logic in the repository layer, the controller fetch data from external API and connects the UI layer with repository layer. UI layer is handled by swagger. 

## Usage of EF Core
The API uses EF Core to interact with the database. The `Pokemon` class used to model the pokemon datas and `DBContext` class that query and save the data. 

## End to end testing using Postman
[![Run in Postman](https://run.pstmn.io/button.svg)](https://app.getpostman.com/run-collection/17071782-560dcaa4-dfa3-410b-a92e-4c500a21b97f?action=collection%2Ffork&collection-url=entityId%3D17071782-560dcaa4-dfa3-410b-a92e-4c500a21b97f%26entityType%3Dcollection%26workspaceId%3D2fa8564e-44e0-4971-b7bf-5f18e1b31955)
All endpoints and its success and fail cases are ran through postman, postman test are also written to test the response statuscode and content. 
![image](https://user-images.githubusercontent.com/88317853/188636092-89eb8587-776c-420a-926f-678f60f905b1.png)
![image](https://user-images.githubusercontent.com/88317853/188636247-247e91c1-333d-4615-a4fe-f70d6d812f83.png)

## Comprehensive unit testing
Unit testing is performed on all services in the API. Aside from the test that overlapped with postman, unit testing also tested the functionality of the repository layer. NUnit was used as framework, NSubstitute was used for mocking, FluentValidation to validate the response content and FluentAssertion to assert test results. 

## Usage of Fluent Validation / Fluent Assertion
Fluent Validation validates all pokemon stored has a name attribute, and a power value no less than 36 and no more than 635 unless the are in a special less of unknow powered pokemons. Fluent Assertion assert test results. 

## API Structure

1. `PokemonController` - contains all CRUD and connection to external api.
2. `DBContext` - a class that defines the database. The database consist of one table holding all pokemon records in the team.
3. `DBRepo` - a class that fetches and adds pokemon object to the `DBContext`.
4. `IDBRepo`, `IDBContext` - interfaces used for testing purposes
5. `UnitTest1` - contains testing to repository, dbcontext, and all methods in the controller.
6. `Configuration files` - appsetting.json and appsetting.Development.json is created. appsetting.Development.json stores the external api address and the name of the database file. This distinction is made to ensure these information are kept private. 
