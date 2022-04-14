using System;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using PiHoleDisablerMultiplatform.Models;
using PiHoleDisablerMultiplatform.StaticPi;
using Newtonsoft.Json;
using System.IO;

namespace PiHoleDisablerMultiplatform.Services
{
    public static class Serializer
    {
        public async static Task<bool> SerializeData<T>(T data, string file)
        {

            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), file);
            string json = JsonConvert.SerializeObject(data, Formatting.Indented);
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

        // I'm not happy with 2 DeserializeFunctions
        // TODO use templates
        public async static Task<object> DeserializeSettingsData()
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), Constants.kSettingsFile);
            if (File.Exists(path))
            {
                try
                {
                    FileStream stream = new FileStream(path, FileMode.Open);
                    StreamReader reader = new StreamReader(stream);
                    string unformattedString = await reader.ReadToEndAsync();
                    Settings settings = JsonConvert.DeserializeObject<Settings>(unformattedString);
                    return await Task.FromResult(settings);
                }
                catch (Exception er)
                {
                    File.Delete(path);
                    Console.WriteLine(er + er.Message);
                    return await Task.FromResult<object>(null);
                }
            }
            else
            {
                return await Task.FromResult<object>(null);
            }
        }

        public async static Task<PiHoleData> DeserializePiData()
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), Constants.kPiDataFile);
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

        public async static Task<bool> DeleteData(string file) 
        {
            try
            {
                File.Delete(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), file));
                return await Task.FromResult(true);
            }
            catch(Exception e)
            {
                Console.Error.WriteLine(e.Message);
            }
            return await Task.FromResult(false);
        }
    }

}
