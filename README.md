# MatePortal API

## List of contents

- ### [Description](#description)
- ### [Stack](#stack)
- ### [Installation](#installation)
- ### [Configuration](#configuration)
- ### [Endpoints](#endpoints)
- ### [Tests](#tests)
- ### [License](#license)

## Description
**MatePortal** is a **.NET 6** implemented Web API for exchanging opinions about different types of yerba mate. Users can create accounts, add yerba mate to their favorites list, rate and comment on yerba mate and yerba mate shops.

## Stack
* **Entity Framework Core** - database ORM
* **MediatR** - for mediator and CQRS patterns
* **JwtBearer** - for authorization
* **Open API** - for API documentation
* **AutoMapper** - for mapping DTO-s and EntityModels data
* **Fluent Validation** - for data validation
* **xUnit** - for tests

## Installation
Make sure you have the **.NET 6.0 SDK** installed on your machine. Then do:  
>`git clone https://github.com/TomWia9/MatePortal.git`  
`cd MatePortal`  
`dotnet run`

## Configuration
This will need to be perfored before running the application for the first time
1. You have to change ConnectionString in [`appsettings.json`](./src/Api/appsettings.json) for ConnectionString that allow you to connect with database in your computer.
2. Issue the Entity Framework command to update the database  
`dotnet ef database update`

- To add migration (make sure you start in Infrastructure directory): <br>
  `dotnet ef migrations add <name of new migration> -c ApplicationDbContext -s ./../Api` <br>

- To remove migration (make sure you start in Infrastructure directory): <br>
  `dotnet ef migrations remove -c ApplicationDbContext -s ./../Api`

## Endpoints

JWT token must be provided in the header when accessing sensitive data.

### Identity 

|Method|Path|Body|Params|Description|Responses|Who can access|
|:----|:----|:----|:----|:----|:----|:----|
|POST|/api/Identity/register|RegisterUserCommand|None|Creates a new user and returns JWT token|AuthSuccessResponse, 400|Everyone|
|POST|/api/Identity/login|LoginUserCommand|None|Returns JWT token|AuthSuccessResponse, 400|Everyone|


### Users

|Method|Path|Body|Params|Description|Responses|Who can access|
|:----|:----|:----|:----|:----|:----|:----|
|GET|/api/Users|none|UsersQueryParameters|Gets all users|IEnumerable<UserDto>|Everyone|
|GET|/api/Users/{id}|none|Id:GUID|Gets user by id|UserDto, 404|Everyone|
|PUT|/api/Users|UpdateUserCommand|None|Updates the user|204, 400, 404, 401, 403| Account owner, Administrator|
|DELETE|/api/Users|DeleteUserCommand|None|Deletes the user|204, 400, 404, 401, 403|Account owner, Administrator|

### Brands

|Method|Path|Body|Params|Description|Responses|Who can access|
|:----|:----|:----|:----|:----|:----|:----|
|GET|/api/Brands|none|BrandsQueryParameters|Gets all brands|IEnumerable<BrandDto>|Everyone|
|GET|/api/Brands /{id}|none|Id:GUID|Gets brand by id|BrandDto, 404|Everyone|
|POST|/api/Brands|CreateBrandCommand|None|Creates brand|201, 400, 401, 409|Administrator|
|PUT|/api/Brands|UpdateBrandCommand|None|Updates the brand|204, 400, 404, 401, 409| Administrator|
|DELETE|/api/Brands|DeleteBrandCommand|None|Deletes the brand|204, 400, 404, 401|Administrator|

### Categories

|Method|Path|Body|Params|Description|Responses|Who can access|
|:----|:----|:----|:----|:----|:----|:----|
|GET|/api/Categories|None|CategoriesQueryParameters|Gets all categories|IEnumerable<CategoryDto>|Everyone|
|POST|/api/Categories|CreateCategoryCommand|None|Creates category|201, 400, 401, 409|Administrator|
|PUT|/api/Categories|UpdateCategoryCommand|None|Updates the category|204, 400, 404, 401, 409| Administrator|
|DELETE|/api/Categories|DeleteCategoryCommand|None|Deletes the category|204, 400, 404, 401|Administrator|

### Countries

|Method|Path|Body|Params|Description|Responses|Who can access|
|:----|:----|:----|:----|:----|:----|:----|
|GET|/api/Countries|None|Countries QueryParameters|Gets all countries|IEnumerable<CountryDto>|Everyone|
|POST|/api/Countries|CreateCountryCommand|None|Creates country|201, 400, 401, 409|Administrator|
|PUT|/api/Countries|UpdateCountryCommand|None|Updates the country|204, 400, 404, 401, 409| Administrator|
|DELETE|/api/Countries|DeleteCountryCommand|None|Deletes the country|204, 400, 404, 401|Administrator|

### Favourites

|Method|Path|Body|Params|Description|Responses|Who can access|
|:----|:----|:----|:----|:----|:----|:----|
|GET|/api/Favourites/{userId}|None|FavouritesQueryParameters|Gets user’s favourites yerba mates|IEnumerable<FavouriteDto>|Everyone|
|POST|/api/Favourites|CreateFavouriteCommand|None|Creates favourite|201, 400, 401, 409|User|
|DELETE|/api/Favourites|DeleteFavouriteCommand|None|Deletes the favourite|204, 400, 404, 401|User|

### Shops

|Method|Path|Body|Params|Description|Responses|Who can access|
|:----|:----|:----|:----|:----|:----|:----|
|GET|/api/Shops|None|Shops QueryParameters|Gets all shops|IEnumerable<ShopDto>|Everyone|
|GET|/api/Shops/{id}|None|Id:GUID|Gets shop by id|ShopDto, 404|Everyone|
|POST|/api/Shops|CreateShopCommand|None|Creates shop|201, 400, 401, 409|Administrator|
|PUT|/api/Shos|UpdateShopCommand|None|Updates the shop|204, 400, 404, 401, 409| Administrator|
|DELETE|/api/Shops|DeleteShopCommand|None|Deletes the shop|204, 400, 404, 401|Administrator|

### Shop opinions

|Method|Path|Body|Params|Description|Responses|Who can access|
|:----|:----|:----|:----|:----|:----|:----|
|GET|/api/ShopOpinions|None|ShopOpinions QueryParameters|Gets all shop opinions|IEnumerable<ShopOpinionDto>|Everyone|
|GET|/api/ShopOpinions /{id}|None|Id:GUID|Gets shop opinion by id|ShopOpinionDto, 404|Everyone|
|POST|/api/ShopOpinions|CreateShopOpinionCommand|None|Creates shop opinion|201, 400, 401, 409|User, Administrator|
|PUT|/api/ShopOpinions|UpdateShopOpinionCommand|None|Updates the shop opinion|204, 400, 404, 401, 409| User, Administrator|
|DELETE|/api/ShopOpinions|DeleteShopOpinionCommand|None|Deletes the shop opinion|204, 400, 404, 401|User, Administrator|

### Yerba Mates

|Method|Path|Body|Params|Description|Responses|Who can access|
|:----|:----|:----|:----|:----|:----|:----|
|GET|/api/YerbaMates|None|YerbaMates QueryParameters|Gets all yerba mates|IEnumerable<YerbaMate>|Everyone|
|GET|/api/YerbaMates /{id}|None|Id:GUID|Gets yerba mate by id|YerbaMateDto, 404|Everyone|
|POST|/api/YerbaMates|CreateYerbaMateCommand|None|Creates yerba mate|201, 400, 401, 409|Administrator|
|PUT|/api/YerbaMates|UpdateYerbaMateCommand|None|Updates the yerba mate|204, 400, 404, 401, 409| Administrator|
|DELETE|/api/YerbaMates|DeleteYerbaMateCommand|None|Deletes the yerba mate|204, 400, 404, 401|Administrator|
|POST|/api/YerbaMates/createImage|CreateYerbaMateImageCommand|None|Creates yerba mate image|201, 400, 401|Administrator|
|DELETE|/api/YerbaMates/deleteImage|DeleteYerbaMateImageCommand|None|Deletes the yerba mate image|204, 400, 404, 401|Administrator|

### Yerba Mates opinions

|Method|Path|Body|Params|Description|Responses|Who can access|
|:----|:----|:----|:----|:----|:----|:----|
|GET|/api/YerbaMateOpinions|None|ShopOpinions QueryParameters|Gets all yerba mate opinions|IEnumerable<YerbaMateOpinionDto>|Everyone|
|GET|/api/YerbaMateOpinions /{id}|None|Id:GUID|Gets yerba mate opinion by id|YerbaMateOpinionDto, 404|Everyone|
|POST|/api/YerbaMateOpinions|CreateYerbaMateOpinionCommand|None|Creates yerba mate opinion|201, 400, 401, 409|User, Administrator|
|PUT|/api/YerbaMateOpinions|UpdateYerbaMateOpinionCommand|None|Updates the yerba mate opinion|204, 400, 404, 401, 409| User, Administrator|
|DELETE|/api/YerbaMateOpinions|DeleteYerbaMateOpinionCommand|None|Deletes the yerba mate opinion|204, 400, 404, 401|User, Administrator|

## Tests
This project use integration tests provided by xUnit.  
To run tests go to the root folder directory and do:  
`dotnet test`

## License
[MIT](https://choosealicense.com/licenses/mit/)