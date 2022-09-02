# Pokemon API

This is an API that allow users to validate a pokemon from the official [pokeapi](pokeapi.co) and read their power (base experience). Users can add these pokemons to their team and set a nickname for each. 

Simply open it in Visual Studio and run, the swagger page with all the endpoints will showup. 

[![Run in Postman](https://run.pstmn.io/button.svg)](https://app.getpostman.com/run-collection/17071782-560dcaa4-dfa3-410b-a92e-4c500a21b97f?action=collection%2Ffork&collection-url=entityId%3D17071782-560dcaa4-dfa3-410b-a92e-4c500a21b97f%26entityType%3Dcollection%26workspaceId%3D2fa8564e-44e0-4971-b7bf-5f18e1b31955)

## API Structure

1. `PokemonController` - contains all CRUD and connection to external api.
2. `DBContext` - a class that defines the database. The database consist of one table holding all pokemon records in the team.
3. `DBRepo` - a class that fetches and adds pokemon object to the `DBContext`.
4. `IDBRepo`, `IDBContext` - interfaces used for testing purposes
5. `UnitTest1` - contains testing to repository, dbcontext, and all methods in the controller.
6. `Configuration files` - appsetting.json and appsetting.Development.json is created. appsetting.Development.json stores the external api address and the name of the database file. This distinction is made to ensure these information are kept private. 

## Phase 2 Requirements - Backend

### Section One
You can:
* `GET` - your pokemon team (initially empty); a pokemon's name, power and nickname if its in the team; a list of all items in pokemon from the external api; raw data of a pokemon from the exteranl api;
* `POST` - add a pokemon to your team if its official;
* `DELETE` - delete a pokemon from your team;
* `PUT` - set or change the nickname of the pokemon's in your team.

### Section Two

`Dependency Injection` is used in many instance in the class, it allows the use of interface for testing and flexible dependency between classes. 

### Section Three

`NSubstitute` is used for the DBRepo class when testing, this allows me to test that the controller methods reaches the repository methods correctly. I also tested the repository will correctly retrieves data from the DBContext instance. 