using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: log4net.Config.XmlConfigurator(ConfigFile = @"log4net.config", Watch = true)]
namespace Jalan.Utils.Log4net
{
    public class Logger
    {
        private Type _type;

        public Type CurrentType
        {
            get
            {
                if (_type == null)
                {
                    var trace = new System.Diagnostics.StackTrace();
                    _type = trace.GetFrame(trace.FrameCount - 1).GetMethod().ReflectedType;
                }
                return _type;
            }
        }
        public Logger()
        {
            //配置log4
            log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log4net.config")));
        }
        public Logger(Type type)
        {
            this._type = type;
        }
        public log4net.ILog GetLogger(Type type)
        {
            return log4net.LogManager.GetLogger(type);
        }
        public void Error<T>(object obj)
        {
            this.GetLogger(typeof(T)).Error(obj);
        }
        public void Info<T>(object obj)
        {
            this.GetLogger(typeof(T)).Info(obj);
        }
        public void WriteError(object obj)
        {
            this.GetLogger(this.CurrentType).Error(obj);
        }
        public void WriteInfo(object obj)
        {
            this.GetLogger(this.CurrentType).Info(obj);
        }
    }
}


