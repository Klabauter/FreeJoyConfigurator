﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;

namespace FreeJoyConfigurator
{
    public class AxesVM : BindableBase
    {

        private DeviceConfig _config;
        private AxesCurvesVM _axesCurvesVM;
        private Joystick _joystick;
        private ObservableCollection<Axis> _axes;

        

        public DeviceConfig Config
        {
            get
            {
                return _config;
            }
            set
            {
                SetProperty(ref _config, value);
            }
        }
        public AxesCurvesVM AxesCurvesVM
        {
            get
            {
                return _axesCurvesVM;
            }
            set
            {
                SetProperty(ref _axesCurvesVM, value);
            }
        }      

        public ObservableCollection<Axis> Axes
        {
            get
            {
                return _axes;
            }
            private set
            {
                SetProperty(ref _axes, value);
            }
        }

        public AxesVM(Joystick joystick, DeviceConfig deviceConfig)
        {
            _joystick = joystick;
            _config = deviceConfig;

            AxesCurvesVM = new AxesCurvesVM(_config);

            Axes = new ObservableCollection<Axis>(_joystick.Axes);

            

        }

        public void Update(DeviceConfig config)
        {
            Config = config;

            for (int i = 0; i < Config.AxisConfig.Count; i++)
            {
                Axes[i].IsEnabled = true;
                Axes[i].AxisConfig = Config.AxisConfig[i];
                for (int k = 0; k < Config.PinConfig.Count; k++)
                {
                    if (Axes[i].AllowedSources.Contains((AxisSourceType)k))
                        Axes[i].AllowedSources.Remove((AxisSourceType)k);
                }
            }

            for (int i = 0; i < Config.PinConfig.Count; i++)
            {
                if (Config.PinConfig[i] == PinType.TLE501x_CS || Config.PinConfig[i] == PinType.Axis_Analog)
                {
                    foreach (var axis in Axes)
                    {
                        if (!axis.AllowedSources.Contains((AxisSourceType)i))
                            axis.AllowedSources.Add((AxisSourceType)i);
                    }
                }
            }


            AxesCurvesVM.Update(Config);
        }

        
    }
}
