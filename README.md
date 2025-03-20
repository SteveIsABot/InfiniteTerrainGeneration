# Infinite Terrain Generation

This project is a simulation that trys to generate infinite terrain.
The terrain generation based on the direction and location of the camera.
The project can be confidured to use textures or colours, change the amount of new terrain being generated, the amount of non-active terrain grids that exists when running the project.

## Perquisite To Run Project

- Unity 2022.1.3f1

## To Run In Unity

1. Fork the project to your own repository.
2. Pull project from your repository to your computer.
3. Open up the untiy hub and click ***add project from disk**.
4. Select the project folder that was pulled into your computer eariler and load project into unity.
5. Once unity has loaded, click the play button to run project.

## Configurations

To configure the project in unity please read and do the steps 1-4 in the ***To Run In Unity*** section.

### Terrain Configuration

Once in Unity find Prefabs folder in the file explorer in unity.
Once there click on the Prefab named ***TerrainGrid*** to see it in unity's inspector.

![Terrain Grid Prefab in folder!](/Screenshots/TerrainGridLocation.png)

From there you can change the colours in the colour gradient, wether you want to use textures or not, and the height scale factor of the terrain.
To adjust the textures you can change the values in ***Base Start Heights***, ***Base Blends*** ,and ***Tex Scales***.

![Terrain Grid Prefab in inspector view!](/Screenshots/TerrainGridInspectorView.png)

***Base Start Heights*** - Changes the height perecntage on where the texture starts from, i.e. 0 starts at the bottom, 0.5 starts at 50% of the terrains max height.  
***Base Blends*** - The percentage amount it blends into the next texture, i.e. 1 is a perfect graident blend while 0 is no blend between textures.  
***Tex Scales*** - The scale for each texture to appear on the terrain.  

To change textures find the Material folder and click on ***TextureMat*** to view it in unity's inspector.

![Material location!](/Screenshots/MaterialLocation.png)

Once there you can enter any new images you like, as long as they are in the project files, ideally in the Textures folder.

![TextureMat in inspector view!](/Screenshots/TerrainMaterialView.png)

Just drag and drop the new texture you would like to use for the previously existing texture you would like to change.

### Camera Configuration

Once in Unity with the project loaded in, find the Game Object named ***Main Camera*** in the Hierarchy tab and click it to view it in the inspector.

![Hierarchy View!](/Screenshots/HierarchyView.png)

From there you can change the camera variables that controls it camera speed, and movement speed.

![Camera in inspector view!](/Screenshots/CameraView.png)

***Main Speed*** - This controls the normal speed of the camera when not being boosted.  
***Speed Booster*** - This controls the increase amount of the speed until it reaches max speed limit.  
***Max Speed*** - This controls the max speed limit that the camera can reach.  
***Cam Sens*** - This controls the camera sensitvity for looking around.  

### Terrain Controller Configuration

Once in Unity with the project loaded in, find the Game Object named ***Terrain Manager*** in the Hierarchy tab and click it to view it in the inspector.

![Hierarchy View!](/Screenshots/HierarchyView.png)

Once there you can only change two varibles there which control the amount grids that can be spawned in, or stay around before getting deleted to make more space.

![Terrain Manager in inspector view!](/Screenshots/TerrainManagerView.png)

***Max Active Grids*** - The max amount of grids that can be spawned up to. (There can be more girds in view than this amount).  
***Max Non Active Grids*** - The max amount of grids that can exist when not in camera view. Once past the limit the furtherst away from camera grids are destroyed.  
