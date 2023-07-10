using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace Wallet
{
    [DependsOn(
        typeof(AbpAutofacModule)
    )]
    public class WalletModule : AbpModule
    {
        
    }
}