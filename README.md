# Infinite Terrain Generation

This project is a simulation that trys to generate infinite terrain.
The terrain generation based on the direction and location of the camera.
The project can be confidured to use textures or colours, change the amount of new terrain being generated, the amount of non-active terrain grids that exists when running the project.

## Perquisite To Run Project

- Unity 2022.1.3f1

## To Run In Unity

1. Fork the project to your own repository.
2. Pull project from your repository to your computer.
3. Open up the untiy hub and add project from disk.
4. Select the project and load project into unity.
5. Once unity has loaded, click the play button to run project.

## Configurations

To configure the project in unity please read and do the steps 1-4 in the ***To Run In Unity*** section.

### Terrain Configuration

Once in Unity find Prefabs folder in the file explorer in unity.
Once there click on the Prefab named ***TerrainGrid*** to see it in unity's inspector.
From there you can change the colours in the colour gradient, wether you want to use textures or not, and the height scale factor of the terrain.
To adjust the textures you can change the values in ***Base Start Heights***, ***Base Blends*** ,and ***Tex Scales***.

***Base Start Heights*** - Changes the height perecntage on where the texture starts from, i.e. 0 starts at the bottom, 0.5 starts at 50% of the terrains max height.  
***Base Blends*** - The percentage amount it blends into the next texture, i.e. 1 is a perfect graident blend while 0 is no blend between textures.  
***Tex Scales*** - The scale for each texture to appear on the terrain.  

To change textures find the Material folder and click on ***TextureMat*** to view it in unity's inspector.
Once there you can enter any new images you like, as long as they are in the project files, ideally in the Textures folder.
Just drag and drop the new texture you would like to use for the previously existing texture you would like to change.

### Camera Configuration

### Terrain Controller Configuration
