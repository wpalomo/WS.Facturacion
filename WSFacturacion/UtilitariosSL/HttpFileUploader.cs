//using System;
//using System.Net;
//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Documents;
//using System.Windows.Ink;
//using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Animation;
//using System.Windows.Shapes;
//using System.IO;
//using System.Windows.Threading;
//using System.ComponentModel;
//using System.Collections.Generic;

//namespace UtilitariosSL
//{

//    // Resumen:
//    //     Indica que los llamadores de JavaScript pueden tener acceso a una propiedad,
//    //     método o evento concreto.
//    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Event, AllowMultiple = false, Inherited = true)]
//    public sealed class ScriptableMemberAttribute : Attribute
//    {
//        // Resumen:
//        //     Inicializa una nueva instancia de la clase System.Windows.Browser.ScriptableMemberAttribute.
//        public ScriptableMemberAttribute();

//        // Resumen:
//        //     Controla la generación de métodos auxiliares del complemento Silverlight
//        //     que se pueden utilizar para crear contenedores para objetos administrados.
//        //
//        // Devuelve:
//        //     true si la característica Puente HTML debe generar automáticamente los métodos
//        //     auxiliares en el ámbito del tipo que admite scripts registrado; en caso contrario,
//        //     false. El valor predeterminado es true.
//        public bool EnableCreateableTypes { get; set; }
//        //
//        // Resumen:
//        //     Obtiene o establece el nombre de una propiedad, método o evento que está
//        //     expuesto al código JavaScript. De forma predeterminada, el alias del script
//        //     es igual que el nombre de la propiedad , método o evento administrado.
//        //
//        // Devuelve:
//        //     Nombre de una propiedad, método o evento.
//        //
//        // Excepciones:
//        //   System.ArgumentException:
//        //     Se ha establecido el alias en una cadena vacía.
//        //
//        //   System.ArgumentNullException:
//        //     El alias está establecido en null.
//        public string ScriptAlias { get; set; }
//    }

//    // Resumen:
//    //     Proporciona los servicios para administrar la cola de elementos de trabajo
//    //     de un subproceso.
//    [CLSCompliant(true)]
//    public sealed class Dispatcher
//    {
//        // Resumen:
//        //     Ejecuta asincrónicamente el delegado especificado en el subproceso al que
//        //     está asociado el objeto System.Windows.Threading.Dispatcher.
//        //
//        // Parámetros:
//        //   a:
//        //     Un delegado a un método que no toma ningún argumento ni devuelve ningún valor,
//        //     que se inserta en la cola de eventos de System.Windows.Threading.Dispatcher.
//        //
//        // Devuelve:
//        //     Objeto, devuelto inmediatamente después de llamar a Overload:System.Windows.Threading.Dispatcher.BeginInvoke,
//        //     que representa la operación expuesta en la cola de System.Windows.Threading.Dispatcher.
//        public DispatcherOperation BeginInvoke(Action a);
//        //
//        // Resumen:
//        //     Ejecuta asincrónicamente el delegado especificado con la matriz de argumentos
//        //     indicada en el subproceso al que está asociado el objeto System.Windows.Threading.Dispatcher.
//        //
//        // Parámetros:
//        //   d:
//        //     Delegado de un método que toma varios argumentos y se inserta en la cola
//        //     de eventos de System.Windows.Threading.Dispatcher.
//        //
//        //   args:
//        //     Matriz de objetos cuyos valores se pasan como argumentos al método especificado.
//        //
//        // Devuelve:
//        //     Objeto, devuelto inmediatamente después de llamar a Overload:System.Windows.Threading.Dispatcher.BeginInvoke,
//        //     que representa la operación expuesta en la cola de System.Windows.Threading.Dispatcher.
//        public DispatcherOperation BeginInvoke(Delegate d, params object[] args);
//        //
//        // Resumen:
//        //     Determina si el subproceso de la llamada es el subproceso asociado a este
//        //     objeto System.Windows.Threading.Dispatcher.
//        //
//        // Devuelve:
//        //     Es true si el subproceso de la llamada es el subproceso asociado a este objeto
//        //     System.Windows.Threading.Dispatcher; de lo contrario, es false.
//        [EditorBrowsable(EditorBrowsableState.Never)]
//        public bool CheckAccess();
//    }

//    public static class Enums
//    {
//        /// <summary>
//        /// Possible States
//        /// </summary>
//        public enum FileStates
//        {
//            Pending = 0,
//            Uploading = 1,
//            Finished = 2,
//            Deleted = 3,
//            Error = 4
//        }
//    }

//    public interface IUserFile
//    {
//        string FileName { get; set; }
//        Enums.FileStates State { get; set; }
//        string StateString { get; }

//        double FileSize { get; }
//        Stream FileStream { get; set; }

//        double BytesUploaded { get; set; }
//        double BytesUploadedFinished { get; set; }

//        float Percentage { get; set; }
//        float PercentageFinished { get; set; }

//        string ErrorMessage { get; set; }

//        void Upload(string initParams, Dispatcher uiDispatcher);
//        void CancelUpload();

//        event PropertyChangedEventHandler PropertyChanged;
//    }

//    // Resumen:
//    //     Notifica a los clientes que un valor de propiedad ha cambiado.
//    public interface INotifyPropertyChanged
//    {
//        // Resumen:
//        //     Se produce cuando cambia el valor de una propiedad.
//        event PropertyChangedEventHandler PropertyChanged;
//    }

//    public class UserFile : INotifyPropertyChanged, IUserFile
//    {
//        private string _fileName;
//        private Stream _fileStream;
//        private Enums.FileStates _state = Enums.FileStates.Pending;
//        private double _bytesUploaded = 0;
//        private double _bytesUploadedFinished = 0;
//        private double _fileSize = 0;
//        private float _percentage = 0;
//        private float _percentageFinished = 0;
//        private IFileUploader _fileUploader;

//        [ScriptableMember()]
//        public string FileName
//        {
//            get { return _fileName; }
//            set
//            {
//                _fileName = value;
//                NotifyPropertyChanged("FileName");
//            }
//        }

//        public Enums.FileStates State
//        {
//            get { return _state; }
//            set
//            {
//                _state = value;


//                NotifyPropertyChanged("State");
//            }
//        }

//        [ScriptableMember()]
//        public string StateString
//        {
//            get { return _state.ToString(); }

//        }

//        public Stream FileStream
//        {
//            get { return _fileStream; }
//            set
//            {
//                _fileStream = value;

//                if (_fileStream != null)
//                    _fileSize = _fileStream.Length;


//            }
//        }

//        public double FileSize
//        {
//            get
//            {
//                return _fileSize;
//            }
//        }

//        public double BytesUploaded
//        {
//            get { return _bytesUploaded; }
//            set
//            {
//                _bytesUploaded = value;

//                NotifyPropertyChanged("BytesUploaded");

//                Percentage = (float)(value / FileSize);

//            }
//        }

//        public double BytesUploadedFinished
//        {
//            get { return _bytesUploadedFinished; }
//            set
//            {
//                _bytesUploadedFinished = value;

//                NotifyPropertyChanged("BytesUploadedFinished");

//                PercentageFinished = (float)(value / FileSize);

//            }
//        }

//        /// <summary>
//        /// From 0 to 1
//        /// </summary>
//        [ScriptableMember()]
//        public float Percentage
//        {
//            get { return _percentage; }
//            set
//            {
//                _percentage = value;
//                NotifyPropertyChanged("Percentage");


//            }
//        }

//        /// <summary>
//        /// From 0 to 1
//        /// </summary>
//        [ScriptableMember()]
//        public float PercentageFinished
//        {
//            get { return _percentageFinished; }
//            set
//            {
//                _percentageFinished = value;
//                NotifyPropertyChanged("PercentageFinished");


//            }
//        }


//        public string ErrorMessage { get; set; }


//        public void Upload(string initParams, Dispatcher uiDispatcher)
//        {
//            this.State = Enums.FileStates.Uploading;

//            _fileUploader = new HttpFileUploader(this, uiDispatcher);

//            _fileUploader.StartUpload(initParams);
//            _fileUploader.UploadFinished += new EventHandler(fileUploader_UploadFinished);

//        }

//        public void CancelUpload()
//        {
//            this.FileStream.Close();
//            this.FileStream.Dispose();
//            this.FileStream = null;

//            if (_fileUploader != null && this.State == Enums.FileStates.Uploading)
//            {
//                _fileUploader.CancelUpload();
//            }

//            _fileUploader = null;
//        }

//        private void fileUploader_UploadFinished(object sender, EventArgs e)
//        {
//            _fileUploader = null;

//            if (this.State != Enums.FileStates.Deleted
//               && this.State != Enums.FileStates.Error)
//                this.State = Enums.FileStates.Finished;
//        }

//        #region INotifyPropertyChanged Members

//        private void NotifyPropertyChanged(string prop)
//        {
//            if (PropertyChanged != null)
//            {
//                PropertyChanged(this, new PropertyChangedEventArgs(prop));
//            }
//        }

//        public event PropertyChangedEventHandler PropertyChanged;

//        #endregion
//    }

//    public interface IFileUploader
//    {
//        void StartUpload(string initParams);
//        void CancelUpload();

//        event EventHandler UploadFinished;
//    }

//    /// <summary>
//    /// Singleton configuration class
//    /// </summary>
//    public class Configuration
//    {

//        public string CustomParams { get; set; }
//        public string FileFilter { get; set; }
//        public string UploadHandlerName { get; set; }

//        public int MaxUploads { get; set; }
//        public int MaxFileSize { get; set; }

//        public long ChunkSize { get; set; }
//        public long WcfChunkSize { get; set; }

//        private int _testInt;
//        private long _testLong;

//        private const string CustomParamsParam = "CustomParams";
//        private const string MaxUploadsParam = "MaxUploads";
//        private const string MaxFileSizeKBParam = "MaxFileSizeKB";
//        private const string ChunkSizeParam = "ChunkSize";
//        private const string FileFilterParam = "FileFilter";
//        private const string UploadHandlerNameParam = "UploadHandlerName";

//        private static Configuration instance;

//        private Configuration() { }

//        public static Configuration Instance
//        {
//            get
//            {
//                if (instance == null)
//                {
//                    instance = new Configuration();
//                }
//                return instance;
//            }
//        }


//        /// <summary>
//        /// Load configuration first from initParams, then from .Config file
//        /// </summary>
//        /// <param name="initParams"></param>
//        public void Initialize(IDictionary<string, string> initParams)
//        {
//            //Defaults:
//            MaxUploads = 2;
//            ChunkSize = 1024 * 4096;
//            WcfChunkSize = 16 * 1024;
//            MaxFileSize = int.MaxValue;

//            //Load settings from Init Params (if available)
//            LoadFromInitParams(initParams);


//            //Overwrite initParams using settings from .config file
//            LoadFromConfigFile();

//        }

//        /// <summary>
//        ///  Load settings from Init Params (if available)
//        /// </summary>
//        /// <param name="initParams"></param>
//        private void LoadFromInitParams(IDictionary<string, string> initParams)
//        {
//            //Load Custom Config String
//            if (initParams.ContainsKey(CustomParamsParam) && !string.IsNullOrEmpty(initParams[CustomParamsParam]))
//                CustomParams = initParams[CustomParamsParam];

//            if (initParams.ContainsKey(MaxUploadsParam) && !string.IsNullOrEmpty(initParams[MaxUploadsParam]))
//            {
//                if (int.TryParse(initParams[MaxUploadsParam], out _testInt))
//                    MaxUploads = int.Parse(initParams[MaxUploadsParam]);
//            }

//            if (initParams.ContainsKey(MaxFileSizeKBParam) && !string.IsNullOrEmpty(initParams[MaxFileSizeKBParam]))
//            {
//                if (int.TryParse(initParams[MaxFileSizeKBParam], out _testInt))
//                    MaxFileSize = int.Parse(initParams[MaxFileSizeKBParam]) * 1024;
//            }

//            if (initParams.ContainsKey(ChunkSizeParam) && !string.IsNullOrEmpty(initParams[ChunkSizeParam]))
//            {
//                if (long.TryParse(initParams[ChunkSizeParam], out _testLong))
//                {
//                    //Minimum Chunksize is 4096 bytes
//                    if (_testLong >= 4096)
//                        ChunkSize = int.Parse(initParams[ChunkSizeParam]);
//                }
//            }

//            if (initParams.ContainsKey(FileFilterParam) && !string.IsNullOrEmpty(initParams[FileFilterParam]))
//                FileFilter = initParams[FileFilterParam];

//            if (initParams.ContainsKey(UploadHandlerNameParam) && !string.IsNullOrEmpty(initParams[UploadHandlerNameParam]))
//                UploadHandlerName = initParams[UploadHandlerNameParam];
//        }

//        /// <summary>
//        /// Load settings from .config file
//        /// </summary>
//        private void LoadFromConfigFile()
//        {
//            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings[MaxFileSizeKBParam]))
//            {
//                if (int.TryParse(ConfigurationManager.AppSettings[MaxFileSizeKBParam], out _testInt))
//                {
//                    MaxFileSize = int.Parse(ConfigurationManager.AppSettings[MaxFileSizeKBParam]) * 1024;
//                }
//            }

//            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings[MaxUploadsParam]))
//            {
//                if (int.TryParse(ConfigurationManager.AppSettings[MaxUploadsParam], out _testInt))
//                {
//                    MaxUploads = int.Parse(ConfigurationManager.AppSettings[MaxUploadsParam]);
//                }
//            }

//            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings[FileFilterParam]))
//                FileFilter = ConfigurationManager.AppSettings[FileFilterParam];
//        }

//    }

//    //public class CustomUri : Uri
//    //{
//    //    public CustomUri(string uri);

//    //    public static string GetAbsoluteUrl(string strRelativePath);
//    //}

//    public class HttpFileUploader : IFileUploader
//    {
//        private UserFile _file;
//        private long _dataLength;
//        private long _dataSent;
//        private string _initParams;
//        private Dispatcher _uiDispatcher { get; set; }
//        private string UploadUrl;
//        private bool _isCanceled = false;

//        /// <summary>
//        /// Constructor
//        /// </summary>
//        /// <param name="file"></param>
//        /// <param name="httpHandlerName"></param>
//        /// <param name="uiDispatcher"></param>
//        public HttpFileUploader(UserFile file, Dispatcher uiDispatcher)
//        {
//            _file = file;
//            _uiDispatcher = uiDispatcher;

//            _dataLength = _file.FileStream.Length;
//            _dataSent = 0;

//            string httpHandlerName = Configuration.Instance.UploadHandlerName;
//            if (string.IsNullOrEmpty(httpHandlerName))
//                httpHandlerName = "HttpUploadHandler.ashx";

//            UploadUrl = new CustomUri(httpHandlerName).ToString();
//        }

//        #region IFileUploader Members


//        /// <summary>
//        /// Start the file upload
//        /// </summary>
//        /// <param name="initParams"></param>
//        public void StartUpload(string initParams)
//        {
//            _initParams = initParams;

//            StartUpload();
//        }

//        /// <summary>
//        /// Cancel the file upload
//        /// </summary>
//        public void CancelUpload()
//        {
//            _isCanceled = true;
//        }

//        public event EventHandler UploadFinished;

//        #endregion

//        private void StartUpload()
//        {
//            long dataToSend = _dataLength - _dataSent;
//            bool isLastChunk = dataToSend <= Configuration.Instance.ChunkSize;
//            bool isFirstChunk = _dataSent == 0;

//            UriBuilder httpHandlerUrlBuilder = new UriBuilder(UploadUrl);
//            httpHandlerUrlBuilder.Query = string.Format("{5}file={0}&offset={1}&last={2}&first={3}&param={4}", HttpUtility.UrlEncode(_file.FileName), _dataSent, isLastChunk, isFirstChunk, _initParams, string.IsNullOrEmpty(httpHandlerUrlBuilder.Query) ? "" : httpHandlerUrlBuilder.Query.Remove(0, 1) + "&");

//            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(httpHandlerUrlBuilder.Uri);
//            webRequest.Method = "POST";
//            webRequest.BeginGetRequestStream(new AsyncCallback(WriteToStreamCallback), webRequest);


//        }

//        private void WriteToStreamCallback(IAsyncResult asynchronousResult)
//        {
//            if (_file.FileStream == null || _isCanceled)
//                return;

//            HttpWebRequest webRequest = (HttpWebRequest)asynchronousResult.AsyncState;
//            Stream requestStream = webRequest.EndGetRequestStream(asynchronousResult);

//            byte[] buffer = new Byte[4096];
//            int bytesRead = 0;
//            int tempTotal = 0;

//            //Set the start position
//            _file.FileStream.Position = _dataSent;

//            long chunkSize = Configuration.Instance.ChunkSize;

//            //Read the next chunk
//            while ((bytesRead = _file.FileStream.Read(buffer, 0, buffer.Length)) != 0
//                && tempTotal + bytesRead <= chunkSize
//                && _file.State != Enums.FileStates.Deleted
//                && _file.State != Enums.FileStates.Error
//                && !_isCanceled)
//            {
//                requestStream.Write(buffer, 0, bytesRead);
//                requestStream.Flush();

//                _dataSent += bytesRead;
//                tempTotal += bytesRead;

//                //Notify progress change of data sent
//                _uiDispatcher.BeginInvoke(delegate()
//                {
//                    OnUploadProgressChanged();
//                });
//            }

//            //Leave the fileStream OPEN
//            //fileStream.Close();

//            requestStream.Close();

//            //Get the response from the HttpHandler
//            webRequest.BeginGetResponse(new AsyncCallback(ReadHttpResponseCallback), webRequest);

//        }

//        private void ReadHttpResponseCallback(IAsyncResult asynchronousResult)
//        {
//            //Check if the response is OK
//            try
//            {
//                HttpWebRequest webRequest = (HttpWebRequest)asynchronousResult.AsyncState;
//                HttpWebResponse webResponse = (HttpWebResponse)webRequest.EndGetResponse(asynchronousResult);
//                StreamReader reader = new StreamReader(webResponse.GetResponseStream());

//                string responsestring = reader.ReadToEnd();
//                reader.Close();

//                //Notify progress change of data successfully processed
//                _uiDispatcher.BeginInvoke(delegate()
//                {
//                    OnUploadFinishedProgressChanged();
//                });


//                if (_dataSent < _dataLength)
//                {
//                    //Not finished yet, continue uploading
//                    if (_file.State != Enums.FileStates.Error
//                       && _file.State != Enums.FileStates.Deleted)
//                        StartUpload();
//                }
//                else
//                {
//                    _file.FileStream.Close();
//                    _file.FileStream.Dispose();

//                    //Finished event
//                    _uiDispatcher.BeginInvoke(delegate()
//                    {
//                        if (UploadFinished != null)
//                            UploadFinished(this, null);
//                    });
//                }

//            }
//            catch
//            {
//                _file.FileStream.Close();
//                _file.FileStream.Dispose();

//                _uiDispatcher.BeginInvoke(delegate()
//                {
//                    if (_file.State != Enums.FileStates.Deleted)
//                        _file.State = Enums.FileStates.Error;
//                });
//            }

//        }

//        private void OnUploadProgressChanged()
//        {
//            _file.BytesUploaded = _dataSent;
//        }

//        private void OnUploadFinishedProgressChanged()
//        {
//            _file.BytesUploadedFinished = _dataSent;
//        }
//    }

//}
