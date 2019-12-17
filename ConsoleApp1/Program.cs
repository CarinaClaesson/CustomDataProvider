using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace GetTechportData
{

    class Program
    {
        static void Main(string[] args)
        {

            MD5 md5 = new MD5CryptoServiceProvider();
            TripleDES des = new TripleDESCryptoServiceProvider();
            var desKey = md5.ComputeHash(Encoding.UTF8.GetBytes("fjdoe85d"));
            des.Key = desKey;
            des.IV = new byte[des.BlockSize / 8];
            des.Padding = PaddingMode.PKCS7;
            des.Mode = CipherMode.ECB;


            //E

            var ct = des.CreateEncryptor();
            var input = Encoding.UTF8.GetBytes("HIP 107310 A");
            var output = ct.TransformFinalBlock(input, 0, input.Length);



            var guidvalue = new Guid(output);

            Console.WriteLine("output" + output);
            Console.WriteLine("guid:" + guidvalue.ToString());

            // bACK TO STRING
           var testing = guidvalue.ToByteArray();
            var result1 = Convert.ToBase64String(testing);
            //  var result1 =  Convert.ToBase64String(output);


            //D

            var ctd = des.CreateDecryptor();
            var input2 = Convert.FromBase64String(result1);
            var output2 = ctd.TransformFinalBlock(input2, 0, input2.Length);
            var result2 =  Encoding.UTF8.GetString(output2);

            Console.WriteLine("Decrypted:" + result2);


            /*
                        var guidvalue = new Guid(encryptedValue);

                        Console.WriteLine("guid:" + guidvalue.ToString());

                        var bytesagain = guidvalue.ToByteArray();

                        var decryptedValue = Crypto.Decrypt(bytesagain, "dosj56sj");

                        Console.WriteLine("Decrypted:" + decryptedValue);
                        */
            Console.ReadLine();
        }
    }

    /*

    // Classes generated for when quering a certain project
    public class LeadOrganization
    {
        public string name { get; set; }
        public string type { get; set; }
        public string acronym { get; set; }
        public string city { get; set; }
        public string state { get; set; }
    }

    public class File
    {
        public int id { get; set; }
        public string url { get; set; }
        public int size { get; set; }
    }

    public class LibraryItem
    {
        public int id { get; set; }
        public string title { get; set; }
        public string type { get; set; }
        public string description { get; set; }
        public object externalUrl { get; set; }
        public object publishedBy { get; set; }
        public object publishedDate { get; set; }
        public List<File> files { get; set; }
    }

    public class SupportingOrganization
    {
        public string name { get; set; }
        public string type { get; set; }
        public object acronym { get; set; }
        public string city { get; set; }
        public string state { get; set; }
    }

    public class PrimaryTa
    {
        public int id { get; set; }
        public string code { get; set; }
        public string title { get; set; }
        public object priority { get; set; }
    }

    public class AdditionalTa
    {
        public int id { get; set; }
        public string code { get; set; }
        public string title { get; set; }
        public object priority { get; set; }
    }

    public class Project2
    {
        public int id { get; set; }
        public string lastUpdated { get; set; }
        public string title { get; set; }
        public string status { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
        public string description { get; set; }
        public string benefits { get; set; }
        public string technologyMaturityStart { get; set; }
        public string technologyMaturityCurrent { get; set; }
        public string technologyMaturityEnd { get; set; }
        public string responsibleProgram { get; set; }
        public string responsibleMissionDirectorateOrOffice { get; set; }
        public LeadOrganization leadOrganization { get; set; }
        public List<string> workLocations { get; set; }
        public List<string> programDirectors { get; set; }
        public List<string> programManagers { get; set; }
        public List<string> projectManagers { get; set; }
        public List<string> principalInvestigators { get; set; }
        public List<LibraryItem> libraryItems { get; set; }
        public List<string> closeoutDocuments { get; set; }
        public List<SupportingOrganization> supportingOrganizations { get; set; }
        public List<PrimaryTa> primaryTas { get; set; }
        public List<AdditionalTa> additionalTas { get; set; }
    }

    public class RootObject2
    {
        public Project2 project { get; set; }
    }

    // Classes generated for when querying all projects 
    public class Project
    {
        public int id { get; set; }
        public string lastUpdated { get; set; }
    }

    public class Projects
    {
        public int totalCount { get; set; }
        public List<Project> projects { get; set; }
    }

    public class RootObject
    {
        public Projects projects { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {

            var webRequest = WebRequest.Create("https://api.nasa.gov/techport/api/projects?api_key=uAPHV2We3eqFJNu2Hl1tvraadSamNxWYOx8A8lvP") as HttpWebRequest;
            if (webRequest == null)
            {
                return;
            }

            webRequest.ContentType = "application/json";

            using (var s = webRequest.GetResponse().GetResponseStream())
            {
                using (var sr = new StreamReader(s))
                {
                    var techportProjectsAsJson = sr.ReadToEnd();
                    var obj = JsonConvert.DeserializeObject<RootObject>(techportProjectsAsJson);
                    var projects = obj.projects;
                    Console.WriteLine(projects.totalCount);
                    var techportProjects = projects.projects;
                    foreach (var techportProject in techportProjects)
                    {
                        // For each id - make a new API call to recieve more info
                        var webRequest2 = WebRequest.Create($"\nhttps://api.nasa.gov/techport/api/projects/{techportProject.id}?api_key=uAPHV2We3eqFJNu2Hl1tvraadSamNxWYOx8A8lvP") as HttpWebRequest;

                        if (webRequest2 != null)
                        {
                            webRequest2.ContentType = "application/json";
                            using (var s2 = webRequest2.GetResponse().GetResponseStream())
                            {
                                using (var sr2 = new StreamReader(s2))
                                {
                                    var detailsAsJson = sr2.ReadToEnd();
                                    var obj2 = JsonConvert.DeserializeObject<RootObject2>(detailsAsJson);
                                    var projDetails = obj2.project;
                                    Console.WriteLine(projDetails.startDate);
                                    Console.WriteLine(projDetails.status);
                                    Console.WriteLine(projDetails.title);
                                    Console.WriteLine(projDetails.description);
                                    Console.WriteLine(projDetails.endDate);
                                    Console.WriteLine(projDetails.benefits);
                                    Console.WriteLine(projDetails.id);
                                }
                            }
                        }
                    }
                }
            }

            Console.ReadLine();
        }
    }
    */
}
