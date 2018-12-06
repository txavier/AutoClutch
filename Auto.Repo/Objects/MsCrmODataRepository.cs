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

namespace AutoClutch.Repo
{
    /// <summary>
    /// http://stackoverflow.com/questions/2038808/the-remote-server-returned-an-error-401-unauthorized
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class MsCrmRestRepository<TEntity> : ODataRepository<TEntity>, IODataRepository<TEntity> where TEntity : class
    {
        public MsCrmRestRepository()
            : base()
        {
        }

        public MsCrmRestRepository(string uriString, HttpClient httpClient = null)
            : base(new Uri(uriString), Format.Json, httpClient)
        {
        }

        public MsCrmRestRepository(Uri uri, Format format)
            : base(uri, format)
        {
        }

        public override ISerializerFactory GetSerializerFactory(Format format)
        {
            switch (format)
            {
                case Format.Pox:
                    return new XmlSerializerFactory(knownTypes);
                case Format.Json:
                    return new MSCrmJsonNetSerializerFactory();

                default:
                    throw new NotImplementedException();
            }
        }

        public override async Task<TEntity> AddAsync(TEntity entity, string loggedInUserName)
        {
            HttpResponseMessage response = await _currentHttpClient.PostAsJsonAsync<TEntity>(_uri, entity);

            if (!response.IsSuccessStatusCode)
            {
                Errors.Concat(new List<Error> { new Error
                {
                    Description = response.Content.ReadAsStringAsync().Result,
                    Property ="AddAsync()"
                }});
            }

            var result = response.Content.ReadAsAsync<Core.Objects.MsCrm.RootObjectReturn<TEntity>>().Result;

            var d = result.d;

            entity = d;

            return entity;
        }

        public class MSCrmJsonNetSerializerFactory : ISerializerFactory
        {
            public ISerializer<T> Create<T>()
            {
                return new JsonNetSerializer<T>();
            }

            public ISerializer<T> Create<T, TSource>()
            {
                var result = this.Create<T>();

                return result;
            }

            public class JsonNetSerializer<T> : ISerializer<T>
            {
                public T Deserialize(string input)
                {
                    return JsonConvert.DeserializeObject<T>(input);
                }

                public IList<T> DeserializeList(string input)
                {
                    return JsonConvert.DeserializeObject<IList<T>>(input);
                }

                public T Deserialize(Stream input)
                {
                    var serializer = new JsonSerializer();

                    using (var sr = new StreamReader(input))
                    using (var jsonTextReader = new JsonTextReader(sr))
                    {
                        return serializer.Deserialize<T>(jsonTextReader);
                    }
                }

                public IEnumerable<T> DeserializeList(Stream input)
                {
                    var serializer = new JsonSerializer();

                    using (var sr = new StreamReader(input))
                    using (var jsonTextReader = new JsonTextReader(sr))
                    {
                        // CRM
                        var result = serializer.Deserialize<Core.Objects.MsCrm.RootObject<TEntity>>(jsonTextReader);

                        return (IEnumerable<T>)result.d.results;
                    }
                }

                public Stream Serialize(T item)
                {
                    throw new NotImplementedException();
                }
            }
        }

    }
}

namespace AutoClutch.Core.Objects.MsCrm
{
    public class Metadata
    {
        public string uri { get; set; }
        public string type { get; set; }
    }

    public class D<TEntity>
    {
        public List<TEntity> results { get; set; }

        TEntity result { get; set; }
    }

    public class RootObject<TEntity>
    {
        public D<TEntity> d { get; set; }
    }

    public class RootObjectReturn<TEntity>
    {
        public TEntity d { get; set; }
    }
}
