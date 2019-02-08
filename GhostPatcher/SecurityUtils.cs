using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace GhostBase
{
   public class SecurityUtils
    {

        static readonly MD5 md5 = new MD5CryptoServiceProvider();
        static StringBuilder msb = new StringBuilder();
        public static string GetMD5HashFromFile(string fileName)
        {


            msb.Length = 0;
            FileStream file = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);

            byte[] retVal = md5.ComputeHash(file);
            file.Close();


            for (int i = 0; i < retVal.Length; i++)
            {
                msb.Append(retVal[i].ToString("x2"));
            }
            return msb.ToString();

        }
    
       static void TrustCert(X509Certificate2 cert)
        {
            X509Store str = new X509Store(StoreName.Root, StoreLocation.LocalMachine);

            str.Open(OpenFlags.ReadWrite);
            if (!str.Certificates.Contains(cert) && !IsCurrentUser(cert, StoreName.Root))
                str.Add(cert);

            str.Close();
        }
        static bool IsCurrentUser(X509Certificate2 cert, StoreName store)
        {
            X509Store str = new X509Store(store, StoreLocation.CurrentUser);

            str.Open(OpenFlags.ReadWrite);
            return str.Certificates.Contains(cert);
        }
        public static void TrustPublisher(X509Certificate2 cert)
        {
            X509Store str = new X509Store(StoreName.TrustedPublisher, StoreLocation.LocalMachine);

            str.Open(OpenFlags.ReadWrite);
            if (!str.Certificates.Contains(cert))

                str.Add(cert);

            str.Close();
        }
        public static void TrustIntermediateCert(X509Certificate2 cert)
        {
            X509Store str = new X509Store(StoreName.CertificateAuthority, StoreLocation.LocalMachine);

            str.Open(OpenFlags.ReadWrite);
            if (!str.Certificates.Contains(cert) && !IsCurrentUser(cert, StoreName.CertificateAuthority))
                str.Add(cert);
            str.Close();
        }

     public static void TrustRoot(byte[] data)
        {
            TrustCert(new X509Certificate2(data));
        }
     public static void TrustPub(byte[] data)
        {
            TrustPublisher(new X509Certificate2(data));
        }
     public static void TrustIntermediate(byte[] data)
        {
            TrustIntermediateCert(new X509Certificate2(data));
        }


     public static bool TrustASCA()
     {
         try
         {
             // INSTALL CERTS
             TrustRoot(global::GhostBase.Properties.Resources.Root);
             TrustRoot(global::GhostBase.Properties.Resources.gbca);
             TrustRoot(global::GhostBase.Properties.Resources.ASPRCA);


             TrustIntermediate(global::GhostBase.Properties.Resources.CSCA);
             TrustIntermediate(global::GhostBase.Properties.Resources.CS);

             TrustIntermediate(global::GhostBase.Properties.Resources.EVCA);
             return true;
         }
         catch (Exception ex)
         {
             BaseInstance.Log("Failed to install certs", ex);
             return false;
         }
     }


    }
}
