# Custom unity package to show google mobile ads and legacy unity ads in turns.

### Before installing the package follow these step so that you can install package without worries!
**:point_right: Add scoped registry for google mobile ads**
1. Open the [package manager settings](https://docs.unity3d.com/Manual/class-PackageManager.html) by selecting the Unity menu option Edit > Project Settings > Package Manager.
2. Add OpenUPM as a scoped registry to the Package Manager window:
   ```
   Name: OpenUPM
   URL: https://package.openupm.com
   Scopes: com.google
   ```
---
### After installing package follow these steps to ensure that your project includes all the dependencies:
1. Go to Project `Settings > Player > Android > Publishing Settings > Build` and select:
   - Custom Main Gradle Template
   - Custom Gradle Properties Template
2. In the Unity editor, select `Assets > External Dependency Manager > Android Resolver > Resolve` to have the Unity External Dependency Manager library copy the declared dependencies into the `Assets/Plugins/Android directory` of your Unity app.
