# PassionProject_N01540458

This is the first part of my Passion project application, where I have created -
- database
- CRUD operations using ViewModels and HTTP reuqests for Food items and Food categories.
- Food items and food categories links are given in header of application, just simply navigate to your localhost (example - for my system it's - https://localhost:44357/), and then start exploring Food items and Food categories of this project.

- The code structure followed for food items and food categories, wll be similarly applied to build relationships between-
1. Food items and recipes
2. Food items and refrigerators, both of them are many-many relationships.
Note: I have made model structure for recipes but the rest is yet to be finished.

# Running this project

NOTE: Following points are valid for all entities of this project. Below is one of the entities - food item.

- Make sure to utilize jsondata/fooditem.json to formulate data you wish to send as part of the POST requests. {id} should be replaced with the fooditem's primary key ID. The port number may not always be the same.

- Get a List of Food items curl https://localhost:44324/api/FoodItemData/ListFoodItems

- Get a Single food item curl https://localhost:44324/api/FoodItemData/FindFoodItem/{id}

- Add a new food item (new food item info is in fooditem.json) curl -H "Content-Type:application/json" -d @fooditem.json https://localhost:44324/api/FoodItemData/AddFoodItem

- Delete a food item curl -d "" https://localhost:44324/api/FoodItemData/DeleteFoodItem/{id}

- Update a food item (existing food item info including id must be included in fooditem.json) curl -H "Content-Type:application/json" -d @fooditem.json https://localhost:44324/api/FoodItemData/UpdateFoodItem/{id}
