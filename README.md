# Custom unity package to show Google mobile ads and legacy unity ads.

### Before installing the package follow these steps to install the package without worries!
**:point_right: Add scoped registry for Google mobile ads**
1. Open the [package manager settings](https://docs.unity3d.com/Manual/class-PackageManager.html) by selecting the Unity menu option Edit > Project Settings > Package Manager.
2. Add OpenUPM as a scoped registry to the Package Manager window:
   ```
   Name: OpenUPM
   URL: https://package.openupm.com
   Scopes: com.google
   ```

**:point_right: This package uses a custom unity remote config package for that install the below package from the package manager using git URL**
```
https://github.com/TrusharRadadiya/my-remote-config.git
```
---
### After installing the package follow these steps to ensure that your project includes all the dependencies:
1. Go to Project `Settings > Player > Android > Publishing Settings > Build` and select:
   - Custom Main Gradle Template
   - Custom Gradle Properties Template
2. In the Unity editor, select `Assets > External Dependency Manager > Android Resolver > Resolve` to have the Unity External Dependency Manager library copy the declared dependencies into the `Assets/Plugins/Android directory` of your Unity app.

---
# Documentation
## You can use this package in two ways, control the ads offline and online.
**:point_right: Online**

Before going into the unity editor, set up the UGS console to use remote config. Create a new template with the below format for each environment in the "Templates" tab of the Remote Config:
```
{
    "title": "Advertisement Configuration Template",
    "description": "Template for Advertisement Configuration",
    "$id": "#advertisementconfig-template",
    "$schema": "http://json-schema.org/draft-07/schema#",
    "type": "object",
    "properties": {
        "BannerAdConfig": {
            "type": "object",
            "properties": {
                "Show": {
                    "type": "boolean"
                },
                "Settings": {
                    "type": "array",
                    "items": {
                        "type": "object",
                        "properties": {
                            "Provider": {
                                "type": "string"
                            },
                            "Enabled": {
                                "type": "boolean"
                            },
                            "Retry": {
                                "type": "number"
                            },
                            "Continue": {
                                "type": "number"
                            }
                        }
                    }
                }
            }
        },
        "InterstitialAdConfig": {
            "type": "object",
            "properties": {
                "Show": {
                    "type": "boolean"
                },
                "Settings": {
                    "type": "array",
                    "items": {
                        "type": "object",
                        "properties": {
                            "Provider": {
                                "type": "string"
                            },
                            "Enabled": {
                                "type": "boolean"
                            },
                            "Retry": {
                                "type": "number"
                            },
                            "Continue": {
                                "type": "number"
                            }
                        }
                    }
                }
            }
        },
        "RewardedAdConfig": {
            "type": "object",
            "properties": {
                "Show": {
                    "type": "boolean"
                },
                "Settings": {
                    "type": "array",
                    "items": {
                        "type": "object",
                        "properties": {
                            "Provider": {
                                "type": "string"
                            },
                            "Enabled": {
                                "type": "boolean"
                            },
                            "Retry": {
                                "type": "number"
                            },
                            "Continue": {
                                "type": "number"
                            }
                        }
                    }
                }
            }
        }
    }
}
```
Now, in the "Config" tab create a new key with the name "AdvertisementConfig" and for the type select the JSON then for the template select the one you created just a while ago.

1. First, attach the "RemoteConfigController" script of the "MyRemoteConfig" package to an empty game object, read the [documentation](https://github.com/TrusharRadadiya/my-remote-config/blob/main/README.md) of the package to set up the remote config settings.
2. Create a new empty game object with the name "Advertisement", and attach the "DontDestroyOnLoad", "RemoteConfigController" of MyAdvertisement Package, and "AdNetworkController" scripts.
3. Assign the "OnRemoteConfigDataFetched" method of the "RemoteConfigController" of MyAdvertisement Package in the "OnDataFetched" event of the "RemoteConfigController" of MyRemoteConfig package.
4. Now, you have to create the scriptable objects for each ad network you wanna use, you can create that from the `Assets > Create > My Advertisement`.
5. Assign the created ad network objects in the "AdNetworks" property of "AdNetworkController".
6. Now, for each ad type there are separate controller scripts. Attach the scripts according to your use.
7. After attaching the scripts, assign the "OnAdNetworksInitialization" method of each ad controller script in the "OnInitializationComplete" event of the "AdNetworkController'.
8. Now, you have to create the scriptable object of each ad type and ad network-wise, follow step 4 for that.
9. Like step 5, assign the create ad scriptable objects to a list of that ad type in their respective ad controller scripts.
10. There is an "AdSettings" property in each ad controller script, in that list there are below properties for each element:
    - Ad: Assign the scriptable object of the ad type, that will be shown in the turn (represented by the element position in the list).
    - Retry Count: Numbers of retries before moving to the next ad in the list, if any retry is successful then the retry will be reset for the next ad.
    - Continue Count: Number of times the ad to show before going to the next ad.
   These properties are also presented in the remote config. in addition, there are enabled properties for each ad setting, and show properties for each ad type, which are self-explanatory.
11. Set the above properties in each controller script of the ad according to your requirements.
12. There are handler scripts for each ad type like controller, use the handler scripts to handle the operations of the ads.
13. Use the sample for more clarity.

**:point_right: Offline**

For the offline, follow each step above mentioned except, for the setup of UGS, steps 1 and 3, and don't attach the "RemoteConfigController" in step 2.
