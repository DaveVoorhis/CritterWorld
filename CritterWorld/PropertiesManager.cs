using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CritterWorld
{
    public class PropertiesManager
    {
        public const string propertiesFileName = "CritterworldProperties.xml";

        private static PropertiesRecord propertiesRecord = null;

        public static PropertiesRecord Properties
        {
            get
            {
                if (propertiesRecord == null)
                {
                    Load();
                }
                return propertiesRecord;
            }
        }

        public static void RestoreDefaults()
        {
            Properties.RestoreDefaults();
        }

        public static bool Load()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(PropertiesRecord));
            serializer.UnknownNode += (obj, eventArgs) => Critterworld.Log(new LogEntry("Unknown node " + eventArgs.Name + " in " + propertiesFileName));
            serializer.UnknownAttribute += (obj, eventArgs) => Critterworld.Log(new LogEntry("Unknown attribute " + eventArgs.Attr + " in " + propertiesFileName));
            try
            {
                using (FileStream fs = new FileStream(propertiesFileName, FileMode.Open))
                {
                    propertiesRecord = (PropertiesRecord)serializer.Deserialize(fs);
                    Critterworld.Log(new LogEntry("Properties file " + propertiesFileName + " loaded."));
                    return true;
                }
            }
            catch (Exception e)
            {
                if (e is FileNotFoundException)
                {
                    Critterworld.Log(new LogEntry("Properties file " + propertiesFileName + " not found. Will be created."));
                }
                else
                {
                    Critterworld.Log(new LogEntry("Exception opening " + propertiesFileName + ": " + e.ToString()));
                }
                propertiesRecord = new PropertiesRecord();
                RestoreDefaults();
                return false;
            }
        }

        public static void Save()
        {
            if (propertiesRecord == null)
            {
                propertiesRecord = new PropertiesRecord();
                RestoreDefaults();
            }
            XmlSerializer serializer = new XmlSerializer(typeof(PropertiesRecord));
            try
            {
                using (FileStream fs = new FileStream(propertiesFileName, FileMode.Create))
                {
                    serializer.Serialize(fs, propertiesRecord);
                    Critterworld.Log(new LogEntry("Properties file " + propertiesFileName + " written."));
                }
            }
            catch (Exception e)
            {
                Critterworld.Log(new LogEntry("Exception writing " + propertiesFileName + ": " + e.ToString()));
                propertiesRecord = new PropertiesRecord();
                RestoreDefaults();
            }
        }

        static PropertiesManager()
        {
            if (!Load())
                Save();
        }
    }
}
