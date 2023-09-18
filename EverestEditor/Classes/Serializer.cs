using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace EverestEditor.Utilities
{
    public static class Serializer
    {
        public static void ToFile<T>(T instance, string path)
        {
            try
            {
                using var newFileStream = new FileStream(path, FileMode.Create);
                var serializer = new DataContractSerializer(typeof(T));
                serializer.WriteObject(newFileStream, instance);
            }
            catch (Exception m_exception)
            {
                Debug.Write(m_exception.Message);

                // TODO - make a better log error system
            }
        }

        internal static T ReadFiles<T>(string path)
        {
            try
            {
                using var newFileStream = new FileStream(path, FileMode.Open);
                Debug.WriteLine(newFileStream);
                var serializer = new DataContractSerializer(typeof(T));
                var serializerJSON = new DataContractJsonSerializer(typeof(T)); // didnot work
                T instance = (T) serializer.ReadObject(newFileStream);
                return instance;
            }
            catch (Exception m_exception)
            {
                Debug.Write(m_exception.Message);
                return default(T);
                // TODO - make a better log error system
            }
        }
    }
}
