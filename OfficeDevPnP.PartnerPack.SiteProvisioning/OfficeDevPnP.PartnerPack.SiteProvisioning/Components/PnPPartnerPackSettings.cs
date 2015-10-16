﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;

namespace OfficeDevPnP.PartnerPack.SiteProvisioning.Components
{
    public static class PnPPartnerPackSettings
    {
        private static String _clientId = ConfigurationManager.AppSettings["ida:ClientId"];
        private static String _clientSecret = ConfigurationManager.AppSettings["ida:ClientSecret"];
        private static String _aadInstance = ConfigurationManager.AppSettings["ida:AADInstance"];
        private static String _infrastructureSiteUrl = ConfigurationManager.AppSettings["pnp:InfrastructureSiteUrl"];
        private static String _overridesScriptUrl = ConfigurationManager.AppSettings["pnp:OverridesScriptUrl"];
        private static X509Certificate2 _appOnlyCertificate = null;
        private static Object _appOnlyCertificateSyncLock = new Object();

        /// <summary>
        /// Provides the Azure AD Client ID
        /// </summary>
        public static String ClientId
        {
            get {
                return (_clientId);
            }
        }

        /// <summary>
        /// Provides the Azure AD Client Secret
        /// </summary>
        public static String ClientSecret
        {
            get
            {
                return (_clientSecret);
            }
        }

        /// <summary>
        /// Provides the Azure AD Instance URL
        /// </summary>
        public static String AADInstance
        {
            get
            {
                return (_aadInstance);
            }
        }

        /// <summary>
        /// Provides the URL of the PnP Partner Pack Infrastructural Site
        /// </summary>
        public static String InfrastructureSiteUrl
        {
            get
            {
                return (_infrastructureSiteUrl);
            }
        }

        /// <summary>
        /// Provides the URL of the PnP Partner Pack Overrides Script URL
        /// </summary>
        public static String OverridesScriptUrl
        {
            get
            {
                return (_overridesScriptUrl);
            }
        }

        /// <summary>
        /// Provides the X.509 certificate for Azure AD AppOnly Authentication
        /// </summary>
        public static X509Certificate2 AppOnlyCertificate
        {
            get
            {
                if (_appOnlyCertificate == null)
                {
                    lock(_appOnlyCertificateSyncLock)
                    {
                        if (_appOnlyCertificate == null)
                        {
                            X509Store certStore = new X509Store(StoreName.My, StoreLocation.CurrentUser);
                            certStore.Open(OpenFlags.ReadOnly);

                            X509Certificate2Collection certCollection = certStore.Certificates.Find(
                                X509FindType.FindByThumbprint,
                                ConfigurationManager.AppSettings["pnp:AppOnlyCertificateThumbprint"],
                                false);

                            // Get the first cert with the thumbprint
                            if (certCollection.Count > 0)
                            {
                                _appOnlyCertificate = certCollection[0];
                            }
                            certStore.Close();
                        }
                    }
                }
                return (_appOnlyCertificate);
            }
        }
    }
}