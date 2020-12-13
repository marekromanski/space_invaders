using Core;
using UnityEditor;
using UnityEditor.AddressableAssets.Settings;

namespace AssetManagement
{
    public class AssetBundleBuilder : UnityEditor.Editor
    {
        [MenuItem("SpaceInvaders/Build Asset Bundles")]
        private static void BuildAssetBundles()
        {
            AddressableAssetSettings.BuildPlayerContent();

            // BuildPipeline.BuildAssetBundles(ProjectConsts.BundlePath, BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.Android);
        }
    }
}
