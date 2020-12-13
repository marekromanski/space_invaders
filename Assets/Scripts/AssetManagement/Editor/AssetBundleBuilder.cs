using UnityEditor;
using UnityEditor.AddressableAssets.Settings;

namespace AssetManagement
{
    public class AssetBundleBuilder : Editor
    {
        [MenuItem("SpaceInvaders/Build Addresables")]
        private static void BuildAssetBundles()
        {
            AddressableAssetSettings.BuildPlayerContent();
        }
    }
}
