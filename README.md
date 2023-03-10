# PassionProject_N01540458

This is my Passion project application (CMS), where I have created -
- database for all three entities - Food items, recipes and refrigerators
- CRUD operations for all three entities - Food items, recipes and refrigerators
- relationships between all three entities - Food items, recipes and refrigerators
- Food items, food categories, recipes and refrigerators' links are given in header of application, just simply navigate to your localhost (example - for my system it's - https://localhost:44357/), and then start exploring these entities of this project.

Note: I have made a one-to-many relationship between food category and food items. Many-to-many relationships is made between - food items X recipes and food items X refrigerators.

# Running this project

NOTE: Following points are valid for all entities of this project. Below is one of the entities - food item.

- Make sure to utilize jsondata/fooditem.json to formulate data you wish to send as part of the POST requests. {id} should be replaced with the fooditem's primary key ID. The port number may not always be the same.

- Get a List of Food items curl https://localhost:44324/api/FoodItemData/ListFoodItems

- Get a Single food item curl https://localhost:44324/api/FoodItemData/FindFoodItem/{id}

- Add a new food item (new food item info is in fooditem.json) curl -H "Content-Type:application/json" -d @fooditem.json https://localhost:44324/api/FoodItemData/AddFoodItem

- Delete a food item curl -d "" https://localhost:44324/api/FoodItemData/DeleteFoodItem/{id}

- Update a food item (existing food item info including id must be included in fooditem.json) curl -H "Content-Type:application/json" -d @fooditem.json https://localhost:44324/api/FoodItemData/UpdateFoodItem/{id}
