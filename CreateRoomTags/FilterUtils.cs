using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreateRoomTags
{
    public class FilterUtils
    {
        public static List<Level> GetLevels(ExternalCommandData commandData)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;

            List<Level> levels = new FilteredElementCollector(doc)
                    .OfClass(typeof(Level))
                    .OfType<Level>()
                    .ToList();

            return levels;
        }

        public static List<Room> GetRooms(ExternalCommandData commandData)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;

            List<Room> rooms = new FilteredElementCollector(doc)                    
                    .OfCategory(BuiltInCategory.OST_Rooms)
                    .Cast<Room>()
                    .ToList();

            return rooms;
        }

        public static List<RoomTagType> GetRoomTags(ExternalCommandData commandData)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;

            List<RoomTagType> roomtags = new FilteredElementCollector(doc)
                    .OfClass(typeof(FamilySymbol))
                    .OfCategory(BuiltInCategory.OST_RoomTags)
                    .Cast<RoomTagType>()
                    .ToList();

            return roomtags;
        }



    }
}
