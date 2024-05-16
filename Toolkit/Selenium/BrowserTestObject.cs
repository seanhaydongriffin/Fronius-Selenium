using System;
using System.IO;
using System.Threading.Tasks;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using Microsoft.Win32;
using System.Drawing;
using System.Collections.Generic;
using OpenQA.Selenium.DevTools;
using OpenQA.Selenium.DevTools.V101.Network;
using System.Linq;

namespace Toolkit.Selenium
{
    public class BrowserTestObject : ChromeDriver
    {
        public static BrowserTestObject CurrentDriver = null;
        public static BrowserTestObject driver2 = null;
        public static String CurrentHubUrl = "";
        //public static DesiredCapabilities CurrentCapability = null;
        public static long NumberOfAjaxRequests = 0;
        public static IntPtr CurrentBrowserHwnd = IntPtr.Zero;
        public static int CurrentBrowserPID = -1;
        private static int accessibility_file_num = 0;
        public static int CurrentBrowserLeft = -1;
        public static int CurrentBrowserRight = -1;
        public static int CurrentBrowserTop = -1;
        public static int CurrentBrowserBottom = -1;
        public static int CurrentBrowserWidth = -1;
        public static int CurrentBrowserHeight = -1;
        public static int CurrentViewportLeft = -1;
        public static int CurrentViewportRight = -1;
        public static int CurrentViewportTop = -1;
        public static int CurrentViewportBottom = -1;
        public static int CurrentViewportWidth = -1;
        public static int CurrentViewportHeight = -1;
        public static ITakesScreenshot CurrentTakesScreenshot = null;

        private static IDevTools _devToolsDriver = null;
        private static OpenQA.Selenium.DevTools.V101.DevToolsSessionDomains _devToolsSessionDomains = null;
        private static Dictionary<string, RequestWillBeSentEventArgs> _requestWillBeSent = new Dictionary<string, RequestWillBeSentEventArgs>();
        private static string _devToolsLoggingPrefix = "";
        private static bool _devToolsLogging = false;
        private static string _devToolsLoggingFilename = "network log.har";

        // Starts a new driver session, of the specified capability, in an existing Selenium Hub

        //private void AddCustomChromeCommand(string commandName, string method, string resourcePath)
        //{
        //    //CommandInfo commandInfo = new CommandInfo(method, resourcePath);
        //    //base.CommandExecutor.CommandInfoRepository.TryAddCommand(commandName, commandInfo);
        //}

        public BrowserTestObject(ChromeOptions Options)
            : base(Options)
        {
        }

        //public BrowserTestObject(ICapabilities capabilities)
        //    : base(capabilities)
        //{
        //}

        //public BrowserTestObject(Uri SeleniumHubUrl, ICapabilities capabilities, TimeSpan Timeout)
        //    : base(SeleniumHubUrl, capabilities, Timeout)
        //{
        //}


        public ChromeDriver BrowserTestObject2(ChromeDriverService cds, ChromeOptions Options)
        {
            return new ChromeDriver(cds, Options);
        }

        //public BrowserTestObject(Uri SeleniumHubUrl, EdgeOptions Options)
        //    : base(SeleniumHubUrl, Options)
        //{
        //}

        public static void DisableDevTools()
        {
            if (_devToolsLogging)
            {
                _devToolsLogging = false;

                try
                {
                    _devToolsDriver.CloseDevToolsSession();
                    _devToolsDriver = null;
                    _devToolsSessionDomains = null;
                    _requestWillBeSent.Clear();
                    _devToolsLoggingPrefix = "";
                }
                catch (Exception e)
                {
                    Log.WriteLine(e.ToString());
                }
            }
        }

        public static void EnableDevTools(DriverOptions options)
        {
            _devToolsDriver = CurrentDriver as IDevTools;

            try
            {
                IDevToolsSession _devTools = _devToolsDriver.GetDevToolsSession();
                
                if (options.BrowserVersion != null && options.BrowserVersion.Equals("latest"))

                    _devToolsSessionDomains = _devTools.GetVersionSpecificDomains<OpenQA.Selenium.DevTools.V101.DevToolsSessionDomains>();
            }
            catch (Exception e)
            {
                Log.WriteLine(e.ToString());
            }

        }

        public static async Task EnableDevToolsLogging(DriverOptions options)
        {
            _devToolsLogging = true;
            _requestWillBeSent.Clear();
            _devToolsLoggingPrefix = "";

            try
            {
                // Selenium v4 ...

                _devToolsSessionDomains.Network.RequestWillBeSent += RequestWillBeSentHandler;
                _devToolsSessionDomains.Network.ResponseReceived += ResponseReceivedHandler;
                _devToolsSessionDomains.Log.EntryAdded += EntryAddedHandler;

                if (options.BrowserVersion.Equals("latest"))
                {
                    await _devToolsSessionDomains.Network.Enable(new OpenQA.Selenium.DevTools.V101.Network.EnableCommandSettings()
                    {
                        MaxTotalBufferSize = 100000000,
                        MaxResourceBufferSize = 100000000
                    });

                    await _devToolsSessionDomains.Log.Enable(new OpenQA.Selenium.DevTools.V101.Log.EnableCommandSettings());
                }
            }
            catch (Exception e)
            {
                Log.WriteLine(e.ToString());
            }
        }

        public static void ClearCache()
        {
            try
            {
                _devToolsSessionDomains.Network.ClearBrowserCache();
                _devToolsSessionDomains.Network.ClearBrowserCookies();
            }
            catch (Exception e)
            {
                Log.WriteLine(e.ToString());
            }
        }

        public static void EntryAddedHandler(object sender, OpenQA.Selenium.DevTools.V101.Log.EntryAddedEventArgs e)
        {
            try
            {
                //($"Response ... kind: { e.Entry.Source.ToString() } level: { e.Entry.Level } message: { e.Entry.Text } ").LogDebug();

                var epoch = new System.DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                var entry_datetime_str = epoch.AddMilliseconds(e.Entry.Timestamp).ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss");

                var message = entry_datetime_str + " [" + e.Entry.Source + "] " + (new Uri(e.Entry.Url)).Host + "/:1 " + e.Entry.Text + " " + e.Entry.Args + Environment.NewLine;
                //Toolkit.File.append(Test_Environment.local_logs_path + "\\console log.txt", message);
            }
            catch (Exception e2)
            {
                Log.WriteLine(e2.ToString());
            }

        }

        public static void RequestWillBeSentHandler(object sender, RequestWillBeSentEventArgs e)
        {

            try
            {
                if (!_requestWillBeSent.ContainsKey(e.RequestId))

                    _requestWillBeSent.Add(e.RequestId, e);
            }
            catch (Exception e2)
            {
                var json = new
                {
                    @_initiator = new
                    {
                        type = "error",
                        url = "",
                        lineNumber = 0
                    },
                    @request = new
                    {
                        method = "ERR",
                        url = "",
                        httpVersion = "http/2.0",
                        headersSize = -1,
                        bodySize = 0
                    },
                    @response = new
                    {
                        status = "422",
                        statusText = "Exception",
                        httpVersion = "http/2.0",
                        @content = new
                        {
                            size = 0,
                            mimeType = ""
                        },
                        redirectURL = "",
                        headersSize = -1,
                        bodySize = -1,
                        _transferSize = 0,
                        error = (string)null
                    },
                    serverIPAddress = "",
                    startedDateTime = System.DateTime.Now.ToISO8601WithZuluDateTimeFormat(),
                    time = 0,
                    @timings = new
                    {
                        blocked = -1,
                        dns = 0,
                        ssl = 0,
                        connect = 0,
                        send = 0,
                        wait = -1,
                        receive = -1,
                        _blocked_queueing = -1
                    }
                };

                Toolkit.File.remove_bytes_from_end(AppDomain.CurrentDomain.BaseDirectory + _devToolsLoggingFilename, 3);
                Toolkit.File.append(AppDomain.CurrentDomain.BaseDirectory + _devToolsLoggingFilename, _devToolsLoggingPrefix + json.ToJSONString() + "]}}");
                _devToolsLoggingPrefix = ", ";
            }

        }

        public static void ResponseReceivedHandler(object sender, ResponseReceivedEventArgs _responseReceived)
        {
            try
            {

                if (_requestWillBeSent.ContainsKey(_responseReceived.RequestId))
                {
                    var wall_datetime = (new System.DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).AddSeconds(_requestWillBeSent[_responseReceived.RequestId].WallTime);

                    if (_requestWillBeSent[_responseReceived.RequestId].Initiator.LineNumber == null || _requestWillBeSent[_responseReceived.RequestId].Initiator.LineNumber.ToString().Equals(""))

                        _requestWillBeSent[_responseReceived.RequestId].Initiator.LineNumber = 0;

                    var size = "0";

                    if (_responseReceived.Response.Headers.ContainsKey("Content-Length"))

                        size = _responseReceived.Response.Headers["Content-Length"];

                    //($"Response ... url: { _requestWillBeSent[_responseReceived.RequestId].Request.Url } requestTime: { (_responseReceived.Response.Timing.RequestTime / 1000) } proxyStart: { (_responseReceived.Response.Timing.ProxyStart) } proxyEnd: { (_responseReceived.Response.Timing.ProxyEnd) } dnsStart: { (_responseReceived.Response.Timing.DnsStart) } dnsEnd: { (_responseReceived.Response.Timing.DnsEnd) } connectStart: { (_responseReceived.Response.Timing.ConnectStart) } connectEnd: { (_responseReceived.Response.Timing.ConnectEnd) } sslStart: { (_responseReceived.Response.Timing.SslStart) } sslEnd: { (_responseReceived.Response.Timing.SslEnd) } workerStart: { (_responseReceived.Response.Timing.WorkerStart) } workerReady: { (_responseReceived.Response.Timing.WorkerReady) } workerFetchStart: { (_responseReceived.Response.Timing.WorkerFetchStart) } workerRespondWithSettled: { (_responseReceived.Response.Timing.WorkerRespondWithSettled) } sendStart: { (_responseReceived.Response.Timing.SendStart) } sendEnd: { (_responseReceived.Response.Timing.SendEnd) } pushStart: { (_responseReceived.Response.Timing.PushStart) } pushEnd: { (_responseReceived.Response.Timing.PushEnd) } receiveHeadersEnd: { (_responseReceived.Response.Timing.ReceiveHeadersEnd) } ").LogDebug();

                    double time = 0;

                    if (_responseReceived.Response.Timing != null)
                    {
                        if (_responseReceived.Response.Timing.ProxyEnd > -1 && _responseReceived.Response.Timing.ProxyStart > -1)

                            time = time + _responseReceived.Response.Timing.ProxyEnd - _responseReceived.Response.Timing.ProxyStart;

                        if (_responseReceived.Response.Timing.DnsEnd > -1 && _responseReceived.Response.Timing.DnsStart > -1)

                            time = time + _responseReceived.Response.Timing.DnsEnd - _responseReceived.Response.Timing.DnsStart;

                        if (_responseReceived.Response.Timing.ConnectEnd > -1 && _responseReceived.Response.Timing.ConnectStart > -1)

                            time = time + _responseReceived.Response.Timing.ConnectEnd - _responseReceived.Response.Timing.ConnectStart;

                        if (_responseReceived.Response.Timing.SslEnd > -1 && _responseReceived.Response.Timing.SslStart > -1)

                            time = time + _responseReceived.Response.Timing.SslEnd - _responseReceived.Response.Timing.SslStart;

                        if (_responseReceived.Response.Timing.SendEnd > -1 && _responseReceived.Response.Timing.SendStart > -1)

                            time = time + _responseReceived.Response.Timing.SendEnd - _responseReceived.Response.Timing.SendStart;

                        if (_responseReceived.Response.Timing.PushEnd > -1 && _responseReceived.Response.Timing.PushStart > -1)

                            time = time + _responseReceived.Response.Timing.PushEnd - _responseReceived.Response.Timing.PushStart;

                        if (_responseReceived.Response.Timing.ReceiveHeadersEnd > -1)

                            time = time + _responseReceived.Response.Timing.ReceiveHeadersEnd;
                    }

                    // debug
                    //if (_requestWillBeSent[_responseReceived.RequestId].Request.Method.Equals("POST") && _requestWillBeSent[_responseReceived.RequestId].Request.Url.Equals("https://automation.sit.insights.janison.com/settings/AntiMalwareSettings/index"))

                    //    time = 0;

                    var json = new
                    {
                        @_initiator = new
                        {
                            type = _requestWillBeSent[_responseReceived.RequestId].Initiator.Type.ToString().ToLower(),
                            url = "",
                            lineNumber = _requestWillBeSent[_responseReceived.RequestId].Initiator.LineNumber
                        },
                        @request = new
                        {
                            method = _requestWillBeSent[_responseReceived.RequestId].Request.Method,
                            url = _requestWillBeSent[_responseReceived.RequestId].Request.Url,
                            httpVersion = "http/2.0",
                            @postData = new
                            {
                                mimeType = _requestWillBeSent[_responseReceived.RequestId].Request.Headers.ContainsKey("Content-Type") == false ? "" : _requestWillBeSent[_responseReceived.RequestId].Request.Headers["Content-Type"],
                                text = _requestWillBeSent[_responseReceived.RequestId].Request.PostData == null ? "" : _requestWillBeSent[_responseReceived.RequestId].Request.PostData
                            },
                            headersSize = -1,
                            bodySize = 0
                        },
                        @response = new
                        {
                            status = _requestWillBeSent[_responseReceived.RequestId].Request.Method,
                            statusText = _requestWillBeSent[_responseReceived.RequestId].Request.Url,
                            httpVersion = "http/2.0",
                            @content = new
                            {
                                size = size,
                                mimeType = _responseReceived.Response.MimeType
                            },
                            redirectURL = "",
                            headersSize = -1,
                            bodySize = -1,
                            _transferSize = size,
                            error = (string)null
                        },
                        serverIPAddress = _responseReceived.Response.RemoteIPAddress == null ? "" : _responseReceived.Response.RemoteIPAddress,
                        startedDateTime = wall_datetime.ToISO8601WithZuluDateTimeFormat(),
                        time = time,
                        @timings = new
                        {
                            blocked = -1,
                            dns = -1, //(_responseReceived.Response.Timing.DnsEnd - _responseReceived.Response.Timing.DnsStart),
                            ssl = -1, //(_responseReceived.Response.Timing.SslEnd - _responseReceived.Response.Timing.SslStart),
                            connect = -1, //(_responseReceived.Response.Timing.ConnectEnd - _responseReceived.Response.Timing.ConnectStart),
                            send = -1, //(_responseReceived.Response.Timing.SendEnd - _responseReceived.Response.Timing.SendStart),
                            wait = -1,
                            receive = -1,
                            _blocked_queueing = -1
                        }
                    };

                    Toolkit.File.remove_bytes_from_end(AppDomain.CurrentDomain.BaseDirectory + _devToolsLoggingFilename, 3);
                    Toolkit.File.append(AppDomain.CurrentDomain.BaseDirectory + _devToolsLoggingFilename, _devToolsLoggingPrefix + json.ToJSONString() + "]}}");
                    _requestWillBeSent.Remove(_responseReceived.RequestId);
                    _devToolsLoggingPrefix = ", ";
                }


            }
            catch (Exception e2)
            {
                var json = new
                {
                    @_initiator = new
                    {
                        type = "error",
                        url = "",
                        lineNumber = 0
                    },
                    @request = new
                    {
                        method = "ERR",
                        url = "",
                        httpVersion = "http/2.0",
                        headersSize = -1,
                        bodySize = 0
                    },
                    @response = new
                    {
                        status = "422",
                        statusText = "Exception",
                        httpVersion = "http/2.0",
                        @content = new
                        {
                            size = 0,
                            mimeType = ""
                        },
                        redirectURL = "",
                        headersSize = -1,
                        bodySize = -1,
                        _transferSize = 0,
                        error = (string)null
                    },
                    serverIPAddress = "",
                    startedDateTime = System.DateTime.Now.ToISO8601WithZuluDateTimeFormat(),
                    time = 0,
                    @timings = new
                    {
                        blocked = -1,
                        dns = 0,
                        ssl = 0,
                        connect = 0,
                        send = 0,
                        wait = -1,
                        receive = -1,
                        _blocked_queueing = -1
                    }
                };

                Toolkit.File.remove_bytes_from_end(AppDomain.CurrentDomain.BaseDirectory + _devToolsLoggingFilename, 3);
                Toolkit.File.append(AppDomain.CurrentDomain.BaseDirectory + _devToolsLoggingFilename, _devToolsLoggingPrefix + json.ToJSONString() + "]}}");
                _requestWillBeSent.Remove(_responseReceived.RequestId);
                _devToolsLoggingPrefix = ", ";
            }
        }


        public static void SetCurrentDriverIfNotSet(bool MinimiseEclipse)
        {
            /*
            if (CurrentDriver == null)
            {
                String CurrentDriverSessionFilename = Toolkit.Selenium.Data.Path.read() + "\\driver_session.cur";
                String CurrentDriverCapabilityFilename = Toolkit.Selenium.Data.Path.read() + "\\driver_capability.cur";
                String CurrentSeleniumHubFilename = Toolkit.Selenium.Data.Path.read() + "\\selenium_hub.cur";

                if (Toolkit.File.exists(CurrentDriverSessionFilename) &&
                    Toolkit.File.exists(CurrentDriverCapabilityFilename) &&
                    Toolkit.File.exists(CurrentSeleniumHubFilename))
                {
                    try
                    {
                        CurrentHubUrl = Toolkit.File.read(CurrentSeleniumHubFilename);

                        //if (Toolkit.File.read(CurrentDriverCapabilityFilename).Equals("firefox"))

                        // CurrentCapability = DesiredCapabilities.Firefox();

                        String CurrentDriverSessionId = Toolkit.File.read(CurrentDriverSessionFilename);


                        //   CurrentDriver = new BrowserTestObject(new Uri(CurrentHubUrl), CurrentDriverSessionId, new TimeSpan(0, 0, 10));
                        CurrentDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);

                        Console.WriteLine("Attached to existing driver session with id " + CurrentDriverSessionId);
                    }
                    catch (Exception e)
                    {
                        e.ToString().LogDebug();
                    }
                }
            }
            */
        }


        public static IWebDriver get_current()
        {
            return CurrentDriver;
        }

        private static DriverOptions SetBrowserOptions(bool Headless, String BrowserVersion, String OS, String BrowserExecutableLocation)
        {
            // Unchecking "Automatically Detect Settings" in Internet Setting will speed up Chrome Startup ...
            Microsoft.Win32.Registry.SetValue("HKEY_CURRENT_USER\\Software\\Microsoft\\Windows\\CurrentVersion\\Internet Settings", "AutoDetect", 0, RegistryValueKind.DWord);

            ChromeOptions options = new ChromeOptions();

            options.BinaryLocation = Environment.ExpandEnvironmentVariables(@"%USERPROFILE%\.cache\selenium\chrome\win64\118.0.5993.70\chrome.exe");

            if (Headless)

                options.AddArgument("headless");

            return options;
        }


        private static void StartBrowser(DriverOptions options, string DevToolsLoggingFilename, String UrlToVisit, String BrowserExecutableLocation) //, Size BrowserSize, Point BrowserPosition)
        {
            var BrowserName = options.BrowserName;


            //ChromeDriverService service = ChromeDriverService.CreateDefaultService("C:\\Selenium", "chromedriver-2.44.exe");
            //service.LogPath = "D:\fred.log";
            //    service.EnableVerboseLogging = true;
            //var driver = new ChromeDriver(service, options);
            //        service.Start();

            // Use the following the URL below to generate a debug log from chromedriver.
            //  Just start chromedriver from an "administrator" command prompt in Windows first with this command:
            //      chromedriver.exe --verbose --log-path=d:\chromedriver.log
            //  And use the following call
            //      CurrentDriver = new BrowserTestObject(new Uri("http://localhost:9515"), options);

            //                    CurrentDriver = new BrowserTestObject(new Uri(SeleniumHubUrl), options);

            // Retry up to three times for a successful connection to chrome / webdriver
            for (int i = 1; i < 3; i++)
            {
                try
                {


                    //("Starting WebDriver with Selenium Hub Url " + SeleniumHubUrl.ToString()).LogDebug();
//                        CurrentDriver = new BrowserTestObject(SeleniumHubUrl, ((ChromeOptions)options).ToCapabilities(), TimeSpan.FromSeconds(10));
                    CurrentDriver = new BrowserTestObject((ChromeOptions)options);
                    //("Started Browser " + ((RemoteWebDriver)CurrentDriver).Capabilities.GetCapability("browserName") + ", Version " + ((RemoteWebDriver)CurrentDriver).Capabilities.GetCapability("version")).LogDebug();

                    if (CurrentDriver.Capabilities.HasCapability("message"))
                    {
                        //CurrentDriver.Capabilities.GetCapability("message").ToString().LogDebug();
                        CloseAll();
                    }
                    else

                        break;
                }
                catch (Exception e)
                {
                    Log.WriteLine(e.ToString());
                }

                //("CurrentDriver Attempt #" + (i + 1) + " of 3").LogDebug();
            }

            // Get the PID and HWND details for a chrome browser
            //Chrome.GetBrowserPIDHwnd(out CurrentBrowserPID, out CurrentBrowserHwnd);
            //Chrome.GetBrowserViewportPosition(CurrentBrowserPID, CurrentBrowserHwnd, out CurrentBrowserLeft, out CurrentBrowserTop, out CurrentBrowserRight, out CurrentBrowserBottom, out CurrentBrowserWidth, out CurrentBrowserHeight, out CurrentViewportLeft, out CurrentViewportTop, out CurrentViewportRight, out CurrentViewportBottom, out CurrentViewportWidth, out CurrentViewportHeight);


            //EnableDevTools(options);
            // Clean the profile before use, so it's not dirty and fails
            //ClearCache();
            

            Size current_window_size = Size.Empty;

            if (CurrentViewportWidth < 0 || CurrentViewportHeight < 0)

                current_window_size = get_current().Manage().Window.Size;

            if (CurrentViewportWidth < 0)

                CurrentViewportWidth = current_window_size.Width;

            if (CurrentViewportHeight < 0)

                CurrentViewportHeight = current_window_size.Height;

            CurrentTakesScreenshot = ((ITakesScreenshot)Selenium.BrowserTestObject.CurrentDriver);

            // The following line enables File Imports to work on remote computers running a Selenium Node
            //  This will "magically" send the files from the current computer to the node computer as needed
            CurrentDriver.FileDetector = new LocalFileDetector();


            //			    CurrentDriver.manage().timeouts().implicitlyWait(5, TimeUnit.SECONDS);
            //CurrentDriver.Manage().Timeouts().AsynchronousJavaScript = TimeSpan.FromSeconds(10);
            CurrentDriver.Manage().Timeouts().AsynchronousJavaScript = TimeSpan.FromSeconds(10);

            //if (ClearBrowserData)
            //{
            //    //if (BrowserName.Equals("chrome"))

            //    //    Toolkit.Chrome.ClearBrowserData();
            //}

            if (!UrlToVisit.Equals(""))

            CurrentDriver.Navigate().GoToUrl(UrlToVisit);

            //("The current URL is " + CurrentDriver.Url).LogDebug();

            //  	    System.out.println("Starting current driver session with id " + CurrentDriver.getSessionId());
            //		    Toolkit.File.overwrite(Toolkit.Selenium.Data.Path.read() + "\\driver_session.cur", CurrentDriver.getSessionId().toString());

            bool browser_rect_changed = false;

            //if (BrowserSize != Size.Empty && (BrowserSize.Width != CurrentBrowserWidth || BrowserSize.Height != CurrentBrowserHeight))
            //{
            //    CurrentDriver.Manage().Window.Size = BrowserSize;
            //    browser_rect_changed = true;
            //}

            //if (BrowserPosition != Point.Empty && (BrowserPosition.X != CurrentBrowserLeft || BrowserPosition.Y != CurrentBrowserTop))
            //{
            //    CurrentDriver.Manage().Window.Position = BrowserPosition;
            //    browser_rect_changed = true;
            //}

            if (browser_rect_changed)
            {
                Chrome.GetBrowserViewportPosition(CurrentBrowserPID, CurrentBrowserHwnd, out CurrentBrowserLeft, out CurrentBrowserTop, out CurrentBrowserRight, out CurrentBrowserBottom, out CurrentBrowserWidth, out CurrentBrowserHeight, out CurrentViewportLeft, out CurrentViewportTop, out CurrentViewportRight, out CurrentViewportBottom, out CurrentViewportWidth, out CurrentViewportHeight);
            }


            if (!DevToolsLoggingFilename.Equals(""))
            {
                _devToolsLoggingFilename = DevToolsLoggingFilename;
                String network_log_har = "{\"log\": {\"version\": \"1.2\", \"creator\": {}, \"browser\": {}, \"pages\": [], \"entries\": []}}";
                Toolkit.File.overwrite(AppDomain.CurrentDomain.BaseDirectory + _devToolsLoggingFilename, network_log_har);
                EnableDevToolsLogging(options);
            }
        }



        //        public static void start(bool BrowserMobProxy, bool Headless, String SeleniumHubUrl, String BrowserName, String BrowserVersion, String OS, String OSVersion, String Resolution, String UrlToVisit, String ProfileToUse, bool ClearBrowserData, String BrowserstackDebug, String BrowserExecutableLocation, Size BrowserSize, Point BrowserPosition, bool DevToolsLogging)
        public static void start(string DevToolsLoggingFilename, bool Headless, String BrowserVersion, String OS, String OSVersion, String Resolution, String UrlToVisit, bool ClearBrowserData, String BrowserExecutableLocation) //, Size BrowserSize, Point BrowserPosition)
        {
            CurrentBrowserPID = -1;
            CurrentBrowserLeft = -1;
            CurrentBrowserRight = -1;
            CurrentBrowserTop = -1;
            CurrentBrowserBottom = -1;
            CurrentBrowserWidth = -1;
            CurrentBrowserHeight = -1;
            CurrentViewportLeft = -1;
            CurrentViewportRight = -1;
            CurrentViewportTop = -1;
            CurrentViewportBottom = -1;
            CurrentViewportWidth = -1;
            CurrentViewportHeight = -1;

            try
            {
                var options = SetBrowserOptions(Headless, BrowserVersion, OS, BrowserExecutableLocation);
//                StartBrowser(options, DevToolsLogging, UrlToVisit, BrowserExecutableLocation, BrowserSize, BrowserPosition);
                StartBrowser(options, DevToolsLoggingFilename, UrlToVisit, BrowserExecutableLocation);
            }
            catch (Exception e)
            {
                Log.WriteLine(e.ToString());
            }
        }

        //public static explicit operator ChromeDriver(BrowserTestObject v)
        //{
        //    throw new NotImplementedException();
        //}

        public static void Close2()
        {
            try
            {
                CurrentDriver.Close();
            }
            catch (Exception e)
            {
                Log.WriteLine(e.ToString());
            }

            CurrentBrowserPID = -1;
            CurrentBrowserLeft = -1;
            CurrentBrowserRight = -1;
            CurrentBrowserTop = -1;
            CurrentBrowserBottom = -1;
            CurrentBrowserWidth = -1;
            CurrentBrowserHeight = -1;
            CurrentViewportLeft = -1;
            CurrentViewportRight = -1;
            CurrentViewportTop = -1;
            CurrentViewportBottom = -1;
            CurrentViewportWidth = -1;
            CurrentViewportHeight = -1;
        }

        public static void CloseAll()
        {
            DisableDevTools();

            if (CurrentDriver != null)
            {
                // The Close call below causes a "failed to close window" error on the Replay app in Mac only.
                //  Skipping the close call for binary (CEF) applications

                //if (Test_Environment.BrowserLocation.Equals(""))

                    Close2();

                CurrentDriver.Quit();
            }

            CurrentBrowserPID = -1;
            CurrentBrowserLeft = -1;
            CurrentBrowserRight = -1;
            CurrentBrowserTop = -1;
            CurrentBrowserBottom = -1;
            CurrentBrowserWidth = -1;
            CurrentBrowserHeight = -1;
            CurrentViewportLeft = -1;
            CurrentViewportRight = -1;
            CurrentViewportTop = -1;
            CurrentViewportBottom = -1;
            CurrentViewportWidth = -1;
            CurrentViewportHeight = -1;
        }

        public static void Stop()
        {
            (Selenium.BrowserTestObject.CurrentDriver).ExecuteScript("return window.stop;");
        }

        public static bool IsClosed()
        {
            try
            {
                var test = CurrentDriver.Url;
            }
            catch (Exception e)
            {
                Log.WriteLine(e.ToString());
                return true;
            }

            return false;
        }

        
        public static void WaitUntilNoAjaxRequests()
        {
            try
            {
                "wait until ready".StartTimer();

                while ("wait until ready".GetTimer() < 10)
                {
                    Selenium.BrowserTestObject.CurrentDriver.ExecuteScript("");
                    long active_jquery_count = (long)(Selenium.BrowserTestObject.CurrentDriver).ExecuteScript("return jQuery.active");

                    if (active_jquery_count < 1)

                        break;

                    //("Number of Ajax Requests = " + active_jquery_count).LogDebug();
                    (0.5).Sleep(0);
                }
            }
            catch (Exception e)
            {
                Log.WriteLine(e.ToString());
            }
        }
        

        public static object ExecuteJavaScript(String javascript)
        {
            return (Selenium.BrowserTestObject.CurrentDriver).ExecuteScript(javascript);
        }


        public static void TraceAjaxRequests()
        {
            NumberOfAjaxRequests = (long)(Selenium.BrowserTestObject.CurrentDriver).ExecuteScript("return jQuery.active");
        }

        
        public static void WaitUntilNoPendingAjaxRequests()
        {
            try
            {
//                WaitForAngular();

                "wait until ready".StartTimer();

                while ("wait until ready".GetTimer() < 10)
                {
                    Selenium.BrowserTestObject.CurrentDriver.ExecuteScript("");
                    long active_jquery_count = (long)(Selenium.BrowserTestObject.CurrentDriver).ExecuteScript("return jQuery.active");

                    if (active_jquery_count <= NumberOfAjaxRequests)

                        break;

                    //("Number of Ajax Requests = " + (active_jquery_count - NumberOfAjaxRequests)).LogDebug();
                    (0.5).Sleep(0);
                }
            }
            catch (Exception e)
            {
                Log.WriteLine(e.ToString());
            }
        }

        public static void WaitUntilNoPendingAjaxRequests(int timeout)
        {
            try
            {
//                WaitForAngular();

                "wait until ready".StartTimer();

                while ("wait until ready".GetTimer() < timeout)
                {
                    Selenium.BrowserTestObject.CurrentDriver.ExecuteScript("");
                    long active_jquery_count = (long)(Selenium.BrowserTestObject.CurrentDriver).ExecuteScript("return jQuery.active");

                    if (active_jquery_count <= NumberOfAjaxRequests)

                        break;

                    //("Number of Ajax Requests = " + (active_jquery_count - NumberOfAjaxRequests)).LogDebug();
                    (0.5).Sleep(0);
                }
            }
            catch (Exception e)
            {
                Log.WriteLine(e.ToString());
            }
        }
        

        public static void SetNetworkConditions(bool Offline = false, long DownloadThroughput = 500 * 1024, long UploadThroughput = 500 * 1024, long Latency = 5)
        {
            EmulateNetworkConditionsCommandSettings command = new EmulateNetworkConditionsCommandSettings();
            command.Latency = Latency;
            command.DownloadThroughput = DownloadThroughput; // Mbps to bytes per second
            command.UploadThroughput = UploadThroughput; // Mbps to bytes per second
            command.Offline = Offline;
            _devToolsSessionDomains.Network.EmulateNetworkConditions(command);
        }



        public void ExecuteChromeCommand(string commandName, Dictionary<string, object> commandParameters)
        {
            if (commandName == null)
                throw new ArgumentNullException("commandName", "commandName must not be null");
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary["cmd"] = commandName;
            dictionary["params"] = commandParameters;
            Execute("sendChromeCommand", dictionary);
        }


        public object ExecuteChromeCommandWithResult(string commandName, Dictionary<string, object> commandParameters)
        {
            if (commandName == null)
            {
                throw new ArgumentNullException("commandName", "commandName must not be null");
            }

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters["cmd"] = commandName;
            parameters["params"] = commandParameters;
            OpenQA.Selenium.Response response = CurrentDriver.Execute("sendChromeCommandWithResult", parameters);
            return response.Value;
        }

        private Dictionary<string, object> EvaluateDevToolsScript(string scriptToEvaluate)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters["returnByValue"] = true;
            parameters["expression"] = scriptToEvaluate;
            object evaluateResultObject = ExecuteChromeCommandWithResult("Runtime.evaluate", parameters);
            Dictionary<string, object> evaluateResultDictionary = evaluateResultObject as Dictionary<string, object>;
            Dictionary<string, object> evaluateResult = evaluateResultDictionary["result"] as Dictionary<string, object>;
            Dictionary<string, object> evaluateValue = evaluateResult["value"] as Dictionary<string, object>;
            return evaluateValue;
        }

        public Screenshot GetFullPageScreenshot()
        {
            // Evaluate this only to get the object that the
            // Emulation.setDeviceMetricsOverride command will expect.
            // Note that we can use the already existing ExecuteChromeCommand
            // method to set and clear the device metrics, because there's no
            // return value that we care about.
            string metricsScript = @"({
width: Math.max(window.innerWidth,document.body.scrollWidth,document.documentElement.scrollWidth)|0,
height: Math.max(window.innerHeight,document.body.scrollHeight,document.documentElement.scrollHeight)|0,
deviceScaleFactor: window.devicePixelRatio || 1,
mobile: typeof window.orientation !== 'undefined'
})";
            Dictionary<string, object> metrics = EvaluateDevToolsScript(metricsScript);
            ExecuteChromeCommand("Emulation.setDeviceMetricsOverride", metrics);

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters["format"] = "png";
            parameters["fromSurface"] = true;
            object screenshotObject = ExecuteChromeCommandWithResult("Page.captureScreenshot", parameters);
            Dictionary<string, object> screenshotResult = screenshotObject as Dictionary<string, object>;
            string screenshotData = screenshotResult["data"] as string;

            ExecuteChromeCommand("Emulation.clearDeviceMetricsOverride", new Dictionary<string, object>());

            Screenshot screenshot = new Screenshot(screenshotData);
            return screenshot;
        }



        public static Bitmap TakeFullPageScreenShot()
        {
            try
            {
                Screenshot sc = CurrentDriver.GetFullPageScreenshot();
                return System.Drawing.Image.FromStream(new MemoryStream(sc.AsByteArray)) as Bitmap;
            }
            catch (Exception e)
            {
                Log.WriteLine(e.ToString());
            }

            return null;
        }


        public static void TakeFullPageScreenShot(String folder_name, String file_name)
        {
            Screenshot screenshot = CurrentDriver.GetFullPageScreenshot();
            screenshot.SaveAsFile(folder_name + "\\" + file_name);
        }








        public static void TakeScreenShot(String folder_name, String file_name)
        {
            ((ITakesScreenshot)Selenium.BrowserTestObject.CurrentDriver).GetScreenshot().SaveAsFile(folder_name + "\\" + file_name, ScreenshotImageFormat.Png);
        }

        public static Bitmap TakeScreenShot()
        {
            try
            {
                var sc = ((ITakesScreenshot)Selenium.BrowserTestObject.CurrentDriver).GetScreenshot();
                //                CurrentTakesScreenshot.GetScreenshot();
                return System.Drawing.Image.FromStream(new MemoryStream(sc.AsByteArray)) as Bitmap;
            }
            catch (Exception e)
            {
                Log.WriteLine(e.ToString());
            }

            return null;
        }



        public static System.Drawing.Bitmap Screenshot(int x, int y, int width, int height)
        {
            Byte[] byteArray = Selenium.BrowserTestObject.CurrentDriver.GetScreenshot().AsByteArray;
            System.Drawing.Bitmap screenshot = new System.Drawing.Bitmap(new System.IO.MemoryStream(byteArray));

            System.Drawing.Rectangle cropRect = new System.Drawing.Rectangle(x, y, width, height);
            Bitmap target = new Bitmap(cropRect.Width, cropRect.Height);

            using (Graphics g = Graphics.FromImage(target))
            {
                g.DrawImage(screenshot, new Rectangle(0, 0, target.Width, target.Height), cropRect, GraphicsUnit.Pixel);
            }

            return target;
        }


        public static void AcceptAlert()
        {
            Selenium.BrowserTestObject.CurrentDriver.SwitchTo().Alert().Accept();
        }

        public static void Refresh()
        {
            Selenium.BrowserTestObject.CurrentDriver.Navigate().Refresh();
        }

        public static void SwitchToTab(int tab_number)
        {
            var tabs_windows = CurrentDriver.WindowHandles;
            CurrentDriver.SwitchTo().Window(tabs_windows[tab_number]);
        }

        // Switch control back to the main browser window
        public static void SwitchToDefaultContent()
        {
            SetCurrentDriverIfNotSet(true);
            CurrentDriver.SwitchTo().DefaultContent();
        }

        public static void WaitForAngular()
        {
            // Check if AngularJS exists, and if so apply the Protractor (.Net) wait algorithm
            bool angular_exists = (bool)(Selenium.BrowserTestObject.CurrentDriver).ExecuteScript("return (window.angular !== undefined) && (angular.element(document).injector() !== undefined)");

            if (angular_exists)
            {
                string GetNg1HooksHelper = @"
                        function getNg1Hooks(selector, injectorPlease) {
                            function tryEl(el) {
                                try {
                                    if (!injectorPlease && angular.getTestability) {
                                        var $$testability = angular.getTestability(el);
                                        if ($$testability) {
                                            return {$$testability: $$testability};
                                        }
                                    } else {
                                        var $injector = angular.element(el).injector();
                                        if ($injector) {
                                            return {$injector: $injector};
                                        }
                                    }
                                } catch(err) {} 
                            }
                            function trySelector(selector) {
                                var els = document.querySelectorAll(selector);
                                for (var i = 0; i < els.length; i++) {
                                    var elHooks = tryEl(els[i]);
                                    if (elHooks) {
                                        return elHooks;
                                    }
                                }
                            }
                            if (selector) {
                                return trySelector(selector);
                            } else if (window.__TESTABILITY__NG1_APP_ROOT_INJECTOR__) {
                                var $injector = window.__TESTABILITY__NG1_APP_ROOT_INJECTOR__;
                                var $$testability = null;
                                try {
                                    $$testability = $injector.get('$$testability');
                                } catch (e) {}
                                return {$injector: $injector, $$testability: $$testability};
                            } else {
                                return tryEl(document.body) ||
                                    trySelector('[ng-app]') || trySelector('[ng\\:app]') ||
                                    trySelector('[ng-controller]') || trySelector('[ng\\:controller]');
                            }
                        };
                        ";

                string WaitForAngular = GetNg1HooksHelper + @"
                        var rootSelector = arguments[0];
                        var callback = arguments[1];
                        if (window.angular && !(window.angular.version && window.angular.version.major > 1)) {
                            /* ng1 */
                            var hooks = getNg1Hooks(rootSelector);
                            if (hooks.$$testability) {
                                hooks.$$testability.whenStable(callback);
                            } else if (hooks.$injector) {
                                hooks.$injector.get('$browser').
                                notifyWhenNoOutstandingRequests(callback);
                            } else if (!!rootSelector) {
                                throw new Error('Could not automatically find injector on page: ""' +
                                    window.location.toString() + '"". Consider setting rootElement');
                            } else {
                            throw new Error('root element (' + rootSelector + ') has no injector.' +
                                ' this may mean it is not inside ng-app.');
                            }
                        } else if (rootSelector && window.getAngularTestability) {
                            var el = document.querySelector(rootSelector);
                            window.getAngularTestability(el).whenStable(callback);
                        } else if (window.getAllAngularTestabilities) {
                            var testabilities = window.getAllAngularTestabilities();
                            var count = testabilities.length;
                            var decrement = function() {
                                count--;
                                if (count === 0) {
                                    callback();
                                }
                            };
                            testabilities.forEach(function(testability) {
                                testability.whenStable(decrement);
                            });
                        } else if (!window.angular) {
                            throw new Error('window.angular is undefined.  This could be either ' +
                                'because this is a non-angular page or because your test involves ' +
                                'client-side navigation, which can interfere with Protractor\'s ' +
                                'bootstrapping.  See http://git.io/v4gXM for details');
                        } else if (window.angular.version >= 2) {
                            throw new Error('You appear to be using angular, but window.' +
                                'getAngularTestability was never set.  This may be due to bad ' +
                                'obfuscation.');
                        } else {
                            throw new Error('Cannot get testability API for unknown angular ' +
                                'version ""' + window.angular.version + '""');
                        }";

                try
                {
                    CurrentDriver.ExecuteAsyncScript(WaitForAngular); //, "body");
                }
                catch (Exception e)
                {
                    Log.WriteLine(e.ToString());
                }
            }
        }



        public int GetDocumentWidth()
        {
            return Convert.ToInt32((BrowserTestObject.CurrentDriver).ExecuteScript("return document.documentElement.clientWidth;"));
        }


        public int GetDocumentHeight()
        {
            return Convert.ToInt32((BrowserTestObject.CurrentDriver).ExecuteScript("return document.documentElement.clientHeight;"));
        }
        
        public static void SetSize(int width, int height)
        {
            "SetSize".StartTimer();

            while ("SetSize".GetTimer() < 10)
            {
                try
                {
                    get_current().Manage().Window.Size = new Size(width, height);
                    return;
                }
                catch (Exception)
                {
                    //("Failed to resize browser").LogDebug();
                }

                (5).Sleep(1);
            }

            throw new System.Exception("Timed out trying to resize the browser");
        }

        public static void SetPosition(int x, int y)
        {
            "SetSize".StartTimer();

            while ("SetSize".GetTimer() < 10)
            {
                try
                {
                    get_current().Manage().Window.Position = new Point(x, y);
                    return;
                }
                catch (Exception)
                {
                    //("Failed to position the browser").LogDebug();
                }

                (5).Sleep(1);
            }

            throw new System.Exception("Timed out trying to position the browser");
        }
        
        public static void WaitUntilReady()
        {
            try
            {
//                WaitForAngular();

                "wait until ready".StartTimer();

                while ("wait until ready".GetTimer() < 10)
                {
                    Selenium.BrowserTestObject.CurrentDriver.ExecuteScript("");

                    String document_status = (String)(Selenium.BrowserTestObject.CurrentDriver).ExecuteScript("return document.readyState;");
                    long active_jquery_count = (long)(Selenium.BrowserTestObject.CurrentDriver).ExecuteScript("return jQuery.active");
                    long animation_count = (long)(Selenium.BrowserTestObject.CurrentDriver).ExecuteScript("return $(\":animated\").length;");
                    long document_state = ((document_status.Equals("complete")) ? 0 : 1);
                    long num_busy_items = document_state + active_jquery_count + animation_count;

                    if (num_busy_items < 1)

                        break;

                    //("Number of busy items = " + num_busy_items + " (Document State: " + document_status + ", Active JQuery Count: " + active_jquery_count + ", Animation Count: " + animation_count + ")").LogDebug();
                    (0.5).Sleep(0);
                }
            }
            catch (Exception e)
            {
                // do not log an exception
            }
        }

    }
}
