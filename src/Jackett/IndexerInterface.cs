﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jackett
{
    public interface IndexerInterface
    {
        // Retrieved for starting setup for the indexer via web API
        Task<ConfigurationData> GetConfigurationForSetup();

        // Called when web API wants to apply setup configuration via web API, usually this is where login and storing cookie happens
        Task ApplyConfiguration(JToken jsonConfig);

        // Called to check if configuration (cookie) is correct and indexer connection works
        Task VerifyConnection();

        // Invoked when the indexer configuration has been applied and verified so the cookie needs to be saved
        event Action<JToken> OnSaveConfigurationRequested;

        // Whether this indexer has been configured, verified and saved in the past and has the settings required for functioning
        bool IsConfigured { get; }

        // Called on startup when initializing indexers from saved configuration
        void LoadFromSavedConfiguration(JToken jsonConfig);
    }
}
