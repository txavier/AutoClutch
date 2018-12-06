using Linq2Rest;
using Linq2Rest.Implementations;
using Linq2Rest.Provider;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using AutoClutch.Core.Objects;
using System.Xml;
using System.Xml.Serialization;
using AutoClutch.Repo.Interfaces;

namespace AutoClutch.Repo
{
    /// <summary>
    /// http://json2csharp.com/
    /// </summary>
    public class RootObject<TEntity>
    {
        public List<TEntity> value { get; set; }
    }

    public enum Format
    {
        Pox = 0,
        Json
    }

    /// <summary>
    /// http://docs.oasis-open.org/odata/odata/v4.0/os/part2-url-conventions/odata-v4.0-os-part2-url-conventions.html#_Toc372793820
    /// http://blog.petegoo.com/2012/03/11/creating-a-net-queryable-client-for-asp-net-web-api-odata-services/
    /// http://json2csharp.com/#
    /// http://stackoverflow.com/questions/10292730/httpclient-getasync-with-network-credentials
    /// http://stackoverflow.com/questions/2038808/the-remote-server-returned-an-error-401-unauthorized
    /// http://odata.github.io/odata.net/#04-01-basic-crud-operations
    /// https://docs.microsoft.com/en-us/aspnet/web-api/overview/advanced/calling-a-web-api-from-a-net-client
    /// http://www.kendar.org/?p=/dotnet/linq2rest
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class ODataRepository<TEntity> : IODataRepository<TEntity> where TEntity : class
    {
        internal RestContext<TEntity> _dbSet;

        internal HttpClient _currentHttpClient;

        public IEnumerable<Error> Errors { get; set; }

        public HttpClient currentHttpClient
        {
            get { return _currentHttpClient; }
            set
            {
                _currentHttpClient = value;

                _dbSet = new RestContext<TEntity>(GetRestClient(_uri, _format, _currentHttpClient), GetSerializerFactory(_format));
            }
        }

        private Format _format { get; set; }
        internal Uri _uri { get; set; }

        public ODataRepository()
        {
            Errors = new List<Error>();
        }

        public ODataRepository(string uriString)
            : this(new Uri(uriString))
        {
        }

        public ODataRepository(Uri uri, Format format = Format.Json, HttpClient httpClient = null)
        {
            Errors = new List<Error>();

            _currentHttpClient = httpClient;

            _format = format;

            _uri = uri;

            _dbSet = new RestContext<TEntity>(GetRestClient(_uri, format, _currentHttpClient), GetSerializerFactory(format));
            
            //SetUri(uri.AbsoluteUri, (int)format);
        }

        //public ODataRepository(string uriString, int format = 1)
        //{
        //    SetUri(uriString, format);
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uriString"></param>
        /// <param name="format">XML = 0, JSON = 1</param>
        public void SetUri(string uriString, int format = 1)
        {
            Format formatValue = (Format)format;

            _dbSet = new RestContext<TEntity>(GetRestClient(new Uri(uriString), formatValue, _currentHttpClient), GetSerializerFactory(formatValue));
        }

        /// <summary>
        /// http://stackoverflow.com/questions/13193784/linq2rest-authentication-asp-net-webapi
        /// </summary>
        internal class PinJsonRestClient : JsonRestClient, IRestClient
        {

            private readonly HttpClient currentHttpClient;

            public PinJsonRestClient(Uri uri) : base(uri)
            {
            }

            public PinJsonRestClient(Uri uri, HttpClient currentHttpClient) : base(uri)
            {
                this.currentHttpClient = currentHttpClient;
            }

            public new Stream Get(Uri uri)
            {
                var result = currentHttpClient.GetStreamAsync(uri).Result;
                return result;
            }
        }

        public static IRestClient GetRestClient(Uri uri, Format format, HttpClient _currentHttpClient = null)
        {
            switch (format)
            {
                case Format.Pox:
                    return new XmlRestClient(uri);
                case Format.Json:
                    {
                        if (_currentHttpClient == null)
                        {

                            var result = new JsonRestClient(uri);

                            return result;
                        }
                        else
                        {
                            var result = new PinJsonRestClient(uri, _currentHttpClient);

                            return result;
                        }

                    }
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Override this method to use your own JsonNetSerializerFactory and your own response
        /// object.
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        public virtual ISerializerFactory GetSerializerFactory(Format format)
        {
            switch (format)
            {
                case Format.Pox:
                    return new Linq2Rest.Implementations.XmlSerializerFactory(knownTypes);
                case Format.Json:
                    return new JsonNetSerializerFactory();

                default:
                    throw new NotImplementedException();
            }
        }

        internal static readonly IEnumerable<Type> knownTypes = new[] {
                typeof (TEntity)
            };

        public virtual IQueryable<TEntity> Queryable()
        {
            return _dbSet.Query;
        }

        public class JsonNetSerializerFactory : ISerializerFactory
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
                        // Default JSon
                        var result = serializer.Deserialize<RootObject<TEntity>>(jsonTextReader);

                        return (IEnumerable<T>)result.value;

                        // CRM
                        //var result = serializer.Deserialize<Core.Objects.MsCrm.RootObject>(jsonTextReader);

                        // return (IEnumerable<T>)result.d.results;
                    }
                }

                public Stream Serialize(T item)
                {
                    throw new NotImplementedException();
                }
            }
        }

        public virtual async Task<TEntity> AddAsync(TEntity entity, string loggedInUserName)
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

            //entity = JsonConvert.DeserializeObject<TEntity>(response.Content.ReadAsStringAsync().Result);
            entity = await response.Content.ReadAsAsync<TEntity>();

            return entity;
        }

        public virtual async Task<TEntity> UpdateAsync(string id, TEntity entity, string loggedInUserName)
        {
            HttpResponseMessage response = await _currentHttpClient.PutAsJsonAsync<TEntity>(_uri + "(guid'" + id + "')", entity);

            if (!response.IsSuccessStatusCode)
            {
                // TODO: Add new error.
                response.EnsureSuccessStatusCode();
            }

            entity = await response.Content.ReadAsAsync<TEntity>();

            return entity;
        }

        public virtual async Task<string> DeleteAsync(string id, string loggedInUserName)
        {
            HttpResponseMessage response = await _currentHttpClient.DeleteAsync(_uri + id);

            return response.StatusCode.ToString();
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // The _dbSet.Dispose() line has been commented out because
                    // we are going to let the dependency injector handle 
                    // the lifecycle of our context objects.

                    // Dispose managed state (managed objects).
                    //_dbSet.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // Override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        ~ODataRepository()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // Uncommented the following line if the finalizer is overridden above.
            GC.SuppressFinalize(this);
        }

        #endregion

        public virtual TEntity Add(TEntity entity, string loggedInUserName = null)
        {
            var result = AddAsync(entity, loggedInUserName).Result;

            return result;
        }

        public virtual TEntity Update(string id, TEntity entity, string loggedInUserName = null)
        {
            var result = UpdateAsync(id, entity, loggedInUserName).Result;

            return result;
        }

        public virtual string Delete(string id, string loggedInUserName = null)
        {
            var result = DeleteAsync(id, loggedInUserName).Result;

            return result;
        }

        public virtual TEntity Find(string id)
        {
            var result = FindAsync(id).Result;

            return result;
        }

        public virtual async Task<TEntity> FindAsync(string id)
        {
            HttpResponseMessage response = await _currentHttpClient.GetAsync(_uri + "('" + id + "')");

            var entity = await response.Content.ReadAsAsync<TEntity>();

            return entity;
        }

        public virtual TEntity Add(TEntity entity)
        {
            entity = Add(entity, null);

            return entity;
        }

        public virtual void Delete(object id, string loggedInUserName = null)
        {
            Delete(id.ToString(), loggedInUserName);

            return;
        }

        public virtual async Task<bool> DeleteAsync(object id, string loggedInUserName = null)
        {
            var result = await DeleteAsync(id.ToString(), loggedInUserName);

            return !string.IsNullOrWhiteSpace(result);
        }

        public virtual TEntity Update(object id, TEntity entity, string loggedInUserName = null)
        {
            var result = Update(id.ToString(), entity, loggedInUserName);

            return result;
        }

        public virtual async Task<TEntity> UpdateAsync(object id, TEntity entity, string loggedInUserName = null)
        {
            var result = await UpdateAsync(id.ToString(), entity, loggedInUserName);

            return result;
        }

    }
}
