﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using Azure.Core.Pipeline;

namespace Azure.Core
{
    internal class DefaultClientOptions: ClientOptions
    {
        public DefaultClientOptions(): base(null)
        {
            Transport = GetDefaultTransport();
            Diagnostics.IsTelemetryEnabled = !EnvironmentVariableToBool(Environment.GetEnvironmentVariable("AZURE_TELEMETRY_DISABLED")) ?? true;
            Diagnostics.IsDistributedTracingEnabled = !EnvironmentVariableToBool(Environment.GetEnvironmentVariable("AZURE_TRACING_DISABLED")) ?? true;
        }

        private static bool? EnvironmentVariableToBool(string? value)
        {
            if (string.Equals(bool.TrueString, value, StringComparison.OrdinalIgnoreCase) ||
                string.Equals("1", value, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            if (string.Equals(bool.FalseString, value, StringComparison.OrdinalIgnoreCase) ||
                string.Equals("0", value, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            return null;
        }

        private static HttpPipelineTransport GetDefaultTransport()
        {
#if NETFRAMEWORK
            if (!AppContextSwitchHelper.GetConfigValue(
                "Azure.Core.Pipeline.DisableHttpWebRequestTransport",
                "AZURE_CORE_DISABLE_HTTPWEBREQUESTTRANSPORT"))
            {
                return HttpWebRequestTransport.Shared;
            }
#endif
            return HttpClientTransport.Shared;
        }
    }
}