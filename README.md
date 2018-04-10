# Unity ARKit Demo

## Steps to Recreate

1. Create a new Unity project.
1. Import [Unity ARKit Plugin](https://assetstore.unity.com/packages/essentials/tutorial-projects/unity-arkit-plugin-92515) (uncheck the `Examples/` dir).
1. Change build target to iOS.
1. Delete all Unity ARKit Plugin scenes from build settings.
1. Import [ARKitObjectController](https://github.com/neogeek/Unity-ARKit-Demo/blob/master/Assets/Scripts/ARKitObjectController.cs)
1. Create a new scene.
1. Create an empty parent game object and attach ARKitObjectController.
1. Create a new 3D cube, scale properly (Unity units are 1 meter) and make a child of the parent game object.
1. Add new scene to build settings.
1. Build and run.

### Optional

1. Add button to call `ARKitObjectController.InvaidatePlane`.
1. Add UI text to show the accuracy of the visible plane.
