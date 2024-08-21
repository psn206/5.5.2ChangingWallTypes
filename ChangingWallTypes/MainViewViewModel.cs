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

        private ExternalCommandData _commandData;

        public DelegateCommand SaveCommand { get; }

        public List<Element> PickedWalls { get; }

        public List<WallType> WallsSystem { get; } = new List<WallType>();

        public WallType SelectedWallsSystem { get; set; }
        MethodRevitApp methodRevitApp;


        public MainViewViewModel(ExternalCommandData commandData)
        {
            _commandData = commandData;
            SaveCommand = new DelegateCommand(OnSaveCommand);
            methodRevitApp = new MethodRevitApp(commandData);
            PickedWalls = methodRevitApp.GetWalls();
            WallsSystem = methodRevitApp.GetSystemType();

        }


        private void OnSaveCommand()
        {
            UIApplication uiApp = _commandData.Application;
            UIDocument uiDoc = uiApp.ActiveUIDocument;
            Document doc = uiDoc.Document;

            if (WallsSystem.Count == 0 || SelectedWallsSystem == null) { return; }

            using (var ts = new Transaction(doc, "Set Type"))
            {
                ts.Start();
                foreach (var PickedWall in PickedWalls)
                {
                    if (PickedWall is Wall)
                    {
                        var oWall = PickedWall as Wall;
                        oWall.WallType = SelectedWallsSystem;
                    }
                }
                ts.Commit();

                RaiseCloseRecuest();
            }

          
        }

        public event EventHandler CloseRecuest;

        public void RaiseCloseRecuest()
        {
            CloseRecuest?.Invoke(this, EventArgs.Empty);
        }

    }
}
