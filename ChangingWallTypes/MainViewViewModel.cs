using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ChangingWallTypes
{
    internal class MainViewViewModel
    {
        public DelegateCommand SaveCommand { get; }
        public List<Element> PickedWalls { get; }
        public List<WallType> WallsSystem { get; } = new List<WallType>();
        public WallType SelectedWallsSystem { get; set; }
        MethodRevitApp methodRevitApp;

        public MainViewViewModel(ExternalCommandData commandData)
        {
            SaveCommand = new DelegateCommand(OnSaveCommand);
            methodRevitApp = new MethodRevitApp(commandData);
            PickedWalls = methodRevitApp.GetWalls();
            WallsSystem = methodRevitApp.GetSystemType();
        }

        private void OnSaveCommand()
        {
            methodRevitApp.SetTypeWall(WallsSystem, PickedWalls, SelectedWallsSystem);
            RaiseCloseRecuest();
        }

        public event EventHandler CloseRecuest;

        public void RaiseCloseRecuest()
        {
            CloseRecuest?.Invoke(this, EventArgs.Empty);
        }

    }
}
