using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;

namespace Mapper
{
    class MainWindowModel
    {
        public static MainWindowModel Instance = new MainWindowModel();

        public ObservableCollection<MapSymbolModel> MapSymbols { get; } = new ObservableCollection<MapSymbolModel>();

        public Point Center { get; set; }
        public double Scale { get; set; }
    }
}
