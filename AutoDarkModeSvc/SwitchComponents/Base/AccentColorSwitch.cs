﻿using AutoDarkModeConfig;
using AutoDarkModeConfig.ComponentSettings.Base;
using AutoDarkModeSvc.Events;
using AutoDarkModeSvc.Handlers;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoDarkModeSvc.SwitchComponents.Base
{
    class AccentColorSwitch : BaseComponent<SystemSwitchSettings>
    {
        public override bool ThemeHandlerCompatibility => true;

        private bool currentDWMColorActive;

        public AccentColorSwitch() : base()
        {
            try
            {
                currentDWMColorActive = RegistryHandler.IsDWMPrevalence();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "couldn't retrieve DWM prevalence state: ");
            }
        }

        public override bool ComponentNeedsUpdate(Theme newTheme)
        {
            if (newTheme == Theme.Dark)
            {
                if (Settings.Component.DWMPrevalenceEnableTheme == Theme.Dark && !currentDWMColorActive)
                {
                    return true;
                }
                else if (Settings.Component.DWMPrevalenceEnableTheme == Theme.Light && currentDWMColorActive)
                {
                    return true;
                }
            }
            else if (newTheme == Theme.Light)
            {
                if (Settings.Component.DWMPrevalenceEnableTheme == Theme.Light && !currentDWMColorActive)
                {
                    return true;
                }
                else if (Settings.Component.DWMPrevalenceEnableTheme == Theme.Dark && currentDWMColorActive)
                {
                    return true;
                }
            }
            return false;
        }

        protected override void HandleSwitch(Theme newTheme, SwitchEventArgs e)
        {
            try
            {
                bool previousSetting = currentDWMColorActive;
                if (newTheme == Theme.Dark && Settings.Component.DWMPrevalenceEnableTheme == Theme.Dark)
                {
                    RegistryHandler.SetDWMPrevalence(1);
                    currentDWMColorActive = true;
                }
                else if (newTheme == Theme.Light && Settings.Component.DWMPrevalenceEnableTheme == Theme.Light)
                {
                    RegistryHandler.SetDWMPrevalence(1);
                    currentDWMColorActive = true;
                }
                else
                {
                    RegistryHandler.SetDWMPrevalence(0);
                    currentDWMColorActive = false;
                }
                Logger.Info($"update info - previous: dwm prevalence {previousSetting}, now: {currentDWMColorActive}, mode: during {Enum.GetName(typeof(Theme), Settings.Component.DWMPrevalenceEnableTheme)}");
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "could not toggle DWM prevalence: ");
            }
        }
    }
}
