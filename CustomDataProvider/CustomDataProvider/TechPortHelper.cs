using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomDataProvider
{
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
}
