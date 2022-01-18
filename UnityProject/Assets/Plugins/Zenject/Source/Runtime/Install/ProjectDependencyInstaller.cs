// using GameTemplate;
// using GameTemplate.AppServices;
// using GameTemplate.AppStates;
// using GameTemplate.AppStates.GameStates;
using ModestTree;
// using PlotTwist.UniApp.Services;
using UnityEngine;
using Zenject;



public class ProjectDependencyInstaller<T> : ScriptableObjectInstaller<T> where T : ScriptableObjectInstaller<T>
{
    // public AppSettings     Settings;
    // public AppDependencies Dependencies;
    [SerializeField]private GlobalAssetBinding[] globalAssetDependencies;

    public override void InstallBindings()
    {
        Debug.Log("#Engine# Installing Bindings");
        InstallDependencies();
    }

    private void InstallDependencies()
    {
        // var globalAssetObjects      = GlobalAssetBinding.GetActiveObjects();

        foreach (var assetBinding in globalAssetDependencies)
        {
            var bindType   = assetBinding.BindType;
            var identifier = assetBinding.Identifier;
            var exist      = false;
            if (assetBinding.Components.Length > 0)
            {
                var componentType = assetBinding.Components[0].GetType();
                switch (bindType)
                {
                    case ZenjectBinding.BindTypes.AllInterfacesAndSelf:
                    case ZenjectBinding.BindTypes.Self:
                    {
                        exist = Container.TryResolve(componentType) != null;
                        break;
                    }
                    case ZenjectBinding.BindTypes.BaseType:
                    {
                        exist = Container.TryResolve(componentType.BaseType) != null;
                        break;
                    }
                    case ZenjectBinding.BindTypes.AllInterfaces:
                    {
                        if (componentType.GetInterfaces().Length > 0)
                            exist = Container.TryResolve(componentType.GetInterfaces()[0]) != null;
                        break;
                    }
                    default:
                    {
                        throw Assert.CreateException();
                    }
                }
            }

            if (!exist)
            {
                var instance = GameObject.Instantiate(assetBinding.gameObject, Container.DefaultParent, true);
                DontDestroyOnLoad(instance);
                ProjectContext.Instance.InstallGlobalAssetBindings(assetBinding);
            }
        }

        //GetAllGlobal Assets Repository/From Resources ?
        //Check if given Type Exists if not Bind/Inject/Spawn
    }

}