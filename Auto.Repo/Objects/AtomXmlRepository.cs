using AutoClutch.Core.Objects;
using AutoClutch.Repo.Interfaces;
using Linq2Rest.Implementations;
using Linq2Rest.Provider;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AutoClutch.Repo
{
    /// <summary>
    /// http://stackoverflow.com/questions/2038808/the-remote-server-returned-an-error-401-unauthorized
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class AtomXmlRepository<TEntity> : ODataRepository<TEntity>, IODataRepository<TEntity> where TEntity : class
    {
        public AtomXmlRepository()
            : base()
        {
        }

        public AtomXmlRepository(string uriString, HttpClient httpClient = null)
            : base(new Uri(uriString), Format.Pox, httpClient)
        {
        }

        public AtomXmlRepository(string uri)
            : base(new Uri(uri), Format.Pox)
        {
        }

        public override ISerializerFactory GetSerializerFactory(Format format)
        {
            return new AtomXmlSerializerFactory();
        }

        //public override async Task<TEntity> AddAsync(TEntity entity, string loggedInUserName)
        //{
        //    HttpResponseMessage response = await _currentHttpClient.PostAsJsonAsync<TEntity>(_uri, entity);

        //    if (!response.IsSuccessStatusCode)
        //    {
        //        Errors.Concat(new List<Error> { new Error
        //        {
        //            Description = response.Content.ReadAsStringAsync().Result,
        //            Property ="AddAsync()"
        //        }});
        //    }

        //    var result = response.Content.ReadAsAsync<Core.Objects.MsCrm.RootObjectReturn<TEntity>>().Result;

        //    var d = result.d;

        //    entity = d;

        //    return entity;
        //}

        public class AtomXmlSerializerFactory : ISerializerFactory
        {

            public ISerializer<T> Create<T>()
            {
                return new AtomXmlSerializer<T>();
            }

            public ISerializer<T> Create<T, TSource>()
            {
                var result = this.Create<T>();

                return result;
            }

            public class AtomXmlSerializer<T> : ISerializer<T>
            {
                public T Deserialize(Stream input)
                {
                    var xmlSerializer = new XmlSerializer(typeof(T));

                    var result = (T)xmlSerializer.Deserialize(input);

                    return result;
                }

                public IEnumerable<T> DeserializeList(Stream input)
                {
                    // To Implement this properly change the type to the root container object or your data list.
                    var xmlSerializer = new XmlSerializer(typeof(Feed<T>));

                    var result = (IEnumerable<T>)((Feed<T>)xmlSerializer.Deserialize(input)).Entry.Select(i => i.Content.Properties);

                    return result;
                }

                public Stream Serialize(T item)
                {
                    var xmlSerializer = new XmlSerializer(typeof(IList<T>));

                    var stream = new MemoryStream();

                    xmlSerializer.Serialize(stream, item);
                    stream.Flush();
                    stream.Position = 0;

                    return stream;
                }
            }
        }

    }

    [XmlRoot(ElementName = "feed", Namespace = "http://www.w3.org/2005/Atom")]
    public class Feed<T>
    {
        //[XmlElement(ElementName = "title", Namespace = "http://www.w3.org/2005/Atom")]
        //public Title Title { get; set; }
        [XmlElement(ElementName = "id", Namespace = "http://www.w3.org/2005/Atom")]
        public string Id { get; set; }
        [XmlElement(ElementName = "updated", Namespace = "http://www.w3.org/2005/Atom")]
        public string Updated { get; set; }
        //[XmlElement(ElementName = "link", Namespace = "http://www.w3.org/2005/Atom")]
        //public List<Link> Link { get; set; }
        [XmlElement(ElementName = "entry", Namespace = "http://www.w3.org/2005/Atom")]
        public List<Entry<T>> Entry { get; set; }
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
        [XmlAttribute(AttributeName = "base", Namespace = "http://www.w3.org/XML/1998/namespace")]
        public string Base { get; set; }
        [XmlAttribute(AttributeName = "d", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string D { get; set; }
        [XmlAttribute(AttributeName = "m", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string M { get; set; }
    }

    [XmlRoot(ElementName = "entry", Namespace = "http://www.w3.org/2005/Atom")]
    public class Entry<T>
    {
        [XmlElement(ElementName = "id", Namespace = "http://www.w3.org/2005/Atom")]
        public string Id { get; set; }
        //[XmlElement(ElementName = "title", Namespace = "http://www.w3.org/2005/Atom")]
        //public Title Title { get; set; }
        [XmlElement(ElementName = "updated", Namespace = "http://www.w3.org/2005/Atom")]
        public string Updated { get; set; }
        //[XmlElement(ElementName = "author", Namespace = "http://www.w3.org/2005/Atom")]
        //public Author Author { get; set; }
        //[XmlElement(ElementName = "category", Namespace = "http://www.w3.org/2005/Atom")]
        //public Category Category { get; set; }
        [XmlElement(ElementName = "content", Namespace = "http://www.w3.org/2005/Atom")]
        public Content<T> Content { get; set; }
    }

    [XmlRoot(ElementName = "content", Namespace = "http://www.w3.org/2005/Atom")]
    public class Content<T>
    {
        [XmlElement(ElementName = "properties", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata")]
        public T Properties { get; set; }
        [XmlAttribute(AttributeName = "type")]
        public string Type { get; set; }
    }


}


