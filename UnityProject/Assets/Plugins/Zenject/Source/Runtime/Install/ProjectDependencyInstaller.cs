// using GameTemplate;
// using GameTemplate.AppServices;
// using GameTemplate.AppStates;
// using GameTemplate.AppStates.GameStates;
using ModestTree;
// using PlotTwist.UniApp.Services;
using UnityEngine;
using Zenject;
using Zenject.ReflectionBaking.Mono.Cecil;


public class ProjectDependencyInstaller<T> : ScriptableObjectInstaller<T> where T : ScriptableObjectInstaller<T>
{
    // public AppSettings     Settings;
    // public AppDependencies Dependencies;
    [SerializeField] private GlobalAssetBinding[] globalAssetDependencies;
    [Inject]                 DiContainer          _container = null;


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
                foreach (var cmp in assetBinding.Components)
                {
                _container.Inject(cmp);
                }
                
                var a = 1;
                // _container.InjectGameObject(instance); 
            }
        }

        //GetAllGlobal Assets Repository/From Resources ?
        //Check if given Type Exists if not Bind/Inject/Spawn
    }

}