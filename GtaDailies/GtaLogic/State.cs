using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace GtaLogic
{
    public sealed class State
    {
        internal StateData Data;

        public static async Task<State> Load()
        {
            return new State { Data = await LoadData() };
        }

        private static async Task<StateData> LoadData()
        {
            var storage = ApplicationData.Current.RoamingFolder;
            try
            {
                var file = await storage.GetFileAsync("State.json");
                using (var stream = await file.OpenStreamForReadAsync())
                using (var reader = new StreamReader(stream))
                {
                    var fileData = await reader.ReadToEndAsync();
                    return JsonConvert.DeserializeObject<StateData>(fileData);
                }
            }
            catch (Exception)
            {
                return new StateData();
            }
        }

        public async Task Save()
        {
            var storage = ApplicationData.Current.RoamingFolder;
            var file = await storage.CreateFileAsync("State.json", CreationCollisionOption.ReplaceExisting);
            using (var stream = await file.OpenStreamForWriteAsync())
            using (var writer = new StreamWriter(stream))
            {
                var data = JsonConvert.SerializeObject(Data);
                await writer.WriteAsync(data);
                await writer.FlushAsync();
            }
        }

        private State()
        {
        }
    }
}
