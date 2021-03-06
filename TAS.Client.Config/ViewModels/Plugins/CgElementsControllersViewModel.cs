﻿using System;
using System.Collections.Generic;
using System.Linq;
using TAS.Common.Interfaces;
using TAS.Common.Interfaces.Configurator;
using TAS.Database.Common.Interfaces;

namespace TAS.Client.Config.ViewModels.Plugins
{
    internal class CgElementsControllersViewModel : PluginTypeViewModelBase
    {
        private IConfigEngine _engine;
        private IPluginConfiguratorViewModel _selectedConfigurator;
                
        public CgElementsControllersViewModel(IConfigEngine engine)
        {
            _engine = engine;
            Name = "CG elements controllers";

            Configurators = ConfigurationPluginManager.Current.ConfigurationProviders
                .Where(p => typeof(ICGElementsController).IsAssignableFrom(p.GetPluginModelType()))
                .Select(p =>
                {
                    var configuratorVm = p.GetConfiguratorViewModel(engine);
                    configuratorVm.PluginChanged += PluginConfigurator_PluginChanged;
                    configuratorVm.Initialize(_engine.CGElementsController);
                    return configuratorVm;
                })
                .ToArray();
            SelectedConfigurator = Configurators.FirstOrDefault();
        }

        private void PluginConfigurator_PluginChanged(object sender, EventArgs e)
        {
            RaisePluginChanged();
        }
        
        public IPluginConfiguratorViewModel SelectedConfigurator
        {
            get => _selectedConfigurator;
            set => SetField(ref _selectedConfigurator, value);
        }
        public IPluginConfiguratorViewModel[] Configurators { get; }

        protected override void OnDispose()
        {
            foreach (var cgConfigurator in Configurators)           
                cgConfigurator.PluginChanged -= PluginConfigurator_PluginChanged;            
        }

        public void Save()
        {
            _selectedConfigurator.Save();
        }       
    }
}
