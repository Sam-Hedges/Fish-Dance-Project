using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace PortfolioProject
{
    public static class SavingNLoading
    {
        public static void Save(FishCollection fishCollection, Speed speed, LineExtension lineExtension, SpawnRates spawnRates, FishCarry fishCarry, MixerController mixer)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            string path = Application.persistentDataPath + "/data.savedata";
            FileStream stream = new FileStream(path, FileMode.Create);

            PlayerData data = new PlayerData(fishCollection, speed, lineExtension, spawnRates, fishCarry, mixer);

            formatter.Serialize(stream, data);
            stream.Close();
        }

        public static PlayerData Load()
        {
            string path = Application.persistentDataPath + "/data.savedata";
            BinaryFormatter formatter = new BinaryFormatter();


            if (File.Exists(path))
            {
                FileStream stream = new FileStream(path, FileMode.Open);
                PlayerData data = formatter.Deserialize(stream) as PlayerData;
                stream.Close();

                return data;
            }
            else
            {
                FishCollection.rodLength = 1;
                return null;
            }
        }

    }
}
