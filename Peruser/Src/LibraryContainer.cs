﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Peruser.ImageLibraries;

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
                    FileInfo[] allDlls = new DirectoryInfo(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "ImageLibraries")).GetFiles("*.dll");

                    foreach (var s in allDlls)
                    {
                        var assembly = Assembly.LoadFrom(s.FullName);
                        _iocContainer.AddRange(assembly.GetTypes().Where(d => typeof (ImageLibrary).IsAssignableFrom(d)));
                    }

                    _iocContainer.Add(typeof (LocalImageLibrary));
                }

                return _iocContainer;
            }
        }
    }
}
