﻿using System;
using System.Net;

namespace DotNetFlicks.Common.Utilities
{
    public static class WebUtility
    {
        public static bool URLExists(string url)
        {
            bool result = true;

            WebRequest webRequest = WebRequest.Create(url);
            webRequest.Timeout = 1200; // miliseconds
            webRequest.Method = "HEAD";

            try
            {
                webRequest.GetResponse();
            }
            catch
            {
                result = false;
            }

            return result;
        }
    }
}
