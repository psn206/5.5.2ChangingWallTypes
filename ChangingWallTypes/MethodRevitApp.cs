using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChangingWallTypes
{
    internal class MethodRevitApp
    {
        UIApplication uiApp;
        UIDocument uiDoc;
        Document doc;

        public UIApplication UiApp { get => uiApp; }
        public UIDocument UiDoc { get => uiDoc; }
        public Document Doc { get => doc; }

        public MethodRevitApp(ExternalCommandData commandData)
        {
            uiApp = commandData.Application;
            uiDoc = UiApp.ActiveUIDocument;
            doc = UiDoc.Document;
        }

        public List<Element> GetWalls()
        {
            var selectElements = UiDoc.Selection.PickObjects(ObjectType.Element, "Выберете стены");
            List<Element> walls = selectElements.Select(selectElement => Doc.GetElement(selectElement)).ToList();
            return walls;
        }

        public List<WallType> GetSystemType()
        {
            List<WallType> wallTypes = new FilteredElementCollector(Doc)
               .OfClass(typeof(WallType))
               .Cast<WallType>()
               .ToList();
            return wallTypes;
        }
        public void SetTypeWall(List<WallType> WallsSystem, List<Element> PickedWalls, WallType SelectedWallsSystem)
        {
            if (WallsSystem.Count == 0 || SelectedWallsSystem == null) { return; }

            using (var ts = new Transaction(Doc, "Set Type"))
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
            }
        }
    }
}
