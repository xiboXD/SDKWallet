using System;
using Wallet;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace AElf.HDWallet.Core
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(WalletModule)
    )]
    public class AElfHDWalletCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var services = context.Services;
        }
    }
}