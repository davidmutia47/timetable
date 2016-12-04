using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Timetable
{
    public partial class Form1 : Form
    {
        Dictionary<int, string> daysString = new Dictionary<int, string> { { 1, "Mon" }, { 2, "Tue" }, { 3, "Wed" }, { 4, "Thu" }, { 5, "Fri" }, { 6, "Sat" }, { 0,"Sun"} };
        Dictionary<string, int> daysInt = new Dictionary<string, int> { { "Mon", 1 }, { "Tue", 2 }, { "Wed", 3 }, { "Thu", 4 }, { "Fri", 5 }, { "Sat", 6 }, {"Sun",0 } };
        Task checkUpcomingLectures;
        private static String conString = "datasource=localhost;port=3306;username=root;password=dave;database=timetable2";
        MySqlConnection con = new MySqlConnection(conString);
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            loadTable(daysInt[DateTime.Now.DayOfWeek.ToString().Substring(0, 3)]);
            checkUpcomingLectures = Task.Factory.StartNew(() => checkUpcoming());
        }

        private void sendMailNotification(string to,string m)
        {
            try
            {
                using (SmtpClient client = new SmtpClient())
                {
                    MailMessage message = new MailMessage();
                    client.Port = 587;
                    client.Host = "smtp.gmail.com";
                    client.Timeout = 10000;
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.UseDefaultCredentials = false;
                    client.EnableSsl = true;
                    client.Credentials = new System.Net.NetworkCredential("youremai@address.com", "you Password");
                    message.From = new MailAddress("yoremail@address.com");
                    message.To.Add(new MailAddress(to));
                    message.Subject = "Timetable notification";
                    message.Body = m;
                    client.Send(message);
                    this.Invoke((MethodInvoker)delegate
                    {
                        mailStatus.Text = "Notification send.";
                    });
                }
            }
            catch (Exception ex)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    mailStatus.Text = ex.Message+" Check network Connection";
                });
            }
        }

        private void getMailsToSentNotification()
        {
            List<string> mails = new List<string>() { };

            var lectures = getUpcommingLectures(daysInt[DateTime.Now.DayOfWeek.ToString().Substring(0, 3)], DateTime.Now.AddMinutes(30).TimeOfDay.ToString().Substring(0,8));
            try
            {
                //for each upcoming lectures get students emails doing that unit
                foreach (var id in lectures)
                {
                    Console.WriteLine("lecture id: " + id);
                    using (var connection = con)
                    {
                        if (connection.State.Equals(ConnectionState.Closed))
                            connection.Open();
                        using (var cmd = connection.CreateCommand())
                        {

                            cmd.CommandText = "SELECT students.email FROM unitsdonebycourse right join students on unitsdonebycourse.course = students.course where unitsdonebycourse.id='" + id + "'";
                            var reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                mails.Add(reader["email"].ToString());
                            }
                        }
                    }
                    //create a message to send as notis=fication based on lecture information
                    string message = "";
                    using (var connection = con)
                    {
                        
                        if (connection.State.Equals(ConnectionState.Closed))
                            connection.Open();
                        using (var cmd = connection.CreateCommand())
                        {
                            cmd.CommandText = "select units.name,courses.initials,lecturers.name as lec,rooms.number,day,timeFrom from unitsdonebycourse left join units on units.code = unitsdonebycourse.unit left join courses on courses.id = unitsdonebycourse.course left join rooms on rooms.id = unitsdonebycourse.room left join lecturers on lecturers.id = unitsdonebycourse.lecturer where unitsdonebycourse.id ='" + id + "'";
                            MySqlDataReader reader = cmd.ExecuteReader();
                            if (reader.Read())
                            {
                                message = "You will be having a Lecture at " + reader["timeFrom"].ToString() + "\nUnit name: " + reader["name"].ToString() + "\nLecturer Name: " + reader["lec"].ToString() + "\nLecture room: " + reader["number"].ToString();
                            }
                            
                        }

                    }
                    //send mail to students suppose to attend that lecture
                    Parallel.ForEach(mails, user =>
                     {
                         this.Invoke((MethodInvoker)delegate
                         {
                             mailStatus.Text = "Sending mail Notification";
                         });
                         sendMailNotification(user, message);
                     });
                    mails.Clear();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private List<int> getUpcommingLectures(int day, string time)
        {
            List<int> lectures = new List<int>() { };
            try
            {
                using(var connection = con)
                {
                    if (connection.State.Equals(ConnectionState.Closed))
                        connection.Open();
                    using (var cmd= connection.CreateCommand())
                    {
                        cmd.CommandText = "select id from unitsdonebycourse where day = @day and timeFrom < @time and timeFrom >= @timeFrom";
                        cmd.Parameters.AddWithValue("@day", day);
                        cmd.Parameters.AddWithValue("@time", time);
                        cmd.Parameters.AddWithValue("@timeFrom", DateTime.Now.TimeOfDay.ToString().Substring(0, 8));
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            lectures.Add(int.Parse(reader["id"].ToString()));
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return lectures;
        }

        private void checkUpcoming()
        {
            while (true)
            {
                Console.WriteLine("Cheking upcoming lectures"+DateTime.Now.TimeOfDay);
                getMailsToSentNotification();
                Thread.Sleep(1000*60*30);

            }
        }

        /// <summary>
        /// Room registration function - registering new lecture room
        /// </summary>
        private void registerRoom_Click(object sender, EventArgs e)
        {
            //check if room number and location fields are filled
            if (txtRoomNo.Text.Equals("") || txtRoomLocation.Text.Equals(""))
            {
                MessageBox.Show("Please fill in all the fields");
            }
            else
                try
                {
                    using (var connection = con)
                    {
                        if (connection.State.Equals(ConnectionState.Closed))
                            connection.Open();
                        using (MySqlCommand cmd = connection.CreateCommand())
                        {
                            //bind sql parameter to prevent sql-injection
                            cmd.CommandText = "insert into rooms (number,location) values(@number,@location)";
                            cmd.Parameters.AddWithValue("@number", txtRoomNo.Text);
                            cmd.Parameters.AddWithValue("@location", txtRoomLocation.Text);
                            cmd.ExecuteNonQuery();


                            //show successful room registration
                            MessageBox.Show("Room successsfully registered");

                            //clear text box
                            txtRoomLocation.Text = txtRoomNo.Text = "";
                            //update datagrid
                            loadRooms();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
        }
        /// <summary>
        /// Lecturer registration function - registering new lecturer
        /// </summary>
        private void registerLec_Click(object sender, EventArgs e)
        {
            //check if lecturer id, name and faculty fields are filled
            if (txtLecId.Text.Equals("") || txtLecName.Text.Equals("") || txtLecFaculty.Text.Equals(""))
            {
                MessageBox.Show("Please fill in all the fields");
            }
            else
                try
                {
                    using (var connection = con)
                    {
                        if (connection.State.Equals(ConnectionState.Closed))
                            connection.Open();
                        using (MySqlCommand cmd = connection.CreateCommand())
                        {
                            cmd.CommandText = "insert into lecturers (id,name,faculty) values(@id,@name,@faculty)";
                            cmd.Parameters.AddWithValue("@id", txtLecId.Text);
                            cmd.Parameters.AddWithValue("@name", txtLecName.Text);
                            cmd.Parameters.AddWithValue("@faculty", txtLecFaculty.Text);
                            cmd.ExecuteNonQuery();

                            //show successful Lecturer registration
                            MessageBox.Show("Lecturer successsfully registered");
                            //clear text box
                            txtLecFaculty.Text = txtLecId.Text = txtLecName.Text = "";
                            //update datagrid
                            loadLecturers();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
        }
        /// <summary>
        /// Unit registration function - registering units taken in university
        /// </summary>
        private void registerUnit_Click(object sender, EventArgs e)
        {
            //check if unit code and name fields are filled
            if (txtUnitCode.Text.Equals("") || txtUnitName.Text.Equals(""))
            {
                MessageBox.Show("Please fill in all the fields");
            }
            else
                try
                {
                    using (var connection = con)
                    {
                        if (connection.State.Equals(ConnectionState.Closed))
                            connection.Open();
                        using (MySqlCommand cmd = connection.CreateCommand())
                        {
                            cmd.CommandText = "insert into units (code,name) values(@code,@name)";
                            cmd.Parameters.AddWithValue("@code", txtUnitCode.Text);
                            cmd.Parameters.AddWithValue("@name", txtUnitName.Text);
                            cmd.ExecuteNonQuery();

                            //show successful unit registration
                            MessageBox.Show("Unit successsfully registered");
                            //clear text box
                            txtUnitCode.Text = txtUnitName.Text = "";
                            //update units
                            loadUnits();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
        }

        private void registerCourse_Click(object sender, EventArgs e)
        {
            //check if Course initials and faculty fields are filled
            if (txtCourseInitials.Text.Equals("") || txtCourseFaculty.Text.Equals(""))
            {
                MessageBox.Show("Please fill in all the fields");
            }
            else
                try
                {
                    using (var connection = con)
                    {
                        if (connection.State.Equals(ConnectionState.Closed))
                            connection.Open();
                        using (MySqlCommand cmd = connection.CreateCommand())
                        {
                            //bind sql parameter to prevent sql-injection
                            cmd.CommandText = "insert into courses (initials,faculty) values(@initials,@faculty)";
                            cmd.Parameters.AddWithValue("@initials", txtCourseInitials.Text);
                            cmd.Parameters.AddWithValue("@faculty", txtCourseFaculty.Text);
                            cmd.ExecuteNonQuery();

                            //show successful room registration
                            MessageBox.Show("Course successsfully registered");
                            //clear text box
                            txtCourseInitials.Text = txtCourseFaculty.Text = "";
                            //update
                            loadCourses();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
        }

        private void registerStudent_Click(object sender, EventArgs e)
        {
            //check if all fields are filled
            if (studentRegNo.Text.Equals("") || studentName.Text.Equals("") || studentCourse.Text.Equals("") || studentEmail.Text.Equals("") || studentTelNo.Text.Equals(""))
            {
                MessageBox.Show("fill in all the fields");
            }
            else
            {
                try
                {
                    int id = getCourseId(studentCourse.Text);
                    using (MySqlConnection connection = con)
                    {
                        if (connection.State.Equals(ConnectionState.Closed))
                            connection.Open();
                        using (MySqlCommand cmd = connection.CreateCommand())
                        {
                            cmd.CommandText = "insert into students (regno,name,course,email,phone) values(@regno,@name,@course,@email,@phone)";
                            cmd.Parameters.AddWithValue("@regno", studentRegNo.Text);
                            cmd.Parameters.AddWithValue("@name", studentName.Text);
                            cmd.Parameters.AddWithValue("@course", id);
                            cmd.Parameters.AddWithValue("@email", studentEmail.Text);
                            cmd.Parameters.AddWithValue("@phone", studentTelNo.Text);
                            cmd.ExecuteNonQuery();

                            MessageBox.Show("Student successfully saved");
                            //clear text boxes
                            studentCourse.Text = studentEmail.Text = studentName.Text = studentRegNo.Text = studentTelNo.Text = "";
                            //updata datagrid
                            loadStudents();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private int getCourseId(string text)
        {
            try
            {
                using (MySqlConnection connection = con)
                {
                    if (connection.State.Equals(ConnectionState.Closed))
                        connection.Open();
                    using (MySqlCommand cmd = connection.CreateCommand())
                    {
                        cmd.CommandText = "select id from courses where initials = @initials";
                        cmd.Parameters.AddWithValue("@initials", text);
                        MySqlDataReader r = cmd.ExecuteReader();
                        if (r.Read())
                            return int.Parse(r["id"].ToString());

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return 0;
        }
        private int getLecturerId(string text)
        {
            try
            {
                using (MySqlConnection connection = con)
                {
                    if (connection.State.Equals(ConnectionState.Closed))
                        connection.Open();
                    using (MySqlCommand cmd = connection.CreateCommand())
                    {
                        cmd.CommandText = "select id from lecturers where name = @name";
                        cmd.Parameters.AddWithValue("@name", text);
                        MySqlDataReader r = cmd.ExecuteReader();
                        if (r.Read())
                            return int.Parse(r["id"].ToString());

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return 0;
        }

        private int getRoomId(string text)
        {
            try
            {
                using (MySqlConnection connection = con)
                {
                    if (connection.State.Equals(ConnectionState.Closed))
                        connection.Open();
                    using (MySqlCommand cmd = connection.CreateCommand())
                    {
                        cmd.CommandText = "select id from rooms where number = @number";
                        cmd.Parameters.AddWithValue("@number", text);
                        MySqlDataReader r = cmd.ExecuteReader();
                        if (r.Read())
                            return int.Parse(r["id"].ToString());

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return 0;
        }

        private void loadRooms()
        {
            try
            {
                using (MySqlConnection connection = con)
                {
                    if (connection.State.Equals(ConnectionState.Closed))
                        connection.Open();
                    using (MySqlCommand cmd = connection.CreateCommand())
                    {
                        cmd.CommandText = "select number,location from rooms";
                        MySqlDataReader reader = cmd.ExecuteReader();
                        dataRooms.Rows.Clear();
                        while (reader.Read())
                        {

                            int n = dataRooms.Rows.Add();
                            dataRooms.Rows[n].Cells[0].Value = n + 1;
                            dataRooms.Rows[n].Cells[1].Value = reader["number"].ToString();
                            dataRooms.Rows[n].Cells[2].Value = reader["location"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void loadLecturers()
        {
            try
            {
                using (MySqlConnection connection = con)
                {
                    if (connection.State.Equals(ConnectionState.Closed))
                        connection.Open();
                    using (MySqlCommand cmd = connection.CreateCommand())
                    {
                        cmd.CommandText = "select * from lecturers";
                        MySqlDataReader reader = cmd.ExecuteReader();
                        dataLecturer.Rows.Clear();
                        lecturersCombo.Items.Clear();
                        while (reader.Read())
                        {
                            int n = dataLecturer.Rows.Add();
                            dataLecturer.Rows[n].Cells[0].Value = reader["id"].ToString();
                            dataLecturer.Rows[n].Cells[1].Value = reader["name"].ToString();
                            dataLecturer.Rows[n].Cells[2].Value = reader["faculty"].ToString();
                            lecturersCombo.Items.Add(reader["name"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void loadStudents()
        {
            try
            {
                using (MySqlConnection connection = con)
                {
                    if (connection.State.Equals(ConnectionState.Closed))
                        connection.Open();
                    using (MySqlCommand cmd = connection.CreateCommand())
                    {
                        cmd.CommandText = "select regno,name,courses.initials from students left join courses on students.course=courses.id";
                        MySqlDataReader reader = cmd.ExecuteReader();
                        dataStudent.Rows.Clear();
                        while (reader.Read())
                        {

                            int n = dataStudent.Rows.Add();
                            dataStudent.Rows[n].Cells[0].Value = reader["regno"].ToString();
                            dataStudent.Rows[n].Cells[1].Value = reader["name"].ToString();
                            dataStudent.Rows[n].Cells[2].Value = reader["initials"].ToString();
                        }
                    }
                }
                studentCourse.Items.Clear();
                comboCourse.Items.Clear();
                foreach (string c in Courses())
                {
                    studentCourse.Items.Add(c);
                    comboCourse.Items.Add(c);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void loadUnits()
        {
            try
            {
                using (MySqlConnection connection = con)
                {
                    if (connection.State.Equals(ConnectionState.Closed))
                        connection.Open();
                    using (MySqlCommand cmd = connection.CreateCommand())
                    {
                        cmd.CommandText = "select code,name from units";
                        MySqlDataReader reader = cmd.ExecuteReader();
                        dataUnits.Rows.Clear();
                        while (reader.Read())
                        {

                            int n = dataUnits.Rows.Add();
                            dataUnits.Rows[n].Cells[0].Value = n + 1;
                            dataUnits.Rows[n].Cells[1].Value = reader["code"].ToString();
                            dataUnits.Rows[n].Cells[2].Value = reader["name"].ToString();
                        }
                    }
                }
                txtAssignCourse.Items.Clear();
                txtAssignUnit.Items.Clear();
                txtAssignLecturer.Items.Clear();
                txtAssignRoom.Items.Clear();
                foreach (var course in Courses())
                {
                    txtAssignCourse.Items.Add(course);
                }
                foreach (var unit in Units())
                {
                    txtAssignUnit.Items.Add(unit);
                }
                foreach (var lec in Lecturers())
                {
                    txtAssignLecturer.Items.Add(lec);
                }
                foreach (var room in Rooms())
                {
                    txtAssignRoom.Items.Add(room);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void loadCourses()
        {
            try
            {
                using (MySqlConnection connection = con)
                {
                    if (connection.State.Equals(ConnectionState.Closed))
                        connection.Open();
                    using (MySqlCommand cmd = connection.CreateCommand())
                    {
                        cmd.CommandText = "select initials,faculty from courses";
                        MySqlDataReader reader = cmd.ExecuteReader();
                        dataCourses.Rows.Clear();
                        comboCourseUnits.Items.Clear();
                        while (reader.Read())
                        {

                            int n = dataCourses.Rows.Add();
                            dataCourses.Rows[n].Cells[0].Value = n + 1;
                            dataCourses.Rows[n].Cells[1].Value = reader["initials"].ToString();
                            dataCourses.Rows[n].Cells[2].Value = reader["faculty"].ToString();
                            comboCourseUnits.Items.Add(reader["initials"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// All registred courses
        /// </summary>
        /// <returns>List of courses</returns>
        private List<string> Courses()
        {
            List<string> courses = new List<string>();
            try
            {
                using (MySqlConnection connection = con)
                {
                    if (connection.State.Equals(ConnectionState.Closed))
                        connection.Open();
                    using (MySqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "select initials from courses";
                        MySqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            courses.Add(reader["initials"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            return courses;

        }
        /// <summary>
        /// All registered lecture rooms
        /// </summary>
        /// <returns>List of registered lecturer room</returns>
        private List<string> Rooms()
        {
            List<string> rooms = new List<string>();
            try
            {
                using (MySqlConnection connection = con)
                {
                    if (connection.State.Equals(ConnectionState.Closed))
                        connection.Open();
                    using (MySqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "select number from rooms";
                        MySqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            rooms.Add(reader["number"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            return rooms;

        }
        /// <summary>
        /// All registered Units
        /// </summary>
        /// <returns>List of registered units</returns>
        private List<string> Units()
        {
            List<string> units = new List<string>();
            try
            {
                using (MySqlConnection connection = con)
                {
                    if (connection.State.Equals(ConnectionState.Closed))
                        connection.Open();
                    using (MySqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "select code from units";
                        MySqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            units.Add(reader["code"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            return units;

        }
        /// <summary>
        /// All registered Lecturers
        /// </summary>
        /// <returns>List of registered lecturers</returns>
        private List<string> Lecturers()
        {
            List<string> lecturers = new List<string>();
            try
            {
                using (MySqlConnection connection = con)
                {
                    if (connection.State.Equals(ConnectionState.Closed))
                        connection.Open();
                    using (MySqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "select name from lecturers";
                        MySqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            lecturers.Add(reader["name"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            return lecturers;

        }
        private void mainTab_Selected(object sender, TabControlEventArgs e)
        {
            switch (e.TabPageIndex)
            {
                case 0:
                    //homepage tab load
                    loadTable(daysInt[DateTime.Now.DayOfWeek.ToString().Substring(0, 3)]);
                    break;
                case 1:
                    //rooms
                    loadRooms();
                    break;
                case 2:
                    //lecturers
                    loadLecturers();
                    break;
                case 3:
                    //units
                    loadUnits();
                    break;
                case 4:
                    //courses
                    loadCourses();
                    break;
                case 5:
                    //students
                    loadStudents();
                    break;
                default:
                    break;
            }

        }

        private int dayIntFromString(string day)
        {
            return daysInt[day];
        }

        private string dayStringFromInt(int day)
        {
            return daysString[day];
        }

        private void Assign_Click(object sender, EventArgs e)
        {
            if (txtAssignCourse.Text.Equals("") || txtAssignLecturer.Text.Equals("") || txtAssignRoom.Text.Equals("") || txtAssignUnit.Text.Equals(""))
            {
                MessageBox.Show("Fill in all the fields");
            }
            else
            {
                try
                {
                    int room = getRoomId(txtAssignRoom.Text);
                    int lecturer = getLecturerId(txtAssignLecturer.Text);
                    int course = getCourseId(txtAssignCourse.Text);
                    using (MySqlConnection connection = con)
                    {
                        if (connection.State.Equals(ConnectionState.Closed))
                            connection.Open();
                        using (MySqlCommand cmd = connection.CreateCommand())
                        {
                            cmd.CommandText = "insert into unitsdonebycourse (unit,course,lecturer,room,day,timeFrom,timeTo) values(@unit,@course,@lecturer,@room,@day,@from,@to)";
                            cmd.Parameters.AddWithValue("@unit", txtAssignUnit.Text);
                            cmd.Parameters.AddWithValue("@course", course);
                            cmd.Parameters.AddWithValue("@lecturer", lecturer);
                            cmd.Parameters.AddWithValue("@room", room);
                            cmd.Parameters.AddWithValue("@day", dayIntFromString(txtAssignDay.Text));
                            cmd.Parameters.AddWithValue("@from", txtAssignFrom.Value.TimeOfDay.ToString().Substring(0, 8));
                            cmd.Parameters.AddWithValue("@to", txtAssignTo.Value.TimeOfDay.ToString().Substring(0, 8));
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Unit assigned");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void loadTable(int day)
        {
            try
            {
                using (MySqlConnection connection = con)
                {
                    if (connection.State.Equals(ConnectionState.Closed))
                        connection.Open();
                    using (MySqlCommand cmd = connection.CreateCommand())
                    {
                        cmd.CommandText = "select units.name,courses.initials,lecturers.name as lec,rooms.number,day,timeFrom from unitsdonebycourse left join units on units.code = unitsdonebycourse.unit left join courses on courses.id = unitsdonebycourse.course left join rooms on rooms.id = unitsdonebycourse.room left join lecturers on lecturers.id = unitsdonebycourse.lecturer where day = '" + day + "'";
                        MySqlDataReader reader = cmd.ExecuteReader();
                        dataLectures.Rows.Clear();
                        while (reader.Read())
                        {
                            int n = dataLectures.Rows.Add();
                            dataLectures.Rows[n].Cells[0].Value = n + 1;
                            dataLectures.Rows[n].Cells[1].Value = reader["name"].ToString();
                            dataLectures.Rows[n].Cells[2].Value = reader["lec"].ToString();
                            dataLectures.Rows[n].Cells[3].Value = reader["number"].ToString();
                            dataLectures.Rows[n].Cells[4].Value = reader["initials"].ToString();
                            dataLectures.Rows[n].Cells[5].Value = reader["timeFrom"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void iTalk_ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
           loadTable(daysInt[iTalk_ComboBox1.Text]);
        }

        private void iTalk_Icon_Info1_Click(object sender, EventArgs e)
        {
            new AboutBox1().ShowDialog();
        }

        private void dataLectures_CellClick(object sender, DataGridViewCellEventArgs e)
        {
         //do nothing            
        }

        private void dataRooms_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if(e.RowIndex >= 0)
                {
                    if (!dataRooms.Rows[e.RowIndex].Cells[1].Value.ToString().Equals(""))
                    {
                       DialogResult r =  MessageBox.Show("You about to delete Room: "+ dataRooms.Rows[e.RowIndex].Cells[1].Value.ToString()+"! \nContinue?", "Delete Room", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                        if(r == DialogResult.Yes)
                        {
                            using (MySqlConnection connection = con)
                            {
                                if (connection.State.Equals(ConnectionState.Closed))
                                    connection.Open();
                                using (MySqlCommand cmd = connection.CreateCommand())
                                {
                                    cmd.CommandText = "delete from rooms where number = @number";
                                    cmd.Parameters.AddWithValue("@number", dataRooms.Rows[e.RowIndex].Cells[1].Value.ToString());
                                    cmd.ExecuteNonQuery();
                                    MessageBox.Show("Room Deleted");
                                    loadRooms();
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("Room not deleted");
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error occured "+ex.Message);
            }
        }

        private void lecturersCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                using(var c = con)
                {
                    if(c.State.Equals(ConnectionState.Closed))
                    c.Open();
                    using(var cmd = c.CreateCommand())
                    {
                        cmd.CommandText = "select units.name,courses.initials,rooms.number,timeFrom from unitsdonebycourse left join units on units.code = unitsdonebycourse.unit left join courses on courses.id = unitsdonebycourse.course left join rooms on rooms.id = unitsdonebycourse.room left join lecturers on lecturers.id = unitsdonebycourse.lecturer where lecturers.name = @name";
                        cmd.Parameters.AddWithValue("@name", lecturersCombo.Text);
                        var r = cmd.ExecuteReader();
                        dataLecUnits.Rows.Clear();
                        while (r.Read())
                        {
                            int n = dataLecUnits.Rows.Add();
                            dataLecUnits.Rows[n].Cells[0].Value = n + 1;
                            dataLecUnits.Rows[n].Cells[1].Value = r["name"].ToString();
                            dataLecUnits.Rows[n].Cells[2].Value = r["number"].ToString();
                            dataLecUnits.Rows[n].Cells[3].Value = r["initials"].ToString();
                            dataLecUnits.Rows[n].Cells[4].Value = r["timeFrom"].ToString();

                        }
                    }
                }
            }
            catch(Exception ex){
                MessageBox.Show(ex.Message);
            }
        }

        private void dataLecturer_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    if (!dataLecturer.Rows[e.RowIndex].Cells[1].Value.ToString().Equals(""))
                    {
                        DialogResult r = MessageBox.Show("You about to delete Lecturer: " + dataLecturer.Rows[e.RowIndex].Cells[1].Value.ToString() + "! \nContinue?", "Delete Lecturer", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                        if (r == DialogResult.Yes)
                        {
                            using (MySqlConnection connection = con)
                            {
                                if (connection.State.Equals(ConnectionState.Closed))
                                    connection.Open();
                                using (MySqlCommand cmd = connection.CreateCommand())
                                {
                                    cmd.CommandText = "delete from lecturers where name = @name";
                                    cmd.Parameters.AddWithValue("@name", dataLecturer.Rows[e.RowIndex].Cells[1].Value.ToString());
                                    cmd.ExecuteNonQuery();
                                    MessageBox.Show("Lecturer Deleted");
                                    loadLecturers();
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("Lecturer not deleted");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occured " + ex.Message);
            }
        }

        private void dataUnits_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    if (!dataUnits.Rows[e.RowIndex].Cells[1].Value.ToString().Equals(""))
                    {
                        DialogResult r = MessageBox.Show("You about to delete Unit: " + dataUnits.Rows[e.RowIndex].Cells[1].Value.ToString() + "! \nContinue?", "Delete Unit", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                        if (r == DialogResult.Yes)
                        {
                            using (MySqlConnection connection = con)
                            {
                                if (connection.State.Equals(ConnectionState.Closed))
                                    connection.Open();
                                using (MySqlCommand cmd = connection.CreateCommand())
                                {
                                    cmd.CommandText = "delete from units where code = @code";
                                    cmd.Parameters.AddWithValue("@code", dataUnits.Rows[e.RowIndex].Cells[1].Value.ToString());
                                    cmd.ExecuteNonQuery();
                                    MessageBox.Show("Unit Deleted");
                                    loadUnits();
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("Unit not deleted");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occured " + ex.Message);
            }
        }

        private void dataCourses_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    if (!dataCourses.Rows[e.RowIndex].Cells[1].Value.ToString().Equals(""))
                    {
                        DialogResult r = MessageBox.Show("You about to delete Course: " + dataCourses.Rows[e.RowIndex].Cells[1].Value.ToString() + "! \nContinue?", "Delete Course", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                        if (r == DialogResult.Yes)
                        {
                            using (MySqlConnection connection = con)
                            {
                                if (connection.State.Equals(ConnectionState.Closed))
                                    connection.Open();
                                using (MySqlCommand cmd = connection.CreateCommand())
                                {
                                    cmd.CommandText = "delete from courses where initials = @initials";
                                    cmd.Parameters.AddWithValue("@initials", dataCourses.Rows[e.RowIndex].Cells[1].Value.ToString());
                                    cmd.ExecuteNonQuery();
                                    MessageBox.Show("Course Deleted");
                                    loadCourses();
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("Course not deleted");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occured " + ex.Message);
            }
        }

        private void comboCourseUnits_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                using (var c = con)
                {
                    if (c.State.Equals(ConnectionState.Closed))
                        c.Open();
                    using (var cmd = c.CreateCommand())
                    {
                        cmd.CommandText = "select units.code,units.name,lecturers.name as lec,rooms.number,timeFrom,day from unitsdonebycourse left join units on units.code = unitsdonebycourse.unit left join courses on courses.id = unitsdonebycourse.course left join rooms on rooms.id = unitsdonebycourse.room left join lecturers on lecturers.id = unitsdonebycourse.lecturer where courses.initials = @initials";
                        cmd.Parameters.AddWithValue("@initials", comboCourseUnits.Text);
                        var r = cmd.ExecuteReader();
                        dataCourseUnits.Rows.Clear();
                        while (r.Read())
                        {
                            int n = dataCourseUnits.Rows.Add();
                            dataCourseUnits.Rows[n].Cells[0].Value = r["code"].ToString();
                            dataCourseUnits.Rows[n].Cells[1].Value = r["name"].ToString();
                            dataCourseUnits.Rows[n].Cells[2].Value = r["lec"].ToString();
                            dataCourseUnits.Rows[n].Cells[3].Value = r["number"].ToString();
                            dataCourseUnits.Rows[n].Cells[4].Value = daysString[int.Parse(r["day"].ToString())]+" "+ r["timeFrom"].ToString();

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dataStudent_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    if (!dataStudent.Rows[e.RowIndex].Cells[0].Value.ToString().Equals(""))
                    {
                        DialogResult r = MessageBox.Show("You about to delete Student: " + dataStudent.Rows[e.RowIndex].Cells[1].Value.ToString() + "! \nContinue?", "Delete Student", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                        if (r == DialogResult.Yes)
                        {
                            using (MySqlConnection connection = con)
                            {
                                if (connection.State.Equals(ConnectionState.Closed))
                                    connection.Open();
                                using (MySqlCommand cmd = connection.CreateCommand())
                                {
                                    cmd.CommandText = "delete from students where regno = @regno";
                                    cmd.Parameters.AddWithValue("@regno", dataStudent.Rows[e.RowIndex].Cells[0].Value.ToString());
                                    cmd.ExecuteNonQuery();
                                    MessageBox.Show("Student Deleted");
                                    loadStudents();
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("Student not deleted");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occured " + ex.Message);
            }
        }

        private void comboCourse_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                using (MySqlConnection connection = con)
                {
                    if (connection.State.Equals(ConnectionState.Closed))
                        connection.Open();
                    using (MySqlCommand cmd = connection.CreateCommand())
                    {
                        cmd.CommandText = "select regno,name,courses.initials from students left join courses on students.course=courses.id where courses.initials =@initials";
                        cmd.Parameters.AddWithValue("@initials", comboCourse.Text);
                        MySqlDataReader reader = cmd.ExecuteReader();
                        dataStudent.Rows.Clear();
                        while (reader.Read())
                        {

                            int n = dataStudent.Rows.Add();
                            dataStudent.Rows[n].Cells[0].Value = reader["regno"].ToString();
                            dataStudent.Rows[n].Cells[1].Value = reader["name"].ToString();
                            dataStudent.Rows[n].Cells[2].Value = reader["initials"].ToString();
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            new Log_In().Show();
        }
    }
}
