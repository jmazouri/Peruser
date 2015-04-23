using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.ComponentModel.Composition.Registration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
namespace Peruser
{
    public static class LibraryContainer
    {
        private static List<Type> _iocContainer;

        public static List<Type> Container
        {
            get
            {
                if (_iocContainer == null)
                {
                    _iocContainer = new List<Type>();
                    FileInfo[] allDlls = new DirectoryInfo(Path.Combine(Directory.GetCurrentDirectory(), "ImageLibraries")).GetFiles("*.dll");

                    /*
                    AppDomain.CurrentDomain.AssemblyResolve += (sender, args) => Assembly.LoadFile(new Uri(args.RequestingAssembly.CodeBase).AbsolutePath);
                    */
                    foreach (var s in allDlls)
                    {
                        var assembly = Assembly.LoadFrom(s.FullName);
                        _iocContainer.AddRange(assembly.GetTypes().Where(d => typeof (ImageLibrary).IsAssignableFrom(d)));
                    }
                }

                return _iocContainer;
            }
        }
    }
}
