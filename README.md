# MatePortal API

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
|POST|/api/Identity/register|[RegisterUserCommand](./src/Application/Users/Commands/RegisterUser/RegisterUserCommand.cs)|None|Creates a new user and returns JWT token|[AuthSuccessResponse](./src/Application/Users/Responses/AuthSuccessResponse.cs), [AuthFailedResponse](./src/Application/Users/Responses/AuthFailedResponse.cs) 400|Everyone|
|POST|/api/Identity/login|[LoginUserCommand](./src/Application/Users/Commands/LoginUser/LoginUserCommand.cs)|None|Returns JWT token|[AuthSuccessResponse](./src/Application/Users/Responses/AuthSuccessResponse.cs), [AuthFailedResponse](./src/Application/Users/Responses/AuthFailedResponse.cs), 400|Everyone|


### Users

|Method|Path|Body|Params|Description|Responses|Who can access|
|:----|:----|:----|:----|:----|:----|:----|
|GET|/api/Users|none|[UsersQueryParameters](./src/Application/Users/Queries/UsersQueryParameters.cs)|Gets all users|IEnumerable<[UserDto](./src/Application/Users/Queries/UserDto.cs)>|Everyone|
|GET|/api/Users/{id}|none|Id:GUID|Gets user by id|[UserDto](./src/Application/Users/Queries/UserDto.cs), 404|Everyone|
|PUT|/api/Users|[UpdateUserCommand](./src/Application/Users/Commands/UpdateUser/UpdateUserCommand.cs)|None|Updates the user|204, 400, 404, 401, 403| Account owner, Administrator|
|DELETE|/api/Users|[DeleteUserCommand](./src/Application/Users/Commands/DeleteUser/DeleteUserCommand.cs)|None|Deletes the user|204, 400, 404, 401, 403|Account owner, Administrator|

### Brands

|Method|Path|Body|Params|Description|Responses|Who can access|
|:----|:----|:----|:----|:----|:----|:----|
|GET|/api/Brands|none|[BrandsQueryParameters](./src/Application/Brands/Queries/BrandsQueryParameters.cs)|Gets all brands|IEnumerable<[BrandDto](./src/Application/Brands/Queries/BrandDto.cs)>|Everyone|
|GET|/api/Brands /{id}|none|Id:GUID|Gets brand by id|[BrandDto](./src/Application/Brands/Queries/BrandDto.cs), 404|Everyone|
|POST|/api/Brands|[CreateBrandCommand](./src/Application/Brands/Commands/CreateBrand/CreateBrandCommand.cs)|None|Creates brand|201, 400, 401, 409|Administrator|
|PUT|/api/Brands|[UpdateBrandCommand](./src/Application/Brands/Commands/UpdateBrand/UpdateBrandCommand.cs)|None|Updates the brand|204, 400, 404, 401, 409| Administrator|
|DELETE|/api/Brands|[DeleteBrandCommand](./src/Application/Brands/Commands/DeleteBrand/DeleteBrandCommand.cs)|None|Deletes the brand|204, 400, 404, 401|Administrator|

### Categories

|Method|Path|Body|Params|Description|Responses|Who can access|
|:----|:----|:----|:----|:----|:----|:----|
|GET|/api/Categories|None|[CategoriesQueryParameters](./src/Application/Categories/Queries/CategoriesQueryParameters.cs)|Gets all categories|IEnumerable<[CategoryDto](./src/Application/Categories/Queries/CategoryDto.cs)>|Everyone|
|POST|/api/Categories|[CreateCategoryCommand](./src/Application/Categories/Commands/CreateCategory/CreateCategoryCommand.cs)|None|Creates category|201, 400, 401, 409|Administrator|
|PUT|/api/Categories|[UpdateCategoryCommand](./src/Application/Categories/Commands/UpdateCategory/UpdateCategoryCommand.cs)|None|Updates the category|204, 400, 404, 401, 409| Administrator|
|DELETE|/api/Categories|[DeleteCategoryCommand](./src/Application/Categories/Commands/DeleteCategory/DeleteCategoryCommand.cs)|None|Deletes the category|204, 400, 404, 401|Administrator|

### Countries

|Method|Path|Body|Params|Description|Responses|Who can access|
|:----|:----|:----|:----|:----|:----|:----|
|GET|/api/Countries|None|[CountriesQueryParameters](./src/Application/Countries/Queries/CountriesQueryParameters.cs)|Gets all countries|IEnumerable<[CountryDto](./src/Application/Countries/Queries/CountryDto.cs)>|Everyone|
|POST|/api/Countries|[CreateCountryCommand](./src/Application/Countries/Commands/CreateCountry/CreateCountryCommand.cs)|None|Creates country|201, 400, 401, 409|Administrator|
|PUT|/api/Countries|[UpdateCountryCommand](./src/Application/Countries/Commands/UpdateCountry/UpdateCountryCommand.cs)|None|Updates the country|204, 400, 404, 401, 409| Administrator|
|DELETE|/api/Countries|[DeleteCountryCommand](./src/Application/Countries/Commands/DeleteCountry/DeleteCountryCommand.cs)|None|Deletes the country|204, 400, 404, 401|Administrator|

### Favourites

|Method|Path|Body|Params|Description|Responses|Who can access|
|:----|:----|:----|:----|:----|:----|:----|
|GET|/api/Favourites/{userId}|None|[FavouritesQueryParameters](./src/Application/Favourites/Queries/FavouritesQueryParameters.cs)|Gets user’s favourites yerba mates|IEnumerable<[FavouriteDto](./src/Application/Favourites/Queries/FavouriteDto.cs)>|Everyone|
|POST|/api/Favourites|[CreateFavouriteCommand](./src/Application/Favourites/Commands/CreateFavourite/CreateFavouriteCommand.cs)|None|Creates favourite|201, 400, 401, 409|User|
|DELETE|/api/Favourites|[DeleteFavouriteCommand](./src/Application/Favourites/Commands/DeleteFavourite/DeleteFavouriteCommand.cs)|None|Deletes the favourite|204, 400, 404, 401|User|

### Shops

|Method|Path|Body|Params|Description|Responses|Who can access|
|:----|:----|:----|:----|:----|:----|:----|
|GET|/api/Shops|None|[ShopsQueryParameters](./src/Application/Shops/Queries/ShopsQueryParameters.cs)|Gets all shops|IEnumerable<[ShopDto](./src/Application/Shops/Queries/ShopDto.cs)>|Everyone|
|GET|/api/Shops/{id}|None|Id:GUID|Gets shop by id|[ShopDto](./src/Application/Shops/Queries/ShopDto.cs), 404|Everyone|
|POST|/api/Shops|[CreateShopCommand](./src/Application/Shops/Commands/CreateShop/CreateShopCommand.cs)|None|Creates shop|201, 400, 401, 409|Administrator|
|PUT|/api/Shos|[UpdateShopCommand](./src/Application/Shops/Commands/UpdateShop/UpdateShopCommand.cs)|None|Updates the shop|204, 400, 404, 401, 409| Administrator|
|DELETE|/api/Shops|[DeleteShopCommand](./src/Application/Shops/Commands/DeleteShop/DeleteShopCommand.cs)|None|Deletes the shop|204, 400, 404, 401|Administrator|

### Shop opinions

|Method|Path|Body|Params|Description|Responses|Who can access|
|:----|:----|:----|:----|:----|:----|:----|
|GET|/api/ShopOpinions|None|[ShopOpinionsQueryParameters](./src/Application/ShopOpinions/Queries/ShopOpinionsQueryParameters.cs)|Gets all shop opinions|IEnumerable<[ShopOpinionDto](./src/Application/ShopOpinions/Queries/ShopOpinionDto.cs)>|Everyone|
|GET|/api/ShopOpinions /{id}|None|Id:GUID|Gets shop opinion by id|ShopOpinionDto, 404|Everyone|
|POST|/api/ShopOpinions|[CreateShopOpinionCommand](./src/Application/ShopOpinions/Commands/CreateShopOpinion/CreateShopOpinionCommand.cs)|None|Creates shop opinion|201, 400, 401, 409|User, Administrator|
|PUT|/api/ShopOpinions|[UpdateShopOpinionCommand](./src/Application/ShopOpinions/Commands/UpdateShopOpinion/UpdateShopOpinionCommand.cs)|None|Updates the shop opinion|204, 400, 404, 401, 409| User, Administrator|
|DELETE|/api/ShopOpinions|[DeleteShopOpinionCommand](./src/Application/ShopOpinions/Commands/DeleteShopOpinion/DeleteShopOpinionCommand.cs)|None|Deletes the shop opinion|204, 400, 404, 401|User, Administrator|

### Yerba Mates

|Method|Path|Body|Params|Description|Responses|Who can access|
|:----|:----|:----|:----|:----|:----|:----|
|GET|/api/YerbaMates|None|[YerbaMatesQueryParameters](./src/Application/YerbaMates/Queries/YerbaMatesQueryParameters.cs)|Gets all yerba mates|IEnumerable<[YerbaMateDto](./src/Application/YerbaMates/Queries/YerbaMateDto.cs)>|Everyone|
|GET|/api/YerbaMates /{id}|None|Id:GUID|Gets yerba mate by id|[YerbaMateDto](./src/Application/YerbaMates/Queries/YerbaMateDto.cs), 404|Everyone|
|POST|/api/YerbaMates|[CreateYerbaMateCommand](./src/Application/YerbaMates/Commands/CreateYerbaMate/CreateYerbaMateCommand.cs)|None|Creates yerba mate|201, 400, 401, 409|Administrator|
|PUT|/api/YerbaMates|[UpdateYerbaMateCommand](./src/Application/YerbaMates/Commands/UpdateYerbaMate/UpdateYerbaMateCommand.cs)|None|Updates the yerba mate|204, 400, 404, 401, 409| Administrator|
|DELETE|/api/YerbaMates|[DeleteYerbaMateCommand](./src/Application/YerbaMates/Commands/DeleteYerbaMate/DeleteYerbaMateCommand.cs)|None|Deletes the yerba mate|204, 400, 404, 401|Administrator|
|POST|/api/YerbaMates/createImage|[CreateYerbaMateImageCommand](./src/Application/YerbaMateImages/Commands/CreateYerbaMateImage/CreateYerbaMateImageCommand.cs)|None|Creates yerba mate image|201, 400, 401|Administrator|
|DELETE|/api/YerbaMates/deleteImage|[DeleteYerbaMateImageCommand](./src/Application/YerbaMateImages/Commands/DeleteYerbaMateImage/DeleteYerbaMateImageCommand.cs)|None|Deletes the yerba mate image|204, 400, 404, 401|Administrator|

### Yerba Mates opinions

|Method|Path|Body|Params|Description|Responses|Who can access|
|:----|:----|:----|:----|:----|:----|:----|
|GET|/api/YerbaMateOpinions|None|[YerbaMateOpinionsQueryParameters](./src/Application/YerbaMateOpinions/Queries/YerbaMateOpinionsQueryParameters.cs)|Gets all yerba mate opinions|IEnumerable<[YerbaMateOpinionDto](./src/Application/YerbaMateOpinions/Queries/YerbaMateOpinionDto.cs)>|Everyone|
|GET|/api/YerbaMateOpinions /{id}|None|Id:GUID|Gets yerba mate opinion by id|[YerbaMateOpinionDto](./src/Application/YerbaMateOpinions/Queries/YerbaMateOpinionDto.cs), 404|Everyone|
|POST|/api/YerbaMateOpinions|[CreateYerbaMateOpinionCommand](./src/Application/YerbaMateOpinions/Commands/CreateYerbaMateOpinion/CreateYerbaMateOpinionCommand.cs)|None|Creates yerba mate opinion|201, 400, 401, 409|User, Administrator|
|PUT|/api/YerbaMateOpinions|[UpdateYerbaMateOpinionCommand](./src/Application/YerbaMateOpinions/Commands/UpdateYerbaMateOpinion/UpdateYerbaMateOpinionCommand.cs)|None|Updates the yerba mate opinion|204, 400, 404, 401, 409| User, Administrator|
|DELETE|/api/YerbaMateOpinions|[DeleteYerbaMateOpinionCommand](./src/Application/YerbaMateOpinions/Commands/DeleteYerbaMateOpinion/DeleteYerbaMateOpinionCommand.cs)|None|Deletes the yerba mate opinion|204, 400, 404, 401|User, Administrator|

## Tests
This project use integration tests provided by xUnit.  
To run tests go to the root folder directory and do:  
`dotnet test`

## License
[MIT](https://choosealicense.com/licenses/mit/)
