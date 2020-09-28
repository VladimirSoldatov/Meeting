using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;

namespace Meeting
{
    class Program
    {

        public class Meeting//Класс встречи
        {
            string name; //Имя встречи
            TimeSpan time_tor_remind; //интервал уведомления
            DateTime date_begin; //Дата начала встречи
            DateTime date_end;//Дата окончания стречи
            string location;//Местро проведения
            bool status;//Статус встречи (активна/не активна)
            public Meeting(string _name, List<DateTime> _date, string _location)//Конструктор с параметрами
            {
                name = _name;
                date_begin = _date[0];
                date_end = _date[1];
                // members = _members;
                status = false;
                location = _location;
                status = true;

            }
            public Meeting()//Конструктор по-умолчанию
            {
                name = "";
                date_begin = DateTime.Now;
                date_end = DateTime.Now;
                // members = _members;
                status = false;
                location = "";
                status = true;
            }
            public void set_status(bool _status)//функция назначения значения статуса встрече
            {
                status = _status;
            }
            public bool get_status()//функция получения значения статуса встрече
            {
                return status;
            }
            public void set_time_to_remind()//функциЯ установки интервала для уведомления

            {
                if (status)
                {
                    string time = Console.ReadLine();

                    TimeSpan.TryParse(time, out time_tor_remind);//парсинг строки для задания интервала
                }

            }
            public string Name{
                get
                {
                    return name;//получение им через свойство
                }

                set
                {
                    name = value;//назачение имени через свойство
                }
            }
            public string Location
            {
                get
                {
                    return name;
                }

                set
                {
                    location = value;
                }
            }
            public string Date
            {
          
                set
                {
                    DateTime temp_date_begin = MeetingManagemant.Parser_time(value)[0];
                    DateTime temp_date_end = MeetingManagemant.Parser_time(value)[1];
                    if (temp_date_begin!=new DateTime() || temp_date_end != new DateTime())
                    {
                        date_begin = temp_date_begin;
                        date_end = temp_date_end;
                    }
                    else
                    {
                        Console.WriteLine("Время введено неверно");
                    }

                }
            }
            public void send_inform()//функция полылки уведомления пользователю
            {
                if (status == true)
                    Console.WriteLine($"Cовещание {name} {date_begin.ToShortDateString()} в {date_begin.TimeOfDay} по адресу {location}, уведомление о встрече за {time_tor_remind}");
            }
            public void send_inform(string path)//функция полылки уведомления пользователю
            {
                if (status == true)
                    save_inf_file(path, $"Cовещание {name} {date_begin.ToShortDateString()} в {date_begin.TimeOfDay} по адресу {location}, уведомление о встрече за {time_tor_remind}");
            }
            public void save_inf_file(string path, string text){
                try{
                using (FileStream fstream = new FileStream($"{path}\\meetings.txt", FileMode.OpenOrCreate)){
                    byte[] array = System.Text.Encoding.Default.GetBytes(text);
                    fstream.Write(array, 0, array.Length);
                    Console.WriteLine("Расписание встреч сохранено");

                    }
                }catch (Exception ex                ){
                    Console.WriteLine(ex.Message);
}
}
            public List<DateTime>get_dates()//функция получения списка дат начал и конца, возможно это сделать и через кортеж
            {
                List<DateTime> temp = new List<DateTime>();
                temp.Add(date_begin);
                temp.Add(date_end);
                return temp;
            }
 
           public string get_name()
            {
                return name;
            }
        }
    
           public class MeetingManagemant {//Класч менеджера встреч
                List<Meeting> My_meetings = new List<Meeting>();//Список встреч
                public MeetingManagemant()
                {
                  
                }
            
                public void Add_Meeting (string _name, string _date, string _location)//Функция добавления встречи
                {

                var my_date = Parser_time(_date);//Парсинг даты встречи
                var new_meeting = new Meeting(_name, my_date, _location);//вешалка для атрибутов встречи
                //--------------Проверка заполненности валидности атрибутов встречи---------------------
                if (_name == "")
                {
                    Console.WriteLine("Не введено название");

                   
                }
                if (this.GetByName(_name)==new Meeting())
                {
                    Console.WriteLine("Встреча с таким названим уже есть, введите другое");


                }
                else if (my_date[0] == new DateTime() || DateTime.Compare(my_date[0], my_date[1]) > 0 || DateTime.Compare(my_date[0], DateTime.Now)<0)
                {
                    Console.WriteLine("Попробуйте выбрать другое время");
       
                }
                else if (_location == "")
                {
                    Console.WriteLine("Не введено место проведения встречи");

                }
                else
                {
  //----------------------------Окончание проверки валидности атрибутов встречи
                    if (My_meetings.Count == 0)
                        My_meetings.Add(new_meeting);
                    else
                    {
                        foreach (var item in My_meetings)//Проверка пересечения времени встречи с другими встречами в списке
                        {
                            List<DateTime> EarlyMeeting =  item.get_dates();
                            List<DateTime> CurrentMeeting = new_meeting.get_dates();
                            if (DateTime.Compare(EarlyMeeting[0], CurrentMeeting[0]) > 0 && DateTime.Compare(EarlyMeeting[0], CurrentMeeting[1]) < 0 || (DateTime.Compare(EarlyMeeting[1], CurrentMeeting[0]) < 0 && DateTime.Compare(EarlyMeeting[1], CurrentMeeting[1]) > 0)){
                                My_meetings.Add(new_meeting);
                              
                            }
                            else
                            {
                                Console.WriteLine($"Конфликт с {item.get_name()} ");
                            }
                        }
                    }
                }
           }

            public Meeting GetByName(string temp)//Получение встречи из списка по имени
            {
                Meeting temp_meet = new Meeting();
                foreach (var item in My_meetings)
                {
                    if (item.get_name() == temp)
                    {
                        temp_meet = item;
                    }
                }
                return temp_meet;
            }
            static public DateTime Parser_time(string _date, string path){
                DateTime temp = DateTime.Parse(_date);
                return temp;
}
            static public List<DateTime> Parser_time(string _date)//Парсер даты для  атрибута дата встречи
            {
                List<DateTime> Meet = new List<DateTime>();
                var date1 = new DateTime();
                var date2 = new DateTime();
                try
                {
                    var my_date = _date.Split(' ');
                    var DDMMYYYY = my_date[0].Split('.');
                    var HHmm = my_date[1].Split(':');
                    var HHMMend = my_date[2].Split(':');

                    bool triger_year = Int32.TryParse(DDMMYYYY[2], out int year);
                    bool trigger_mounth = Int32.TryParse(DDMMYYYY[1], out int mounth);
                    bool trigger_day = Int32.TryParse(DDMMYYYY[0], out int day);
                    bool trigger_hours = Int32.TryParse(HHmm[0], out int hours_begin);
                    bool trigger_minuts = Int32.TryParse(HHmm[1], out int minuts_begin);
                    bool trigger_hours_end = Int32.TryParse(HHmm[0], out int hours_end);
                    bool trigger_minuts_end = Int32.TryParse(HHmm[1], out int minuts_end);
                    //---------------------------------валидация даты встречи-------------------------------------------
                    if (!(triger_year || trigger_day || trigger_hours || trigger_mounth || trigger_minuts || trigger_hours_end || trigger_minuts_end) || day > DateTime.DaysInMonth(year, mounth) || mounth > 12 || year != DateTime.Now.Year)
                    {
                        Console.WriteLine("Неверный формат даты");


                    }
                    else
                    {
                        date1 = new DateTime(year, mounth, day, hours_begin, minuts_begin, 0);
                        date2 = new DateTime(year, mounth, day, hours_end, minuts_end, 0);
                    }
                    if (DateTime.Compare(date1, DateTime.Now) < 0)
                    {
                        Console.WriteLine("Время встречи должны быть больше текущего");


                    }
                    if (DateTime.Compare(date1, date2) > 0)
                    {
                        Console.WriteLine("Время Окончания встречи должно пыть после ее начала.");


                    }
                }
                catch
                {

                }
                //---------------------------------окончание валидация даты встречи-------------------------------------------
                Meet.Add(date1);
                Meet.Add(date2);


                return Meet;

            }
            public void remove_meeting(string _name)//удаление встречи по имени
            {
                var counts = My_meetings.Count;
                List<Meeting> temp = new List<Meeting>();
                foreach(var tempus in My_meetings)//Создание клона списка встреч
                {
                    temp.Add(tempus);
                }
                  
                foreach (var item in temp)
                {
                    if(item.get_name()==_name)
                    My_meetings.Remove(item);
                    Console.WriteLine($"Удаление встречи {item.Name } выполнено");
                }
                if (counts == My_meetings.Count)
                    Console.WriteLine($"Удаление {_name} не выполнено");//Сообщение об отсутствии встречи по имени
            }
            public void change_meeting(string _name)//Смена времени встречи
            {
                foreach (var item in My_meetings)
                {
                    if (item.get_name() == _name)
                    {
                        Console.WriteLine("Введите новое время");
                        item.Date=Console.ReadLine();
                    }
                       
                }
            }
            public void get_meetings()//Получение всех встреч (были и будут)
            {
                if (My_meetings.Count != 0)
                {
                    foreach (var item in My_meetings)
                    {
                        item.send_inform();
                    }
                }
                else Console.WriteLine("Встреч не найдено");
            }
           public void get_meetings(string path)//Получение всех встреч (были и будут)
            {
                if (My_meetings.Count != 0)
                {
                    foreach (var item in My_meetings)
                    {
                        item.send_inform(path);
                    }
                }
                else Console.WriteLine("Встреч не найдено");
            }
            public void get_meetings(string _date, string path)//Получение встреч на конкретный день
            {
                int i = 0;
                DateTime value = Parser_time(_date, path);
                foreach (var item in My_meetings)
                {
                    if (DateTime.Compare(item.get_dates()[0].Date, value.Date) == 0)
                    {
                        item.send_inform(path);
                        i++;
                    }
                    
                }
                if (i == 0)
                {
                    Console.WriteLine($"На {value.ToShortDateString()} совещаний нет");
                }
            }

            public void get_meetings(DateTime value)//Получение встреч на конкретный день
            {
                int i = 0;
                foreach (var item in My_meetings)
                {
                    if (DateTime.Compare(item.get_dates()[0].Date, value.Date) == 0)
                    {
                        item.send_inform();
                        i++;
                    }
                    
                }
                if (i == 0)
                {
                    Console.WriteLine($"На {value.ToShortDateString()} совещаний нет");
                }
            }
        }
        static void Main(string[] args)
        {
            string _path = "d:";
            MeetingManagemant mine = new MeetingManagemant();
            mine.Add_Meeting("Первое", "21.09.2020 11:00 12:00", "Moscow");
            mine.Add_Meeting("Второе", "21.09.2020 10:00 12:00", "Moscow");
            mine.Add_Meeting("Третье", "20.09.2020 19:00 20:00", "Moscow");
            mine.get_meetings("21.09.2020", _path);
            mine.change_meeting("Первое");

  
            
            /*План тестирования:
            1. Компиляция программы. Ожидание -  программа скомпилируется из оверштться с кодом 0
            2. Создание встречи с корретными атрибутами: Ожидание - Оперция завешиться нормально
            3. Вывод встречи на экран. Ождание встреча выводится на кран, данные встречи корретны
            4. Добавление новой встречи и вод списка встречь на экран. Ожидание ввод без ошибок и водится на одну встречу больше, а именно на добавленную
            5. Удаление встречи по имени. Ожидание операция без ошибки, встреч на одну меньше а именно не вывдется удаленная встреча
            6. Вывод встречь на текущий или заданный день. Ожидание - выводятся  встречи на заданный день.
            7. Изменение даты и времени встречи, новые данные водтся корректные. Ожидание дата и время встречи будут изменены на указанные.
            8. Ввод не корретных данных при создании встречи. Ожидание встреча не бдет создана.
            9. Удаление не существющей встречи. Ожидание - сообщение, что встречи не найдено. Количество встреч не изменится.
            10. ввод некорретных данный при измененении времени встречи. Ожижание - сооющение о некорретткности данных и данные встречи не изменятся.

             */


        }
    }
}
