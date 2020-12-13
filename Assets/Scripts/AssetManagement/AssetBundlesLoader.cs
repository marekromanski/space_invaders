using Core;
using UnityEngine;

namespace AssetManagement
{

    public class AssetBundlesLoader
    {
        public AssetBundlesLoader()
        {
            AssetBundleCreateRequest bundle = AssetBundle.LoadFromFileAsync(ProjectConsts.BundlePath);
            
        }
    }
}