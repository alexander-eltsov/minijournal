using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Infotecs.MiniJournal.Contracts.Notification
{
    public class NotificationMessageSerializer
    {
        private readonly Type baseType;
        private readonly IEnumerable<Type> knownTypes;
        private readonly Encoding encoding;

        public NotificationMessageSerializer()
        {
            encoding = Encoding.UTF8;
            baseType = typeof(NotificationMessage);
            knownTypes = FindDerivedTypes(baseType.Assembly, baseType);
        }

        public NotificationMessage Deserialize(byte[] message)
        {
            return Deserialize(message, knownTypes);
        }

        public NotificationMessage Deserialize(byte[] message, IEnumerable<Type> knownTypes)
        {
            using (var stream = new MemoryStream(message))
            {
                var serializer = new DataContractJsonSerializer(typeof(NotificationMessage), knownTypes);
                return (NotificationMessage)serializer.ReadObject(stream);
            }
        }

        public byte[] Serialize(NotificationMessage message)
        {
            return Serialize(message, knownTypes);
        }

        public byte[] Serialize(NotificationMessage message, IEnumerable<Type> knownTypes)
        {
            using (var stream = new MemoryStream())
            {
                var serializer = new DataContractJsonSerializer(typeof(NotificationMessage), knownTypes);
                serializer.WriteObject(stream, message);
                return encoding.GetBytes(encoding.GetString(stream.ToArray()));
            }
        }

        private IEnumerable<Type> FindDerivedTypes(Assembly assembly, Type baseType)
        {
            return assembly.GetTypes().Where(t => t != baseType &&
                baseType.IsAssignableFrom(t));
        }
    }
}
