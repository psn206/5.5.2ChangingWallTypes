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

        public UIApplication UiApp { get => uiApp;  }
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
    }
}
