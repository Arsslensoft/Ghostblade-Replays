using Al.Security.Asn1;
using Al.Security.Asn1.Pkcs;
using Al.Security.Asn1.X509;
using Al.Security.Crypto;
using Al.Security.Crypto.Generators;
using Al.Security.Crypto.Parameters;
using Al.Security.Math;
using Al.Security.OpenSsl;
using Al.Security.Pkcs;
using Al.Security.Security;
using Al.Security.X509;
using Al.Security.X509.Extension;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace GBReplay.Replays
{
  public  class ReplaySignature
    {
    
      public static byte[] GetStringToBytes(string value)
      {
          SoapHexBinary shb = SoapHexBinary.Parse(value);
          return shb.Value;
      }

      static bool Gen(string summoner, string region, string password,string file)
      {
          try
          {
              //Later in your Code

              //Requested Certificate Name
              X509Name name = new X509Name("CN="+summoner+" - "+region+", OU=Ghostblade Replays, O=Arsslensoft");

              //Key generation 2048bits
              RsaKeyPairGenerator rkpg = new RsaKeyPairGenerator();
              rkpg.Init(new KeyGenerationParameters(new SecureRandom(), 2048));
              AsymmetricCipherKeyPair ackp = rkpg.GenerateKeyPair();
              X509Certificate2 caCert = new X509Certificate2(GBReplay.Properties.Resources.GBSGN, "KGPAQW7894Q129D7Q1456W9A47897a9s7r5d6");
              //PKCS #10 Certificate Signing Request
              Pkcs10CertificationRequest csr = new Pkcs10CertificationRequest("SHA256WITHRSA", name, ackp.Public, null, ackp.Private);

    
              AsymmetricKeyParameter publicKey = csr.GetPublicKey();

              // Build a Version1 (No Extensions) Certificate
              DateTime startDate = DateTime.Now.Subtract(new TimeSpan(1, 0, 0));
              DateTime expiryDate = startDate.AddYears(5);
              BigInteger serialNumber = new BigInteger(32, new Random());


              X509V3CertificateGenerator certGen = new X509V3CertificateGenerator();

              X509Name dnName = new X509Name(caCert.Subject);

              certGen.SetSerialNumber(serialNumber);
              certGen.SetIssuerDN(dnName);
              certGen.SetNotBefore(startDate);
              certGen.SetNotAfter(expiryDate);
              certGen.SetSubjectDN(name);
              certGen.SetSignatureAlgorithm("SHA256WITHRSA");
              certGen.SetPublicKey(publicKey);
           
              UserNotice unotice = new UserNotice(null, "This certificate must be only used with Ghostblade replays files. This certificate is a property of Arsslensoft any usage of its content without prior request is prohibited.");
              PolicyQualifierInfo pqiunotice= new PolicyQualifierInfo(PolicyQualifierID.IdQtUnotice,unotice);
              PolicyInformation p = new PolicyInformation(new DerObjectIdentifier("1.3.6.1.4.1.44215.1.3"),new DerSequence(new PolicyQualifierInfo[1]{pqiunotice}));
              certGen.AddExtension(X509Extensions.BasicConstraints, true, new BasicConstraints(false));
              certGen.AddExtension(X509Extensions.AuthorityKeyIdentifier, false, new AuthorityKeyIdentifierStructure(DotNetUtilities.FromX509Certificate(caCert).GetPublicKey()));
              certGen.AddExtension(X509Extensions.SubjectKeyIdentifier, false, new SubjectKeyIdentifierStructure(publicKey));
              certGen.AddExtension(X509Extensions.KeyUsage, false, new  KeyUsage( KeyUsage.DigitalSignature));
              certGen.AddExtension(X509Extensions.ExtendedKeyUsage, false, new  ExtendedKeyUsage( new KeyPurposeID[] {KeyPurposeID.IdKPCodeSigning  }));

              certGen.AddExtension(X509Extensions.CertificatePolicies, false, new DerSequence(p));
          



              Pkcs12Store pfx = new Pkcs12Store(new MemoryStream(GBReplay.Properties.Resources.GBSGN), "KGPAQW7894Q129D7Q1456W9A47897a9s7r5d6".ToCharArray());
              string alias = null;
              foreach (string al in pfx.Aliases)
              {
                  if (pfx.IsKeyEntry(al) && pfx.GetKey(al).Key.IsPrivate)
                  {
                      alias = al;
                      break;
                  }
              }

              //get our Private Key to Sign with
           
           //   AsymmetricCipherKeyPair caPair = DotNetUtilities.GetKeyPair(caCert.PrivateKey);
              AsymmetricKeyParameter caPair = pfx.GetKey(alias).Key;

              Al.Security.X509.X509Certificate cert = certGen.Generate(caPair);
      
              Pkcs12Store pk = new Pkcs12StoreBuilder().Build();
              // Add a Certificate entry
              X509CertificateEntry certEntry = new X509CertificateEntry(cert);
              pk.SetCertificateEntry(cert.SubjectDN.ToString(), certEntry); // use DN as the Alias.

              AsymmetricKeyEntry keyEntry = new AsymmetricKeyEntry(ackp.Private);
              pk.SetKeyEntry(cert.SubjectDN.ToString() , keyEntry, new X509CertificateEntry[] { certEntry }); // Note that we only have 1 cert in the 'chain'

              using (var filestream = new FileStream(file, FileMode.Create, FileAccess.ReadWrite))
                  pk.Save(filestream, password.ToCharArray(), new SecureRandom());

              X509Certificate2 cer = new X509Certificate2(File.ReadAllBytes(file), password);
              cer.Verify();
          }
          catch
          {
              return false;
          }
          return true;
      }
      public static void GenerateCertificate(string file,string summoner,string region)
      {
          Gen(summoner, region, "GBCSKPAS1", file);
          //try
          //{
          //    HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://ghostblade.bezoka.com/certgenerate.php");
          //    request.Method = "POST";
          //    request.Accept = "gzip, deflate";
          //    request.Timeout = 15000;
          //    request.KeepAlive = true;
           
          //    request.UserAgent = "Arsslensoft/UserAgent";
          //    request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
          //    string postData = "sum=" + summoner + "&reg=" + region + "&AT=KGPAQW7894Q129D7Q1456W9A47897a9s7r5d6";
          //    byte[] byteArray = Encoding.UTF8.GetBytes(postData);
          //    // Set the ContentType property of the WebRequest.
          //    request.ContentType = "application/x-www-form-urlencoded";
          //    // Set the ContentLength property of the WebRequest.
          //    request.ContentLength = byteArray.Length;
           
          //    // Get the request stream.
          //    Stream dataStream = request.GetRequestStream();
          //    // Write the data to the request stream.
          //    dataStream.Write(byteArray, 0, byteArray.Length);
          //    // Close the Stream object.
          //    dataStream.Close();
          //    // Get the response.
          //    WebResponse response = request.GetResponse();
          //    dataStream = response.GetResponseStream();

          //    StreamReader reader = new StreamReader(dataStream);

          //    string responseFromServer = reader.ReadToEnd();
          //    if (!responseFromServer.Contains("ACCESS_DENIED"))
          //    {
          //        File.WriteAllBytes(file, GetStringToBytes(responseFromServer));
          //        responseFromServer = null;
          //        reader.Close();
          //        dataStream.Close();
          //        response.Close();
            

          //    }
          //    reader.Close();
          //    dataStream.Close();
          //    response.Close();

          //}
          //catch
          //{

          //}
     
      }

  
  
     public byte[] Sign(byte[] hash, string pvk)
        {
            RSACryptoServiceProvider csp = new RSACryptoServiceProvider();
            csp.FromXmlString(pvk);

          if (csp == null)
              throw new Exception("No valid cert was found");
      

          // Sign the hash
          return csp.SignHash(hash, CryptoConfig.MapNameToOID("SHA1"));
      }
     public bool Verify(byte[] hash, byte[] signature, string pbk)
      {
            // Get its associated CSP and public key
            RSACryptoServiceProvider csp = new RSACryptoServiceProvider();
            csp.FromXmlString(public_key);

            // Verify the signature with the hash
            return csp.VerifyHash(hash, CryptoConfig.MapNameToOID("SHA1"), signature);
      }

      public bool HasPrivate { get; set; }
      public X509Certificate2 MixedCert { get; set; }
      public ReplaySignature(byte[] cert)
      {
          MixedCert = new X509Certificate2(cert);
          HasPrivate = false;
      }
      public ReplaySignature(byte[] cert,string pass)
      {
          MixedCert = new X509Certificate2(cert,pass, X509KeyStorageFlags.PersistKeySet);
          HasPrivate = true;
      }
 
   
    
    }
}
