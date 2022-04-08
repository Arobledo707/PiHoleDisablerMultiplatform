using System;
using System.Text;
using System.Threading.Tasks;
using PiHoleDisablerMultiplatform.Models;
using Newtonsoft.Json;
using System.IO;

namespace PiHoleDisablerMultiplatform.Services
{
    public static class PiholeDataSerializer
    {
        public static string file = "data.json";
        public async static Task<bool> SerializeData(PiHoleData piHoleData) 
        { 
            
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), file);
            string json = JsonConvert.SerializeObject(piHoleData, Formatting.Indented);
            byte[] bytes = Encoding.UTF8.GetBytes(json);
            try
            {
                FileStream stream = new FileStream(path, FileMode.Create);
                await stream.WriteAsync(bytes, 0, bytes.Length);
                stream.Close();
                return await Task.FromResult(true);
            }
            catch (Exception er) 
            {
                Console.WriteLine(er + er.Message);
                return await Task.FromResult(false);
            }
        }

        public async static Task<PiHoleData> DeserializeData()
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), file);
            if (File.Exists(path))
            {
                try
                {
                    FileStream stream = new FileStream(path, FileMode.Open);
                    StreamReader reader = new StreamReader(stream);
                    string unformattedString = await reader.ReadToEndAsync();
                    PiHoleData piHoleData = JsonConvert.DeserializeObject<PiHoleData>(unformattedString);
                    return await Task.FromResult(piHoleData);
                }
                catch (Exception er)
                {
                    File.Delete(path);
                    Console.WriteLine(er + er.Message);
                    return await Task.FromResult(new PiHoleData());
                }
            }
            else
            {
                return await Task.FromResult(new PiHoleData());
            }
        }

        public async static Task<bool> DeleteData() 
        {
            try
            {
                File.Delete(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), file));
                return await Task<bool>.FromResult(true);
            }
            catch(Exception e)
            {
                Console.Error.WriteLine(e.Message);
            }
            return await Task<bool>.FromResult(false);
        }
    }

}
