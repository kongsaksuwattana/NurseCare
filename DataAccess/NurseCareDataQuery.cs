using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NurseCare.Model;

namespace NurseCare.DataAccess
{
    internal class NurseCareDataQuery
    {
        NurseCareDBContext dBContext = new NurseCareDBContext();
        public NurseCareDataQuery()
        {
            // Initialize the database context if needed
            // For now, we will keep it simple and use an in-memory database
            // dBContext = new NurseCareDBContext();
        }
        public List<Model.Nurse> GetNurses()
        {
            try
            {              
                return dBContext.Nurses.ToList();
            }
            catch (Exception ex)
            {
                // Handle exceptions, e.g., log the error
                Console.WriteLine($"Error fetching nurses: {ex.Message} {ex.Source}");
                return new List<Model.Nurse>();
            }
        }
        public Nurse? GetNursebyId(string id)
        {
            try
            {
                // Fetch a nurse by ID from the database
                return dBContext.Nurses.FirstOrDefault(n => n.Id == id);
            }
            catch (Exception ex)
            {
                // Handle exceptions, e.g., log the error
                Console.WriteLine($"Error fetching nurse by ID: {ex.Message} {ex.Source}");
                return null;
            }
        }
        public bool UpdateNurse(Nurse nurse)
        {
            try
            {
                dBContext.Update(nurse);
                dBContext.SaveChanges();
                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Update error: {ex.Message}");
                return false;
            }
        }
        public List<Model.Patient> GetPatients()
        {
            try
            {
                // Fetch all patients from the database
                return dBContext.Patients.ToList();
            }
            catch (Exception ex)
            {
                // Handle exceptions, e.g., log the error
                Console.WriteLine($"Error fetching patients: {ex.Message} {ex.Source}");
                return new List<Model.Patient>();
            }
        }
        public List<Model.UpdateTeamInfo> GetUpdateTeamInfos()
        {
            try
            {
                // Fetch all update team infos from the database
                return dBContext.UpdateTeamInfos.ToList();
            }
            catch (Exception ex)
            {
                // Handle exceptions, e.g., log the error
                Console.WriteLine($"Error fetching update team infos: {ex.Message} {ex.Source}");
                return new List<Model.UpdateTeamInfo>();
            }
        }
        public UpdateTeamInfo? GetTeamInfoById(string id)
        {
            try
            {
                // Fetch a team info by ID from the database
                return dBContext.UpdateTeamInfos.Where(t => t.NurseId == id).LastOrDefault();
            }
            catch (Exception ex)
            {
                // Handle exceptions, e.g., log the error
                Console.WriteLine($"Error fetching team info by ID: {ex.Message} {ex.Source}");
                return null;
            }
        }
        
        public List<Model.ContactAddress> GetContactAddresses()
        {
            try
            {
                // Fetch all contact addresses from the database
                return dBContext.ContactAddresses.ToList();
            }
            catch (Exception ex)
            {
                // Handle exceptions, e.g., log the error
                Console.WriteLine($"Error fetching contact addresses: {ex.Message} {ex.Source}");
                return new List<Model.ContactAddress>();
            }
        }
    }
}
