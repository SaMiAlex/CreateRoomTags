using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.UI;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreateRoomTags
{
    public class MVVM
    {
        private ExternalCommandData _commandData;

        public List<Level> Levels { get; }
        public List<Room> Rooms { get; }
        public List<RoomTagType> RoomTags { get; }
        public DelegateCommand SaveCommand { get; }
        public Level SelectedLevel { get; set; }
        public RoomTagType SelectedTag { get; set; }

        public MVVM(ExternalCommandData commandData)
        {
            _commandData = commandData;
            Levels = FilterUtils.GetLevels(commandData); //находим все уровни в открытом проекте
            Rooms = FilterUtils.GetRooms(commandData); // находим все помещения в открытом проекте
            RoomTags = FilterUtils.GetRoomTags(commandData);//находим все марки помещений в открытом проекте
            SaveCommand = new DelegateCommand(OnSaveCommand); //добавляем марки в проект
        }

        private void OnSaveCommand()
        {
            UIApplication uiapp = _commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            if (SelectedLevel == null || SelectedTag == null)
                return;           
                
            
            //находим все помещения на выбранном уровне
            List <Room> rooms = new List<Room>();
            foreach (Room room in Rooms)
            {
                Parameter roomLevel = room.get_Parameter(BuiltInParameter.LEVEL_NAME);
                var level = roomLevel.AsString();            

                if (level == SelectedLevel.Name)
                    rooms.Add(room);
            }     
            
            
            if (rooms.Count==0)
                TaskDialog.Show("Ошибка", "На указанном уровне отсутствуют помещения");

            //добавляем марки всем помещениям на указанном уровне
            foreach (Room room in rooms)
            {
                Transaction ts = new Transaction(doc);
                ts.Start("Расстановка меток");
                LocationPoint locationPoint = room.Location as LocationPoint;
                UV point = new UV(locationPoint.Point.X, locationPoint.Point.Y);
                RoomTag newTag = doc.Create.NewRoomTag(new LinkElementId(room.Id), point, null);
                newTag.RoomTagType = SelectedTag;
                ts.Commit();
            }
            RaiseCloseRequest();
        }


        //закрытие окна приложения
        public event EventHandler CloseRequest;
        private void RaiseCloseRequest()
        {
            CloseRequest?.Invoke(this, EventArgs.Empty);
        }
    }
}
